using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Client
{
    public class Repair : ITableData
    {
        public string PasportNumber { get; }
        public string Type { get; }
        public DateTime Date { get; }

        public Repair(string PasportNumber, string Type, DateTime Date)
        {
            this.PasportNumber = PasportNumber;
            this.Type = Type;
            this.Date = Date;
        }
    }
}
