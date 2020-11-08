using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaginInterface
{
    public interface IPlugin
    {
        string Name { get; }
        void Do(object a);
    }
}
