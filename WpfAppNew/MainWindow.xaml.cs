using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

using genetics;

namespace WpfAppNew
{
    public partial class MainWindow : Window
    {
        private Evolution square = null!;
        private int a = 10, b = 5, c = 5, n = 0;
        private int multiplier = 20;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LetsGo(object sender, RoutedEventArgs e)
        {
            a = int.Parse(box1.Text);
            b = int.Parse(box2.Text);
            c = int.Parse(box3.Text);
            n = a + b + c;

            square = new Evolution(a, b, c);
            ResultBlock.Text = $"{square.MakeString()}";
        }
        private void Continue(object sender, RoutedEventArgs e)
        {

            square.Iteration();
            ResultBlock.Text = $"{square.MakeString()}";

            Nice.Children.Clear();

            for (int i = 0; i < a; i++)
            {
                Line a = new Line();
                a.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a.X2 = square.Population[0].Genotype_X[i] * multiplier + multiplier;
                a.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a.Y2 = square.Population[0].Genotype_Y[i] * multiplier;
                a.Stroke = Brushes.Black;
                Nice.Children.Add(a);

                Line a2 = new Line();
                a2.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a2.X2 = square.Population[0].Genotype_X[i] * multiplier;
                a2.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a2.Y2 = square.Population[0].Genotype_Y[i] * multiplier + multiplier;
                a2.Stroke = Brushes.Black;
                Nice.Children.Add(a2);

                Line a3 = new Line();
                a3.X1 = square.Population[0].Genotype_X[i] * multiplier + multiplier;
                a3.X2 = square.Population[0].Genotype_X[i] * multiplier + multiplier;
                a3.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a3.Y2 = square.Population[0].Genotype_Y[i] * multiplier + multiplier;
                a3.Stroke = Brushes.Black;
                Nice.Children.Add(a3);

                Line a4 = new Line();
                a4.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a4.X2 = square.Population[0].Genotype_X[i] * multiplier + multiplier;
                a4.Y1 = square.Population[0].Genotype_Y[i] * multiplier + multiplier;
                a4.Y2 = square.Population[0].Genotype_Y[i] * multiplier + multiplier;
                a4.Stroke = Brushes.Black;
                Nice.Children.Add(a4);
            }
            for (int i = a; i < a+b; i++)
            {
                Line a = new Line();
                a.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a.X2 = square.Population[0].Genotype_X[i] * multiplier + 2 * multiplier;
                a.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a.Y2 = square.Population[0].Genotype_Y[i] * multiplier;
                a.Stroke = Brushes.Black;
                Nice.Children.Add(a);

                Line a2 = new Line();
                a2.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a2.X2 = square.Population[0].Genotype_X[i] * multiplier;
                a2.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a2.Y2 = square.Population[0].Genotype_Y[i] * multiplier + 2 * multiplier;
                a2.Stroke = Brushes.Black;
                Nice.Children.Add(a2);

                Line a3 = new Line();
                a3.X1 = square.Population[0].Genotype_X[i] * multiplier + 2 * multiplier;
                a3.X2 = square.Population[0].Genotype_X[i] * multiplier + 2 * multiplier;
                a3.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a3.Y2 = square.Population[0].Genotype_Y[i] * multiplier + 2 * multiplier;
                a3.Stroke = Brushes.Black;
                Nice.Children.Add(a3);

                Line a4 = new Line();
                a4.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a4.X2 = square.Population[0].Genotype_X[i] * multiplier + 2 * multiplier;
                a4.Y1 = square.Population[0].Genotype_Y[i] * multiplier + 2 * multiplier;
                a4.Y2 = square.Population[0].Genotype_Y[i] * multiplier + 2 * multiplier;
                a4.Stroke = Brushes.Black;
                Nice.Children.Add(a4);
            }
            for (int i = a + b; i < n; i++)
            {
                Line a = new Line();
                a.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a.X2 = square.Population[0].Genotype_X[i] * multiplier + 3 * multiplier;
                a.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a.Y2 = square.Population[0].Genotype_Y[i] * multiplier;
                a.Stroke = Brushes.Black;
                Nice.Children.Add(a);

                Line a2 = new Line();
                a2.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a2.X2 = square.Population[0].Genotype_X[i] * multiplier;
                a2.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a2.Y2 = square.Population[0].Genotype_Y[i] * multiplier + 3 * multiplier;
                a2.Stroke = Brushes.Black;
                Nice.Children.Add(a2);

                Line a3 = new Line();
                a3.X1 = square.Population[0].Genotype_X[i] * multiplier + 3 * multiplier;
                a3.X2 = square.Population[0].Genotype_X[i] * multiplier + 3 * multiplier;
                a3.Y1 = square.Population[0].Genotype_Y[i] * multiplier;
                a3.Y2 = square.Population[0].Genotype_Y[i] * multiplier + 3 * multiplier;
                a3.Stroke = Brushes.Black;
                Nice.Children.Add(a3);

                Line a4 = new Line();
                a4.X1 = square.Population[0].Genotype_X[i] * multiplier;
                a4.X2 = square.Population[0].Genotype_X[i] * multiplier + 3 * multiplier;
                a4.Y1 = square.Population[0].Genotype_Y[i] * multiplier + 3 * multiplier;
                a4.Y2 = square.Population[0].Genotype_Y[i] * multiplier + 3 * multiplier;
                a4.Stroke = Brushes.Black;
                Nice.Children.Add(a4);
            }
        }
    }
}