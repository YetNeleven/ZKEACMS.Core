/* http://www.zkea.net/ Copyright 2016 ZKEASOFT http://www.zkea.net/licenses */
using Easy;
using Easy.Constant;
using Easy.Extend;
using Easy.MetaData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using ZKEACMS.Article.Service;
using ZKEACMS.MetaData;
using ZKEACMS.Widget;

namespace ZKEACMS.Article.Models
{
    [ViewConfigure(typeof(ArticleTopWidgetMetaData)), Table("ArticleTopWidget")]
    public class ArticleTopWidget : BasicWidget
    {
        public int ArticleTypeID { get; set; }
        public int? Tops { get; set; }
        public string SubTitle { get; set; }
        public string MoreLink { get; set; }
        public string DetailPageUrl { get; set; }
    }
    class ArticleTopWidgetMetaData : WidgetMetaData<ArticleTopWidget>
    {
        protected override void ViewConfigure()
        {
            base.ViewConfigure();
            ViewConfig(m => m.SubTitle).AsTextBox().Order(NextOrder());
            var articleTypeService = new ServiceLocator().GetService<IArticleTypeService>();
            ViewConfig(m => m.ArticleTypeID).AsDropDownList().Order(NextOrder()).DataSource(articleTypeService.GetAll().ToDictionary(m => m.ID.ToString(), m => m.Title)).Required();
            ViewConfig(m => m.Tops).AsTextBox().Order(NextOrder()).RegularExpression(RegularExpression.PositiveIntegers);
            ViewConfig(m => m.MoreLink).AsTextBox().Order(NextOrder()).AddClass("select").AddProperty("data-url", Urls.SelectPage);
            ViewConfig(m => m.DetailPageUrl).AsTextBox().Order(NextOrder()).AddClass("select").AddProperty("data-url", Urls.SelectPage);

            ViewConfig(m => m.PartialView).AsDropDownList().Order(NextOrder()).DataSource(() =>
            {
                var path = (new ServiceLocator().GetService<IApplicationContext>() as CMSApplicationContext).MapPath("~/Modules/Article/Views");
                Dictionary<string, string> templates = new Dictionary<string, string>();
                Directory.GetFiles(path, "Widget.ArticleTops*.cshtml").Each(f =>
                {
                    string fileName = Path.GetFileNameWithoutExtension(f);
                    templates.Add(fileName, fileName.Replace("Widget.", ""));

                });
                return templates;
            });
        }
    }

}