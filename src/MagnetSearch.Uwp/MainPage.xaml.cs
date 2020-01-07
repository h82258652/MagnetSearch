using System;
using System.Collections.ObjectModel;
using System.Linq;
using MagnetSearch.Models;
using MagnetSearch.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace MagnetSearch.Uwp
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            serviceCollection.AddTransient<IMagnetService, BtsowService>();
            serviceCollection.AddTransient<IMagnetService, SukebeiNyaaService>();
            serviceCollection.AddTransient<IMagnetService, TorrentKittyService>();
            serviceCollection.AddTransient<IAggregateService, AggregateService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IAggregateService>();
            var collection = new ObservableCollection<MagnetItem>();
            DataGrid.ItemsSource = collection;
            _c = collection;
            await service.SearchAsync("cos").ForEachAsync(async item =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    collection.Add(item);
                });
            });
        }

        private ObservableCollection<MagnetItem> _c;

        private void DataGrid_OnSorting(object sender, DataGridColumnEventArgs e)
        {
            if (_c == null)
            {
                return;
            }

            if (e.Column.Tag == null)
            {
                return;
            }

            if (string.Equals(e.Column.Tag.ToString(), "Size", StringComparison.OrdinalIgnoreCase))
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    _c = new ObservableCollection<MagnetItem>(_c.OrderBy(temp => temp.Size));
                    DataGrid.ItemsSource = _c;

                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    _c = new ObservableCollection<MagnetItem>(_c.OrderByDescending(temp => temp.Size));
                    DataGrid.ItemsSource = _c;

                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
            else if (string.Equals(e.Column.Tag.ToString(), "Date", StringComparison.OrdinalIgnoreCase))
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    _c = new ObservableCollection<MagnetItem>(_c.OrderBy(temp => temp.Date));
                    DataGrid.ItemsSource = _c;

                    e.Column.SortDirection = DataGridSortDirection.Ascending;
                }
                else
                {
                    _c = new ObservableCollection<MagnetItem>(_c.OrderByDescending(temp => temp.Date));
                    DataGrid.ItemsSource = _c;

                    e.Column.SortDirection = DataGridSortDirection.Descending;
                }
            }
        }
    }
}
