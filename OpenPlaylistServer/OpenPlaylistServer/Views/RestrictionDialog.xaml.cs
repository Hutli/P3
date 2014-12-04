using OpenPlaylistServer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenPlaylistServer.Views
{
    /// <summary>
    /// Interaction logic for RestrictionDialog.xaml
    /// </summary>
    public partial class RestrictionDialog : Window
    {
        public RestrictionDialog(Restriction restriction)
        {
            InitializeComponent();
            DataContext = restriction;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
