using System;
using System.Text;
using System.Threading;
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
using Experiments;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WpfAppNew
{
    public class Experiment
    {
        public string Id { get; set; } = null!;
        public int side_one { get; set; }
        public int side_two { get; set; }
        public int side_three { get; set; }
        public int[] Survival { get; set; } = null!;
        public string Genotype_X { get; set; } = null!;
        public string Genotype_Y { get; set; } = null!;
        public int iter_num { get; set; }

    }
    class LaboratoryContext : DbContext
    {
        public DbSet<Experiment> Experiments { get; set; }
        public LaboratoryContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder o)
        {
            o.UseSqlite("Data Source=laboratory.db");
        }
    }

    public partial class MainWindow : Window
    {
        private Evolution square = null!;
        private int a = 10, b = 5, c = 5, n = 0;
        private int multiplier = 20;
        private CancellationTokenSource cancell = null!;
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
        private async void Run(object sender, RoutedEventArgs e)
        {
            cancell = new CancellationTokenSource();
            var token = cancell.Token;
            await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    Continue();
                    Thread.Sleep(300);
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        private void SaveExperiment(object sender, RoutedEventArgs e)
        {
            if (square != null)
            {
                try
                {
                    string name;
                    name = exp_name.Text;
                    if (name.Length > 0)
                    {
                        using (LaboratoryContext db = new LaboratoryContext())
                        {
                            int pn = square.Population_numbers;             //количество особей в популяции
                            var Survival_curr = new int[pn];

                            var Genotype_X_new = new string[pn];
                            var Genotype_Y_new = new string[pn];

                            for (int i = 0; i < pn; i++)
                            {
                                Survival_curr[i] = square.Population[i].Survival;
                                Genotype_X_new[i] = string.Join(";", square.Population[i].Genotype_X);
                                Genotype_Y_new[i] = string.Join(";", square.Population[i].Genotype_Y);
                            }
                            string Genotype_X_all = string.Join(" ", Genotype_X_new);
                            string Genotype_Y_all = string.Join(" ", Genotype_Y_new);

                            Experiment new_exp = new Experiment
                            {
                                Id = name,
                                side_one = a,
                                side_two = b,
                                side_three = c,
                                Survival = Survival_curr,
                                Genotype_X = Genotype_X_all,
                                Genotype_Y = Genotype_Y_all,
                                iter_num = square.iter_num
                            };
                            db.Experiments.Add(new_exp);
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception exc) 
                {
                    MessageBox.Show("This name is taken. Enter another one");
                }
            }
        }
        private void StartNewEvolution(Experiment exp)
        {
            a = exp.side_one;
            b = exp.side_two;
            c = exp.side_three;

            box1.Text = a.ToString();
            box2.Text = b.ToString();
            box3.Text = c.ToString();
            exp_name.Text = exp.Id;

            square = new Evolution(a, b, c, exp.iter_num, exp.Genotype_X, exp.Genotype_Y);
            ResultBlock.Text = $"{square.MakeString()}";
        }
        private void GetExperiment(object sender, RoutedEventArgs e)
        {
            using (LaboratoryContext db = new LaboratoryContext())
            {
                db.Experiments.Load();
                var exps = db.Experiments.ToList();
                List<string> exp_names = new List<string>();
                foreach (Experiment ex in exps)
                {
                    exp_names.Add(ex.Id);
                }
                string[] list_of_experiments = exp_names.ToArray();
                ExperimentsWindow littleWindow = new ExperimentsWindow(list_of_experiments);
                if (littleWindow.ShowDialog() == true)
                {
                    string exp_id= littleWindow.TheExperiment;
                    var exp = db.Experiments.Find(exp_id);
                    StartNewEvolution(exp);
                }
            }
        }
        private void StopRunning(object sender, RoutedEventArgs e)
        {
            if (cancell != null)
            {
                cancell.Cancel();
            }
        }
        private void Continue()
        {

            square.Iteration();
            Dispatcher.Invoke(() =>
            {
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
            });
        }
    }
}