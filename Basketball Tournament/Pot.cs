using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basketball_Tournament
{
    public class Pot
    {
        public List<Country> Teams { get; set; } = new List<Country>();
        public string Name { get; set; }

        public Pot(List<Country> teams, string potName) 
        {
            Teams = teams;
            Name = potName;
        }
        public Pot(string potName)
        {
            Name = potName;
        }
        public Pot() { }

        public override string ToString()
        {
            return $"  {Name}\n" + $"   {Teams[0].Team}\n" + $"   {Teams[1].Team}";
        }
    }
}
