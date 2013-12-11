using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Data;
using TimetableApp.Models;

namespace TimetableApp.ViewModel
{
    public class EditSubjectViewModel : ViewModelBase
    {
        private readonly string[] colors = new string[] 
        {
            "LightCoral", "Crimson", "LightPink", "PaleVioletRed",
            "LightSalmon", "Orange", "LightYellow", "PaleGoldenrod",
            "Khaki", "Lavender", "MediumOrchid", "MediumSlateBlue",
            "GreenYellow", "YellowGreen", "LightGreen", "MediumAquamarine",
            "CadetBlue", "SkyBlue", "RoyalBlue", "NavajoWhite","Wheat",
            "MediumBlue", "MistyRose", "DarkGray", "Purple", "Green"
        };

        private SubjectModel newSubject;
        private string currentColor = "LightGray";

        public EditSubjectViewModel()
        {
            this.newSubject = new SubjectModel();
            this.AddSubject = new RelayCommand(this.HandleAddCommand);
        }

        public ICommand AddSubject { get; private set; }

        public IEnumerable<string> Colors 
        {
            get
            {
                return this.colors.OrderBy(x => x);
            }
        }

        public SubjectModel NewSubject
        {
            get
            {
                return this.newSubject;
            }

            set
            {
                if (this.newSubject != value)
                {
                    this.newSubject = value;
                    this.OnPropertyChanged("NewSubject");
                }
            }
        }

        public string CurrentColor
        {
            get
            {
                return this.currentColor;
            }

            set
            {
                this.currentColor = value;
                this.OnPropertyChanged("CurrentColor");
            }
        }

        public override void LoadState(object navParameter, Dictionary<string, object> viewModelState)
        {
            this.NewSubject = navParameter as SubjectModel;
            this.CurrentColor = NewSubject.Color;
        }

        public async void HandleAddCommand(object parametar)
        {
            var subject = this.NewSubject;

            var response = await DataPersister.EditSubject(subject, subject.Id);

            if (response == true)
            {
                NavigationService.GoBack();
            }
        }
    }
}
