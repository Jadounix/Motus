using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Nettoyage_dico
{
    class Program
    {
        static void Main(string[] args)
        {
            
            try
            {
                int longueurMot = 10;

                //Création d'une instance de StreamReader pour permettre la lecture de notre fichier source 
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
                StreamReader monStreamReader = new StreamReader("dico_fr.txt", encoding);

                //Création d'une instance de StreamWriter pour permettre l'ecriture de notre fichier cible
                StreamWriter monStreamWriter = File.CreateText("dico_fr_reduit10.txt");

                string mot = monStreamReader.ReadLine();
                //Lecture de tous les mots du fichier (un par lignes) 
                while (mot != null)
                {
                    if (mot.Length == longueurMot)
                    {
                        monStreamWriter.WriteLine(mot);//On écrit dans le fichier cible les mots ayant la bonne longueur
                    }
                    mot = monStreamReader.ReadLine();
                }
                // Fermeture du StreamReader
                monStreamReader.Close();
                // Fermeture du StreamWriter
                monStreamWriter.Close();
            }

            catch (Exception ex)
            {
                // Code exécuté en cas d'exception 
                Console.Write("Une erreur est survenue au cours de l'opération :");
                Console.WriteLine(ex.Message);
            }



        }
    }
}
