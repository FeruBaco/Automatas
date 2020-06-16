using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatas
{
    class Program
    {
        static void Main(string[] args)
        {
            #region InitProgram
            Console.WriteLine("Ingrese numero de palabras(maximo 3)");
            int numberWords = Convert.ToInt32(Console.ReadLine());
            CheckNumber(numberWords, 3);

            string[] words = new string[numberWords];
            //Insert words
            for (int i = 0; i < numberWords; i++)
            {
                Console.WriteLine("Ingrese " + (i + 1) + "° palabra");
                words[i] = Console.ReadLine();
            }

            //Insert level of Kleene
            Console.WriteLine("Ingrese nivel de clausura de Kleene(maximo 10)");
            int kleeneLevel = Convert.ToInt32(Console.ReadLine());
            CheckNumber(kleeneLevel, 10);

            //Show words
            Console.WriteLine("Palabras ingresadas");
            for (int i = 0; i < numberWords; i++)
            {
                Console.Write(words[i] + " ");
            }
            #endregion

            KleeneOperation(words, kleeneLevel, words);

            Console.ReadLine();
        }

        public static void KleeneOperation(string[] words, int level, string[] newL)
        {
            if (level == 0)
            {
                Console.WriteLine("{ }");
                Console.ReadLine();
                Environment.Exit(0);
            }
            else if (level == 1)
            {
                Console.Write("\n{ ");
                for (int i = 0; i < words.Length; i++)
                {
                    Console.Write(words[i]);
                    if (i == words.Length - 1)
                        break;
                    Console.Write(",");
                }
                Console.Write(" }");
                Console.ReadLine();
                Environment.Exit(0);
            }

            string[] words2 = newL;
            double comb = newL.Length * words.Length;
            string[] final = new string[Convert.ToInt32(comb)];

            for (int i = 0; i < words.Length; i++)
            {
                for (int t = 0; t < words2.Length; t++)
                {
                    int pos = (i * words2.Length) + t;
                    final[pos] = words[i] + "" + words2[t];
                }
            }

                if (words[words.Length - 1].Length * level == final[final.Length - 1].Length)
                {
                    Console.Write("\n{ ");
                    for (int i = 0; i < comb; i++)
                    {
                        Console.Write(final[i]);
                        if (i == comb - 1)
                            break;
                        Console.Write(",");
                    }
                    Console.Write(" }");
                    Console.ReadLine();
                    Environment.Exit(0);
                return;
                }
                KleeneOperation(words, level, final);
            }

        public static void CheckNumber(int number, int maxNumber)
        {
            if (number > maxNumber)
            {
                Console.Write("Ingrese un numero valido menor igual a " + maxNumber);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

    }
}
