using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_TD_2
{
    class Iterateur
    {
        //Variables
        /// <summary>
        /// prends la valeur true si c'est la case de gauche, false sinon
        /// </summary>
        private bool IsLeft
        {
            get
            {
                if (X < 6)
                    return X % 2 == 0;
                else if (X == 6)
                    throw new ApplicationException();
                else
                    return X % 2 == 1;
            }
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        private bool up  = true;
        private bool[,] possible;


        //Constructeurs
        public Iterateur( bool[,] possible)
        {
           
            this.X = possible.GetLength(0)-1;
            this.Y = possible.GetLength(1) - 1;
            this.possible = possible;
        }

        //Methodes
        /// <summary>
        /// Méthode qui permet à l'itérateur d'aller sur la case de gauche
        /// </summary>
        private void GoLeft()
        {
            if (X == 7)
                X -= 1;
            X -= 1;
        }

        /// <summary>
        /// Permet à l'iterateur d'aller sur la case de droite
        /// </summary>
        private void GoRight()
        {
            if (X == 5)
                X += 1;
            X += 1;
        }

        /// <summary>
        /// renvoie true si l'itérateur est au bout de la colonne
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool AtBound(int i)
        {
            if (up && i == 0)
                return true;
            if (!up && i == possible.GetLength(0) - 1)
                return true;
            return false;
        }

        /// <summary>
        /// Renvoie true si suivant, false si fin
        /// </summary>
        /// <returns></returns>
        public bool HasNext()
        {
            return X >= 0;
        }

        /// <summary>
        /// Permet d'appeler la fonction pour passer a la case suivante qu'il reste des case et que l'on peut ecrire sur la case
        /// </summary>
        public void Next()
        {

            do
            {
                NextPos();
            } while (HasNext() && !possible[Y,X]);
                
            
        }

        /// <summary>
        /// Permet de passer a la case suivnte : haut droite si on est a gauche, gauche si on est a droite
        /// </summary>
        private void NextPos()
        {
            if (IsLeft)
                if (AtBound(Y))
                {
                    GoLeft();
                    up=!up;
                }
                else
                {
                    GoRight();
                    Y += up ? -1 : 1; //if else
                }
            else
                GoLeft();
        }
    }
}

