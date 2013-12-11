using System;
using System.Runtime.Serialization;

namespace TimetableApp.Models
{
    [DataContract]
    public class HomeworkModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "submitDate")]
        public DateTime SubmitDate { get; set; }

        [DataMember(Name = "isDone")]
        public bool IsDone { get; set; }

        public string DateAsString
        {
            get
            {
                return this.SubmitDate.ToString("dd/MM/yyyy");
            }
        }

        public int DaysLeft
        {
            get
            {
                var daysLeft = CalculateDaysLeft();
                return daysLeft.Days;
            }
        }

        public string ForegroundColor
        {
            get
            {
                if (this.DaysLeft <= 1)
                {
                    return "Tomato";
                }

                return "LimeGreen";
            }
        }

        private TimeSpan CalculateDaysLeft()
        {
            var days = this.SubmitDate - DateTime.Today;
            return days;
        }
    }
}
