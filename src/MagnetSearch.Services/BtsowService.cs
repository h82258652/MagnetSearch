using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using MagnetSearch.Models;
using MagnetSearch.Utils;

namespace MagnetSearch.Services
{
    public class BtsowService : IMagnetService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BtsowService(IHttpClientFactory httpClientFactory)
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

            var url = $"https://btos.pw/search/{HttpUtility.UrlEncode(query)}/page/{page}";

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("accept-language", "zh-CN,zh;q=0.9,ja;q=0.8");

            var html = await httpClient.GetStringAsync(url);

            var context = BrowsingContext.New();
            var document = await context.OpenAsync(request => request.Content(html), cancel: cancellationToken);

            var items = new List<MagnetItem>();
            foreach (var row in document.QuerySelectorAll<IHtmlDivElement>("div.data-list > div.row:not(.hidden-xs)"))
            {
                var anchor = row.QuerySelector<IHtmlAnchorElement>("a");
                var hash = anchor.Href.Split('/').Last();
                var size = row.QuerySelector("div.size").Text();
                var date = row.QuerySelector("div.date").Text();

                var item = new MagnetItem
                {
                    Name = anchor.Title,
                    Magnet = "magnet:?xt=urn:btih:" + hash,
                    Size = SizeHelper.GetSize(size),
                    Date = DateTime.Parse(date)
                };
                items.Add(item);
            }

            var lastPageAnchor = document.QuerySelectorAll<IHtmlAnchorElement>("ul.pagination a[name='numbar']").LastOrDefault();
            var lastPage = 0;
            if (lastPageAnchor != null)
            {
                int.TryParse(lastPageAnchor.Href.Split('/').LastOrDefault(), out lastPage);
            }

            return new MagnetSearchResult
            {
                Items = items.ToArray(),
                LastPage = lastPage
            };
        }
    }
}
