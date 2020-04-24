using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using MetroLesMonitor;
using LeSMonitor;
using System.IO.Compression;
using System.IO;
using System.Web.UI;

namespace MetroLesMonitor
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }


        protected void Session_Start(object sender, EventArgs e)
        {

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }


        protected void Session_End(object sender, EventArgs e)
        {
          
        }

        void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //HttpApplication app = sender as HttpApplication;
            //string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            //Stream prevUncompressedStream = app.Response.Filter;

            //if (app.Context.CurrentHandler != null)
            //{
            //    if (!(app.Context.CurrentHandler is Page ||
            //        app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler") ||
            //        app.Request["HTTP_X_MICROSOFTAJAX"] != null)
            //        return;
            //}
            //if (acceptEncoding == null || acceptEncoding.Length == 0)
            //    return;

            //acceptEncoding = acceptEncoding.ToLower();

            //if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
            //{
            //    // defalte
            //    app.Response.Filter = new DeflateStream(prevUncompressedStream,
            //        CompressionMode.Compress);
            //    app.Response.AppendHeader("Content-Encoding", "deflate");
            //}
            //else if (acceptEncoding.Contains("gzip"))
            //{
            //    // gzip
            //    app.Response.Filter = new GZipStream(prevUncompressedStream,
            //        CompressionMode.Compress);
            //    app.Response.AppendHeader("Content-Encoding", "gzip");
            //}

        }
    }
}
