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
        Dictionary<string, Dictionary<string, string[]>> primaryStateTransitions;
        DataTable initTable, finalTable;
        Queue<NewStateHelper> createdStates;
        Queue<string> qInitialStates;

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
            createdStates = new Queue<NewStateHelper>();
            qInitialStates = new Queue<string>();
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
                qInitialStates.Enqueue(state[i]);
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
        private void GenerateAfd_Click(object sender, RoutedEventArgs e)
        {
            InitAfd();
        }

        private void InitAfd()
        {
            primaryStateTransitions = new Dictionary<string, Dictionary<string, string[]>>();
            for (int i = 0; i < state.Length; i++)
            {
                Dictionary<string, string[]> tempDictionaryTransitions = new Dictionary<string, string[]>();
                primaryStateTransitions.Add(state[i], tempDictionaryTransitions);
                for (int j = 0; j < alphabet.Length; j++)
                {
                    string tempStr = initTable.Rows[i].Field<string>(j + 1);
                    if (tempStr == null)
                    {
                        tempStr = "-";
                    }
                    string[] tempStrSplit = tempStr.Split(',');
                    string[] tempTransitions = new string[tempStrSplit.Length];
                    for (int k = 0; k < tempStrSplit.Length; k++)
                    {
                        tempTransitions[k] = tempStrSplit[k].Trim();
                    }
                    primaryStateTransitions[state[i]].Add(alphabet[j], tempTransitions);
                }
            }
            Queue<NewStateHelper> qPendingStates = new Queue<NewStateHelper>();
            foreach (string stateName in qInitialStates)
            {
                foreach (string alphabetElement in alphabet)
                {
                    if (primaryStateTransitions[stateName][alphabetElement].Length > 1)
                    {
                        NewStateHelper newStateHelper = new NewStateHelper();
                        newStateHelper.Name = String.Join("", primaryStateTransitions[stateName][alphabetElement]);
                        newStateHelper.baseStates = primaryStateTransitions[stateName][alphabetElement];
                        newStateHelper.isFinal = false;
                        foreach (string state in primaryStateTransitions[stateName][alphabetElement])
                        {
                            if (statesClasification[state] == 2)
                            {
                                newStateHelper.isFinal = true;
                            }
                        }
                        qPendingStates.Enqueue(newStateHelper);
                    }
                }
            }
            insertData(qPendingStates);
        }

        private void insertData(Queue<NewStateHelper> pendingStates)
        {
            finalTable.Columns.Add("Estado", typeof(string));
            int i = 0, actualRow = 0;
            while (i < alphabet.Length)
            {
                finalTable.Columns.Add(alphabet[i], typeof(string));
                i++;
            }
            while (pendingStates.Count > 0)
            {
                NewStateHelper temp = pendingStates.Dequeue();
                foreach (string baseState in temp.baseStates)
                {
                    foreach (string alphabetElement in alphabet)
                    {
                        string[] temporalTransitionsStates = primaryStateTransitions[baseState][alphabetElement];
                        string newTransition = "";
                        foreach (string item in temporalTransitionsStates)
                        {
                            if (newTransition.IndexOf(item) == -1)
                            {
                                newTransition += item;
                            }
                        }
                    }
                }
            }
        }
    }

    public class NewStateHelper
    {
        public string Name { get; set; }
        public string[] baseStates { get; set; }
        public bool isFinal { get; set; }
    }

    //public class Transtition
    //{
    //    public string Name { get; set; }
    //    public string[] Transitions { get; set; }
    //}

    //public class NewState
    //{
    //    public string Name { get; set; }
    //    public Transition[] MyProperty { get; set; }
    //}
}
