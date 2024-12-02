using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class GenericIdDescDTO
    {
        public int id { get; set; }
        public string name { get; set; }

        public GenericIdDescDTO() { }
        public GenericIdDescDTO(int id, string description)
        {
            this.id = id;
            this.name = description;
        }
    }
}
