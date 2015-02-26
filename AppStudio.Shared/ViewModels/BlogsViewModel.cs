using System;
using System.Windows;
using System.Windows.Input;

using Windows.UI.Xaml;

using AppStudio.Services;
using AppStudio.Data;

namespace AppStudio.ViewModels
{
    public class BlogsViewModel : ViewModelBase<RssSchema>
    {
        override protected string CacheKey
        {
            get { return "BlogsDataSource"; }
        }

        override protected IDataSource<RssSchema> CreateDataSource()
        {
            return new BlogsDataSource(); // RssDataSource
        }

        override public Visibility GoToSourceVisibility
        {
            get { return ViewType == ViewTypes.Detail ? Visibility.Visible : Visibility.Collapsed; }
        }

        override protected void GoToSource()
        {
            base.GoToSource("{FeedUrl}");
        }

        override public Visibility RefreshVisibility
        {
            get { return ViewType == ViewTypes.List ? Visibility.Visible : Visibility.Collapsed; }
        }

        override public void NavigateToSectionList()
        {
            NavigationServices.NavigateToPage("BlogsList");
        }

        override protected void NavigateToSelectedItem()
        {
            NavigationServices.NavigateToPage("BlogsDetail");
        }
    }
}
