using System;
using System.Collections.Generic;
using System.Linq;
using TimetableApp.Data;
using TimetableApp.Models;

namespace TimetableApp.ViewModel
{
    public class SelectedSubjectViewModel : ViewModelBase
    {
        private SubjectModel selectedSubject;
        private IEnumerable<LessonModel> selectedSubjectLessons;
        private IEnumerable<HomeworkModel> selectedSubjectHomeworks;

        public SubjectModel SelectedSubject
        {
            get
            {
                return this.selectedSubject;
            }

            set
            {
                if (this.selectedSubject != value)
                {
                    this.selectedSubject = value;
                    this.OnPropertyChanged("SelectedSubject");
                }
            }
        }

        public IEnumerable<LessonModel> SelectedSubjectLessons
        {
            get
            {
                return this.selectedSubjectLessons;
            }

            set
            {
                this.selectedSubjectLessons = value;
                this.OnPropertyChanged("SelectedSubjectLessons");
            }
        }

        public IEnumerable<HomeworkModel> SelectedSubjectHomeworks
        {
            get
            {
                return this.selectedSubjectHomeworks;
            }

            set
            {
                this.selectedSubjectHomeworks = value;
                this.OnPropertyChanged("SelectedSubjectHomeworks");
            }
        }

        public override async void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
            this.SelectedSubject = navParameter as SubjectModel;
            this.SelectedSubjectLessons = await DataPersister.GetLessonBySubject(this.SelectedSubject.Name);
            this.SelectedSubjectHomeworks = await DataPersister.GetHomeworkBySubject(this.SelectedSubject.Name);
        }
    }
}
