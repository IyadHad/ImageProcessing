using Projet;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;

namespace Projet
{
    public class Jeu
    {
        private Dictionaire dicoFrancais; //notre jeu est en francais
        private List<Plateau> explateaux; //anciens plateaux
        private Plateau plateau_actuel; //plateau en cour de jeu
        private Joueur[] joueur; // la taille du tableau

        #region Constructeur
        public Jeu(string[] nom_joueurs,int nombre_mots, int level, int ligne, int colonne)
        {
            this.dicoFrancais = new Dictionaire("MotsPossiblesFR (1).txt");
            this.joueur = new Joueur[nom_joueurs.Length]; 
            for (int i=0;i<nom_joueurs.Length;i++)
            {
                this.joueur[i] = new Joueur(nom_joueurs[i]);
            }
            this.plateau_actuel = new Plateau(nombre_mots,level,ligne,colonne);
            /*this.explateaux = new List<Plateau> { plateau_actuel };
            this.explateaux = plateau_actuel.ToRead("Enregitrements.csv") ;*/

        }
        #endregion

        #region propriete
        public Plateau Plat
        {
            get { return plateau_actuel; }
            set { plateau_actuel = value; }
        }
        #endregion

        #region Methodes
        /// <summary>
        /// Permet de gérer le chronomètre pour vérifier si le joueur joue toujours dans le temps imparti
        /// </summary>
        /// <param name="max">Durée de la partie</param>
        /// <param name="debut">Début du compte à rebours</param>
        /// <returns>Booléen pour savoir si le temps est terminé</returns>
        static bool TestTime(int max, DateTime debut)
        {
           bool res = true;
           TimeSpan inter = DateTime.Now - debut;
           if (inter.Minutes>=max)
            {
                res = false;
            }
           return res;
        }

        /*public void test()
        {
            explateaux = plateau_actuel.ToRead("Enregitrements.csv");
            for (int j=0;j<explateaux[0].Mots_a_rechercher.Count;j++)
            {
                Console.WriteLine(explateaux[0].Mots_a_rechercher[j]);
            }
            Console.WriteLine("grille suivante");
        }*/
        

        /// <summary>
        /// Affiche le score du joueur puis le plateau de jeu
        /// /// <param name="nom"> Joueur dont on veux faire l'affichage</param>
        /// </summary>
        /// <returns></returns>
        public void AfficherPoints(Joueur nom)
        {
            Console.WriteLine(nom.toString());
            for (int i = 0; i < plateau_actuel.Plat.GetLength(0); i++)
            {
                for (int j = 0; j < plateau_actuel.Plat.GetLength(1); j++)
                {
                    Console.Write(plateau_actuel.Plat[i, j] + ";");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Demande au joueur le mot qu'il a trouvé
        /// </summary>
        /// <returns>Mot trouvé</returns>
        public string Demande_mot()
        {
            Console.WriteLine("Saisir le mot trouvé");
            string rep = Console.ReadLine();
            rep.ToUpper();
            return rep;
        }

        /// <summary>
        /// Demande au joueur dans quelle direction va son mot
        /// </summary>
        /// <returns>Retourne la direction du mot (N,S,0,E)</returns>
        public string Demande_direction()
        {
            string rep = "";
                Console.WriteLine("Saisir la direction du mot");
                rep = Console.ReadLine();
                rep.ToUpper();
            return rep;
        }

        /// <summary>
        /// Demande l'index de la ligne de la première lettre du mot trouvé sur le plateau
        /// </summary>
        /// <returns>Index de la ligne</returns>
        public int Demande_index_ligne()
        {
            Console.WriteLine("Saisir l'index de la ligne où commence le mot (l'index commence à 0)");
            int ligne = Convert.ToInt32(Console.ReadLine());
            while (ligne < 0 || ligne > plateau_actuel.Plat.GetLength(0))
            {
                ligne = Convert.ToInt32(Console.ReadLine());
            }
            return ligne;
        }

        /// <summary>
        /// Demande l'index de la colonne de la première lettre du mot trouvé sur le plateau
        /// </summary>
        /// <returns>Index de la colonne</returns>
        public int Demande_index_colonne()
        {
            Console.WriteLine("Saisir l'index de la colonne où commence le mot (l'index commence à 0)");
            int colonne = Convert.ToInt32(Console.ReadLine());
            while (colonne < 0 || colonne > plateau_actuel.Plat.GetLength(1))
            {
                colonne = Convert.ToInt32(Console.ReadLine());
            }
            return colonne;
        }

        /// <summary>
        /// Permet de savoir si le joueur a terminé sa partie avant la fin du chrono
        /// </summary>
        /// <returns>Boléen qui indique la réponse</returns>
        public int Victoire()
        {
            int res=0;
            for (int i=0;i<this.plateau_actuel.Mots_a_rechercher.Count;i++)
            {
                res += plateau_actuel.Mots_a_rechercher[i].Length;
            }
            return res;
        }

        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("Bienvenue dans le jeu des mots-mêlés");
            bool continu = true;
            DateTime debut;
            Joueur[] joueurs = new Joueur[2];
            Console.WriteLine("Veuillez entrer le nom du premier joueur");
            joueurs[0] = new Joueur(Console.ReadLine());
            Console.WriteLine("Veuillez entrer le nom du deuxième joueur");
            joueurs[1] = new Joueur(Console.ReadLine());
            Console.Clear();
            string[] liste = { joueurs[0].Nom, joueurs[1].Nom };
            Console.WriteLine("Veuillez entrer le nombre de mots");
            int nb_mots = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Veuillez entrer votre niveau de difficulté entre 1 et 2");
            int difficulte = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Veuillez entrer le nombre de lignes du plateau de jeu");
            int ligne = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Veuillez entrer le nombre de colonnes du plateau de jeu");
            int colonne = Convert.ToInt32(Console.ReadLine());
            Jeu fin = new Jeu(liste,nb_mots,difficulte,ligne,colonne);
            string mot_choisi;
            string direction_choisi;
            int col_choisi;
            int ligne_choisi;
            while (continu == true)
            {
                debut = DateTime.Now;
                while (TestTime(1, debut) != false || joueurs[0].Scores[0] != fin.Victoire() )
                {
                    Console.Clear();
                    foreach(string i in fin.plateau_actuel.Mots_a_rechercher)
                    {
                        Console.Write(i + ";");
                    }
                    fin.AfficherPoints(joueurs[0]);
                    mot_choisi = fin.Demande_mot();
                    direction_choisi = fin.Demande_direction();
                    ligne_choisi = fin.Demande_index_ligne();
                    col_choisi =  fin.Demande_index_colonne();
                    if (fin.Plat.Test_Plateau(mot_choisi, direction_choisi, ligne_choisi, col_choisi)== true)
                    {
                        Console.WriteLine("Bravooo!!! Un mot trouvé");
                        joueurs[0].Add_Mot(mot_choisi);
                        joueurs[0].Add_Score(mot_choisi.Length);
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Ce mot n'existe pas ou n'est pas dans la liste");
                        Console.ReadKey();
                    }
                }
                debut = DateTime.Now;
                while (TestTime(1, debut) != false|| joueurs[1].Scores[0] != fin.Victoire())
                {
                    Console.Clear();
                    /*foreach(string i in fin.plateau_actuel.Mots_a_rechercher)
                    {
                        Console.Write(i + ";");
                    }*/
                    fin.AfficherPoints(joueurs[1]);
                    mot_choisi = fin.Demande_mot();
                    direction_choisi = fin.Demande_direction();
                    ligne_choisi = fin.Demande_index_ligne();
                    col_choisi = fin.Demande_index_colonne();
                    if (fin.Plat.Test_Plateau(mot_choisi, direction_choisi, ligne_choisi, col_choisi) == true)
                    {
                        Console.WriteLine("Bravooo!!! Un mot trouvé");
                        joueurs[1].Add_Mot(mot_choisi);
                        joueurs[1].Add_Score(mot_choisi.Length);
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Ce mot n'existe pas ou n'est pas dans la liste");
                        Console.ReadKey();
                    }
                }
                Console.WriteLine("Les réponses : ");
                foreach(string i in fin.Plat.Mots_a_rechercher)
                {
                    Console.WriteLine(i + ";");
                }
                Console.WriteLine("Voulez-vous continuer ? Si oui écrivez True, sinon False");
                if (joueurs[0].Scores[0] < joueurs[1].Scores[0])
                {
                    Console.WriteLine("Joueur" + joueurs[1].Nom +" a gagné");
                }
                if (joueurs[0].Scores[0] > joueurs[1].Scores[0])
                {
                    Console.WriteLine("Joueur" + joueurs[0].Nom + " a gagné");
                }
                else
                {
                    Console.WriteLine("Egalite");
                }
                while (bool.TryParse(Console.ReadLine(), out continu) == false)
                {
                    Console.WriteLine("Voulez-vous continuer ? Si oui écrivez True, sinon False");
                }
            }
            
        }
    }
}
/*string[] nom_joueur = { "Iyad", "Hadifé" };
Jeu jeu = new Jeu(nom_joueur);
jeu.test();
Console.ReadKey();*/
/*Dictionaire dico = new Dictionaire("MotsPossiblesFR (1).txt");
            Console.WriteLine(dico.toString());
            Console.WriteLine(dico.RechDichoRecursif("UN"));
            Console.ReadLine();
*/