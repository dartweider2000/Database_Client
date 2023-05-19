using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Client
{
    public class Reference : ITableData
    {
        public string Name { get; }
        public Reference(string Name)
        {
            this.Name = Name;
        }
    }
}
