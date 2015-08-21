using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceModel.PeerResolvers;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevOffice.Common.Models;
using DevOffice.Common.ViewModels;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Data.Providers;
using Orchard.Logging;
using Orchard.MediaLibrary.Models;
using Orchard.Services;
using Orchard.Taxonomies.Models;
using Orchard.Data;
using Orchard.MediaLibrary.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using Orchard.Taxonomies.Services;

namespace DevOffice.Common.Services
{
    public class CommonDataService : ICommonDataService
    {
        private readonly IRepository<FeedbackInformationRecord> _feedbackInformationRepository;
        private readonly IRepository<ViewCountRecord> _viewCountRepository;
        private readonly IRepository<TermContentItem> _termContentItemRepository;
        private readonly IRepository<RelatedLinkRecord> _relatedLinksRepository;
        private readonly IContentManager _contentManager;
        private readonly ITaxonomyService _taxonomyService;
        private readonly ICacheManager _cacheManager;
        private readonly IClock _clock;
        private readonly string _cacheKey;
        public ILogger Logger { get; set; }

        private List<CodeSample> AllCodeSamples { get; set; }
        private List<PatternsAndPractice> AllPatternsAndPractices { get; set; }

        public CommonDataService(
            IRepository<FeedbackInformationRecord> feedbackInformationRepository,
            IContentManager contentManager,
            ITaxonomyService taxonomyService,
            ICacheManager cacheManager,
            IClock clock, IRepository<TermContentItem> termContentItemRepository,
            IRepository<RelatedLinkRecord> relatedLinksRepository,
            IRepository<ViewCountRecord> viewCountRepository
            )
        {
            _feedbackInformationRepository = feedbackInformationRepository;
            _contentManager = contentManager;
            _taxonomyService = taxonomyService;
            _cacheManager = cacheManager;
            _clock = clock;
            _termContentItemRepository = termContentItemRepository;
            _relatedLinksRepository = relatedLinksRepository;
            _viewCountRepository = viewCountRepository;
            _cacheKey = "TopFourEditorPicks";
            Logger = NullLogger.Instance;
        }

        #region Content Item Queries

        public List<QuickLink> GetQuickLinks()
        {
            var quickLinkItems = _contentManager.Query(VersionOptions.Published, "QuickLinks").List().ToList();

            var mediaDictionary = GetMediaDictionary(quickLinkItems);

            var links = (from dynamic item in quickLinkItems
                         let titleValue = item.TitlePart.Title
                         let subText = item.QuickLinksPart.SubText.Value
                         let linkValue = item.QuickLinksPart.ExternalLink.Value
                         let sortOrder = item.QuickLinksPart.SortOrder.Value
                         let smallImageIds = item.QuickLinksPart.SmallImage.Ids
                         let smallImage = smallImageIds.Length > 0 && mediaDictionary.ContainsKey(smallImageIds[0]) ? mediaDictionary[smallImageIds[0]].MediaUrl : String.Empty
                         let bigImageIds = item.QuickLinksPart.BigImage.Ids
                         let bigImage = smallImageIds.Length > 0 && mediaDictionary.ContainsKey(bigImageIds[0]) ? mediaDictionary[bigImageIds[0]].MediaUrl : String.Empty
                         select new QuickLink
                         {
                             Title = titleValue,
                             SubText = subText,
                             ExternalLink = linkValue,
                             SmallImage = smallImage,
                             BigImage = bigImage,
                             SortOrder = sortOrder
                         }).ToList();

            return links.OrderBy(x => x.SortOrder).ToList();
        }

        public List<Event> GetEvents()
        {
            var eventItems = _contentManager.Query(VersionOptions.Published, "Event").List();

            var events = (from dynamic item in eventItems
                          let titleValue = item.TitlePart.Title
                          let subText = item.EventPart.SubText.Value
                          let location = item.EventPart.Location.Value
                          let externalLink = item.EventPart.ExternalLink.Value
                          let startDate = item.EventPart.StartDate.DateTime
                          let terms = item.EventPart.EventType.Terms
                          let eventTypes = GetTerms(item, "EventType")
                          let eventDate = Convert.ToDateTime(startDate)
                          let date = ((DateTime)eventDate).ToString("dd")
                          let year = ((DateTime)eventDate).ToString("yyyy")
                          let month = ((DateTime)eventDate).ToString("MMM").ToUpper()
                          select new Event
                          {
                              Title = titleValue,
                              SubText = subText,
                              Location = location,
                              EventType = eventTypes,
                              Month = month,
                              Date = date,
                              Year = year,
                              FullStartDate = Convert.ToDateTime(startDate),
                              ExternalLink = externalLink
                          }).ToList();
            return events.OrderBy(x => x.FullStartDate).ThenBy(x => x.Title).ToList();
        }

        public List<CommunityItem> GetCommunityItems()
        {
            var communityItems = _contentManager.Query(VersionOptions.Published, "Community").List().ToList();

            var mediaDictionary = GetMediaDictionary(communityItems);

            return (from dynamic item in communityItems
                    let subText = item.CommunityPart.SubText.Value
                    let description = item.CommunityPart.ItemDescription.Value
                    let externalLink = item.CommunityPart.ExternalLink.Value
                    let titleValue = item.TitlePart.Title
                    let imageIds = item.CommunityPart.Image.Ids
                    let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                    select new CommunityItem()
                    {
                        Title = titleValue,
                        SubText = subText,
                        Description = description,
                        ExternalLink = externalLink,
                        Image = image
                    }).ToList();
        }

        public List<Training> GetTrainingItems()
        {
            var trainingItems = _contentManager.Query(VersionOptions.Published, "Training").List();

            var mediaDictionary = GetMediaDictionary(trainingItems);

            var trainings = (from trainingItem in trainingItems
                             let item = (dynamic)trainingItem
                             let subText = item.TrainingPart.SubText.Value
                             let imageIds = item.TrainingPart.Image.Ids
                             let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                             let externalLink = item.TrainingPart.ExternalLink.Value
                             let trainingTags = GetTermsWithTitle(trainingItem, "TrainingType")
                             let datePublished = item.CommonPart.PublishedUtc.Date
                             let startDate = item.TrainingPart.StartDate.DateTime
                             let titleValue = item.TitlePart.Title
                             let links = item.TrainingPart.RelatedLinksPart.Links
                             let permalinkTag = item.TrainingPart.PermalinkTag.Value
                             let id = (int)item.CommonPart.Id
                             let viewCount = _viewCountRepository.Fetch(r => r.LinkId == id).ToList().Count
                             select new Training
                             {
                                 TermsTagged = trainingTags,
                                 TermsTaggedList = trainingTags.Values.ToList(),
                                 Title = titleValue,
                                 SubText = subText,
                                 Image = image,
                                 FullStartDate = Convert.ToDateTime(startDate),
                                 ExternalLink = externalLink,
                                 DatePublished = Convert.ToDateTime(datePublished),
                                 PermalinkTag = permalinkTag,
                                 Id = id,
                                 ViewCount = viewCount,
                                 Links = ConvertLinksModel(links),
                             }).OrderBy(x => x.FullStartDate).ThenBy(x => x.Title).ToList();

            return trainings;
        }

        public IList<RelatedLink> ConvertLinksModel(IEnumerable<RelatedLinkRecord> links)
        {
            return links.Select(relatedLinkRecord => new RelatedLink
            {
                Id = relatedLinkRecord.Id,
                SortOrder = relatedLinkRecord.SortOrder,
                Title = relatedLinkRecord.Title,
                Url = relatedLinkRecord.Url,
                Type = relatedLinkRecord.Type
            }).ToList();
        }

        public List<Resource> GetResources()
        {
            var resourcesItems = _contentManager.Query(VersionOptions.Published, "Resource").List().ToList();

            var mediaDictionary = GetMediaDictionary(resourcesItems);

            return (from dynamic item in resourcesItems
                    let id = item.Id
                    let title = item.TitlePart.Title
                    let subText = item.ResourcePart.SubText.Value
                    let linkText = item.ResourcePart.LearnMoreText.Value
                    let linkUrl = item.ResourcePart.LearnMoreUrl.Value
                    let imageIds = item.ResourcePart.Image.Ids
                    let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                    select new Resource()
                    {
                        Title = title,
                        SubText = subText,
                        Image = image,
                        LinkText = linkText,
                        LinkUrl = linkUrl,
                        Id = id
                    }).ToList();
        }

        public List<Video> GetVideos()
        {
            var videoItems = _contentManager.Query(VersionOptions.Published, "VideoItem").List().ToList();

            var mediaDictionary = GetMediaDictionary(videoItems);

            var videos = (from dynamic item in videoItems
                          let videoId = item.Id
                          let subText = item.VideoItemPart.SubText.Value
                          let imageIds = item.VideoItemPart.Image.Ids
                          let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                          let location = item.VideoItemPart.Location.Value
                          let embedCode = item.VideoItemPart.EmbedCode.Value
                          let terms = item.VideoItemPart.VideoType.Terms
                          let videoTypes = GetTerms(item, "VideoType")
                          let startDate = item.VideoItemPart.StartDate.DateTime
                          let titleValue = item.TitlePart.Title
                          let trainingDate = Convert.ToDateTime(startDate)
                          let date = ((DateTime)trainingDate).ToString("dd")
                          let year = ((DateTime)trainingDate).ToString("yyyy")
                          let month = ((DateTime)trainingDate).ToString("MMM").ToUpper()
                          select new Video
                          {
                              VideoId = videoId,
                              Title = titleValue,
                              SubText = subText,
                              Image = image,
                              Location = location,
                              VideoTypes = videoTypes,
                              Month = month,
                              Date = date,
                              Year = year,
                              FullStartDate = Convert.ToDateTime(startDate),
                              EmbedCode = embedCode
                          }).ToList();

            return videos.OrderBy(x => x.FullStartDate).ThenBy(x => x.Title).ToList();
        }

        public List<Training> GetPodcasts()
        {
            var podcastItems = _contentManager.Query(VersionOptions.Published, "Podcast")
                .WithQueryHintsFor("Podcast").List();

            var mediaDictionary = GetMediaDictionary(podcastItems);

            var podcasts = (from dynamic podcastItem in podcastItems
                            let subText = podcastItem.PodcastPart.SubText.Value
                            let imageIds = podcastItem.PodcastPart.Image.Ids
                            let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                            let location = podcastItem.PodcastPart.Location.Value
                            let externalLink = podcastItem.PodcastPart.ExternalLink.Value
                            let podcastTypes = GetTerms(podcastItem, "PodcastType")
                            let startDate = podcastItem.PodcastPart.StartDate.DateTime
                            let titleValue = podcastItem.TitlePart.Title
                            let trainingDate = Convert.ToDateTime(startDate)
                            let date = ((DateTime)trainingDate).ToString("dd")
                            let year = ((DateTime)trainingDate).ToString("yyyy")
                            let month = ((DateTime)trainingDate).ToString("MMM").ToUpper()
                            let datePublished = podcastItem.CommonPart.PublishedUtc.Date
                            let links = podcastItem.RelatedLinksPart.Links
                            let permalinkTag = podcastItem.PodcastPart.PermalinkTag.Value
                            let id = podcastItem.CommonPart.Id
                            select new Training
                            {
                                Title = titleValue,
                                SubText = subText,
                                Image = image,
                                Location = location,
                                TrainingTypes = podcastTypes,
                                Month = month,
                                Date = date,
                                Year = year,
                                FullStartDate = Convert.ToDateTime(startDate),
                                ExternalLink = externalLink,
                                DatePublished = datePublished,
                                Links = ConvertLinksModel(links),
                                PermalinkTag = permalinkTag,
                                Id = id
                            }).ToList();

            return podcasts.OrderByDescending(x => x.FullStartDate).ThenBy(x => x.Title).ToList();
        }

        public List<GettingStartedTab> GetGettingStartedTabs()
        {
            var gettingStartedItems = _contentManager.Query(VersionOptions.Published, "GettingStarted").List().ToList();

            var mediaDictionary = GetMediaDictionary(gettingStartedItems);

            var gettingStartedTabs = (from dynamic item in gettingStartedItems
                                      let title = item.TitlePart.Title
                                      let ordering = item.GettingStartedPart.Ordering.Value
                                      let order = ordering != null ? (int)ordering : 0
                                      let hashtagValue = item.GettingStartedPart.HashTag.Value
                                      let hashtag = !string.IsNullOrEmpty(hashtagValue) ? hashtagValue.Replace(" ", string.Empty) : string.Empty
                                      let firstBlockIcon1Ids = item.GettingStartedPart.FirstBlockIcon1.Ids
                                      let firstBlockIcon1 = firstBlockIcon1Ids.Length > 0 && mediaDictionary.ContainsKey(firstBlockIcon1Ids[0]) ? mediaDictionary[firstBlockIcon1Ids[0]].MediaUrl : String.Empty
                                      let firstBlockIcon2Ids = item.GettingStartedPart.FirstBlockIcon2.Ids
                                      let firstBlockIcon2 = firstBlockIcon2Ids.Length > 0 && mediaDictionary.ContainsKey(firstBlockIcon2Ids[0]) ? mediaDictionary[firstBlockIcon2Ids[0]].MediaUrl : String.Empty
                                      let firstBlockIcon3Ids = item.GettingStartedPart.FirstBlockIcon3.Ids
                                      let firstBlockIcon3 = firstBlockIcon1Ids.Length > 0 && mediaDictionary.ContainsKey(firstBlockIcon3Ids[0]) ? mediaDictionary[firstBlockIcon3Ids[0]].MediaUrl : String.Empty
                                      let firstBlockIcon4Ids = item.GettingStartedPart.FirstBlockIcon4.Ids
                                      let firstBlockIcon4 = firstBlockIcon1Ids.Length > 0 && mediaDictionary.ContainsKey(firstBlockIcon4Ids[0]) ? mediaDictionary[firstBlockIcon4Ids[0]].MediaUrl : String.Empty
                                      let firstBlockIcon5Ids = item.GettingStartedPart.FirstBlockIcon5.Ids
                                      let firstBlockIcon5 = firstBlockIcon1Ids.Length > 0 && mediaDictionary.ContainsKey(firstBlockIcon5Ids[0]) ? mediaDictionary[firstBlockIcon5Ids[0]].MediaUrl : String.Empty
                                      let firstBlockIcon6Ids = item.GettingStartedPart.FirstBlockIcon6.Ids
                                      let firstBlockIcon6 = firstBlockIcon1Ids.Length > 0 && mediaDictionary.ContainsKey(firstBlockIcon6Ids[0]) ? mediaDictionary[firstBlockIcon6Ids[0]].MediaUrl : String.Empty

                                      select new GettingStartedTab
                                      {
                                          Title = title,
                                          HashTag = hashtag.ToLower(),
                                          Intro = item.GettingStartedPart.Intro.Value,
                                          FirstBlockTitle = item.GettingStartedPart.FirstBlockTitle.Value,
                                          FirstBlockContent = item.GettingStartedPart.FirstBlockContent.Value,
                                          SecondBlockTitle = item.GettingStartedPart.SecondBlockTitle.Value,
                                          SecondBlockContent = item.GettingStartedPart.SecondBlockContent.Value,
                                          ThirdBlockTitle = item.GettingStartedPart.ThirdBlockTitle.Value,
                                          ThirdBlockContent = item.GettingStartedPart.ThirdBlockContent.Value,
                                          FirstBlockLayoutStyle = item.GettingStartedPart.FirstBlockLayoutStyle.Value,
                                          DocumentsLink = item.GettingStartedPart.DocumentsLink.Value,
                                          MVALink = item.GettingStartedPart.MVALink.Value,
                                          SamplesLink = item.GettingStartedPart.SamplesLink.Value,
                                          Icons = GetMediaUrlsWithLinks(item.GettingStartedPart.Icons),
                                          FirstBlockTitle1 = item.GettingStartedPart.FirstBlockTitle1.Value,
                                          FirstBlockTitle2 = item.GettingStartedPart.FirstBlockTitle2.Value,
                                          FirstBlockTitle3 = item.GettingStartedPart.FirstBlockTitle3.Value,
                                          FirstBlockTitle4 = item.GettingStartedPart.FirstBlockTitle4.Value,
                                          FirstBlockTitle5 = item.GettingStartedPart.FirstBlockTitle5.Value,
                                          FirstBlockTitle6 = item.GettingStartedPart.FirstBlockTitle6.Value,
                                          FirstBlockIcon1 = firstBlockIcon1,
                                          FirstBlockIcon2 = firstBlockIcon2,
                                          FirstBlockIcon3 = firstBlockIcon3,
                                          FirstBlockIcon4 = firstBlockIcon4,
                                          FirstBlockIcon5 = firstBlockIcon5,
                                          FirstBlockIcon6 = firstBlockIcon6,
                                          FirstBlockScreenshots1 = GetMediaUrlsWithLinks(item.GettingStartedPart.FirstBlockScreenshots1),
                                          FirstBlockScreenshots2 = GetMediaUrlsWithLinks(item.GettingStartedPart.FirstBlockScreenshots2),
                                          FirstBlockScreenshots3 = GetMediaUrlsWithLinks(item.GettingStartedPart.FirstBlockScreenshots3),
                                          FirstBlockScreenshots4 = GetMediaUrlsWithLinks(item.GettingStartedPart.FirstBlockScreenshots4),
                                          FirstBlockScreenshots5 = GetMediaUrlsWithLinks(item.GettingStartedPart.FirstBlockScreenshots5),
                                          FirstBlockScreenshots6 = GetMediaUrlsWithLinks(item.GettingStartedPart.FirstBlockScreenshots6),
                                          Ordering = order
                                      }).ToList();
            return gettingStartedTabs.OrderBy(x => x.Ordering).ToList();
        }
        public BlogPost GetArticleById(int id)
        {

            dynamic item = _contentManager.Get(id);
            if (item == null || item.ContentType != "Article")
            {
                return null;
            }
            var mediaDictionary = GetMediaDictionary(item);

            var imageIds = item.ArticlePart.Image.Ids;

            var blogPost = new BlogPost()
            {
                Id = id,
                Title = item.TitlePart.Title,
                Author = string.Concat(item.ArticlePart.AuthorFirstName.Value, " ", item.ArticlePart.AuthorLastName.Value),
                Description = item.ArticlePart.Description.Value,
                Image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty,
                DatePublished = Convert.ToDateTime(item.CommonPart.PublishedUtc),
                PostBody = item.ArticlePart.PostBody.Value
            };

            return blogPost;

        }
        public List<BlogPost> GetArticles(int pageNumber = 0, int count = 0)
        {
            var articles = new List<BlogPost>();
            if (count == 0)
            {
                return articles;
            }
            var start = 0;
            if (pageNumber > 1)
            {
                start = (pageNumber - 1) * count;
            }
            var articleItems = _contentManager.Query(VersionOptions.Published, "Article")
                .Join<CommonPartRecord>()
                .OrderByDescending(cr => cr.PublishedUtc)
                .Slice(start, count).ToList();

            var mediaDictionary = GetMediaDictionary(articleItems);

            return articles = (from dynamic item in articleItems
                               let id = item.Id
                               let titleValue = item.TitlePart.Title
                               let authorFirstName = item.ArticlePart.AuthorFirstName.Value
                               let authorLastName = item.ArticlePart.AuthorLastName.Value
                               let description = item.ArticlePart.Description.Value
                               let imageIds = item.ArticlePart.Image.Ids
                               let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                               let link = item.ArticlePart.Link.Value
                               let topics = GetTermNames(item, "ArticleTopic")
                               let startDate = item.CommonPart.PublishedUtc
                               let postBody = item.ArticlePart.PostBody.Value
                               select new BlogPost
                               {
                                   Id = id,
                                   Title = titleValue,
                                   Author = string.Join(" ", new string[] { authorFirstName, authorLastName }),
                                   Description = description,
                                   Image = image,
                                   Link = link,
                                   Topics = topics,
                                   DatePublished = Convert.ToDateTime(startDate),
                                   PostBody = postBody
                               }).ToList();
        }

        public int GetAllArticlesCount()
        {
            return _contentManager.Query(VersionOptions.Published, "Article").Count();
        }

        private List<ImageLinks> GetMediaUrlsWithLinks(dynamic mediaField)
        {
            var icons = new List<ImageLinks>();
            if (mediaField.Ids.Length > 0)
            {
                foreach (MediaPart mediaPart in mediaField.MediaParts)
                {
                    icons.Add(new ImageLinks
                    {
                        Image = mediaPart.MediaUrl,
                        ImageUrl = mediaPart.Caption,
                        Title = mediaPart.Title
                    });
                }
            }
            return icons;
        }


        public List<CodeSample> GetCodeSamples()
        {
            if (AllCodeSamples == null)
            {
                AllCodeSamples = CodeSamplesQuery();
            }
            return AllCodeSamples;

        }

        private List<CodeSample> CodeSamplesQuery()
        {
            var codeSampleItems = _contentManager.Query(VersionOptions.Published, "CodeSample").List();

            var mediaDictionary = GetMediaDictionary(codeSampleItems);

            var codeSamples = (from codeSampleItem in codeSampleItems
                               let item = (dynamic)codeSampleItem
                               let subText = item.CodeSamplePart.SubText.Value
                               let imageIds = item.CodeSamplePart.Image.Ids
                               let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                               let location = item.CodeSamplePart.Location.Value
                               let externalLinkValue = item.CodeSamplePart.ExternalLink.Value
                               let externalLink = !string.IsNullOrEmpty(externalLinkValue) && externalLinkValue.StartsWith("https://github.com") ? string.Format("code-samples-detail/{0}", item.Id) : externalLinkValue
                               let terms = _taxonomyService.GetTermsForContentItem(codeSampleItem.Id).Select(x => x.Name.ToLower()).ToList()
                               let startDate = item.CodeSamplePart.StartDate.DateTime
                               let titleValue = item.TitlePart.Title
                               let datePublished = item.CommonPart.PublishedUtc.Date
                               let links = item.CodeSamplePart.RelatedLinksPart.Links
                               let permalinkTag = item.CodeSamplePart.PermalinkTag.Value
                               let id = (int)item.CommonPart.Id
                               let viewCount = _viewCountRepository.Fetch(r => r.LinkId == id).ToList().Count
                               select new CodeSample
                               {
                                   Title = titleValue,
                                   SubText = subText,
                                   Image = image,
                                   Location = location,
                                   TermTypes = terms,
                                   FullStartDate = Convert.ToDateTime(startDate),
                                   ExternalLink = (string)externalLink,
                                   DatePublished = Convert.ToDateTime(datePublished),
                                   PermalinkTag = permalinkTag,
                                   Id = id,
                                   Links = ConvertLinksModel(links),
                                   ViewCount = viewCount
                               }).OrderByDescending(x => x.DatePublished).ThenBy(x => x.Title).ToList();

            return codeSamples;
        }



        private List<PatternsAndPractice> GetPatternsAndPracticesQuery()
        {
            var patsAndPracsItems = _contentManager.Query(VersionOptions.Published, "PatternsAndPractices").List();

            var mediaDictionary = GetMediaDictionary(patsAndPracsItems);

            var patAndPracs = (from patsAndPracsItem in patsAndPracsItems
                               let item = (dynamic)patsAndPracsItem
                               let titleValue = item.TitlePart.Title
                               let subText = item.PatternsAndPracticesPart.SubText.Value
                               let ordering = item.PatternsAndPracticesPart.Ordering.Value
                               let order = ordering != null ? (int)ordering : 0
                               let externalLinkValue = item.PatternsAndPracticesPart.ExternalLink.Value
                               let externalLink = !string.IsNullOrEmpty(externalLinkValue) && externalLinkValue.StartsWith("https://github.com") ? string.Format("patterns-and-practices-detail/{0}", item.Id) : externalLinkValue
                               let imageIds = item.PatternsAndPracticesPart.Icon.Ids
                               let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                               let terms = _taxonomyService.GetTermsForContentItem(patsAndPracsItem.Id).Select(x => x.Name.ToLower()).ToList()
                               let permalinkTag = item.PatternsAndPracticesPart.PermalinkTag.Value
                               let id = (int)item.CommonPart.Id
                               let viewCount = _viewCountRepository.Fetch(r => r.LinkId == id).ToList().Count
                               let updatedDate = item.CommonPart.ModifiedUtc
                               let links = item.PatternsAndPracticesPart.RelatedLinksPart.Links
                               select new PatternsAndPractice
                               {
                                   Title = titleValue,
                                   SubText = subText,
                                   Ordering = order,
                                   ExternalLink = externalLink,
                                   Image = image,
                                   PatternsAndPracticesTypes = terms,
                                   PermalinkTag = permalinkTag,
                                   Id = id,
                                   ViewCount = viewCount,
                                   UpdatedDate = updatedDate,
                                   Links = ConvertLinksModel(links),
                               }).ToList();
            return patAndPracs.OrderBy(x => x.Ordering).ToList();
        }

        public List<PatternsAndPractice> GetPatternsAndPractices()
        {
            if (AllPatternsAndPractices == null)
            {
                AllPatternsAndPractices = GetPatternsAndPracticesQuery();
            }
            return AllPatternsAndPractices;
        }

        public List<int> GetTopViewedItems(string type)
        {
            // get all unique items from the last 30 days
            DateTime oneMonthAgo = DateTime.UtcNow.AddDays(-30);


            //     return (from view in _viewCountRepository.
            //             from orderedProduct in db.OrderedProducts
            //             where orderedProduct.ProductID == product.ProductID
            //             group orderedProduct by product into productGroups
            //             select new ProductOrders
            //             {
            //                 product = productGroups.Key,
            //                 numberOfOrders = productGroups.Count()
            //             }
            //).OrderByDescending(x => x.numberOfOrders).Distinct().Take(10);


            var results = _viewCountRepository.Fetch(r => r.Date >= oneMonthAgo).Where(r => r.Type == type).Select(r => r.LinkId).ToList();
            var viewDictionary = new Dictionary<int, int>();
            foreach (var result in results)
            {
                if (viewDictionary.ContainsKey(result))
                {
                    viewDictionary[result] += 1;
                }
                else
                {
                    viewDictionary[result] = 1;
                }

            }
            List<KeyValuePair<int, int>> viewList = viewDictionary.ToList();

            viewList.Sort((firstPair, nextPair) => nextPair.Value.CompareTo(firstPair.Value));
            var topViewed = viewList.Take(9).Select(kvp => kvp.Key).ToList();

            return topViewed;


        }

        public List<Solution> GetSolutions()
        {
            var solutionItems = _contentManager.Query(VersionOptions.Published, "Solution").List().ToList();

            var mediaDictionary = GetMediaDictionary(solutionItems);

            var solutions = (from solutionItem in solutionItems
                             let item = (dynamic)solutionItem
                             let titleValue = item.TitlePart.Title
                             let content = item.SolutionPart.Content.Value
                             let ordering = item.SolutionPart.Ordering.Value
                             let order = ordering != null ? (int)ordering : 0
                             let externalLink = item.SolutionPart.ExternalLink.Value
                             let contentItemTerms = _taxonomyService.GetTermsForContentItem(solutionItem.Id).Select(x => x.Name.ToLower()).ToList()
                             let solutionTypesTerms = item.SolutionPart.SolutionType.Terms
                             let solutionTypes = (List<int>)(solutionTypesTerms != null && solutionTypesTerms.Count > 0 ? ((List<TermPart>)(solutionTypesTerms)).Select(x => x.Weight).ToList() : new List<int>())
                             let solutionDevicesTerms = item.SolutionPart.SolutionType.Terms
                             let solutionDevices = (List<int>)(solutionDevicesTerms != null && solutionDevicesTerms.Count > 0 ? ((List<TermPart>)(solutionDevicesTerms)).Select(x => x.Weight).ToList() : new List<int>())
                             let solutionIdentityTerms = item.SolutionPart.SolutionType.Terms
                             let solutionIdentityIntegrationMethods = (List<int>)(solutionIdentityTerms != null && solutionIdentityTerms.Count > 0 ? ((List<TermPart>)(solutionIdentityTerms)).Select(x => x.Weight).ToList() : new List<int>())
                             let solutionPrimaryTerms = item.SolutionPart.SolutionType.Terms
                             let solutionPrimaryWorkloadInvolved = (List<int>)(solutionPrimaryTerms != null && solutionPrimaryTerms.Count > 0 ? ((List<TermPart>)(solutionPrimaryTerms)).Select(x => x.Weight).ToList() : new List<int>())
                             let imageIds = item.SolutionPart.Icon.Ids
                             let image = imageIds.Length > 0 && mediaDictionary.ContainsKey(imageIds[0]) ? mediaDictionary[imageIds[0]].MediaUrl : String.Empty
                             select new Solution
                             {
                                 Title = titleValue,
                                 Content = content,
                                 Image = image,
                                 Ordering = order,
                                 ExternalLink = externalLink,
                                 FilterTerms = contentItemTerms,
                                 SolutionType = solutionTypes,
                                 SolutionDevice = solutionDevices,
                                 SolutionIdentityIntegrationMethod = solutionIdentityIntegrationMethods,
                                 SolutionPrimaryWorkloadInvolved = solutionPrimaryWorkloadInvolved
                             }).ToList();
            return solutions.OrderBy(x => x.Ordering).ToList();
        }

        public List<Solution> GetSolutions(string[] solutionFiltersArray)
        {
            var solutions = GetSolutions();
            var query = from solution in solutions
                        select solution;

            var termFilters = new List<string>();
            termFilters.AddRange(solutionFiltersArray.Select(x => x.ToLower()));

            if (termFilters.Any())
            {
                query = query.Where(s => termFilters.Any(x => s.FilterTerms.Contains(x)));
            }

            return query.ToList();
        }

        public List<Solution> GetTopFourSolutionsFromCache()
        {

            return _cacheManager.Get(_cacheKey, ctx =>
            {
                ctx.Monitor(_clock.WhenUtc(CacheExpiryTime()));
                return GetTopFourSolutions();
            });
        }

        private List<Solution> GetTopFourSolutions()
        {
            var solutions = new List<Solution>();
            try
            {
                const string url = "https://store.office.com/search.aspx?ui=en-US&rs=en-US&ad=US&catfltr=4&category=Editor%2527s%2BPicks";
                var web = new HtmlWeb();
                var htmlDoc = web.Load(url);
                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='appCell']");

                for (int i = 0; i < 4; i++)
                {

                    var iconUrl = nodes[i].SelectSingleNode(".//img").GetAttributeValue("src", null);
                    var externalLink = nodes[i].SelectSingleNode(".//a").GetAttributeValue("href", null);
                    var desc = nodes[i].SelectSingleNode(".//div[@class='appDescription']").InnerText;
                    var appTitle = nodes[i].SelectSingleNode(".//div[@class='appTitle']").InnerText;
                    var solType = new List<int>();
                    solType.Add(3);
                    var topFourSolutionViewModel = new Solution
                    {
                        Image = iconUrl,
                        ExternalLink = externalLink,
                        Title = appTitle,
                        Content = desc,
                        SolutionType = solType,
                        Ordering = 0
                    };
                    solutions.Add(topFourSolutionViewModel);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An unexpected error occurred at GetTopFourSolutions");
            }
            if (solutions.Count > 0)
            {
                // add or overwrite the never expiring global cache with new solutions
                if (HttpRuntime.Cache["TopFourEditorPicksGlobal"] == null)
                {
                    HttpRuntime.Cache.Add("TopFourEditorPicksGlobal", solutions, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
                else
                {
                    HttpRuntime.Cache.Insert("TopFourEditorPicksGlobal", solutions);
                }
            }
            if (solutions.Count == 0 && HttpRuntime.Cache["TopFourEditorPicksGlobal"] != null)
            {
                // return global cache if there are no solutions, i.e. in case of an exception
                return (List<Solution>)HttpRuntime.Cache.Get("TopFourEditorPicksGlobal");
            }
            return solutions;

        }

        private DateTime CacheExpiryTime()
        {
            // cache needs to expire at 1 am UTC time everyday
            DateTime oneAmUTCTime = DateTime.UtcNow.Date.AddHours(8);
            if (DateTime.UtcNow < oneAmUTCTime)
            {
                return oneAmUTCTime;
            }
            return oneAmUTCTime.AddDays(1);
        }

        public SectionsWithTilesPageViewModel GetSectionsWithTilesPage(ContentItem pageItem)
        {
            dynamic item = pageItem;

            var mediaDictionary = GetMediaDictionary(item);
            var resources = GetResources();
            var tiles = GetAllTiles().ToList();

            var Section1TilesIds = ((Array)item.SectionsWithTilesPagePart.Section1Tiles.Ids).OfType<int>().ToList();
            var Section2TilesIds = ((Array)item.SectionsWithTilesPagePart.Section2Tiles.Ids).OfType<int>().ToList();
            var resourceIds = ((Array)item.SectionsWithTilesPagePart.BottomRowResources.Ids).OfType<int>().ToList();

            return new SectionsWithTilesPageViewModel
            {
                ExternalLink = item.SectionsWithTilesPagePart.ExternalLink.Value,
                BannerImage = item.SectionsWithTilesPagePart.BannerImage.Ids.Length > 0 && mediaDictionary.ContainsKey(item.SectionsWithTilesPagePart.BannerImage.Ids[0])
                            ? item.SectionsWithTilesPagePart.BannerImage.MediaParts[0].MediaUrl : string.Empty,
                LinkText = item.SectionsWithTilesPagePart.LinkText.Value,
                Body = item.SectionsWithTilesPagePart.Body.Value,
                Subtitle = item.SectionsWithTilesPagePart.Subtitle.Value,
                Image = item.SectionsWithTilesPagePart.Image.Ids.Length > 0 && mediaDictionary.ContainsKey(item.SectionsWithTilesPagePart.Image.Ids[0])
                            ? item.SectionsWithTilesPagePart.Image.MediaParts[0].MediaUrl : string.Empty,
                Title = item.TitlePart.Title,
                TilePageStyle = item.SectionsWithTilesPagePart.TilesPageStyle.Value,


                Section1Title = item.SectionsWithTilesPagePart.Section1Title.Value,
                Section1Body = item.SectionsWithTilesPagePart.Section1Body.Value,
                Section1ExternalLink = item.SectionsWithTilesPagePart.Section1ExternalLink.Value,
                Section1LinkText = item.SectionsWithTilesPagePart.Section1LinkText.Value,
                Section1Tiles = tiles.Where(x => Section1TilesIds.Contains(x.Id)).ToList(),

                Section2Title = item.SectionsWithTilesPagePart.Section2Title.Value,
                Section2Body = item.SectionsWithTilesPagePart.Section2Body.Value,
                Section2ExternalLink = item.SectionsWithTilesPagePart.Section2ExternalLink.Value,
                Section2LinkText = item.SectionsWithTilesPagePart.Section2LinkText.Value,
                Section2Tiles = tiles.Where(x => Section2TilesIds.Contains(x.Id)).ToList(),
                Resources = resources.Where(x => resourceIds.Contains(x.Id)).ToList(),
                PartnerLogos = GetPartnerLogos(item)

            };
        }

        private List<string> GetPartnerLogos(dynamic item)
        {
            List<string> logoPaths = new List<string>();
            foreach (var logo in item.SectionsWithTilesPagePart.PartnerLogos.MediaParts)
            {
                logoPaths.Add(logo.MediaUrl);
            }
            return logoPaths;
        }

        private IEnumerable<Tile> GetAllTiles()
        {
            var tileItems = _contentManager.Query(VersionOptions.Published, "Tile").List().ToList();

            var mediaDictionary = GetMediaDictionary(tileItems);

            return (from dynamic item in tileItems
                    let title = item.TitlePart.Title
                    let id = item.Id
                    let url = item.TilePart.Url.Value
                    let icon = item.TilePart.Icon.Ids.Length > 0 && mediaDictionary.ContainsKey(item.TilePart.Icon.Ids[0])
                                ? item.TilePart.Icon.MediaParts[0].MediaUrl : string.Empty
                    let linkText = item.TilePart.LinkText.Value
                    let description = item.TilePart.Description.Value
                    select new Tile
                    {
                        Id = id,
                        Url = url,
                        Icon = icon,
                        LinkText = linkText,
                        Description = description,
                        Title = title
                    }
            ).ToList();
        }



        #endregion


        #region CRUD Operations

        public void CreateFeedbackRecord(FeedbackInformationRecord feedback)
        {
            feedback.DateCreated = DateTime.UtcNow;
            _feedbackInformationRepository.Create(feedback);
        }

        public RelatedLinksViewModel BuildEditorViewModel(RelatedLinksPart part, string itemsJson = "")
        {
            var ivm = new RelatedLinksViewModel();
            ivm.Links = new List<RelatedLinkRecord>();
            ivm.RelatedLinksGroup = part.RelatedLinksGroup;
            ivm.LinksJson = part.LinksJson;

            foreach (var item in part.Links)
            {
                ivm.Links.Add(new RelatedLinkRecord
                {
                    Id = item.Id,
                    Type = item.Type,
                    Title = item.Title,
                    Url = item.Url,
                    SortOrder = item.SortOrder,
                });
            }
            ivm.Links = ivm.Links.OrderBy(x => x.SortOrder).ToList();
            if (!string.IsNullOrEmpty(itemsJson))
            {
                ivm.LinksJson = itemsJson;
            }
            return ivm;
        }

        public void UpdateRelatedLinks(ContentItem item, RelatedLinksViewModel model)
        {
            var relatedLinks = item.As<RelatedLinksPart>().Record;
            var links = relatedLinks.Links;
            relatedLinks.RelatedLinksGroup = model.RelatedLinksGroup;
            relatedLinks.LinksJson = model.LinksJson;

            var linksJson = model.LinksJson;

            var oldLinks = _relatedLinksRepository.Fetch(r => r.RelatedLinksRecord.Id == relatedLinks.Id).ToList();
            var updatedLinks = (List<RelatedLinkRecord>)JsonConvert.DeserializeObject(linksJson, typeof(List<RelatedLinkRecord>));

            var deletedLinks = oldLinks.Where(x => !updatedLinks.Select(ui => ui.Id).Contains(x.Id));

            foreach (var deletedLink in deletedLinks)
            {
                _relatedLinksRepository.Delete(deletedLink);
            }

            foreach (var updatedLink in updatedLinks)
            {
                // Check if request is coming from a new lanugage translation edit page (/translate/id) OR english edit page (/edit/id)

                // "!links.Any()" is true if request is coming from (/translate/id) edit page - Create new RelatedLinkRecord irrespective of Id's - Fix for missing English links when creating translations
                // OR
                // "updatedLink.Id == 0" is true if request is coming from (/edit/id) edit page as brand new link
                if (updatedLink.Id == 0 || !links.Any())
                {
                    updatedLink.RelatedLinksRecord = relatedLinks;
                    _relatedLinksRepository.Create(updatedLink);
                }
                // "updatedLink.Id > 0" is true if request is coming from (/edit/id) admin page as existing links
                else if (updatedLink.Id > 0)
                {
                    updatedLink.RelatedLinksRecord = relatedLinks;
                    _relatedLinksRepository.Update(updatedLink);
                }
            }

            //Updating Links in the part manually. 
            //NOTE: Deleting this line will have a delay of 5 mins to update Links on the edit page
            relatedLinks.Links = _relatedLinksRepository.Fetch(r => r.RelatedLinksRecord.Id == relatedLinks.Id).ToList();
        }


        #endregion


        #region Utility functions

        /// <summary>
        /// Get a Dictionary of all Media items referenced by the ContentItems
        /// </summary>
        /// <param name="items">ContentItems for which to retrieve all referenced Media items</param>
        /// <param name="firstOnly">Retrieve only the first media item?</param>
        /// <returns>Dictionary of all Media items referenced by the ContentItems</returns>
        private Dictionary<int, MediaPart> GetMediaDictionary(IEnumerable<ContentItem> items, bool firstOnly = true)
        {

            // Get all MediaLibraryPickerFields from all the parts of all the content items
            var mediaLibraryFields = items.SelectMany(i => i.Parts.SelectMany(p => p.Fields.Where(f => f is MediaLibraryPickerField)))
                                          .Cast<MediaLibraryPickerField>();

            return GetMediaDictionary(mediaLibraryFields, firstOnly);
        }

        /// <summary>
        /// Get a dictionary of all Media items referenced by the ContentItem
        /// </summary>
        /// <param name="item">ContentItem for which to retrieve all referenced Media items</param>
        /// <param name="firstOnly">Retrieve only the first media item?</param>
        /// <returns>Dictionary of all Media items referenced by the ContentItem</returns>
        private Dictionary<int, MediaPart> GetMediaDictionary(ContentItem item, bool firstOnly = true)
        {

            // Get all MediaLibraryPickerFields from all parts of the content item
            var mediaLibraryFields = item.Parts.SelectMany(p => p.Fields.Where(f => f is MediaLibraryPickerField))
                                         .Cast<MediaLibraryPickerField>();

            return GetMediaDictionary(mediaLibraryFields, firstOnly);
        }

        /// <summary>
        /// Get a dictionary of all Media items referenced by the MediaLibraryPickerFields
        /// </summary>
        /// <param name="mediaLibraryFields">Fields for which to retrieve all referenced Media items</param>
        /// <param name="firstOnly">Retrieve only the first media item?</param>
        /// <returns>Dictionary of all Media items referenced by the MediaLibraryPickerFields</returns>
        private Dictionary<int, MediaPart> GetMediaDictionary(IEnumerable<MediaLibraryPickerField> mediaLibraryFields, bool firstOnly)
        {

            int[] mediaIds;
            if (firstOnly)
            {
                // Get the first MediaPart Id (Ids array is not lazy loaded, so safe to access without triggering a SQL query)
                mediaIds = mediaLibraryFields.Select(f => f.Ids.FirstOrDefault())
                                             .Where(id => id != default(int))
                                             .Distinct()
                                             .ToArray();
            }
            else
            {
                // Get all MediaPart Ids
                mediaIds = mediaLibraryFields.SelectMany(f => f.Ids)
                                             .Where(id => id != default(int))
                                             .Distinct()
                                             .ToArray();
            }

            // Get and return the MediaParts by their Id
            return _contentManager.GetMany<MediaPart>(mediaIds, VersionOptions.Published, QueryHints.Empty)
                                  .ToDictionary(media => media.Id);
        }

        private List<int> GetTerms(ContentItem item, string field)
        {
            //var ids = items.Select(x => x.Id).ToList();
            var id = item != null ? item.Id : 0;
            var termIds = String.IsNullOrEmpty(field)
                ? _termContentItemRepository.Fetch(x => x.TermsPartRecord.ContentItemRecord.Id == id).Select(t => t.TermRecord.Id).ToList()
                : _termContentItemRepository.Fetch(x => x.TermsPartRecord.ContentItemRecord.Id == id && x.Field == field).Select(t => t.TermRecord.Id).ToList();

            return _contentManager.GetMany<TermPart>(termIds, VersionOptions.Published, QueryHints.Empty).Select(term => term.Weight).ToList();
        }

        private List<string> GetTermNames(ContentItem item, string field)
        {
            //var ids = items.Select(x => x.Id).ToList();
            var id = item != null ? item.Id : 0;
            var termIds = String.IsNullOrEmpty(field)
                ? _termContentItemRepository.Fetch(x => x.TermsPartRecord.ContentItemRecord.Id == id).Select(t => t.TermRecord.Id).ToList()
                : _termContentItemRepository.Fetch(x => x.TermsPartRecord.ContentItemRecord.Id == id && x.Field == field).Select(t => t.TermRecord.Id).ToList();

            return _contentManager.GetMany<TermPart>(termIds, VersionOptions.Published, QueryHints.Empty).Select(term => term.Name).ToList();
        }

        private Dictionary<int, string> GetTermsWithTitle(ContentItem item, string field)
        {
            //var ids = items.Select(x => x.Id).ToList();
            var id = item != null ? item.Id : 0;
            var termIds = String.IsNullOrEmpty(field)
                ? _termContentItemRepository.Fetch(x => x.TermsPartRecord.ContentItemRecord.Id == id).Select(t => t.TermRecord.Id).ToList()
                : _termContentItemRepository.Fetch(x => x.TermsPartRecord.ContentItemRecord.Id == id && x.Field == field).Select(t => t.TermRecord.Id).ToList();

            return _contentManager.GetMany<TermPart>(termIds, VersionOptions.Published, QueryHints.Empty).ToDictionary(term => term.Weight, term => term.Name);
            //.Select(term => myTerms.Add(term.Weight, term.Name));

            // return _contentManager.GetMany<TermPart>(termIds, VersionOptions.Published, QueryHints.Empty).Select(term => new KeyValuePair<int,string>(term.Weight, term.Name)).ToList();
        }


        #endregion
    }
}