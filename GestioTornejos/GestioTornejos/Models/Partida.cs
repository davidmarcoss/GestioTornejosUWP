using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioTornejos.Models
{
    public class Partida
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private Soci sociA;

        public Soci SociA
        {
            get { return sociA; }
            set { sociA = value; }
        }

        private Soci sociB;

        public Soci SociB
        {
            get { return sociB; }
            set { sociB = value; }
        }

        private Grup grup;

        public Grup Grup
        {
            get { return grup; }
            set { grup = value; }
        }

        private Torneig torneig;

        public Torneig Torneig
        {
            get { return torneig; }
            set { torneig = value; }
        }


        private int carambolesA;

        public int CarambolesA
        {
            get { return carambolesA; }
            set { carambolesA = value; }
        }

        private int carambolesB;

        public int CarambolesB
        {
            get { return carambolesB; }
            set { carambolesB = value; }
        }

        private DateTime? dataRealitzacio;

        public DateTime? DataRealitzacio
        {
            get { return dataRealitzacio; }
            set { dataRealitzacio = value;}
        }

        private int numEntradesA;

        public int NumEntradesA
        {
            get { return numEntradesA; }
            set { numEntradesA = value; }
        }

        private int numEntradesB;

        public int NumEntradesB
        {
            get { return numEntradesB; }
            set { numEntradesB = value; }
        }

        private EstatPartida estatPartida;

        public EstatPartida EstatPartida
        {
            get { return estatPartida; }
            set { estatPartida = value; }
        }

        private ModeVictoria modeVictoria;

        public ModeVictoria ModeVictoria
        {
            get { return modeVictoria; }
            set { modeVictoria = value; }
        }

        private Guanyador guanyador;

        public Guanyador Guanyador
        {
            get { return guanyador; }
            set { guanyador = value; }
        }

        public Partida(int id, Soci sociA, Soci sociB, Grup grup, Torneig torneig, int carambolesA, int carambolesB, int numEntradesA, int numEntradesB, EstatPartida estatPartida)
        {
            Id = id;
            SociA = sociA;
            SociB = sociB;
            Grup = grup;
            Torneig = torneig;
            CarambolesA = carambolesA;
            CarambolesB = carambolesB;
            NumEntradesA = numEntradesA;
            NumEntradesB = numEntradesB;
            EstatPartida = estatPartida;
            DataRealitzacio = null;
        }

        public Partida(int id, Soci sociA, Soci sociB, Grup grup, Torneig torneig, int carambolesA, int carambolesB, DateTime? dataRealitzacio, int numEntradesA, int numEntradesB, EstatPartida estatPartida, ModeVictoria modeVictoria, Guanyador guanyador)
        {
            Id = id;
            SociA = sociA;
            SociB = sociB;
            Grup = grup;
            Torneig = torneig;
            CarambolesA = carambolesA;
            CarambolesB = carambolesB;
            DataRealitzacio = dataRealitzacio;
            NumEntradesA = numEntradesA;
            NumEntradesB = numEntradesB;
            EstatPartida = estatPartida;
            ModeVictoria = ModeVictoria;
        }
    }

    public enum EstatPartida
    {
        PENDENT,
        JUGAT
    }

    public enum ModeVictoria
    {
        PER_CARAMBOLES,
        ENTRADES_ASSOLIDES,
        ABANDONAMENT
    }

    public enum Guanyador
    {
        A,
        B
    }
}
