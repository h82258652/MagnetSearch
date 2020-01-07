using System.Collections.Generic;
using System.Text.Json;
using Windows.Storage;

namespace MagnetSearch.Configuration
{
    public class AppSettings : IAppSettings
    {
        private const string SearchHistoriesKey = "SearchHistories";

        public ISet<string> SearchHistories
        {
            get
            {
                try
                {
                    var json = (string)ApplicationData.Current.LocalSettings.Values[SearchHistoriesKey];
                    return JsonSerializer.Deserialize<HashSet<string>>(json);
                }
                catch
                {
                    return new HashSet<string>();
                }
            }
            set
            {
                var json = JsonSerializer.Serialize(value);
                ApplicationData.Current.LocalSettings.Values[SearchHistoriesKey] = json;
            }
        }
    }
}
