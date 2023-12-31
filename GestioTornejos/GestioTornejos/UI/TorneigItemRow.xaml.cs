﻿using GestioTornejos.Models;
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
    public sealed partial class TorneigItemRow : UserControl
    {
        public TorneigItemRow()
        {
            this.InitializeComponent();
        }

        public Torneig Torneig
        {
            get { return (Torneig)GetValue(TorneigProperty); }
            set
            {
                SetValue(TorneigProperty, value);

                this.DataContext = value;
            }
        }

        public static readonly DependencyProperty TorneigProperty =
            DependencyProperty.Register("Torneig", typeof(Torneig), typeof(TorneigItemRow), new PropertyMetadata(0));
    }
}
