using System;
using System.Runtime.Serialization;
using System.Globalization;

namespace Library.Web.Models
{
    [DataContract(Name = "book")]
    public class Book
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "author")]
        public string Author { get; set; }
        [DataMember(Name = "isbn")]
        public string ISBN { get; set; }
        [DataMember(Name = "releaseDate")]
        private string jsonReleaseDate { get; set; }
        [IgnoreDataMember]
        public DateTime ReleaseDate => DateTime.ParseExact(jsonReleaseDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}