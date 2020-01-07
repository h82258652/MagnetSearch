using System.Collections.Generic;

namespace MagnetSearch.Configuration
{
    public interface IAppSettings
    {
        ISet<string> SearchHistories { get; set; }
    }
}
