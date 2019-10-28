using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class StarLite
    {
        public StarLite(int id, string name, string galaxy)
        {
            Id = id;
            Name = name;
            Galaxy = galaxy;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Galaxy { get; set; }
        public StarLite() { }
    }
}
