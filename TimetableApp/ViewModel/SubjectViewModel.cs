using System;
using System.Collections.Generic;
using System.Windows.Input;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Behavior;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class SubjectViewModel : ViewModelBase 
    {
        private IEnumerable<SubjectModel> subjects;
        private SubjectModel oldSubject;

        public SubjectViewModel()
        {
            this.ShowDetailsCommand = new RelayCommand(this.HandleShowDetailsCommand);
            this.AddSubjectCommand = new RelayCommand(this.HandleAddSubjectCommand);
            this.EditSubjectCommand = new RelayCommand(this.HandleEditSubjectCommand);
            this.RemoveSubjectCommand = new RelayCommand(this.HandleRemoveSubject);
        }

        public SubjectModel SelectedSubject { get; set; }
        public ICommand ShowDetailsCommand { get; private set; }
        public ICommand AddSubjectCommand { get; private set; }
        public ICommand EditSubjectCommand { get; private set; }
        public ICommand RemoveSubjectCommand { get; private set; }

        public IEnumerable<SubjectModel> Subjects
        {
            get
            {
                if (this.subjects == null)
                {
                    this.GetData();
                    return null;
                }

                return this.subjects;
            }

            private set
            {
                this.subjects = value;
                this.OnPropertyChanged("Subjects");
            }
        }

        private async void GetData()
        {
            this.Subjects = await DataPersister.GetAllSubjects();
        }

        private void HandleShowDetailsCommand(object parameter)
        {
            var selectedSubject = this.SelectedSubject;
            NavigationService.Navigate(ViewType.SelectedSubjectPage, selectedSubject);
        }

        private void HandleRemoveSubject(object subject)
        {
            var selectedSubject = this.SelectedSubject;

            DataPersister.DeleteSubject(selectedSubject.Id);

            this.Subjects = null;
        }

        private void HandleAddSubjectCommand(object subject)
        {
            NavigationService.Navigate(ViewType.AddNewSubjectPage);
        }

        private async void HandleEditSubjectCommand(object subject)
        {
            var selectedSubject = this.SelectedSubject;

            oldSubject = await DataPersister.GetSubjectById(selectedSubject.Id);

            NavigationService.Navigate(ViewType.EditSubjectPage, this.oldSubject);
        }
    }
}
