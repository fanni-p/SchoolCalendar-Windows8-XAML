using System;

namespace TimetableApp.Services
{
    public interface INavigationService
    {
        void GoHome();

        void GoBack();

        void GoForward();

        void Navigate(ViewType pageName);

        void Navigate(ViewType pageName, object parameter);

        bool CanGoBack { get; }

        bool CanGoForward { get; }
    }
}
