using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TimetableApp.Behavior;
using TimetableApp.Data;
using TimetableApp.Models;

namespace TimetableApp.ViewModel
{
    public class AddSubjectViewModel : ViewModelBase
    {
        private readonly string[] colors; 

        private SubjectModel newSubject;
        private string currentColor;

        public AddSubjectViewModel()
        {
            this.colors = new string[] 
            {
            "LightCoral", "Crimson", "LightPink", "PaleVioletRed",
            "LightSalmon", "Orange", "LightYellow", "PaleGoldenrod",
            "Khaki", "Lavender", "MediumOrchid", "MediumSlateBlue",
            "GreenYellow", "YellowGreen", "LightGreen", "MediumAquamarine",
            "CadetBlue", "SkyBlue", "RoyalBlue", "NavajoWhite","Wheat",
            "MediumBlue", "MistyRose", "DarkGray", "Purple", "Green"
            };

            this.colors = this.colors.OrderBy(x => x).ToArray();
            this.currentColor = "LightGray";
            this.AddSubject = new RelayCommand(this.HandleAddCommand);
        }

        public IEnumerable<string> Colors 
        {
            get
            {
                return this.colors;
            }
        }

        public ICommand AddSubject { get; private set; }

        public SubjectModel NewSubject
        {
            get
            {
                if (this.newSubject == null)
                {
                    return this.newSubject = new SubjectModel();
                }

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

        public async void HandleAddCommand(object parametar)
        {
            var subject = this.NewSubject;

            var response = await DataPersister.AddSubject(subject);

            if (response == true)
            {
                NavigationService.GoBack();
            }
        }
    }
}
