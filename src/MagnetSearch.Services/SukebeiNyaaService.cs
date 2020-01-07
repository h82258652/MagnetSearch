using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MagnetSearch.Models;
using MagnetSearch.Models.SukebeiNyaa;

namespace MagnetSearch.Services
{
    public class SukebeiNyaaService : IMagnetService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SukebeiNyaaService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MagnetSearchResult> SearchAsync(string query, int page, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (page <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }

            var uriBuilder = new UriBuilder(new Uri("https://sukebei.nyaa.net/api/search/"));
            var queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            queryString["q"] = query;
            queryString["page"] = page.ToString();
            const int limit = 50;
            queryString["limit"] = limit.ToString();

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(uriBuilder.Uri, cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<SukebeiNyaaError>(json);
                throw new SukebeiNyaaException(error);
            }

            var result = JsonSerializer.Deserialize<SukebeiNyaaSearchResult>(json);
            return new MagnetSearchResult
            {
                Items = result
                    .Torrents
                    .Select(temp => new MagnetItem
                    {
                        Name = temp.Name,
                        Magnet = temp.Magnet,
                        Size = temp.FileSize,
                        Date = temp.Date
                    })
                    .ToArray(),
                LastPage = (result.TotalRecordCount + limit - 1) / limit
            };
        }
    }
}
