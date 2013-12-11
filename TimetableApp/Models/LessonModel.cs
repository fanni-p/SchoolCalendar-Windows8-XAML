using System;
using System.Runtime.Serialization;

namespace TimetableApp.Models
{
    [DataContract]
    public class LessonModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "subjectColor")]
        public string SubjectColor { get; set; }

        [DataMember(Name = "day")]
        public string Day { get; set; }

        [DataMember(Name = "dayNumber")]
        public int? DayNumber { get; set; }

        [DataMember(Name = "startTime")]
        public TimeSpan StartTime { get; set; }

        [DataMember(Name = "endTime")]
        public TimeSpan EndTime { get; set; }

        [DataMember(Name = "type")]
        public LessonType Type { get; set; }

        [DataMember(Name = "room")]
        public string Room { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }

        public string StartTimeString
        {
            get
            {
                return this.StartTime.ToString(@"hh\:mm");
            }
        }

        public string EndTimeString
        {
            get
            {
                return this.EndTime.ToString(@"hh\:mm");
            }
        }

        public string LessonRoom
        {
            get
            {
                if (this.Room == "0" || String.IsNullOrEmpty(this.Room))
                {
                    return "No room";
                }

                return this.Room.ToString();
            }
        }
    }
}
