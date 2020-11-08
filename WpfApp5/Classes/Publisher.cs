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
    public class Publisher
    {
        [DataMember]
        public string PubName { get; set; }
        [DataMember]
        public string PubCity { get; set; }
        public override string ToString()
        {
            string ret = PubName + ", " + PubCity;
            return ret;
        }
    }
}
