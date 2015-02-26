using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.NetworkInformation;

using Windows.UI.Xaml;

using AppStudio.Services;
using AppStudio.Data;

namespace AppStudio.ViewModels
{
    public class MainViewModel : BindableBase
    {
       private HowStuffWorksViewModel _howStuffWorksModel;
       private VideosViewModel _videosModel;
       private BlogsViewModel _blogsModel;
        private PrivacyViewModel _privacyModel;

        private ViewModelBase _selectedItem = null;

        public MainViewModel()
        {
            _selectedItem = HowStuffWorksModel;
            _privacyModel = new PrivacyViewModel();

        }
 
        public HowStuffWorksViewModel HowStuffWorksModel
        {
            get { return _howStuffWorksModel ?? (_howStuffWorksModel = new HowStuffWorksViewModel()); }
        }
 
        public VideosViewModel VideosModel
        {
            get { return _videosModel ?? (_videosModel = new VideosViewModel()); }
        }
 
        public BlogsViewModel BlogsModel
        {
            get { return _blogsModel ?? (_blogsModel = new BlogsViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            HowStuffWorksModel.ViewType = viewType;
            VideosModel.ViewType = viewType;
            BlogsModel.ViewType = viewType;
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public Visibility AppBarVisibility
        {
            get
            {
                return SelectedItem == null ? AboutVisibility : SelectedItem.AppBarVisibility;
            }
        }

        public Visibility AboutVisibility
        {

         get { return Visibility.Visible; }
        }

        public void UpdateAppBar()
        {
            OnPropertyChanged("AppBarVisibility");
            OnPropertyChanged("AboutVisibility");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadData(bool isNetworkAvailable)
        {
            var loadTasks = new Task[]
            { 
                HowStuffWorksModel.LoadItems(isNetworkAvailable),
                VideosModel.LoadItems(isNetworkAvailable),
                BlogsModel.LoadItems(isNetworkAvailable),
            };
            await Task.WhenAll(loadTasks);
        }

        /// <summary>
        /// Refresh ViewModel items asynchronous
        /// </summary>
        public async Task RefreshData(bool isNetworkAvailable)
        {
            var refreshTasks = new Task[]
            { 
                HowStuffWorksModel.RefreshItems(isNetworkAvailable),
                VideosModel.RefreshItems(isNetworkAvailable),
                BlogsModel.RefreshItems(isNetworkAvailable),
            };
            await Task.WhenAll(refreshTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await RefreshData(NetworkInterface.GetIsNetworkAvailable());
                });
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutThisAppPage");
                });
            }
        }

        public ICommand PrivacyCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateTo(_privacyModel.Url);
                });
            }
        }
    }
}
