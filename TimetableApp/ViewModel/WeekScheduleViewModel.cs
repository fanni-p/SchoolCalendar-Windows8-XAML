using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Behavior;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class WeekScheduleViewModel : ViewModelBase
    {
        private LessonModel oldLesson = new LessonModel();
        private IEnumerable<LessonModel> tuesdayLessons;
        private IEnumerable<LessonModel> mondayLessons;
        private IEnumerable<LessonModel> wednesdayLessons;
        private IEnumerable<LessonModel> thursdayLessons;
        private IEnumerable<LessonModel> fridayLessons;

        public WeekScheduleViewModel()
        {
            this.GetData();
            this.AddLessonCommand = new RelayCommand(this.HandleAddLessonCommand);
            this.EditLessonCommand = new RelayCommand(this.HandleEditLessonCommand);
            this.RemoveLessonCommand = new RelayCommand(this.HandleRemoveLessonCommand);
        }

        public LessonModel SelectedLesson { get; set; }
        public ICommand AddLessonCommand { get; private set; }
        public ICommand EditLessonCommand { get; private set; }
        public ICommand RemoveLessonCommand { get; private set; }

        public IEnumerable<LessonModel> Monday
        {
            get
            {
                return this.mondayLessons;
            }

            set
            {
                this.mondayLessons = value;
                this.OnPropertyChanged("Monday");
            }
        }

        public IEnumerable<LessonModel> Tuesday
        {
            get
            {
                return this.tuesdayLessons;
            }

            set
            {
                this.tuesdayLessons = value;
                this.OnPropertyChanged("Tuesday");
            }
        }

        public IEnumerable<LessonModel> Wednesday
        {
            get
            {
                return this.wednesdayLessons;
            }

            set
            {
                this.wednesdayLessons = value;
                this.OnPropertyChanged("Wednesday");
            }
        }

        public IEnumerable<LessonModel> Thursday
        {
            get
            {
                return this.thursdayLessons;
            }

            set
            {
                this.thursdayLessons = value;
                this.OnPropertyChanged("Thursday");
            }
        }

        public IEnumerable<LessonModel> Friday
        {
            get
            {
                return this.fridayLessons;
            }

            set
            {
                this.fridayLessons = value;
                this.OnPropertyChanged("Friday");
            }
        }

        private async void GetData()
        {
            var allLessons = await DataPersister.GetLessonsPerWeek();
            this.Monday = allLessons.Where(l => l.Day == "Monday");
            this.Tuesday = allLessons.Where(l => l.Day == "Tuesday");
            this.Wednesday = allLessons.Where(l => l.Day == "Wednesday");
            this.Thursday = allLessons.Where(l => l.Day == "Thursday");
            this.Friday = allLessons.Where(l => l.Day == "Friday");
        }

        private void HandleAddLessonCommand(object parameter)
        {
            NavigationService.Navigate(ViewType.AddLessonPage);
        }

        private async void HandleEditLessonCommand(object parameter)
        {
            var selectedLesson = this.SelectedLesson;

            oldLesson = await DataPersister.GetLessonById(selectedLesson.Id);

            NavigationService.Navigate(ViewType.EditLessonPage, this.oldLesson);
        }

        private async void HandleRemoveLessonCommand(object parameter)
        {
            var selectedLesson = this.SelectedLesson;

            bool isDeleted = await DataPersister.DeleteLesson(selectedLesson.Id);

            if (isDeleted == true)
            {
                this.GetData();
            }
        }
    }
}
