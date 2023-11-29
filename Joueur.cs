using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet
{
    public class Joueur
    {
        private string nom;
        private List<string> mots_trouves;
        private List<int> scores;

        #region Constructeurs
        public Joueur(string nom)
        {
            this.nom = nom;
            this.mots_trouves = new List<string> { };
            this.scores = new List<int>() {0};
        }
        #endregion

        #region Propriétés
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        public List<string> Mots_trouves
        {
            get { return this.mots_trouves; }
            set { this.mots_trouves = value; }
        }
        public List<int> Scores
        {
            get { return this.scores; }
            set { this.scores = value; }
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Ajoute un mot aux mots déjà trouvés par le joueur
        /// </summary>
        /// <param name="mot">Nouveau mot trouvé par le joueur</param>
        public void Add_Mot(string mot)
        {
            if (mots_trouves!=null && mot.Length>=2)
            {
                mots_trouves.Add(mot);
            }
        }
        /// <summary>
        /// Description du joueur avec ses gains
        /// </summary>
        /// <returns>Chaine descriptive</returns>
        public string toString()
        {
            string chaine = "Le joueur " + this.nom+ " a eu comme scores " ;
            foreach (int sco in this.Scores)
            {
                chaine = chaine + sco+", ";
            }
            chaine = chaine + "grâce aux mots trouvés ";
            foreach (string mot in mots_trouves)
            {
                chaine = chaine + mot + " ";
            }
            return chaine;
        }

        /// <summary>
        /// Ajouter un score au tableau de résultats du joueur
        /// </summary>
        /// <param name="val">Score de la partie à ajouter</param>
        public void Add_Score(int val)
        {
            if (scores != null)
            {
                scores[0]+=val;
            }
        }
        #endregion
    }
}
