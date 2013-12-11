using System;
using System.Collections.Generic;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class AddNewHomeworkViewModel : ViewModelBase
    {
        private readonly List<string> subjectsName;
        private HomeworkModel newHomework;

        public AddNewHomeworkViewModel()
        {
            GetData();
            this.subjectsName = new List<string>();
            this.newHomework = new HomeworkModel() { IsDone = false, SubmitDate = DateTime.Today };
            this.CreateNewSubject = new RelayCommand(this.HandleCreateNewSubject);
            this.AddHomework = new RelayCommand(this.HandleAddHomework);
        }

        public ICommand AddHomework { get; private set; }
        public ICommand CreateNewSubject { get; private set; }

        public HomeworkModel NewHomework
        {
            get
            {
                if (this.newHomework == null)
                {
                    return this.newHomework;
                }

                return this.newHomework;
            }

            set
            {
                if (this.newHomework != value)
                {
                    this.newHomework = value;
                    this.OnPropertyChanged("NewHomework");
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

        private async void HandleAddHomework(object parameter)
        {
            var homework = this.NewHomework;

            var response = await DataPersister.AddHomework(homework);
            if (response == true)
            {
                NavigationService.GoBack();
            }
        }
    }
}
