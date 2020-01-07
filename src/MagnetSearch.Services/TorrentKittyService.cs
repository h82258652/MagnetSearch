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
    public class TorrentKittyService : IMagnetService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TorrentKittyService(IHttpClientFactory httpClientFactory)
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

            var url = $"https://www.torrentkitty.app/search/{HttpUtility.UrlEncode(query)}/{page}";

            var httpClient = _httpClientFactory.CreateClient();
            var html = await httpClient.GetStringAsync(url);

            var context = BrowsingContext.New();
            var document = await context.OpenAsync(request => request.Content(html), cancel: cancellationToken);

            var items = new List<MagnetItem>();
            foreach (var row in document.QuerySelectorAll<IHtmlTableRowElement>("#archiveResult tr:not(:first-child)"))
            {
                var name = row.QuerySelector<IHtmlTableDataCellElement>("td.name");
                var size = row.QuerySelector<IHtmlTableDataCellElement>("td.size");
                var date = row.QuerySelector<IHtmlTableDataCellElement>("td.date");
                var magnet = row.QuerySelector<IHtmlAnchorElement>("td.action > a[rel='magnet']");

                var item = new MagnetItem
                {
                    Name = name.Text(),
                    Magnet = magnet.Href,
                    Size = SizeHelper.GetSize(size.Text()),
                    Date = DateTime.Parse(date.Text())
                };
                items.Add(item);
            }

            int lastPage = 0;
            var pageAnchors = document.QuerySelectorAll<IHtmlAnchorElement>(".pagination > a").ToList();
            if (pageAnchors.Any())
            {
                lastPage = pageAnchors.Select(temp =>
                {
                    var text = temp.Text();
                    int.TryParse(text, out var i);
                    return i;
                }).Max();
            }

            return new MagnetSearchResult()
            {
                Items = items.ToArray(),
                LastPage = lastPage
            };
        }
    }
}
