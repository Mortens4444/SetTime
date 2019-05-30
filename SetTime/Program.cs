using System;
using System.IO;
using System.Windows.Forms;

namespace SetTime
{
	class Program
    {
		private const string SettingsFile = "settings.csv";

		static void Main(string[] args)
        {
            var siteDownloader = new SiteDownloader();

			if ((args.Length != 0) && (args.Length == 5))
			{
				Console.WriteLine($"Usage: {Application.ProductName}   timeserver_uri   time_start_search_pattern   time_end_search_pattern   date_start_search_pattern   date_end_search_pattern");
			}
			if (File.Exists(Path.Combine(Application.StartupPath, SettingsFile)))
			{
				try
				{
					var content = File.ReadAllText(SettingsFile);
					args = content.Split(';');
				}
				catch (Exception)
				{
					Console.WriteLine($"{SettingsFile} content: timeserver_uri;time_start_search_pattern;time_end_search_pattern;date_start_search_pattern;date_end_search_pattern");
					return;
				}
			}

			var url = args.Length == 0 ? "https://www.pontosido.com/" : args[0];
			var timeStartSearchPattern = args.Length == 0 ? "<div class=\"time\" id=\"clock\">" : args[1];
			var timeEndSearchPattern = args.Length == 0 ? "</div>" : args[2];
			var dateStartSearchPattern = args.Length == 0 ? "<div class=\"date\">" : args[3];
			var dateEndSearchPattern = args.Length == 0 ? "</div>" : args[4];

			var siteContent = siteDownloader.GetSiteContent(url);

			var timeParser = new TimeParser();

			var systemTime = timeParser.Get(siteContent, timeStartSearchPattern, timeEndSearchPattern, dateStartSearchPattern, dateEndSearchPattern);

            var timeSetter = new TimeSetter();
            timeSetter.Set(systemTime);
        }
    }
}
