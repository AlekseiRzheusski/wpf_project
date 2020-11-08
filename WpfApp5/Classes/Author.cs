using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace WpfApp5
{
    [Serializable]
    [DataContract]
    public class Author
    {
        [DataMember]
        public string AutName { get; set; }
        [DataMember]
        public string BirthDate { get; set; }
        public override string ToString()
        {
            return AutName;
        }
    }
}
