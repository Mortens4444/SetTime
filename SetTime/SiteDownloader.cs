using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading;
using System.Web;

namespace SetTime
{
    public class SiteDownloader : ISiteDownloader
    {
        public string GetSiteContent(string url)
        {
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.DefaultConnectionLimit = ServicePointManager.DefaultPersistentConnectionLimit;

            //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            //var client = new WebClient();

            //var query = HttpUtility.ParseQueryString(string.Empty);
            //query["q"] = "exact time";
            //query["addon"] = "opensearch";

            //var url = new UriBuilder("https://www.ecosia.org/search");
            //url.Query = query.ToString();
            //var content = client.DownloadString(url.ToString());

            var request = (HttpWebRequest)WebRequest.Create(url.ToString());
            request.Headers.Add("UserAgent", "SetTimeClient");
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.ProtocolVersion = HttpVersion.Version11;
            request.Timeout = Timeout.Infinite;
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            //request.Expect = ;
            request.KeepAlive = true;
            request.ContentType = "text/html";
            request.UseDefaultCredentials = true;
            request.Credentials = CredentialCache.DefaultCredentials;

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
