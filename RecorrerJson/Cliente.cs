using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecorrerJson
{
    public class Cliente
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public List<Propiedad> Propiedades { get; set; }
    }
}
