using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NQR.CINC.Web.App_Code
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.WebPages;

    public static class WebGridExtensions
    {
        public static HelperResult PagerList(
            this WebGrid webGrid,
            WebGridPagerModes mode = WebGridPagerModes.NextPrevious | WebGridPagerModes.Numeric,
            string firstText = null,
            string previousText = null,
            string nextText = null,
            string lastText = null,
            int numericLinksCount = 5, bool isHttpPost = false)
        {
            return PagerList(webGrid, mode, firstText, previousText, nextText, lastText, numericLinksCount, isHttpPost, explicitlyCalled: true);
        }

        private static HelperResult PagerList(
            WebGrid webGrid,
            WebGridPagerModes mode,
            string firstText,
            string previousText,
            string nextText,
            string lastText,
            int numericLinksCount,
            bool isHttpPost = false, bool explicitlyCalled = true)
        {

            int currentPage = webGrid.PageIndex;
            int totalPages = webGrid.PageCount;
            int lastPage = totalPages - 1;

            var ul = new TagBuilder("ul");
            var li = new List<TagBuilder>();


            if (ModeEnabled(mode, WebGridPagerModes.FirstLast) && (totalPages > 0))
            {
                if (String.IsNullOrEmpty(firstText))
                {
                    firstText = "First";
                }

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(0), firstText, 0, isHttpPost)
                };

                if (currentPage == 0)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);
            }

            if (ModeEnabled(mode, WebGridPagerModes.NextPrevious) && (totalPages > 0))
            {
                if (String.IsNullOrEmpty(previousText))
                {
                    previousText = "<";
                }

                int page = currentPage == 0 ? 0 : currentPage - 1;

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(page), previousText, page, isHttpPost)
                };

                if (currentPage == 0)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);
            }


            if (ModeEnabled(mode, WebGridPagerModes.Numeric) && (totalPages > 1))
            {
                int last = currentPage + (numericLinksCount / 2);
                int first = last - numericLinksCount + 1;
                if (last > lastPage)
                {
                    first -= last - lastPage;
                    last = lastPage;
                }
                if (first < 0)
                {
                    last = Math.Min(last + (0 - first), lastPage);
                    first = 0;
                }
                for (int i = first; i <= last; i++)
                {

                    var pageText = (i + 1).ToString(CultureInfo.InvariantCulture);
                    var part = new TagBuilder("li")
                    {
                        InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(i), pageText, i, isHttpPost)
                    };

                    if (i == currentPage)
                    {
                        part.MergeAttribute("class", "active");
                    }

                    li.Add(part);
                }
            }

            if (ModeEnabled(mode, WebGridPagerModes.NextPrevious) && (totalPages > 0))
            {
                if (String.IsNullOrEmpty(nextText))
                {
                    nextText = ">";
                }

                int page = currentPage == lastPage ? lastPage : currentPage + 1;

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(page), nextText, page, isHttpPost)
                };

                if (currentPage == lastPage)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);
            }

            if (ModeEnabled(mode, WebGridPagerModes.FirstLast) && (totalPages > 0))
            {
                if (String.IsNullOrEmpty(lastText))
                {
                    lastText = "Last";
                }

                var part = new TagBuilder("li")
                {
                    InnerHtml = GridLink(webGrid, webGrid.GetPageUrl(lastPage), lastText, lastPage, isHttpPost)
                };

                if (currentPage == lastPage)
                {
                    part.MergeAttribute("class", "disabled");
                }

                li.Add(part);
            }

            ul.InnerHtml = string.Join("", li);

            var html = "";
            if (explicitlyCalled && webGrid.IsAjaxEnabled)
            {
                var span = new TagBuilder("span");
                span.MergeAttribute("data-swhgajax", "true");
                span.MergeAttribute("data-swhgcontainer", webGrid.AjaxUpdateContainerId);
                span.MergeAttribute("data-swhgcallback", webGrid.AjaxUpdateCallback);

                span.InnerHtml = ul.ToString();
                html = span.ToString();
            }
            else
            {
                html = ul.ToString();
            }

            return new HelperResult(writer =>
            {
                writer.Write(html);
            });
        }

        private static String GridLink(WebGrid webGrid, string url, string text, int pageNumber, bool isHttpPost = false)
        {
            if (isHttpPost) // construct button pagination for http post
                return GetGridHttpPostLink(webGrid, url, text, pageNumber);
            else // construct anchor pagination for http get
                return GetGridHttpGetLink(webGrid, url, text, pageNumber);
        }

       
        /// <summary>
        /// construct anchor pagination for http get
        /// </summary>
        /// <param name="webGrid"></param>
        /// <param name="url"></param>
        /// <param name="text"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        private static String GetGridHttpGetLink(WebGrid webGrid, string url, string text, int pageNumber)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.SetInnerText(text);
            builder.MergeAttribute("href", url);
            if (webGrid.IsAjaxEnabled)
            {
                builder.MergeAttribute("data-swhglnk", "true");
            }

            return builder.ToString(TagRenderMode.Normal);
        }
       
        /// <summary>
        /// construct button pagination for http post
        /// </summary>
        /// <param name="webGrid"></param>
        /// <param name="url"></param>
        /// <param name="text"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        private static String GetGridHttpPostLink(WebGrid webGrid, string url, string text, int pageNumber)
        {
            TagBuilder builder = new TagBuilder("button");
            builder.SetInnerText(text);
            builder.MergeAttribute("type", "submit");
            builder.Attributes.Add("value", (pageNumber + 1).ToString());
            builder.Attributes.Add("name", "pageNumber");
            builder.Attributes.Add("class", "paginationButton");

            if (webGrid.IsAjaxEnabled)
            {
                builder.MergeAttribute("data-swhglnk", "true");
            }

            return builder.ToString(TagRenderMode.Normal);
        }

        private static bool ModeEnabled(WebGridPagerModes mode, WebGridPagerModes modeCheck)
        {
            return (mode & modeCheck) == modeCheck;
        }
    }

}