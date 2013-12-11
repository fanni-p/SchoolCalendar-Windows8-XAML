using System;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Data;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class SettingsPanelViewModel : ViewModelBase
    {
        public SettingsPanelViewModel()
        {
            this.LogoutCommand = new RelayCommand(this.HandleLogoutCommand);
        }

        public ICommand LogoutCommand { get; private set; }

        private async void HandleLogoutCommand(object parameter)
        {
            var logout = await DataPersister.LogoutUser();

            if (logout)
            {
                NavigationService.Navigate(ViewType.HomePage);
            }
        }
    }
}
