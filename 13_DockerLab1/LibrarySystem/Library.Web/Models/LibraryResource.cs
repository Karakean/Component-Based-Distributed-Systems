using System.Runtime.Serialization;

namespace Library.Web.Models
{
    [DataContract]
    public class LibraryResource
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        [DataMember(Name="book")]
        public Book Book { get; set; }
    }
}