using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Experiments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ExperimentsWindow : Window
    {
        public string? TheExperiment { get; private set; }
        public ExperimentsWindow(string[] list_of_experiments)
        {
            InitializeComponent();
            Experiments_List.ItemsSource = list_of_experiments;
        }
        private void Ok_Button(object sender, RoutedEventArgs e)
        {
            if (Experiments_List.SelectedItem != null)
            {
                TheExperiment = Experiments_List.SelectedItem.ToString();
                DialogResult = true;
            }
        }
        private void Cancel_Button(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}