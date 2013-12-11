using System;
using System.Collections.Generic;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class EditHomeworkViewModel :ViewModelBase
    {
        private readonly List<string> subjectsName;
        private HomeworkModel editedHomework;

        public EditHomeworkViewModel()
        {
            GetData();
            this.subjectsName = new List<string>();
            this.editedHomework = new HomeworkModel();
            this.EditCommand = new RelayCommand(this.HandleEditCommand);
            this.CreateNewSubject = new RelayCommand(this.HandleCreateNewSubject);
        }

        public ICommand EditCommand { get; private set; }
        public ICommand CreateNewSubject { get; private set; }

        public HomeworkModel EditedHomework
        {
            get
            {
                return this.editedHomework;
            }

            set
            {
                if (this.editedHomework != value)
                {
                    this.editedHomework = value;
                    this.OnPropertyChanged("EditedHomework");
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

        public override void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
            this.EditedHomework = navParameter as HomeworkModel;
        }

        private async void GetData()
        {
            var subjects = await DataPersister.GetAllSubjects();

            foreach (var subject in subjects)
            {
                this.subjectsName.Add(subject.Name);
            }
        }

        public async void HandleEditCommand(object parametar)
        {
            var homework = this.EditedHomework;

            var response = await DataPersister.EditHomework(homework, homework.Id);

            if (response == true)
            {
                NavigationService.GoBack();
            }
        }

        private void HandleCreateNewSubject(object parameter)
        {
            NavigationService.Navigate(ViewType.AddNewSubjectPage);
        }
    }
}
