using System;
using System.Collections.Generic;
using System.Linq;
using TimetableApp.Services;
using TimetableApp.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace TimetableApp.Common
{
    public class ViewBaseModel : LayoutAwarePage
    {
        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            var viewModel = DataContext as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.LoadState(navigationParameter, pageState);
            }
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
            var viewModel = DataContext as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.SaveState(pageState);
            }
        }

        public static readonly DependencyProperty DataContextChangedWatcherProperty = DependencyProperty.Register(
            "DataContextChangedWatcher",
            typeof(object),
            typeof(ViewBaseModel),
            new PropertyMetadata(null, OnDataContextChanged));

        public object DataContextChangedWatcher
        {
            get
            {
                return GetValue(DataContextChangedWatcherProperty);
            }
            set
            {
                SetValue(DataContextChangedWatcherProperty, value);
            }
        }

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((ViewBaseModel)d).DataContext as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.NavigationService = NavigationService.Current;
            }
        }

        public ViewBaseModel()
        {
            BindingOperations.SetBinding(this, DataContextChangedWatcherProperty, new Binding());
        }
    }
}
