using System.Collections.Generic;
using System.Threading;
using MagnetSearch.Models;

namespace MagnetSearch.Services
{
    public interface IAggregateService
    {
        IAsyncEnumerable<MagnetItem> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
