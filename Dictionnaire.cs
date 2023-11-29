using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet
{
    public class Dictionaire
    {
        private List<string> dico;

        #region Constructeurs
        public Dictionaire (string nom)
        {
            this.dico = CopieDico(nom);
        }
        

        
        #endregion

        #region Propirété
        public List<string> Dico
        {
            get { return dico; }
            set { dico = value; }
        }

        #endregion

        #region Methode
        /// <summary>
        /// Permet de copier le dictionnaire ici français
        /// </summary>
        /// <param name="nom">Nom du fichier correspondant au dictionnaire français</param>
        /// <returns>Retourne une liste de string avec tous les mots du dictionnaire</returns>
        public List<string> CopieDico(string nom)
        {
            List<string> res = new List<string> { };
            StreamReader r = null;
            try
            {
                r = new StreamReader(nom);
                string[] tabl = File.ReadAllLines(nom);
                string[] l;
                for (int i = 0; i < tabl.Length; i++)
                {
                    if (tabl != null && tabl.Length != 0)
                    {
                        l = tabl[i].Split(" ");
                        foreach (string s in l)
                        {
                            res.Add(s);
                        }
                    }
                }

            }
            catch
            {
                Console.WriteLine("Le nom du fichier n'existe pas");
            }
            return res;
        }

        /// <summary>
        /// Permet de classer les mots par nombre de lettre
        /// </summary>
        /// <param name="nbLettres">Nombre de lettres des mots que l'on cherche à trier</param>
        /// <returns>Retourne une liste de chaine de caractères avec tous les mots de même nombre de lettres</returns>
        public List<string> Mot_Nombre_lettre(int nbLettres)
        {
            List<string> res = new List<string> { };
            for (int i=0;i<dico.Count;i++)
            {
                if (dico[i].Length == nbLettres )
                {
                    res.Add(dico[i]);
                }
                
            }
            return res;
        }

        // <summary>
        /// Recherche du mot dans le dictionnaire de façon récursive
        /// </summary>
        /// <param name="mot">Mot que l'on cherche</param>
        /// <param name="lnbmot">Liste de tours les mots du bon nombre de lettres</param>
        /// <param name="indexdebut">Index de début pour parcourir la liste</param>
        /// <param name="indexfin">Index de fin pour parcourir la liste</param>
        /// <returns>Retourne un  boléen pour savoir si ce mot existe ou non dans le dictionnaire</returns>
        public bool RechDichoRecursif(string mot, List<string> lnbmot=null,int indexdebut=-1, int indexfin=0)
        {
            if (indexdebut == -1)
            {
                lnbmot = Mot_Nombre_lettre(mot.Length);
                indexdebut = 0;
                indexfin = lnbmot.Count - 1;
            }
            if (lnbmot !=null&&lnbmot.Count != 0)
            {
                if (indexdebut == indexfin && lnbmot[indexdebut] != mot)
                {
                    return false;
                }
                else if (indexdebut == indexfin && lnbmot[indexdebut] == mot)
                {
                    return true;
                }
                else if (indexdebut==indexfin-1 && (lnbmot[indexdebut] == mot || lnbmot[indexfin] == mot))
                {
                    return true;
                }
                else 
                {
                    return false;
                }
                int mill = (indexdebut + indexfin) / 2;
                if (mot.CompareTo(lnbmot[mill]) < 0) { indexfin = mill-1; }
                else if (mot.CompareTo(lnbmot[mill]) > 0) { indexdebut = mill+1; }
                else if (mot.CompareTo(lnbmot[mill]) == 0) { return true; }
                return RechDichoRecursif(mot, lnbmot, indexdebut, indexfin);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Décrit le dictionnaire avec sa langue et son nombre de mots pour chaque nombre de lettres possible
        /// </summary>
        /// <returns>Chaine descriptive du dictionnaire que l'on pourra afficher</returns>
        public string toString()
        {
            string res="C'est le dictionaire Francais\n";
            for(int i=2;i<10;i++)
            {
                res += $"Le nomnre de mot de {i} lettres est : {Mot_Nombre_lettre(i).Count}\n";
            }
            return res;
        }

        #endregion
    }
}
