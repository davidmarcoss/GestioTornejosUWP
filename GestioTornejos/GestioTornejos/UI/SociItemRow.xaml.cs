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
    public sealed partial class SociItemRow : UserControl
    {
        public SociItemRow()
        {
            this.InitializeComponent();
        }



        public SociRanking SociRanking
        {
            get { return (SociRanking)GetValue(SociRankingProperty); }
            set { SetValue(SociRankingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SociRanking.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SociRankingProperty =
            DependencyProperty.Register("SociRanking", typeof(SociRanking), typeof(SociItemRow), new PropertyMetadata(0));



    }
}
