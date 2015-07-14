using System.Collections.Generic;
using System.Threading.Tasks;
using DevOffice.Common.Models;
using DevOffice.Common.Services;
using DevOffice.Common.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace DevOffice.Common.Drivers
{
    public class ArticlesWidgetDriver : ContentPartDriver<ArticlesWidgetPart>
    {
        private readonly IContentManager _contentManager;

        public ArticlesWidgetDriver(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        protected override DriverResult Display(ArticlesWidgetPart part, string displayType, dynamic shapeHelper)
        {
            var model = new ArticlesViewModel()
            {
                BlogPostsUrl = part.BlogPostsRssUrl,
                ArticlesUrl =  part.ArticlesRssUrl,
                MvpBlogsUrl =  part.MvpBlogsRssUrl
            };
            
            return ContentShape("Parts_ArticlesWidget",
                () => shapeHelper.Partial(
                    TemplateName: "Parts/ArticlesWidget",
                    Model: model
                    )
            );
        }

        protected override DriverResult Editor(ArticlesWidgetPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_ArticlesWidgetPart_Edit",
                () => shapeHelper.EditorTemplate(TemplateName: "Parts/ArticlesWidget.Edit", Model: part));
        }

        protected override DriverResult Editor(ArticlesWidgetPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, "", null, null);
            return Editor(part, shapeHelper);
        }
    }

}