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

namespace Proyecto2_Automatas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] alphabet;
        Dictionary<string, bool> alphabetMap;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            int x = 0;
            alphabetMap = new Dictionary<string, bool>();
            string[] alphabetBeforetrim = txtAlphabet.Text.Split(',');
            alphabet = new string[alphabetBeforetrim.Length];
            while (x < alphabetBeforetrim.Length)
            {
                string temp = alphabetBeforetrim[x].Trim();
                try
                {
                    if (alphabetMap[temp])
                    {
                        MessageBox.Show("No se aceptan estados repetidos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                catch (KeyNotFoundException ex)
                {
                    alphabetMap.Add(temp, true);
                    alphabet[x] = temp;
                }
                x++;
            }
            Console.WriteLine(alphabet[0]);
        }
    }
}
