using System.Windows;
using OpenPlaylistServer.Models;

namespace OpenPlaylistServer.Views {
    /// <summary>
    ///     Interaction logic for RestrictionDialog.xaml
    /// </summary>
    public partial class RestrictionDialog : Window {
        public RestrictionDialog(Restriction restriction) {
            InitializeComponent();
            DataContext = restriction;
        }
    }
}