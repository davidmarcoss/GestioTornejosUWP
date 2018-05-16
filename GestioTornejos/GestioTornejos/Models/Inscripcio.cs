using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Inscripcio
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime dataCreacio;

        public DateTime DataCreacio
        {
            get { return dataCreacio; }
            set { dataCreacio = value; }
        }

        private Soci soci;

        public Soci Soci
        {
            get { return soci; }
            set { soci = value; }
        }

        private Grup grup;

        public Grup Grup
        {
            get { return grup; }
            set { grup = value; }
        }

        public Inscripcio(int id, DateTime dataCreacio, Soci soci)
        {
            Id = id;
            DataCreacio = dataCreacio;
            Soci = soci;
        }
    }
}
