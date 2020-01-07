using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MagnetSearch.Configuration;
using MagnetSearch.Models;
using MagnetSearch.Services;

namespace MagnetSearch.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAggregateService _aggregateService;
        private readonly IAppSettings _appSettings;
        private TimeSpan? _costTime;
        private RelayCommand _deleteAllSearchHistoriesCommand;
        private RelayCommand<string> _deleteSearchHistoryCommand;
        private bool _isLoading;
        private ObservableCollection<MagnetItem> _items;
        private CancellationTokenSource _lastLoadCts;
        private RelayCommand<string> _searchCommand;

        public MainViewModel(IAggregateService aggregateService, IAppSettings appSettings)
        {
            _aggregateService = aggregateService;
            _appSettings = appSettings;
        }

        public TimeSpan? CostTime
        {
            get => _costTime;
            private set => Set(ref _costTime, value);
        }

        public RelayCommand DeleteAllSearchHistoriesCommand
        {
            get
            {
                _deleteAllSearchHistoriesCommand ??= new RelayCommand(() =>
                {
                    SearchHistories = new HashSet<string>();
                });
                return _deleteAllSearchHistoriesCommand;
            }
        }

        public RelayCommand<string> DeleteSearchHistoryCommand
        {
            get
            {
                _deleteSearchHistoryCommand ??= new RelayCommand<string>(searchHistoryToDelete =>
                {
                    SearchHistories = new HashSet<string>(SearchHistories.Where(temp => temp != searchHistoryToDelete));
                });
                return _deleteSearchHistoryCommand;
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set => Set(ref _isLoading, value);
        }

        public ObservableCollection<MagnetItem> Items
        {
            get => _items;
            private set => Set(ref _items, value);
        }

        public RelayCommand<string> SearchCommand
        {
            get
            {
                _searchCommand ??= new RelayCommand<string>(async query =>
                {
                    var lastLoadCts = _lastLoadCts;
                    var loadCts = new CancellationTokenSource();
                    loadCts.CancelAfter(TimeSpan.FromSeconds(90));// 搜索时长不允许超过 90 秒
                    _lastLoadCts = loadCts;
                    lastLoadCts?.Cancel();

                    var stopwatch = new Stopwatch();
                    try
                    {
                        IsLoading = true;
                        CostTime = null;

                        UpdateSearchHistories(query);

                        var collection = new ObservableCollection<MagnetItem>();
                        Items = collection;
                        stopwatch.Start();
                        var items = _aggregateService.SearchAsync(query, loadCts.Token);
                        await foreach (var item in items.WithCancellation(loadCts.Token))
                        {
                            collection.Add(item);
                        }
                    }
                    catch (TaskCanceledException)
                    {
                    }
                    finally
                    {
                        stopwatch.Stop();
                        if (_lastLoadCts == loadCts)
                        {
                            CostTime = stopwatch.Elapsed;
                            IsLoading = false;
                        }
                    }
                });
                return _searchCommand;
            }
        }

        public ISet<string> SearchHistories
        {
            get => _appSettings.SearchHistories;
            private set
            {
                _appSettings.SearchHistories = value;
                RaisePropertyChanged();
            }
        }

        public void SortDateAsc()
        {
            if (Items != null && !IsLoading)
            {
                Items = new ObservableCollection<MagnetItem>(Items.OrderBy(temp => temp.Date));
            }
        }

        public void SortDateDesc()
        {
            if (Items != null && !IsLoading)
            {
                Items = new ObservableCollection<MagnetItem>(Items.OrderByDescending(temp => temp.Date));
            }
        }

        public void SortNameAsc()
        {
            if (Items != null && !IsLoading)
            {
                Items = new ObservableCollection<MagnetItem>(Items.OrderBy(temp => temp.Name));
            }
        }

        public void SortNameDesc()
        {
            if (Items != null && !IsLoading)
            {
                Items = new ObservableCollection<MagnetItem>(Items.OrderByDescending(temp => temp.Name));
            }
        }

        public void SortSizeAsc()
        {
            if (Items != null && !IsLoading)
            {
                Items = new ObservableCollection<MagnetItem>(Items.OrderBy(temp => temp.Size));
            }
        }

        public void SortSizeDesc()
        {
            if (Items != null && !IsLoading)
            {
                Items = new ObservableCollection<MagnetItem>(Items.OrderByDescending(temp => temp.Size));
            }
        }

        private void UpdateSearchHistories(string query)
        {
            var list = new List<string>();
            list.Add(query);
            list.AddRange(SearchHistories);
            SearchHistories = new HashSet<string>(list.Take(10));
        }
    }
}
