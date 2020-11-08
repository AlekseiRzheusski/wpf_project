using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;



namespace WpfApp5
{
    [Serializable]
    [DataContract]
    public class Book : IEnumerable
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public long NumberOfPages { get; set; }
        [DataMember]
        public string releaseDate { get; set; }
        [DataMember]
        public long ISBN { get; set; }
        [DataMember]
        public ObservableCollection<Author> Authors { get; set; }
        [DataMember]
        public Publisher Publish { get; set; }
        [DataMember]
        public string Publ { get; set; }
        public Book()
        {
            Publish = new Publisher();
            Authors = new ObservableCollection<Author>();
        }

        public IEnumerator GetEnumerator()
        {
            return Authors.GetEnumerator();
        }

    }
}
