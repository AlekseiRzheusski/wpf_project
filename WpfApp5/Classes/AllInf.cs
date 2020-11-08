using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5.Classes
{
    [Serializable]
    public class AllInf : IEnumerable
    {
        public ObservableCollection<Book> Ser_Book { get; set; }
        public AllInf()
        {
            Ser_Book = new ObservableCollection<Book>();
        }

        public IEnumerator GetEnumerator()
        {
            return Ser_Book.GetEnumerator();
        }

    }
}
