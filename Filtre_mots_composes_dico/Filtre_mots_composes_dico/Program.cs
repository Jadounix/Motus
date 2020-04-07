using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Filtre_mots_composes_dico
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Création d'une instance de StreamReader pour permettre la lecture de notre fichier source
                System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
                StreamReader monStreamReader = new StreamReader("dico.txt", encoding);

                //Création d'une instance de StreamWriter pour permettre l'ecriture de notre fichier cible
                StreamWriter monStreamWriter = File.CreateText("dico_filtre.txt");

                string mot = monStreamReader.ReadLine();
                //Lecture de tous les mots du fichier (un par ligne)
                while (mot != null)
                {
                    if (mot.Contains("-")) //si le mot contient le caractère recherché
                    {
                        //ne rien faire
                    }

                    else
                    {
                        monStreamWriter.WriteLine(mot); //On écrit dans le fichier cible le mot non composé
                    }

                    mot = monStreamReader.ReadLine(); //passe à la ligne suivante
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
