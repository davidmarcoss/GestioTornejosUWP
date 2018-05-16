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
    public sealed partial class GrupItemRow : UserControl
    {
        public GrupItemRow()
        {
            this.InitializeComponent();
        }

        public Grup Grup
        {
            get { return (Grup)GetValue(GrupProperty); }
            set
            {
                SetValue(GrupProperty, value);

                this.DataContext = value;
            }
        }

        public static readonly DependencyProperty GrupProperty =
            DependencyProperty.Register("Grup", typeof(Grup), typeof(GrupItemRow), new PropertyMetadata(0));
    }
}
