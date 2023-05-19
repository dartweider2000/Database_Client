using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase_Client
{
    public class Pasport : ITableData
    {
        public string PasportNumber { get; }
        public string Name { get; }
        public string Producer { get; }
        public string Type { get; }
        public string Place { get; }
        public DateTime BirthDay { get; }

        public Pasport(string PasportNumber, string Name, string Producer, string Type, string Place, DateTime BirthDay)
        {
            this.PasportNumber = PasportNumber;
            this.Name = Name;
            this.Producer = Producer;
            this.Type = Type;
            this.Place = Place;
            this.BirthDay = BirthDay;
        }

        public override string ToString()
        {
            return $"PasportNumber - {this.PasportNumber}," +
                $"Name - {this.Name}," +
                $"Producer - {this.Producer}," +
                $"Type - {this.Type}," +
                $"Place - {this.Place}," +
                $"BirthDay - {this.BirthDay}";
        }
    }
}
