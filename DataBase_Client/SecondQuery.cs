using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Client
{
    public class SecondQuery : ITableData
    {
        public string PasportNumber { get; }
        public int Count { get; }
        public DateTime FirstRepair { get; }
        public DateTime LastRepair { get; }
        public SecondQuery(string PasportNumber, int Count, DateTime FirstRepair, DateTime LastRepair)
        {
            this.PasportNumber = PasportNumber;
            this.Count = Count;
            this.FirstRepair = FirstRepair;
            this.LastRepair = LastRepair;
        }
    }
}
