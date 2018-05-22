using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class EstadisticaModalitat
    {
        private Soci soci;

        public Soci Soci
        {
            get { return soci; }
            set { soci = value; }
        }

        private Modalitat modalitat;

        public Modalitat Modalitat
        {
            get { return modalitat; }
            set { modalitat = value; }
        }

        private float coeficientBase;

        public float CoeficientBase
        {
            get { return coeficientBase; }
            set { coeficientBase = value; }
        }

        private int carambolesTemporadaActual;

        public int CarambolesTemporadaActual
        {
            get { return carambolesTemporadaActual; }
            set { carambolesTemporadaActual = value; }
        }

        private int entradesTemporadaActual;

        public int EntradesTemporadaActual
        {
            get { return entradesTemporadaActual; }
            set { entradesTemporadaActual = value; }
        }

        public EstadisticaModalitat(Soci soci, Modalitat modalitat, float coeficientBase, int carambolesTemporadaActual, int entradesTemporadaActual)
        {
            Soci = soci;
            Modalitat = modalitat;
            CoeficientBase = coeficientBase;
            CarambolesTemporadaActual = carambolesTemporadaActual;
            EntradesTemporadaActual = entradesTemporadaActual;
        }
    }
}
