using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Client
{
    public class FirstQuery : ITableData
    {
        public string Producer { get; }
        public int Count { get; }
        public FirstQuery(string Producer, int Count)
        {
            this.Producer = Producer;
            this.Count = Count;
        }
    }
}
