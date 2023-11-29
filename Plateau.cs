using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace Projet
{
   public class Plateau
    {
        private int difficulte;
        private List<string> mots_a_rechercher;
        private char[,] plat;

        #region Constructeurs
        public Plateau(int nombre_mot, int difficulte, int ligne, int colonne)
        {
            this.difficulte = difficulte;
            this.plat = new char[ligne,colonne];
            this.mots_a_rechercher = new List<string> { };
            for (int i = 0; i < plat.GetLength(0); i++)
            {
                for (int j = 0; j < plat.GetLength(1); j++)
                {
                    this.plat[i, j] = ' ';
                }
            }
            this.plat = Generation_matrice(nombre_mot);
            for (int i = 0; i < plat.GetLength(0); i++)
            {
                for (int j = 0; j < plat.GetLength(1); j++)
                {
                    if (this.plat[i,j] == ' ' )
                    {
                        this.plat[i, j] = Lettres_alea();
                    }
                }
            }

        }

        #endregion

        #region Propriétés
        public int Difficulte
        {
            get { return this.difficulte; }
            set { this.difficulte = value; }
        }
        public List<string> Mots_a_rechercher
        {
            get { return this.mots_a_rechercher; }
            set { this.mots_a_rechercher = value; }
        }
        public char[,] Plat
        {
            get { return this.plat; }
            set { this.plat = value; }
        }
        #endregion

        #region Méthodes

        /// <summary>
        /// Génère aléatoirement des lettres à placer entre les mots de la grille
        /// </summary>
        /// <returns>Lettre aléatoire</returns>
        public char Lettres_alea()
        {
            Random r = new Random();
            string liste = "AZERTYUIOPQSDFGHJKLMWXCVBN";
            int res = r.Next(0, liste.Length);
            return liste[res];
        }
        /// <summary>
        /// Vérifie si le mot est dans notre liste de mots à rechercher
        /// </summary>
        /// <param name="mots">Mot écrit par le joueur</param>
        /// <returns>Booléen pour savoir si oui ou non il en fait partie</returns>
        public bool Verif_Liste_mots(string mots)
        {
            bool res = false;
            int compt = 0;
            foreach (string i in this.mots_a_rechercher)
            {
                if (i  == mots_a_rechercher[compt])
                {
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Décrit le contenu d'un plateau avec sa difficulté, sa taille et tous les mots à trouver
        /// </summary>
        /// <returns>Phrase de description</returns>
        public string toString()
        {
            string descri = "Le plateau à difficulté "+difficulte+" contient "+plat.GetLength(0) + " lignes et "+plat.GetLength(1)+" colonnes pour "+mots_a_rechercher.Count+" mots qui sont ";
            for (int i=0;i<mots_a_rechercher.Count-1;i++)
            {
                descri = descri + mots_a_rechercher[i]+", ";
            }
            descri = descri + mots_a_rechercher[mots_a_rechercher.Count - 1];
            return descri;
        }

        /// <summary>
        /// Permet d'écrire le plateau de jeu dans un fichier
        /// </summary>
        /// <param name="nomfile">Fichier sur lequel on écrit</param>
        public void ToFile (string nomfile)
        {
            StreamWriter sw = new StreamWriter(nomfile);
            try
            {
                for (int i = 0; i < plat.GetLength(0); i++)
                {
                    for (int j=0;j<plat.GetLength(1);j++)
                    {
                        sw.WriteLine(plat[i,j]+";");
                    }
                    sw.WriteLine();
                }
            }
            catch
            {
                Console.WriteLine("problème sur la grille");
            }
            finally
            {
                sw.Close();
            }
        }

        // <summary>
        /// Permet d'extraire le plateau de jeu à partir du fichier
        /// </summary>
        /// <param name="nomfile">Fichier en dur ou aléatoire à utiliser</param>
        public List<Plateau> ToRead(string nomfile)
        {
            StreamReader sr = null;
            try
            {
                List<Plateau> extableaux= new List<Plateau>();
                Plateau ex;
                sr = new StreamReader(nomfile);
                string[] decoupe = File.ReadAllLines(nomfile);
                if (decoupe!=null && decoupe.Length!=0 )
                {
                    for (int k = 0; k < decoupe.Length; k++)
                    {
                        string[] decoupe_iemeplat = decoupe[k].Split(";");
                        if (decoupe_iemeplat[0] == "ENR")
                        {
                            ex = new Plateau(5, 5, 6, 5);
                            ex.Mots_a_rechercher = new List<string> { };
                            ex.difficulte = Convert.ToInt32(decoupe_iemeplat[1]);
                            ex.plat = new char[Convert.ToInt32(decoupe_iemeplat[2]), Convert.ToInt32(decoupe_iemeplat[3])];
                            k= k + 1;
                            decoupe_iemeplat = decoupe[k].Split(";");
                            foreach (string mot in decoupe_iemeplat)
                            {
                                ex.mots_a_rechercher.Add(mot);
                            }
                            for (int j = 0; j < ex.plat.GetLength(0); j++)
                            {
                                k = k + 1;
                                decoupe_iemeplat = decoupe[k].Split(";");
                                for (int i = 0; i < ex.plat.GetLength(1); i++)
                                {
                                    ex.plat[i, j] = Convert.ToChar(decoupe_iemeplat[i]);
                                }
                            }
                            extableaux[k] = ex;
                        }
                    }
                
                }
                return extableaux;
            }
            catch
            {
                Console.WriteLine("Fichier non conforme");
                return null;
            }
            /*finally
            {
                sr.Close();
            }*/
        }

        /// <summary>
        /// Permet de savoir si un mot est éligible c'est-à-dire s'il ne dépasse pas sur le plateau, fait partie du dictionnaire et corespond aux bonnes lettres
        /// </summary>
        /// <param name="mot">Mot proposé par le joueur</param>
        /// <param name="direction">Direction dans laquelle le joueur a trouvé le mot</param>
        /// <param name="ligne">Ligne choisie où se trouve la première lettre du mot</param>
        /// <param name="colonne">Colonne choisie où se trouve la première lettre du mot</param>
        /// <returns>Boléen pour savoir si oui ou non ce mot convient</returns>
        public bool Test_Plateau(string mot, string direction, int ligne, int colonne)
        {
            bool eligible = true;
            Dictionaire dico = new Dictionaire("MotsPossiblesFR (1).txt");
            if (mot != null && mot.Length != 0)
            {
                if (Verif_Liste_mots(mot) != true)
                {
                    eligible = false;
                }
                switch (direction)
                {
                    case "N":
                        if (ligne + 1 - mot.Length < 0) { eligible = false; }
                        else
                        {
                            int compteur = 0;
                            for (int i = ligne;i>0 && compteur < mot.Length; i--)
                            {
                                if (plat[i, colonne] != mot[compteur])
                                {
                                    eligible = false;
                                }
                                compteur++;
                            }
                        }
                        break;
                    case "S":
                        if (plat.GetLength(0) < mot.Length + ligne) { eligible = false; }
                        else
                        {
                            int compteur = 0;
                            for (int i = ligne;i<plat.GetLength(0) && compteur < mot.Length; i++)
                            {
                                if (plat[i, colonne] != mot[compteur])
                                {
                                    eligible = false;
                                }
                                compteur++;
                            }
                        }
                        break;
                    case "E":
                        if (plat.GetLength(1) < mot.Length + colonne) { eligible = false; }
                        else
                        {
                            int compteur = 0;
                            for (int j = colonne;j <plat.GetLength(1)&& compteur < mot.Length; j++)
                            {
                                if (plat[ligne, j] != mot[compteur])
                                {
                                    eligible = false;
                                }
                                compteur++;
                            }
                        }
                        break;
                    case "O":
                        if (colonne + 1 - mot.Length < 0) { eligible = false; }
                        else
                        {
                            int compteur = 0;
                            for (int j = colonne; j>0 && compteur < mot.Length; j--)
                            {
                                if (plat[ligne, j] != mot[compteur])
                                {
                                    eligible = false;
                                }
                                compteur++;
                            }
                        }
                        break;
                }
            }
            return eligible;
        }

        /// <summary>
        /// Cherche un mot du dictionnaire qui correspond à la place laissée dans la grille pour remplir aléatoirement la plateau
        /// </summary>
        /// <param name="condition">Lettres déjà présentes qui doivent être compatibles</param>
        /// <returns>Mot à placer</returns>
        public string Recherche_mot(List<char> condition)
        {
            string mot = null;
            Dictionaire dico = new Dictionaire("MotsPossiblesFR (1).txt");
            List<string> Dico_n_lettre = dico.Mot_Nombre_lettre(condition.Count)  ;
            bool flag = false;
            Random n = new Random();
            int commancement = 0;
            int k=0;
            if (Dico_n_lettre.Count - 1 > 0)
            {
                while (mot == null)
                {
                    flag = true;
                    commancement = n.Next(0, Dico_n_lettre.Count - 1);
                    for (int i = commancement; i < Dico_n_lettre.Count - 1 && flag ==true; i++)
                    {
                        mot = Dico_n_lettre[i];
                        if (int.TryParse(mot, out k)==false)
                        {
                            for (int l = 0; l < condition.Count && flag == true; l++)
                            {
                                if (mot[l] != condition[l] && condition[l] != ' ')
                                {
                                    mot = null;
                                    flag = false;
                                }
                            }
                            flag = true;
                            if (mot != null)
                            {
                                flag = false;
                            }
                        }
                    }
                }  
            }
            return mot;
        }

        /// <summary>
        /// Méthode pour obtenir une direction aléatoire en fonction du niveau
        /// </summary>
        /// <param name="niveau">Niveau de difficulté exigé</param>
        /// <returns>Entier qui correspond en fait à la direction dans lequel va aller le mot</returns>
        public int generation_direction(int niveau)
        {
            int direction=0;
            Random r = new Random();
            if (niveau == 1)
            {
                direction = r.Next(2, 4);
            }
            else if (niveau == 2)
            {
                direction = r.Next(0, 4);
            }
            else if (niveau == 3)
            {
                direction = r.Next(0, 5);
            }
            else if(niveau == 4)
            {
                direction = r.Next(0, 6);
            }
            else if (niveau == 5)
            {
                direction = r.Next(0, 8);
            }
            return direction;
        }

        /// <summary>
        /// Création globale de la matrice en aléatoire selon le niveau de difficulté
        /// </summary>
        /// <param name="nombre_de_mot">Nombre de mots à placer</param>
        /// <returns>Plateau de lettres conçu aléatoirement et sur lequel 
        public char[,] Generation_matrice(int nombre_de_mot)
        {
            string mot_dans_plat;
            List<char> mot = new List<char>();
            int i = 0;
            int colonne;
            int ligne;
            int k = 0;
            Random r = new Random();
            int direction;
            while (i<nombre_de_mot)
            {
                direction = generation_direction(this.difficulte);
                colonne = r.Next(0,plat.GetLength(1)-1);
                ligne = r.Next(0, plat.GetLength(0)-1);
                if (direction == 0) ///Direction à = nord
                {
                    for (int j=ligne; j>0; j--)
                    {
                        mot.Add(plat[j, colonne]);
                    }
                    mot_dans_plat = Recherche_mot(mot);
                    if (mot_dans_plat != null)
                    {
                        for (int j = ligne; j > 0; j--)
                        {
                            plat[j, colonne] = mot_dans_plat[k];
                            k++;
                        }
                        mots_a_rechercher.Add(mot_dans_plat);
                        i++;
                    }
                }
                if (direction == 1) ///Direction à = ouest
                {
                    for (int j = colonne; j > 0; j--)
                    {
                        if (plat[ligne, j] == ' ')
                        {
                            mot.Add(' ');
                        }
                        else
                        {
                            mot.Add(plat[ligne, j]);
                        }
                    }
                    mot_dans_plat = Recherche_mot(mot);
                    if (mot_dans_plat != null)
                    {
                        for (int j = colonne; j > 0; j--)
                        {
                            plat[ligne, j] = mot_dans_plat[k];
                            k++;
                        }
                        
                        mots_a_rechercher.Add(mot_dans_plat);
                        i++;
                    }
                }
                if (direction == 2) ///Direction à = sud
                {
                    for (int j = ligne; j <plat.GetLength(0); j++)
                    {
                        mot.Add(plat[j, colonne]);
                    }
                    mot_dans_plat = Recherche_mot(mot);
                    if (mot_dans_plat != null)
                    {
                        for (int j = ligne; j < plat.GetLength(0); j++)
                        {
                            plat[j, colonne] = mot_dans_plat[k];
                            k++;
                        }
  
                        mots_a_rechercher.Add(mot_dans_plat);
                        i++;
                    }
                }
                if (direction == 3) ///Direction à = est
                {
                    for (int j = colonne; j < plat.GetLength(1); j++)
                    {
                        mot.Add(plat[ligne, j]);
                    }
                    mot_dans_plat = Recherche_mot(mot);
                    if (mot_dans_plat != null)
                    {
                        for (int j = colonne; j < plat.GetLength(1); j++)
                        {
                            plat[ligne, j] = mot_dans_plat[k];
                            k++;
                        }
                        
                        mots_a_rechercher.Add(mot_dans_plat);
                        i++;
                    }
                }          
                if (direction == 4) ///Direction à = SudOuest                
                {
                    k = ligne;
                    for (int j = colonne; j < plat.GetLength(1) && k >= 0; j++)
                    {
                        mot.Add(plat[k, j]);
                        k--;
                    }
                    k = 0;
                    mot_dans_plat = Recherche_mot(mot);
                    if (mot_dans_plat != null)
                    {
                        for (int j = colonne; j < plat.GetLength(1) && ligne>=0 &&k<mot_dans_plat.Length; j++)
                        {
                            plat[ligne, j] = mot_dans_plat[k];
                            ligne--;
                            k++;
                        }
                        mots_a_rechercher.Add(mot_dans_plat);
                        i++;
                    }
                }
                if (direction == 5) ///Direction à = SudEst
                {
                    k = ligne;
                    for (int j = colonne; j >= 0 && k >= 0; j--)
                    {
                        mot.Add(plat[k, j]);
                        k--;
                    }
                    k = 0;
                    mot_dans_plat = Recherche_mot(mot);
                    if (mot_dans_plat != null)
                    {
                        for (int j = colonne; j >= 0 && k >= 0 && ligne >= 0; j--)
                        {
                            plat[ligne, j] = mot_dans_plat[k];
                            ligne--;
                            k++;
                        }
                        
                        mots_a_rechercher.Add(mot_dans_plat);
                        i++;
                    }
                }
                if (direction == 6) ///Direction à = Nordouest
                {
                    k = ligne;
                    for (int j = colonne; j < plat.GetLength(1) && k<plat.GetLength(0); j++)
                    {
                        mot.Add(plat[k, j]);
                        k++;
                    }
                    k = 0;
                    mot_dans_plat = Recherche_mot(mot);
                    if (mot_dans_plat != null)
                    {
                        for (int j = colonne; j >= 0 && ligne <plat.GetLength(0) && k<mot_dans_plat.Length; j--)
                        {
                            plat[ligne, j] = mot_dans_plat[k];
                            ligne++;
                            k++;
                        }
                        
                        mots_a_rechercher.Add(mot_dans_plat);
                        i++;
                    }
                }
                if (direction == 7) ///Direction à = NordOuest                
                {
                    k = ligne;
                    for (int j = colonne; j >=0 && k <plat.GetLength(0); j--)
                    {
                        mot.Add(plat[k, j]);
                        k++;
                    }
                    k = 0;
                    mot_dans_plat = Recherche_mot(mot);
                    if (mot_dans_plat != null)
                    {
                        for (int j = colonne; j >= 0 && ligne < plat.GetLength(0); j--)
                        {
                            plat[ligne, j] = mot_dans_plat[k];
                            ligne++;
                            k++;
                        }
                        
                        mots_a_rechercher.Add(mot_dans_plat);
                        i++;
                    }
                }
                k = 0;
                mot = new List<char> { };
            }
            return plat;
        }


        #endregion
    }
}
