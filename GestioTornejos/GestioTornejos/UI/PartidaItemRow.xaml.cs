using GestioTornejos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace GestioTornejos.UI
{
    public sealed partial class PartidaItemRow : UserControl
    {
        public PartidaItemRow()
        {
            this.InitializeComponent();
        }

        public Partida Partida
        {
            get { return (Partida)GetValue(PartidaProperty); }
            set { SetValue(PartidaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Partida.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PartidaProperty =
            DependencyProperty.Register("Partida", typeof(Partida), typeof(PartidaItemRow), new PropertyMetadata(0));


    }
}
