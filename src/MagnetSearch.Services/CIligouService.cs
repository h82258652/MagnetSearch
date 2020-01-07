using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using MagnetSearch.Models;

namespace MagnetSearch.Services
{
    //public class CIligouService : IMagnetService
    //{
    //    private readonly IHttpClientFactory _httpClientFactory;

    //    public CIligouService(IHttpClientFactory httpClientFactory)
    //    {
    //        _httpClientFactory = httpClientFactory;
    //    }

    //    public async Task<MagnetSearchResult> SearchAsync(string query, int page, CancellationToken cancellationToken = default)
    //    {
    //        // 磁力链接无法获取
    //        throw new NotImplementedException();
            
    //        if (query == null)
    //        {
    //            throw new ArgumentNullException(nameof(query));
    //        }
    //        if (page <= 0)
    //        {
    //            throw new ArgumentOutOfRangeException(nameof(page));
    //        }

    //        var url = $"http://ciligou.top/search?word={HttpUtility.UrlEncode(query)}&sort=rele&p={page}";

    //        var httpClient = _httpClientFactory.CreateClient();
    //        var html = await httpClient.GetStringAsync(url);

    //        var context = BrowsingContext.New();
    //        var document = await context.OpenAsync(request => request.Content(html), cancel: cancellationToken);

    //        var items = new List<MagnetItem>();
    //        foreach (var li in document.QuerySelectorAll<IHtmlListItemElement>("#Search_list_wrapper > li"))
    //        {
    //            var name = li.QuerySelector("a.SearchListTitle_result_title").Text();

    //            var item = new MagnetItem
    //            {
    //                Name = name,
    //                Magnet = "",
    //                Date = DateTime.Parse("")
    //            };
    //            items.Add(item);
    //        }

    //        return new MagnetSearchResult()
    //        {
    //            Items = items.ToArray(),
    //            LastPage = 0
    //        };

    //        throw new NotImplementedException();
    //    }
    //}
}
