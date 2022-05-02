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
        private void GoLeft()
        {
            if (X == 7)
                X -= 1;
            X -= 1;
        }
        private void GoRight()
        {
            if (X == 5)
                X += 1;
            X += 1;
        }
        private bool AtBound(int i)
        {
            if (up && i == 0)
                return true;
            if (!up && i == possible.GetLength(0) - 1)
                return true;
            return false;
        }
        public bool HasNext()
        {
            return X >= 0;
        }
        public void Next()
        {

            do
            {
                NextPos();
            } while (HasNext() && !possible[Y,X]);
                
            
        }
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

