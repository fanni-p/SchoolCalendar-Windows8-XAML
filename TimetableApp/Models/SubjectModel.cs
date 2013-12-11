using System;
using System.Runtime.Serialization;
using TimetableApp.ViewModel;

namespace TimetableApp.Models
{
    [DataContract]
    public class SubjectModel: ViewModelBase
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "teacher")]
        public string Teacher { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }
    }
}
