using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class SociRanking
    {
        private string nom;

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        private int partidesJugades;

        public int PartidesJugades
        {
            get { return partidesJugades; }
            set { partidesJugades = value; }
        }

        private int partidesGuanyades;

        public int PartidesGuanyades
        {
            get { return partidesGuanyades; }
            set { partidesGuanyades = value; }
        }

        private int partidesPerdudes;

        public int PartidesPerdudes
        {
            get { return partidesPerdudes; }
            set { partidesPerdudes = value; }
        }

        private float coeficient;

        public float Coeficient
        {
            get { return coeficient; }
            set { coeficient = value; }
        }

    }
}
