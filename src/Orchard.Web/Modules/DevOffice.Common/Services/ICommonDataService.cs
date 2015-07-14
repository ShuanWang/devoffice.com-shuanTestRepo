using DevOffice.Common.Models;
using DevOffice.Common.ViewModels;
using Orchard;
using System.Collections.Generic;
using Orchard.ContentManagement;

namespace DevOffice.Common.Services
{
    public interface ICommonDataService : IDependency {
        void CreateFeedbackRecord(FeedbackInformationRecord feedback);
        List<QuickLink> GetQuickLinks();
        List<Event> GetEvents();
        List<CommunityItem> GetCommunityItems();
        List<Training> GetTrainingItems();
        List<Resource> GetResources();
        List<Video> GetVideos();
        List<Training> GetPodcasts();
        List<CodeSample> GetCodeSamples();
        //List<CodeSample> GetCodeSamples(string[] filtersArray);
        List<Solution> GetSolutions();
        List<Solution> GetSolutions(string[] filtersArray);
        List<Solution> GetTopFourSolutionsFromCache(); 
        List<GettingStartedTab> GetGettingStartedTabs();
        List<PatternsAndPractice> GetPatternsAndPractices();
        //List<PatternsAndPractice> GetPatternsAndPractices(string[] typeFiltersArray);
        void UpdateRelatedLinks(ContentItem item, RelatedLinksViewModel model);
        RelatedLinksViewModel BuildEditorViewModel(RelatedLinksPart part, string itemsJson = "");
        SectionsWithTilesPageViewModel GetSectionsWithTilesPage(ContentItem pageItem);
        List<BlogPost> GetArticles(int pageNumber = 0, int count = 0);
        int GetAllArticlesCount();
        List<int> GetTopViewedItems(string type);
        BlogPost GetArticleById(int id);
    }
}