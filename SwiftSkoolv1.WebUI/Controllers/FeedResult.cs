using System;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace SwiftSkool.Controllers
{
    public class FeedResult : ActionResult
    {
        private Rss20FeedFormatter formattedFeed;

        public FeedResult(Rss20FeedFormatter formattedFeed)
        {
            this.formattedFeed = formattedFeed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";
            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                formattedFeed.WriteTo(writer);
            }
        }
    }
}