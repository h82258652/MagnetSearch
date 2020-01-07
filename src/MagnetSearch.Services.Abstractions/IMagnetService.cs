using System.Threading;
using System.Threading.Tasks;
using MagnetSearch.Models;

namespace MagnetSearch.Services
{
    public interface IMagnetService
    {
        Task<MagnetSearchResult> SearchAsync(string query, int page, CancellationToken cancellationToken = default);
    }
}
