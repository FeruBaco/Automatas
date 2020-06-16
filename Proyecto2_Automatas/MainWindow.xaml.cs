using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
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
        public string[] alphabet, state, finalState;
        public string initialState;
        Dictionary<string, int> statesClasification;

        DataTable initTable;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            initialState = txtInitialState.Text.Trim();
            InitializeHeaders();
            GenerateGrid();
        }

        private void InitializeHeaders()
        {
            int x = 0;
            statesClasification = new Dictionary<string, int>();
            //State
            string[] stateBeforeTrim = txtStates.Text.Split(',');
            state = new string[stateBeforeTrim.Length];
            while (x < stateBeforeTrim.Length)
            {
                string temp = stateBeforeTrim[x].Trim();
                state[x] = temp;
                statesClasification.Add(temp, 0);
                x++;
            }
            //Alphabet
            x = 0;
            string[] alphabetBeforetrim = txtAlphabet.Text.Split(',');
            alphabet = new string[alphabetBeforetrim.Length];
            while (x < alphabetBeforetrim.Length)
            {
                string temp = alphabetBeforetrim[x].Trim();
                alphabet[x] = temp;
                x++;
            }
            //FinalStates
            x = 0;
            string[] finalStateBeforeTrim = txtFinalStates.Text.Split(',');
            finalState = new string[finalStateBeforeTrim.Length];
            while (x < finalStateBeforeTrim.Length)
            {
                string temp = finalStateBeforeTrim[x].Trim();
                finalState[x] = temp;
                statesClasification[temp] = 2;
                x++;
            }
            //Set initial State
            statesClasification[initialState] = 1;
        }
        private string[] stateOrder(int lng)
        {
            string[] fakeArray = new string[lng];
            int i = 0, actualPosition = 1;
            fakeArray[0] = initialState;
            while (i < state.Length)
            {
                if (statesClasification[state[i]] == 0)
                {
                    fakeArray[actualPosition] = state[i];
                    actualPosition++;
                }
                i++;
            }
            i = 0;
            while (i < state.Length)
            {
                if (statesClasification[state[i]] == 2)
                {
                    fakeArray[actualPosition] = state[i];
                    actualPosition++;
                }
                i++;
            }
            return fakeArray;
        }
        private void GenerateGrid()
        {
            initTable = new DataTable();
            initTable.Columns.Add("Estado", typeof(string));
            int i = 0;
            while (i < alphabet.Length)
            {
                initTable.Columns.Add(alphabet[i], typeof(string));
                i++;
            }
            i = 0;
            state = stateOrder(state.Length);
            while (i < state.Length)
            {
                DataRow tableRow;
                tableRow = initTable.NewRow();
                switch (statesClasification[state[i]])
                {
                    case 1:
                        tableRow["Estado"] = "->" + state[i];
                        break;
                    case 2:
                        tableRow["Estado"] = "*" + state[i];
                        break;
                    default:
                        tableRow["Estado"] = state[i];
                        break;
                }
                int x = 1;
                while (x < alphabet.Length)
                {
                    tableRow[alphabet[x - 1]] = "";
                    x++;
                }
                initTable.Rows.Add(tableRow);
                i++;
            }

            InitialDataGrid.ItemsSource = initTable.DefaultView;
        }
    }
}
