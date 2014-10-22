using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wizard {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public int progress = 0;
        public List<List<FrameworkElement>> Elements = new List<List<FrameworkElement>>();
        public MainWindow() {
            InitializeComponent();
            Elements.Add(new List<FrameworkElement> { GNUAccept, GNUAText, GNUScroll, GNUText });
            Elements.Add(new List<FrameworkElement> { SPAccept, SPAText, SPScroll, SPText });
            Elements.Add(new List<FrameworkElement> { userTxt, passTxt, loginBtn });
            Elements.Add(new List<FrameworkElement> { BarNameTxt, OtherInfoTxt, OtherInfo2Txt });
            Elements.Add(new List<FrameworkElement> { searchResultsLst, searchBarTxt, AlbumsBtn, Artistsbtn, tracksBtn });
            Elements.Add(new List<FrameworkElement> { DoneTxt });
            searchResultsLst.Items.Add("Carry On My Wayward Son");
            searchResultsLst.Items.Add("Welcome To The Jungle");
            searchResultsLst.Items.Add("Buffalo Soldier");
            searchResultsLst.Items.Add("Smack My Bitch Up");
            LoadNext();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e) {
            if(CheckPrerequisites()) {
                progress++;
                LoadNext();
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e) {
            progress--;
            if(CheckPrerequisites()) {
                if(progress == Elements.Count - 2)
                    NextButton.Content = "Next";
                LoadNext();
            }
            else
                progress++;
        }

        private bool CheckPrerequisites() {
            switch(progress) {
                case 0:
                    return GNUAccept.IsChecked.Value;
                case 1:
                    return SPAccept.IsChecked.Value;
                case 2:
                    return (userTxt.Text == "admin" && passTxt.Password == "admin");
                case 3:
                    return (BarNameTxt.Text != "" && OtherInfoTxt.Text != "" && OtherInfo2Txt.Text != "");
                case 4:
                    return true;
                default:
                    return false;
            }
        }

        private void LoadNext() {
            foreach(List<FrameworkElement> l in Elements) {
                foreach(FrameworkElement f in l) {
                    f.Visibility = System.Windows.Visibility.Hidden;
                }
            }
            if(progress < Elements.Count && progress >= 0)
                foreach(FrameworkElement f in Elements[progress]) {
                    f.Visibility = System.Windows.Visibility.Visible;
                }
            if(progress == 0)
                BackButton.Visibility = System.Windows.Visibility.Hidden;
            else {
                BackButton.Visibility = System.Windows.Visibility.Visible;
                if(progress == Elements.Count-1)
                    NextButton.Content = "Done";
            }

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e) {
            NextButton_Click(sender, e);
        }

        private void clearOnFocus(object sender, RoutedEventArgs e) {
            TextBox tb = (TextBox)sender;
            tb.Text = "";
        }

    }
}
