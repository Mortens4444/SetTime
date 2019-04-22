using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading;

namespace SetTime
{
    public class SiteDownloader
    {
        public string GetSiteContent(string site)
        {
            var request = (HttpWebRequest)WebRequest.Create(site);
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Timeout = Timeout.Infinite;

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                {
                    return String.Empty;
                }

                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                {
                    return String.Empty;
                }

                responseStream.ReadTimeout = Timeout.Infinite;
                using (var responseStreamReader = new StreamReader(responseStream))
                {
                    responseStreamReader.BaseStream.ReadTimeout = Timeout.Infinite;
                    return responseStreamReader.ReadToEnd();
                }
            }
        }
    }
}
