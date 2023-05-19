using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Client
{
    public class PasportProperty : ITableData
    {
        public string PasportNumber { get; }
        public string Property { get; }
        public PasportProperty(string PasportNumber, string Property)
        {
            this.PasportNumber = PasportNumber;
            this.Property = Property;
        }
    }
}
