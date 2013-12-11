using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class EditLessonViewModel :ViewModelBase
    {
        private readonly LessonType[] lessonTypes = new LessonType[] 
        { 
            LessonType.Exam, LessonType.Lecture, LessonType.Practice, LessonType.Other 
        };
        private readonly string[] days = new string[]
        {
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        };
        private List<string> subjectsName = new List<string>();
        private LessonModel newLesson;
        private DateTime startTime;
        private DateTime endTime;
        private string subjectName;

        public EditLessonViewModel()
        {
            this.newLesson = new LessonModel();
            this.EditLesson = new RelayCommand(this.HandleEditLesson);
        }

        public ICommand EditLesson { get; private set; }

        public LessonModel NewLesson
        {
            get
            {
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

        public string SubjectName
        {
            get
            {
                return this.subjectName;
            }

            set
            {
                this.subjectName = value;
                this.OnPropertyChanged("SubjectName");
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

        public override async void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
            this.GetData();
            this.NewLesson = navParameter as LessonModel;
            this.StartTime = DateTime.Parse(this.NewLesson.StartTime.ToString());
            this.EndTime = DateTime.Parse(this.NewLesson.EndTime.ToString());
            this.SubjectName = this.NewLesson.Subject;
        }

        private async void GetData()
        {
            var subjects = await DataPersister.GetAllSubjects();

            foreach (var subject in subjects)
            {
                this.subjectsName.Add(subject.Name);
            }
        }

        private async void HandleEditLesson(object parameter)
        {
            var lesson = this.NewLesson;
            lesson.Subject = this.SubjectName;
            lesson.StartTime = TimeSpan.Parse(this.StartTime.ToString("HH:mm"));
            lesson.EndTime = TimeSpan.Parse(this.EndTime.ToString("HH:mm"));

            var response = await DataPersister.EditLesson(lesson,lesson.Id);

            if (response == true)
            {
                NavigationService.GoBack();
            }
        }
    }
}
