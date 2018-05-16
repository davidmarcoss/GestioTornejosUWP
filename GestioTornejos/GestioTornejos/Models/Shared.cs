using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace GestioTornejos.Models
{
    public class Shared
    {
        public Shared(int idxSelected, ObservableCollection<Torneig> ocTornejos, ListView lvTornejos)
        {
            IdxSelected = idxSelected;
            OcTornejos = ocTornejos;
            LvTornejos = lvTornejos;
        }


        private int idxSelected;

        public int IdxSelected
        {
            get { return idxSelected; }
            set { idxSelected = value; }
        }


        private ObservableCollection<Torneig> ocTornejos;

        public ObservableCollection<Torneig> OcTornejos
        {
            get { return ocTornejos; }
            set { ocTornejos = value; }
        }

        private ListView lvTornejos;

        public ListView LvTornejos
        {
            get { return lvTornejos; }
            set { lvTornejos = value; }
        }

    }
}
