using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace V1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue au jeu MOTUS");
            Console.WriteLine("=================================");
            Console.WriteLine("Règles : \n");
            Console.WriteLine("Le jeu Motus consiste à deviner un mot avec pour seul indice sa première lettre (pas de verbe conjugué, \nde mot composé ou de nom propre)." +
            "Vous disposez d’un nombre limité de tentatives afin de trouver le mot, \ndont vous aurez choisi la longueur au préalable." +
            "Vous commencez par proposer un mot, puis vous devez déduire les lettres qui le composent grâce au code couleur suivant : \n" +
            "Une lettre colorée en ROUGE signifie que celle - ci est bien placée\n" +
            "Une lettre colorée en JAUNE signifie qu’elle est présente dans le mot, mais qu’elle n’est pas bien placée\n" +
            "Une lettre colorée en BLEU signifie qu’elle ne fait pas partie du mot.\n");
            Console.WriteLine("Amusez vous bien ! ");
            Console.WriteLine("=================================");


            //Variable qui prend la valeur true si l'utilisateur a trouvé le bon mot
            bool trouvé = false;

            //Variable qui permet à l'utilisateur de pouvoir rejouer ou non
            bool rejouer = false;

            //Variable qui permet à l'utilisateur de pouvoir voir l'historique ou non
            bool voirHistorique = false;

            Console.WriteLine("HISOTIQUE DES SCORES : 1");
            Console.WriteLine("COMMENCER LE JEU : 2");

            //Variable qui prend la valeur 1 si l'utilisateur veut consulter l'historique et 2 s'il veut jouer
            int menu = int.Parse(Console.ReadLine());

            if (menu == 1)
            {
                voirHistorique = true;
            }
            else
            {
                if (menu == 2)
                {
                    rejouer = true;
                }
                else
                {
                    Console.WriteLine("Merci de taper 1 ou 2");
                }
            }

            while (rejouer == true) //Tant que rejouer est vraie on continue à jouer
            {
                //Initialisation de la difficulté du jeu
                //On recupere le nb de tentatives qui correspond au nb de lignes, et le nb de lettres dans le mot qui correpond au nb de colonnes
                int nbLettres = InitialiserNombreLettres();
                int nbTentatives = InitialiserNombreTentatives();

                int nbLignes = nbTentatives;
                int nbColonnes = nbLettres;

                //Génération du mot à trouver à partir d'un dictionnaire
                string motAdeviner = GenererMot(nbLettres);

                //Numéro de la ligne en cours de remplissage dans le tableau
                int ligneAremplir = 0;

                //Création d'un tableau vide de la bonne taille, qui servira pendant le jeu
                char[,] tableauJeu = CreerTableau(nbLignes, nbColonnes);

                trouvé = false;

                Console.WriteLine("=================================");
                Console.WriteLine("C'est parti !");
                Console.WriteLine("=================================");

                //Initialisation du chronomètre
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                //Affichage de la première lettre
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(motAdeviner.ToUpper()[0]); //Affichage de la lettre en question
                for (int j = 0; j < nbColonnes; j++)
                {
                    Console.Write("| "); //On ajoute les espaces
                }
                Console.ResetColor(); //On reinitialise la couleur
                Console.WriteLine();

                //Avancement du jeu
                while (nbTentatives > 0 && trouvé == false) //Tant qu'il reste des tentatives et que le mot n'a pas été trouvé on continue à faire tourner le jeu
                {
                    //A chaque tour l'utilisateur entre un mot
                    Console.WriteLine("Entrez un mot de " + nbColonnes + " lettres");
                    string motDonné = Console.ReadLine();
                    while (motDonné.Length != nbColonnes)
                    {
                        Console.WriteLine("Le mot que vous avez donné ne contient pas le bon nombre de lettres.");
                        motDonné = Console.ReadLine();
                    }

                    //On remplit la ligne en question avec le mot que l'utilisateur vient d'entrer
                    RemplirLigne(tableauJeu, ligneAremplir, motDonné.ToUpper()); //Le ToUpper permet de mettre le mot en majuscule dans le tableau
                    AfficherTableau(tableauJeu, motAdeviner); //Affiche le tableau et met les bonnes couleurs en fonction du mot

                    trouvé = VerifierMot(tableauJeu, motAdeviner);

                    nbTentatives--; //On enlève une tentative à chaque tour
                    ligneAremplir++;//On passe à la ligne suivante dans le tableau
                }

                //Fin du chronomètre
                stopWatch.Stop();
                //Intervalle de temps du chronomètre
                TimeSpan ts = stopWatch.Elapsed;

                //Affichage du temps de jeu
                string elapsedTime = String.Format("{0:00} heure(s) {1:00} minute(s) {2:00} seconde(s)", ts.Hours, ts.Minutes, ts.Seconds);
                Console.WriteLine("La durée de la partie est de " + elapsedTime);

                //Affichage des résultats
                if (trouvé == true)
                {
                    Console.WriteLine("Vous avez gagné, Félicitations !");

                    //On rentre le résultat dans l'historique des parties
                    Console.WriteLine("Entrez votre nom pour sauver votre résultat");
                    string nom = Console.ReadLine();
                    string score = nom + " : " + elapsedTime;

                    //Entre le score dans le fichier texte
                    //le fichier texte est créé au même moment
                    //même si la commande de création est exécutée à chaque itération, StreamWriter complète le fichier avec le même nom s'il existe déjà
                    using (System.IO.StreamWriter monStreamWriter = new System.IO.StreamWriter("Historique_score.txt", true))
                    {
                        monStreamWriter.WriteLine(score);
                        monStreamWriter.Close();
                    }
                  
                    Console.WriteLine("Pour rejouer à ce fantastique jeu tapez 1, sinon tapez 0");
                    rejouer = Convert.ToBoolean(int.Parse(Console.ReadLine())); //convertit le chiffre entré en un booléen
                    if (rejouer == false)
                    {
                        Console.WriteLine("Merci d'avoir joué, à bientôt sur notre antenne !");
                    }
                }
                else
                {
                    Console.WriteLine("Vous avez perdu... Pour rejouer à cet incroyable jeu tapez 1, sinon tapez 0");
                    Console.WriteLine("Le mot à trouver était " + motAdeviner + " !");
                    rejouer = Convert.ToBoolean(int.Parse(Console.ReadLine()));

                    if (rejouer == false)
                    {
                        Console.WriteLine("Merci d'avoir joué, à bientôt sur notre antenne !");
                    }
                }
            }

            //Affichage des scores
            if (voirHistorique == true)
            {
                Console.WriteLine("=================================");
                Console.WriteLine(" - Scores - ");
                AfficherHistorique();
            }

            Console.ReadLine();

        }

        //Fonction qui demande à l'utilisateur le nombre de lettres du mot à deviner
        public static int InitialiserNombreLettres()
        {
            int nbLettres = 0;

            Console.Write("Entrez le nombre de lettres (de 6 à 10) du mot à deviner : \n"); //choix du nb de lettres du mot
            nbLettres = int.Parse(Console.ReadLine());
            while (nbLettres < 6 || nbLettres > 10) //correction si le nombre de lettres n'est pas respecté
            {
                Console.Write("Mauvaise longueur. Entrez un nombre de lettres entre 6 et 10. \n");
                nbLettres = int.Parse(Console.ReadLine());
            }

            return (nbLettres);
        }

        //Fonction qui demande à l'utilisateur le nombre de tentatives
        public static int InitialiserNombreTentatives()
        {
            int nbTentatives = 0;

            Console.Write("Entrez un nombre de tentatives (de 5 à 10) : \n"); //choix du nb de tentatives pour trouver le mot
            nbTentatives = int.Parse(Console.ReadLine());
            while (nbTentatives < 5 || nbTentatives > 10) //correction si le nombre de tentatives n'est pas respecté
            {
                Console.Write("Mauvaise longueur. Entrez un nombre de tentatives entre 5 et 10 \n");
                nbTentatives = int.Parse(Console.ReadLine());
            }

            return (nbTentatives);
        }

        //Fonction qui remplit une ligne du tableau avec le mot tapé par l'utilisateur
        public static void RemplirLigne(char[,] tab, int ligneAremplir, string motAmettreDansLigne)
        {
            int nbColonnes = tab.GetLength(1);

            for (int k = 0; k < nbColonnes; k++)
            {
                tab[ligneAremplir, k] = motAmettreDansLigne[k];
            }
        }

        //Fonction qui crée un tableau vide pouvant contenir des char
        public static char[,] CreerTableau(int nbLignes, int nbColonnes)
        {
            char[,] tableau = new char[nbLignes, nbColonnes];
            return (tableau);
        }

        //Fonction qui génère un mot aléatoire
        public static string GenererMot(int nbLettres)
        {
            int[] TabNbLettres = new int[] { 6, 7, 8, 9, 10 }; //Tableau avec tous tous les nombres de lettres possibles dans le mot à deviner
            int[] TabNbLettresDico = new int[] { 15727, 27899, 40682, 49278, 50987 }; //Tableau avec les longueur de chaque sous dico qui contiennent chaucun le bon nombre de lettres
            int nombresMotDico = 100; //Nombre de mot dans le dictionnaire choisi (par exemple le dictionnaire des 6 lettres)

            //On veut avoir le bon nombre de lettres dans le dictionnaire choisi afin de choisir un nombre aléatoire dans l'intervalle du nombre de lettres du dictionnaire
            for (int i = 0; i < TabNbLettres.Length; i++)
            {
                if (nbLettres == TabNbLettres[i])
                {
                    nombresMotDico = TabNbLettresDico[i];
                }
            }
            //Création d'un nombre random qui correspondra à la ligne du dico dans laquelle on va chercher le mot
            Random rnd = new Random();
            int rangDansLeDico = rnd.Next(nombresMotDico);

            // Création d'une instance de StreamReader pour permettre la lecture du dictionnaire
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader monStreamReader = new StreamReader("dico_fr_reduit" + nbLettres + ".txt", encoding);

            string mot = monStreamReader.ReadLine();
            //Choix du mot correspondant au rang généré aléatoirement
            while (rangDansLeDico > 0)
            {
                mot = monStreamReader.ReadLine();
                rangDansLeDico--;
            }
            //Fermeture du StreamReader 
            monStreamReader.Close();
            return mot;
        }

        //Fonction qui affiche un tableau quelconque à 2 dimensions et qui gère la couleur
        public static void AfficherTableau(char[,] tab, string motAdeviner)
        {
            int nbLignes = tab.GetLength(0); //Nombre de lignes du tableau
            int nbColonnes = tab.GetLength(1); //Nombre de colonnes du tableau

            for (int i = 0; i < nbLignes; i++)
            {
                for (int j = 0; j < nbColonnes; j++)
                {
                    //Si la lettre proposée par l'utilisateur appartient au mot mais n'est pas à la bonne place
                    for (int k = 0; k < motAdeviner.Length; k++)
                    {
                        if (tab[i, j] == motAdeviner.ToUpper()[k])
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                    }
                    //Si la lettre proposée par l'utilisateur est exactement à la bonne place
                    if (tab[i, j] == motAdeviner.ToUpper()[j])
                    {
                        Console.BackgroundColor = ConsoleColor.Red; //Le fond de la couleur devient rouge
                    }
                    //Si la lettre proposée par l'utilisateur ne fait pas parti du mot
                    else
                    {
                        if (Console.BackgroundColor != ConsoleColor.Yellow) //Le fond n'est pas jaune, donc la lettre ne fait pas partie du mot
                        {
                            Console.BackgroundColor = ConsoleColor.Blue; //Le fond de la couleur devient bleu
                        }
                    }
                    Console.Write(tab[i, j] + "|"); //Affichage de la lettre en question
                    Console.ResetColor(); //On reinitialise la couleur pour la lettre suivante
                }
                Console.Write("\n");

                for (int k = 0; k < nbColonnes; k++)
                {
                    Console.Write("__"); //Permet de séparer les lignes du tableau
                }

                Console.Write("\n");
            }
        }

        //Fonction qui renvoie true si le bon mot a été trouvé
        public static bool VerifierMot(char[,] tab, string motAdeviner)
        {
            int nbLignes = tab.GetLength(0); //Nombre de lignes du tableau
            int nbColonnes = tab.GetLength(1); //Nombre de colonnes du tableau

            int cpt = 0;

            bool check = false; //Variable boleenne qui va servir à vérifier si le mot entré par l'utilisateur est juste

            for (int i = 0; i < nbLignes; i++)
            {
                for (int j = 0; j < nbColonnes; j++)
                {
                    if (tab[i, j] == motAdeviner.ToUpper()[j])
                    {
                        cpt++;
                    }
                }
                if (cpt == motAdeviner.Length)
                {
                    check = true;
                }
                cpt = 0;
            }
            return check;
        }

        //Fonction qui affiche l'historique des scores depuis un fichier texte
        public static void AfficherHistorique()
        {
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            StreamReader monStreamReader = new StreamReader("Historique_score.txt", encoding);

            string score = monStreamReader.ReadLine();

            //Lecture de tous les mots du fichier (un par ligne)
            while (score != null)
            {
                Console.WriteLine(score); //On affiche le score contenu dans une ligne
                score = monStreamReader.ReadLine(); //On passe à la ligne suivante
            }

            // Fermeture du StreamReader
            monStreamReader.Close();
        }
    }
}
