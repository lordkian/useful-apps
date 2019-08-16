using System;
using System.Linq;
using XX_photo_downloader;

namespace XX_photo_downloader_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = (from i in args
                        where i.Contains("multi.xnxx.com/gallery") && Downloader.SiteRegex.Match(i).Success
                        select i).ToArray();
            var xnxxDownloadList = (from i in list where i.Contains("xnxx") select i).ToList();
            Console.WriteLine($"{xnxxDownloadList.Count} item was found.");
            Downloader.Loger = Console.WriteLine;
            Downloader.XnxxDownloader(args[0], xnxxDownloadList);
        }
    }
}
