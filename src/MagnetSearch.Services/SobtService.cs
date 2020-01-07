using System;
using System.Collections.Generic;
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
    public class SobtService : IMagnetService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SobtService(IHttpClientFactory httpClientFactory)
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

            var url = $"http://www.sobt5.pw/q/{HttpUtility.UrlEncode(query)}.html?sort=rel&page={page}";

            var httpClient = _httpClientFactory.CreateClient();

            var html = await httpClient.GetStringAsync(url);

            var context = BrowsingContext.New();
            var document = await context.OpenAsync(request => request.Content(html), cancel: cancellationToken);

            var items = new List<MagnetItem>();
            foreach (var element in document.QuerySelectorAll<IHtmlDivElement>("div.search-item"))
            {
                var anchor = element.QuerySelector<IHtmlAnchorElement>("div.item-title a");

                var strings = anchor.Href.Split('/', '.');
                var hash = strings[strings.Length - 2];

                var size = element.QuerySelector(".item-bar > span:nth-last-child(2) > b").Text();

                var date = element.QuerySelector("div.item-bar b").Text();

                var item = new MagnetItem
                {
                    Name = anchor.Text,
                    Magnet = "magnet:?xt=urn:btih:" + hash,
                    Size = SizeHelper.GetSize(size),
                    Date = DateTime.Parse(date)
                };
                items.Add(item);
            }

            var lastPage = 0;
            var lastPageAnchor = document.QuerySelector<IHtmlAnchorElement>(".pagination > .last_p > a");
            if (lastPageAnchor != null)
            {
                var queryString = HttpUtility.ParseQueryString(new Uri(lastPageAnchor.Href, UriKind.RelativeOrAbsolute).Query);
                int.TryParse(queryString["page"], out lastPage);
            }

            return new MagnetSearchResult
            {
                Items = items.ToArray(),
                LastPage = lastPage
            };
        }
    }
}
