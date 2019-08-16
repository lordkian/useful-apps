using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace XX_photo_downloader
{
    public class Downloader
    {
        public static readonly Regex SiteRegex = new Regex(@"(http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?");
        public static readonly Regex URLToFileNameRegex1 = new Regex(@"\s+$");
        public static readonly Regex URLToFileNameRegex2 = new Regex(@"[\\\/?<>*]");
        public static readonly string FolderSprator;
        public static Action<string> Loger;
        static Downloader()
        {
            var os = Environment.OSVersion;
            if (os.Platform == PlatformID.Win32NT)
                FolderSprator = "\\";
            else
                FolderSprator = "/";
        }
        public static string URLToFileName(string URL)
        {
            var res = URL.Replace("\"", "");
            res = res.Replace(".", " ");
            res = URLToFileNameRegex1.Replace(res, "");
            res = URLToFileNameRegex2.Replace(res, "");
            return res;
        }

        #region Xnxx
        public static readonly Regex XnxxPhotoRegex = new Regex(@"\/\d+\/$");
        public static void XnxxDownloader(string Path, List<string> Urls)
        {
            foreach (var item in Urls)
            {
                var url = XnxxURLCleaner(item);
                var name = URLToFileName(GroupedNodes(url, "//h1 [@class='title gallery']//span")[0][0]);
                XnxxDownloadFiles(XnxxGetLinks(url), Path + FolderSprator + name);
            }
        }
        private static void XnxxDownloadFiles(List<string> URLs, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            var i = 0;
            var wc = new WebClient();
            if (Loger != null)
                Loger($"{URLs.Count}: ");
            foreach (var item in URLs)
            {
                wc.DownloadFile(item, directoryPath + FolderSprator + i++ + ".jpg");
                if (Loger != null)
                    Loger($"{i}, ");
            }
            Console.WriteLine();
        }
        private static List<string> XnxxGetLinks(string URL)
        {
            return (from i in GroupedNodes(URL, new string[] { "//a/@href" })
                    where i[0].Contains("hwcdn.net") && i[0].Contains("/full/")
                    select i[0]).Distinct().ToList();
        }
        private static string XnxxURLCleaner(string input)
        {
            var match = XnxxPhotoRegex.Match(input);
            if (match.Success)
                return input.Replace(match.Value, "/");
            else
                return input;
        }
        #endregion

        #region HTML_analyzer
        /// <summary>
        /// You can get all Ordered data you want , from web page.
        /// Each row is an Item.
        /// Each column is property of that item. 
        /// </summary>
        /// <param name="URL">URL of web page</param>
        /// <param name="XPathes">XPath patterns of Columns</param>
        /// <returns>Grouped List</returns>
        public static List<List<string>> GroupedNodes(string URL, params string[] XPathes)
        {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;

            var HTML = client.DownloadString(URL);

            var doc = new HtmlDocument();
            doc.LoadHtml(HTML);
            var firstDraft = new List<List<string>>();
            int rowsCount = XPathes.Length;
            HtmlNodeCollection nodes = null;
            var regex = new Regex(@"/@\w+$");

            for (int i = 0; i < rowsCount; i++)
            {
                var list = new List<string>();
                var m = regex.Match(XPathes[i]);
                nodes = doc.DocumentNode.SelectNodes(XPathes[i]);
                if (nodes == null)
                    list.Add("");
                else
                {
                    if (!m.Success)
                        foreach (var node in nodes)
                            list.Add(node.InnerText.Trim());

                    else
                    {
                        string attribute = m.Value.Substring(2);
                        foreach (var node in nodes)
                            list.Add(node.GetAttributeValue(attribute, "Attribute not found !"));
                    }
                }
                firstDraft.Add(list);
            }

            //Transpose
            var grouped = new List<List<string>>();
            var len = firstDraft.First().Count;
            for (int i = 0; i < len; i++)
            {
                var list = new List<string>();
                foreach (var item in firstDraft)
                    if (item.Count > 0)
                    {
                        list.Add(item.First());
                        item.RemoveAt(0);
                    }
                    else
                        list.Add("");
                grouped.Add(list);
            }
            return grouped;
        }
        #endregion
    }
}
