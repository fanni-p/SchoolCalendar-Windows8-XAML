using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class HomeworksViewModel : ViewModelBase
    {
        private IEnumerable<HomeworkModel> homeworks;
        private HomeworkModel oldHomework;

        public HomeworksViewModel()
        {
            this.MarkAsDoneCommand = new RelayCommand(this.HandleMarkAsDoneCommand);
            this.AddHomeworkCommand = new RelayCommand(this.HandleAddHomeworkCommand);
            this.EditHomeworkCommand = new RelayCommand(this.HandleEditHomeworkCommand);
            this.RemoveHomeworkCommand = new RelayCommand(this.HandleRemoveHomeworkCommand);
        }

        public HomeworkModel SelectedHomework { get; set; }
        public ICommand MarkAsDoneCommand { get; private set; }
        public ICommand AddHomeworkCommand { get; private set; }
        public ICommand EditHomeworkCommand { get; private set; }
        public ICommand RemoveHomeworkCommand { get; private set; }

        public IEnumerable<HomeworkModel> Homeworks
        {
            get
            {
                if (this.homeworks == null)
                {
                    this.GetData();
                    return null;
                }

                return this.homeworks;
            }

            private set
            {
                this.homeworks = value;
                this.OnPropertyChanged("Homeworks");
            }
        }

        private async void GetData()
        {
            this.Homeworks = await DataPersister.GetAllHomeworks();
        }

        private void HandleMarkAsDoneCommand(object parameter)
        {
            var selectedHomework = this.SelectedHomework;

            DataPersister.MarkHomeworkAsDone(selectedHomework.Id);
           
            this.Homeworks = null;
        }

        private void HandleAddHomeworkCommand(object parameter)
        {
            NavigationService.Navigate(ViewType.AddNewHomeworkPage);
        }

        private async void HandleEditHomeworkCommand(object parameter)
        {
            var selectedHomework = this.SelectedHomework;

            oldHomework = await DataPersister.GetHomeworkById(selectedHomework.Id);

            NavigationService.Navigate(ViewType.EditHomeworkPage, this.oldHomework);
        }

        private void HandleRemoveHomeworkCommand(object parameter)
        {
            var selectedHomework = this.SelectedHomework;

            DataPersister.DeleteHomework(selectedHomework.Id);

            this.Homeworks = null;
        }
    }
}
