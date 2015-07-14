using DevOffice.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevOffice.Common.Services
{
    public class RSSDataService : IRssDataService
    {
        public List<Article> GetRssFeed(string rssUrl, int numberOfItems = 3)
        {
            //RSS feed failing intermitently

            var retries = 1;

            while (retries-- > 0)
            {
                try
                {
                    return getRssFeed(rssUrl, numberOfItems);
                }
                catch (Exception ex) { }
            }

            return null;
        }

        private List<Article> getRssFeed(string rssUrl, int numberOfItems)
        {
            if (String.IsNullOrEmpty(rssUrl))
            {
                return null;
            }

            var articles = new List<Article>();
            var reader = XmlReader.Create(rssUrl);
            var feed = SyndicationFeed.Load(reader);

            reader.Close();

            if (feed == null)
            {
                return null;
            }

            for (var i = 0; i < numberOfItems; i++)
            {
                var item = feed.Items.ElementAt(i);
                var url = new Uri(item.Id);
                var baseUrl = (item.BaseUri != null && item.BaseUri.Host != "") ? item.BaseUri.Host : url.Host;

                articles.Add(new Article()
                {
                    Title = item.Title.Text,
                    SubText = item.Summary.Text,
                    Url = item.Id,
                    BaseUrl = "http://" + baseUrl
                });
            }

            return articles;
        }
    }
}