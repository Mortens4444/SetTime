using System;

namespace SetTime
{
    class Program
    {

        static void Main(string[] args)
        {
            var siteDownloader = new SiteDownloader();
            var siteContent = siteDownloader.GetSiteContent("https://www.pontosido.com/");

            var timeParser = new TimeParser();
            var systemTime = timeParser.Get(siteContent);

            var timeSetter = new TimeSetter();
            timeSetter.Set(systemTime);
        }
    }
}
