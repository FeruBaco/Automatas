using System;
using System.Collections;
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
        Queue<NewStateHelper> qPendingStates;
        Queue<NewStateHelper> qGeneratedStates;
        ArrayList AFD;

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

            initMainStates();

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
            // Obtener información de la tabla
            getAFNDData();

            // Encolamos los estados que se generan a partir de la data inicial
            setPendingStates();

            // Genera el AFD equivalente
            generateAFD( );

            // Genera la tabla
            generateAFDGrid();
        }

        private void getAFNDData( )
        {
            primaryStateTransitions = new Dictionary<string, Dictionary<string, string[]>>(); // Inicialización del diccionario de la data inicial

            for (int i = 0; i < state.Length; i++)
            {
                Dictionary<string, string[]> tempDictionaryTransitions = new Dictionary<string, string[]>(); // Diccionario temporal
                primaryStateTransitions.Add(state[i], tempDictionaryTransitions); // Diccionario para guardar la data de la tabla

                for (int j = 0; j < alphabet.Length; j++)
                {
                    string tempStr = initTable.Rows[i].Field<string>(j + 1);
                    if (tempStr == null)
                    {
                        tempStr = "";
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
        }

        private void setPendingStates()
        {
            qPendingStates = new Queue<NewStateHelper>();
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
                        newStateHelper.baseStatesAsDict = new Dictionary<string, bool>();

                        foreach (string state in primaryStateTransitions[stateName][alphabetElement])
                        {
                            // Adding states dictionary
                            newStateHelper.baseStatesAsDict.Add(state, true);

                            if (statesClasification[state] == 2)
                            {
                                newStateHelper.isFinal = true;
                            }
                        }
                        qPendingStates.Enqueue(newStateHelper);

                    }
                }
            }
        }
        private void generateAFD( )
        {
            AFD = new ArrayList();

            while (qPendingStates.Count > 0)
            {
                NewStateHelper temp = qPendingStates.Dequeue();
                if( !newStateExists(temp) ) // Si el estado no existe
                {
                    generateStateTransitions(temp);
                }
            }

        }

        public void generateAFDGrid( )
        {
            // Se inicializa la tabla
            finalTable = new DataTable();

            // Insertamos columnas ----------------------------------------
            finalTable.Columns.Add("Estado", typeof(string));
            int i = 0;
            while (i < alphabet.Length)
            {
                finalTable.Columns.Add(alphabet[i], typeof(string));
                i++;
            }
            // ------------------------------------------------------------

            // Estado inicial ------------------------------
            DataRow initialRow;
            string firstStateName = state[0];
            initialRow = finalTable.NewRow();

            initialRow["Estado"] = "->" + firstStateName;

            foreach (string alphElem in alphabet)
            {
                initialRow[alphElem] = String.Join("", primaryStateTransitions[firstStateName][alphElem]);
            }

            finalTable.Rows.Add(initialRow);
            // ---------------------------------------------------

            destroyInnecessaryStates( );

            // Inserta nuevos estados
            foreach (NewState newState in AFD)
            {
                DataRow tableRow;
                tableRow = finalTable.NewRow();

                tableRow["Estado"] = (newState.isFinal ? "*" : "") + newState.Name;

                foreach(string alphElem in alphabet)
                {
                    tableRow[alphElem] = newState.transitions[alphElem];
                }

                finalTable.Rows.Add(tableRow);
            }

            FinalDataGrid.ItemsSource = finalTable.DefaultView;
        }

        private void destroyInnecessaryStates()
        {
            ArrayList statesToDestroy = new ArrayList();
            foreach(NewState finalState in AFD)
            {
                foreach(string alphElem in alphabet)
                {
                    Dictionary<string, string[]> dummyVal = new Dictionary<string, string[]>();
                    if(primaryStateTransitions.TryGetValue(finalState.transitions[alphElem], out dummyVal))
                    {
                        if(statesClasification[finalState.transitions[alphElem]] != 1) statesToDestroy.Add(finalState);
                    }
                }
            }

            foreach(NewState stateToDestroy in statesToDestroy)
            {
                AFD.Remove(stateToDestroy);
            }
        }

        public void generateStateTransitions(NewStateHelper newState)
        {
            NewState tempState = new NewState();
            
            tempState.Name = newState.Name;
            tempState.baseStates = newState.baseStates;
            tempState.baseStatesAsDict = newState.baseStatesAsDict;
            tempState.transitions = new Dictionary<string, string>();
            tempState.isFinal = false;

            for (int i = 0; i < alphabet.Length; i++)
            {
                NewStateHelper possibleNewState = new NewStateHelper();
                string alphabetElem = alphabet[i];
                string[] tempTransitions;
                ArrayList tempTransitionsAl = new ArrayList();

                Dictionary<string, bool> tempTransitionsDict = new Dictionary<string, bool>();

                for (int j = 0; j < newState.baseStates.Length; j++) // Itera sobre las transiciones que generan el nuevo estado
                {
                    string baseTransitionName = newState.baseStates[j];

                    if(baseTransitionName != "")
                    {
                        string[] baseTransitionTransitions = primaryStateTransitions[baseTransitionName][alphabetElem];

                        // Define si el nuevo estado es final
                        tempState.isFinal = statesClasification[baseTransitionName] == 2;

                        for (int k = 0; k < baseTransitionTransitions.Length; k++) // Itera sobre las transiciones de una transición dada que genera el nuevo estado
                        {
                            string transition = baseTransitionTransitions[k];
                            bool dummyVal = false;

                            if(transition != "") // Verifica que la transición no sea un espacio vacío
                            {
                                // Verificar si existe la transición en las transiciones usadas, sino la agregamos a la lista
                                if (!tempTransitionsDict.TryGetValue(transition, out dummyVal))
                                {
                                    tempTransitionsDict.Add(transition, true);
                                    tempTransitionsAl.Add(transition);
                                }
                            }
                        }
                    }

                }

                if(tempTransitionsAl.Count != 0)
                {
                    tempTransitions = convertArrayListToArray(tempTransitionsAl);
                    string newStateName = String.Join("", tempTransitions);
                    tempState.transitions.Add(alphabetElem, newStateName);

                    possibleNewState.Name = newStateName;
                    possibleNewState.baseStates = tempTransitions;
                    possibleNewState.baseStatesAsDict = tempTransitionsDict;

                    qPendingStates.Enqueue(possibleNewState);
                }
            }

            // Se agrega a la cola de estados generados
            qGeneratedStates.Enqueue(newState);

            // Se agrega el nuevo estado a la lista de nuevos estados
            AFD.Add(tempState);
        }

        private void initMainStates(  )
        {
            qGeneratedStates = new Queue<NewStateHelper>();

            foreach (string stateName in state)
            {
                NewStateHelper tempState = new NewStateHelper();
                string[] tempTransitions = new string[] { stateName };

                tempState.Name = stateName;
                tempState.baseStates = tempTransitions;
                tempState.isFinal = statesClasification[stateName] == 2;
                
                // Add states to dictionary
                tempState.baseStatesAsDict = new Dictionary<string, bool>();
                tempState.baseStatesAsDict.Add(stateName, true);

                qGeneratedStates.Enqueue(tempState);
            }
        }

        private bool newStateExists( NewStateHelper preState )
        {
            foreach (NewStateHelper stateGenerated in qGeneratedStates) 
            {
                if (preState.baseStates.Length == stateGenerated.baseStates.Length)
                {
                    int i = preState.baseStates.Length;
                    foreach(string baseState in preState.baseStates)
                    {
                        if(stateGenerated.isTransitionUsedForStateCreation(baseState)) // Verifica si la transición del nuevo estado existe en las transiciones que se usaron como base para el estado ya existente
                        {
                            i--;
                        }
                    }
                    if (i == 0) return true;
                }
            }

            return false;
        }


        private string[] convertArrayListToArray(ArrayList arrayList)
        {
            string[] newArray = new string[arrayList.Count];
            int i = 0;

            foreach(string item in arrayList)
            {
                newArray[i] = item;
                i++;
            }

            return newArray;
        }
    }

    public class NewStateHelper
    {
        public string Name { get; set; }
        public string[] baseStates { get; set; }
        public bool isFinal { get; set; }
        public Dictionary<string, bool> baseStatesAsDict { get; set; }

        public bool isTransitionUsedForStateCreation(string baseTransition)
        {
            bool val = false;
            return this.baseStatesAsDict.TryGetValue(baseTransition, out val);
        }
    }

    public class NewState : NewStateHelper
    {
        public Dictionary<string, string> transitions;
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
