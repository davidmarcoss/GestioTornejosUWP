using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Modalitat
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String descripcio;

        public String Descripcio
        {
            get { return descripcio; }
            set { descripcio = value; }
        }

        public Modalitat(int id, string descripcio)
        {
            Id = id;
            Descripcio = descripcio;
        }
    }
}
