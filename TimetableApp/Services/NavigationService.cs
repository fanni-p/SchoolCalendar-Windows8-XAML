using System;
using TimetableApp.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TimetableApp.Services
{
    public class NavigationService : INavigationService
    {
        private static Frame Frame { get; set; }

        private static NavigationService instance;

        public static INavigationService Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new NavigationService();
                    Frame = (Frame)Window.Current.Content;
                }

                return instance;
            }
        }

        public bool CanGoBack
        {
            get
            {
                if (Frame != null)
                {
                    return Frame.CanGoBack;
                }

                return false;
            }
        }

        public bool CanGoForward
        {
            get
            {
                if (Frame != null)
                {
                    return Frame.CanGoForward;
                }

                return false;
            }
        }

        public void GoHome()
        {
            if (Frame != null)
            {
                while (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        public void GoBack()
        {
            if (Frame != null && Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        public void GoForward()
        {
            if (Frame != null && Frame.CanGoForward)
            {
                Frame.GoForward();
            }
        }

        public void Navigate(ViewType pageName)
        {
            this.Navigate(pageName, null);
        }

        public void Navigate(ViewType pageName, object parameter)
        {
            var pageType = this.GetViewType(pageName);

            if (Frame != null && pageType != null)
            {
                Frame.Navigate(pageType, parameter);
            }
        }

        private Type GetViewType(ViewType view)
        {
            switch (view)
            {
                case ViewType.HomePage:
                    return typeof(HomePage);
                case ViewType.SubjectsPage:
                    return typeof(SubjectsPage);
                case ViewType.AddNewSubjectPage:
                    return typeof(AddNewSubjectPage);
                case ViewType.EditSubjectPage:
                    return typeof(EditSubjectPage);
                case ViewType.HomeworksPage:
                    return typeof(HomeworksPage);
                case ViewType.AddNewHomeworkPage:
                    return typeof(AddNewHomeworkPage);
                case ViewType.EditHomeworkPage:
                    return typeof(EditHomeworkPage);
                case ViewType.AddLessonPage:
                    return typeof(AddLessonPage);
                case ViewType.EditLessonPage:
                    return typeof(EditLessonPage);
                case ViewType.WeekSchedulePage:
                    return typeof(WeekSchedulePage);
                case ViewType.SelectedSubjectPage:
                    return typeof(SelectedSubjectPage);
                default:
                    break;
            }

            return null;
        }
    }
}
