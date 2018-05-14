using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Grup
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private Torneig torneig;

        public Torneig Torneig
        {
            get { return torneig; }
            set { torneig = value; }
        }

        private String descripcio;

        public String Descripcio
        {
            get { return descripcio; }
            set { descripcio = value; }
        }

        private int carambolesVictoria;

        public int CarambolesVictoria
        {
            get { return carambolesVictoria; }
            set { carambolesVictoria = value; }
        }

        private int limitEntrades;

        public int LimitEntrades
        {
            get { return limitEntrades; }
            set { limitEntrades = value; }
        }
    }
}
