using System;
using MagnetSearch.Models;
using MagnetSearch.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MagnetSearch.Uwp.Views
{
    public sealed partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        public MainViewModel ViewModel => (MainViewModel)DataContext;

        private async void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await new MessageDialog("Yoyoyo~ 是一个磁力聚合搜索工具，内容基于第三方提供。", "关于 Yoyoyo~").ShowAsync();
            }
            catch
            {
                // ignored
            }
        }

        private void CopyMagnetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement)?.DataContext as MagnetItem;
            if (item == null)
            {
                return;
            }

            var dataPackage = new DataPackage();
            dataPackage.SetText(item.Magnet);
            Clipboard.SetContent(dataPackage);
        }

        private void CopyNameAndMagnetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement)?.DataContext as MagnetItem;
            if (item == null)
            {
                return;
            }

            var dataPackage = new DataPackage();
            dataPackage.SetText(string.Join(Environment.NewLine, item.Name, item.Magnet));
            Clipboard.SetContent(dataPackage);
        }

        private void CopyNameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement)?.DataContext as MagnetItem;
            if (item == null)
            {
                return;
            }

            var dataPackage = new DataPackage();
            dataPackage.SetText(item.Name);
            Clipboard.SetContent(dataPackage);
        }

        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            if (e.Column.Tag == null || ViewModel.IsLoading)
            {
                return;
            }

            var dataGrid = sender as DataGrid;
            if (dataGrid == null)
            {
                return;
            }

            if (string.Equals(e.Column.Tag.ToString(), "Name", StringComparison.OrdinalIgnoreCase))
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ViewModel.SortNameAsc();

                    foreach (var column in dataGrid.Columns)
                    {
                        if (column == e.Column)
                        {
                            column.SortDirection = DataGridSortDirection.Ascending;
                        }
                        else
                        {
                            column.SortDirection = null;
                        }
                    }
                }
                else
                {
                    ViewModel.SortNameDesc();

                    foreach (var column in dataGrid.Columns)
                    {
                        if (column == e.Column)
                        {
                            column.SortDirection = DataGridSortDirection.Descending;
                        }
                        else
                        {
                            column.SortDirection = null;
                        }
                    }
                }
            }
            else if (string.Equals(e.Column.Tag.ToString(), "Size", StringComparison.OrdinalIgnoreCase))
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ViewModel.SortSizeAsc();

                    foreach (var column in dataGrid.Columns)
                    {
                        if (column == e.Column)
                        {
                            column.SortDirection = DataGridSortDirection.Ascending;
                        }
                        else
                        {
                            column.SortDirection = null;
                        }
                    }
                }
                else
                {
                    ViewModel.SortSizeDesc();

                    foreach (var column in dataGrid.Columns)
                    {
                        if (column == e.Column)
                        {
                            column.SortDirection = DataGridSortDirection.Descending;
                        }
                        else
                        {
                            column.SortDirection = null;
                        }
                    }
                }
            }
            else if (string.Equals(e.Column.Tag.ToString(), "Date", StringComparison.OrdinalIgnoreCase))
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                {
                    ViewModel.SortDateAsc();

                    foreach (var column in dataGrid.Columns)
                    {
                        if (column == e.Column)
                        {
                            column.SortDirection = DataGridSortDirection.Ascending;
                        }
                        else
                        {
                            column.SortDirection = null;
                        }
                    }
                }
                else
                {
                    ViewModel.SortDateDesc();

                    foreach (var column in dataGrid.Columns)
                    {
                        if (column == e.Column)
                        {
                            column.SortDirection = DataGridSortDirection.Descending;
                        }
                        else
                        {
                            column.SortDirection = null;
                        }
                    }
                }
            }
        }

        private void DeleteAllSearchHistoriesButton_Click(object sender, RoutedEventArgs e)
        {
            SearchHistoryFlyout.Hide();

            ViewModel.DeleteAllSearchHistoriesCommand.Execute(null);
        }

        private void DeleteSearchHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            SearchHistoryFlyout.Hide();

            var searchHistory = (sender as FrameworkElement)?.DataContext as string;
            if (searchHistory == null)
            {
                return;
            }

            ViewModel.DeleteSearchHistoryCommand.Execute(searchHistory);
        }

        private async void OpenMagnetMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement)?.DataContext as MagnetItem;
            if (item == null)
            {
                return;
            }

            await Launcher.LaunchUriAsync(new Uri(item.Magnet));
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ViewModel.SearchCommand.Execute(args.QueryText);
        }

        private void SearchHistoryItem_Click(object sender, RoutedEventArgs e)
        {
            SearchHistoryFlyout.Hide();

            var searchHistory = (sender as FrameworkElement)?.DataContext as string;
            if (searchHistory == null)
            {
                return;
            }

            SearchBox.Text = searchHistory;
            ViewModel.SearchCommand.Execute(searchHistory);
        }
    }
}
