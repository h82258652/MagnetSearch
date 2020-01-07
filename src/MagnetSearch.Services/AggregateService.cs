using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MagnetSearch.Models;
using Microsoft.Extensions.Logging;

namespace MagnetSearch.Services
{
    public class AggregateService : IAggregateService
    {
        private readonly ILogger<AggregateService> _logger;
        private readonly IEnumerable<IMagnetService> _magnetServices;

        public AggregateService(IEnumerable<IMagnetService> magnetServices, ILogger<AggregateService> logger)
        {
            _magnetServices = magnetServices;
            _logger = logger;
        }

        public IAsyncEnumerable<MagnetItem> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return _magnetServices.Select(magnetService => SearchAsync(magnetService, query, cancellationToken)).Merge();
        }

        private async IAsyncEnumerable<MagnetItem> SearchAsync(IMagnetService magnetService, string query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            MagnetSearchResult firstPageResult;
            try
            {
                firstPageResult = await magnetService.SearchAsync(query, 1, cancellationToken);
            }
            catch (Exception ex)
            {
                if (!(ex is HttpRequestException))
                {
                    _logger.LogError(ex, ex.Message);
                }

                yield break;
            }

            if (firstPageResult.Items != null)
            {
                foreach (var magnetItem in firstPageResult.Items)
                {
                    yield return magnetItem;
                }
            }

            if (firstPageResult.LastPage <= 1)
            {
                yield break;
            }

            var totalPage = firstPageResult.LastPage;
            totalPage = Math.Min(totalPage, 10);
            var list = new List<IAsyncEnumerable<MagnetItem>>();
            for (int i = 2; i <= totalPage; i++)
            {
                list.Add(SearchAsync(magnetService, query, i, cancellationToken));
            }

            await foreach (var magnetItem in list.Merge().WithCancellation(cancellationToken))
            {
                yield return magnetItem;
            }
        }

        private async IAsyncEnumerable<MagnetItem> SearchAsync(IMagnetService magnetService, string query, int page, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            MagnetSearchResult result;
            try
            {
                result = await magnetService.SearchAsync(query, page, cancellationToken);
            }
            catch (Exception ex)
            {
                if (!(ex is HttpRequestException))
                {
                    _logger.LogError(ex, ex.Message);
                }

                yield break;
            }

            if (result.Items != null)
            {
                foreach (var magnetItem in result.Items)
                {
                    yield return magnetItem;
                }
            }
        }
    }
}
