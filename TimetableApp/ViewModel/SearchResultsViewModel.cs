using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimetableApp.Data;
using TimetableApp.Models;
using TimetableApp.Services;

namespace TimetableApp.ViewModel
{
    public class SearchResultsViewModel : ViewModelBase
    {
        private string queryText;
        private readonly ObservableCollection<SubjectModel> results;
        private SubjectModel selectedSubject;

        public SearchResultsViewModel()
        {
            this.queryText = string.Empty;
            this.results = new ObservableCollection<SubjectModel>();
        }

        public string QueryText
        {
            get
            {
                return this.queryText;
            }
            set
            {
                this.queryText = value;
                this.OnPropertyChanged("QueryText");
            }
        }

        public SubjectModel SelectedSubject
        {
            get
            {
                return this.selectedSubject;
            }
            set
            {
                this.selectedSubject = value;
                NavigationService.Navigate(ViewType.SelectedSubjectPage, selectedSubject);
            }
        }

        public IEnumerable<SubjectModel> Results
        {
            get
            {
                return this.results;
            }
            set
            {
                this.results.Clear();

                foreach (var item in value)
                {
                    this.results.Add(item);
                }
            }
        }

        public override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            string queryTextAsNavParameter = navigationParameter as String;
            this.QueryText = queryTextAsNavParameter;
            this.LoadSubjectNames();
        }

        private async void LoadSubjectNames()
        {
            var subjects = await DataPersister.GetAllSubjects();

            foreach (var subject in subjects)
            {
                if (subject.Name.ToLower().Contains(this.QueryText.ToLower()))
                {
                    this.results.Add(subject);
                }
            }
        }
    }
}