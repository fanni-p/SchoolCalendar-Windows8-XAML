using System;
using System.Collections.Generic;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class AddLessonViewModel : ViewModelBase
    {
        private readonly LessonType[] lessonTypes = new LessonType[] 
        { 
            LessonType.Exam, LessonType.Lecture, LessonType.Practice, LessonType.Other 
        };
        private readonly string[] days = new string[]
        {
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        };
        private readonly List<string> subjectsName;
        private LessonModel newLesson;
        private DateTime startTime;
        private DateTime endTime;

        public AddLessonViewModel()
        {
            GetData();
            this.subjectsName = new List<string>();
            this.newLesson = new LessonModel();
            this.CreateNewSubject = new RelayCommand(this.HandleCreateNewSubject);
            this.AddLesson = new RelayCommand(this.HandleAddLesson);
        }

        public ICommand CreateNewSubject { get; private set; }
        public ICommand AddLesson { get; private set; }

        public DateTime StartTime
        {
            get
            {
                return this.startTime;
            }

            set
            {
                this.startTime = value;
                this.OnPropertyChanged("StartTime");
            }
        }

        public DateTime EndTime
        {
            get
            {
                return this.endTime;
            }

            set
            {
                this.endTime = value;
                this.OnPropertyChanged("EndTime");
            }
        }
        
        public LessonModel NewLesson
        {
            get
            {
                if (this.newLesson == null)
                {
                    return this.newLesson;
                }

                return this.newLesson;
            }

            set
            {
                if (this.newLesson != value)
                {
                    this.newLesson = value;
                    this.OnPropertyChanged("NewLesson");
                }
            }
        }

        public IEnumerable<string> SubjectsName
        {
            get
            {
                return this.subjectsName;
            }
        }

        public IEnumerable<LessonType> LessonTypes
        {
            get
            {
                return this.lessonTypes;
            }
        }

        public IEnumerable<string> Days
        {
            get
            {
                return this.days;
            }
        }

        private async void GetData()
        {
            var subjects = await DataPersister.GetAllSubjects();

            foreach (var subject in subjects)
            {
                this.subjectsName.Add(subject.Name);
            }
        }

        private void HandleCreateNewSubject(object parameter)
        {
            NavigationService.Navigate(ViewType.AddNewSubjectPage);
        }

        private async void HandleAddLesson(object parameter)
        {
            var lesson = this.NewLesson;
            lesson.StartTime = TimeSpan.Parse(StartTime.ToString("HH:mm"));
            lesson.EndTime = TimeSpan.Parse(EndTime.ToString("HH:mm"));

            var response = await DataPersister.AddLesson(lesson);

            if (response == true)
            {
                NavigationService.GoBack();
            }
        }
    }
}
