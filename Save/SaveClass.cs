using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaginInterface;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;


using System.IO;
using WpfApp5.Classes;




namespace Save
{ 
    public class SaveClass: IPlugin
    {
        public string Name
        {
            get
            {
                return "Сохранить обьект";
            }
        }

        public void Do(object a)
        {


            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("pluginbook.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, (a as WpfApp5.Book));

            }

        }
    }
}
