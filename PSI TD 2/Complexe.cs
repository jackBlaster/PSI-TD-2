using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_TD_2
{
    public class Complexe
    {
        #region<Attributs/Propriétés>
        double im;
        double re;
        double module;

        public double Im => im;
        public double Re => re;
        public double Module => module;
        #endregion

        #region<Constructeur>
        /// <summary>
        /// Créer un complexe à partir d'une partie réelle et imaginaire.
        /// </summary>
        /// <param name="re">partie réelle</param>
        /// <param name="im">partie imaginaire</param>
        public Complexe(double re,double im)
        {
            this.im = im;
            this.re = re;
            this.module = Math.Sqrt((im * im) + (re * re));
        }
        #endregion

        #region<Méthodes>
        /// <summary>
        /// Calcule la somme de 2 nombres complexes
        /// </summary>
        /// <param name="unComplexe">complexe à additionner</param>
        /// <returns> Somme des deux complexes </returns>
        public Complexe SommeComplexe(Complexe unComplexe)
        {
            double IM = this.im + unComplexe.im;
            double RE = this.re + unComplexe.re;
            return new Complexe(RE, IM);
        }

        /// <summary>
        /// Calcule la multiplication de 2 nombres complexes
        /// </summary>
        /// <param name="unComplexe">complexe à multiplier</param>
        /// <returns>multiplication des 2 nombres</returns>
        public Complexe MultiplicationComplexe(Complexe unComplexe)
        {
            double RE = (this.re * unComplexe.re) - (this.im * unComplexe.im);
            double IM = (this.re * unComplexe.im) + (this.im * unComplexe.re);
            return new Complexe(RE, IM);
        }

        public void toString()
        {
            Console.WriteLine(this.Re + " + " + Im + "i");
        }
        #endregion
    }
}
