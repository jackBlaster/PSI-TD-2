using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

namespace PSI_TD_2
{

    public class MyImage
    {


        #region <Attributs/Propriétés>

        public int Padding { get { return (4 - (Width * 3) % 4) % 4; } }
        public int FileSize { get { return 54 + Height * (Width + Padding); } }
        public int Height { get ; private set ; }
        public int Width { get; private set; }
        public Pixel[,] pic { get; set; }

        #endregion

        #region <Constructeurs>

        /// <summary>
        /// Créer une instance MyImage à partir d'une hauteur et d'une largeur (en pixels)
        /// </summary>
        /// <param name="height">hauteur de l'image</param>
        /// <param name="width">largeur de l'image</param>
        public MyImage(int height, int width)
        {
            this.pic = new Pixel[height, width];
            this.Height = height;
            this.Width = width;
        }

        /// <summary>
        /// Créer un clone d'une instance MyImage à partir d'une instance déjà existante (utilise la méthode Clone)
        /// </summary>
        /// <param name="Clone">Image à cloner</param>
        public MyImage(MyImage Clone)
        {
            this.pic = (Pixel[,])Clone.pic.Clone();
            this.Height = Clone.Height;
            this.Width = Clone.Width;
        }

        /// <summary>
        /// Créer une instance MyImage grâce à un Stream qui va parcourir le fichier.
        /// </summary>
        /// <param name="s">Stream</param>
        public MyImage(Stream s)
        {
            s.Position = 18;

            this.Width = Program.ReadInt(s);
            this.Height = Program.ReadInt(s);
            this.pic = new Pixel[Height, Width];

            s.Position = 54;

            for (int i = Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < Width; j++)
                    this.pic[i, j] = new Pixel((byte)s.ReadByte(), (byte)s.ReadByte(), (byte)s.ReadByte());

                for (int j = 0; j < Padding; j++)
                    s.ReadByte(); //Bourrage
            }
        }

        /// <summary>
        /// Permet de créer une instance MyImage grâce à un nom de fichier (utilise le constructeur avec le Stream)
        /// </summary>
        /// <param name="file">nom du fichier</param>
        public MyImage(string file) : this(new FileStream(file, FileMode.Open)) { }

        #endregion

        #region <Méthodes>

        /// <summary>
        /// Permet de cloner l'instance à l'identique via le constrcteur MyImage(MyImage)
        /// </summary>
        /// <returns>Clone</returns>
        public MyImage Clone()
        {
            return new MyImage(this);
        }

        /// <summary>
        /// Sauvegarde l'image en fichier avec le nom mit en paramètre (utilise le Save(Stream))
        /// </summary>
        /// <param name="file">nom du fichier</param>
        public void Save(string file)
        {
            Save(new FileStream(file, FileMode.Create));
        }

        /// <summary>
        /// Le stream va lire les informations de l'instance et écrire le Header et les pixels en conséquence
        /// </summary>
        /// <param name="s">Stream</param>
        public void Save(Stream s)
        {
            //Header
            s.WriteByte((byte)'B');
            s.WriteByte((byte)'M');
            Program.WriteInt(FileSize, s);
            Program.WriteInt(0, s);
            Program.WriteInt(54, s);

            //Informations
            Program.WriteInt(40, s);
            Program.WriteInt(Width, s);
            Program.WriteInt(Height, s);
            s.WriteByte(1);
            s.WriteByte(0);
            s.WriteByte(24);
            s.WriteByte(0);
            for (int i = 0; i < 6; i++)
                Program.WriteInt(0, s);

            //Pixels    
            Pixel p;
            for (int i = Height - 1; i >= 0; i--)
            {
                for (int j = 0; j < Width; j++)
                {
                    p = pic[i, j];
                    if (p == null)
                        for (int k = 0; k < 3; k++)
                            s.WriteByte(0xFF);//pixel blanc
                    else
                    {
                        s.WriteByte(p.R);
                        s.WriteByte(p.G);
                        s.WriteByte(p.B);
                    }
                }
                for (int j = 0; j < Padding; j++)
                    s.WriteByte(0);
            }

            //s.Flush();
            s.Close();
        }

        /// <summary>
        /// Permet de modifier un pixel dans l'image à une certaine coordonnée
        /// </summary>
        /// <param name="i">ligne</param>
        /// <param name="j">colonne</param>
        /// <param name="pixel">pixel à modifier</param>
        public void SetPixel(int i, int j, Pixel pixel)
        {
            this.pic[i, j] = pixel;
        }


        #region<Filtres>
        
        /// <summary>
        /// Applique un filtre gris à un clone de l'instance
        /// </summary>
        /// <returns>Nouvelle instance filtrée</returns>
        public MyImage Grey()
        {
            MyImage copy = new MyImage(this.Height, this.Width);

            Pixel p;
            byte gray;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    p = this.pic[i, j];
                    if (p == null)
                        p = new Pixel(0xFF, 0xFF, 0xFF);

                    gray = (byte)((p.R + p.G + p.B) / 3);
                    copy.pic[i, j] = new Pixel(gray, gray, gray);
                }
            }

            return copy;

        }
        

        /// <summary>
        /// Applique un filtre noir et Blanc au clone de l'instance
        /// </summary>
        /// <returns>Nouvelle instance filtrée</returns>
        public MyImage ToBlackAndWhite()
        {

            MyImage copy = new MyImage(this.Height, this.Width);

            Pixel p;
            byte gray;



            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    p = this.pic[i, j];
                    if (p == null)
                        p = new Pixel(255, 255, 255);

                    gray = (byte)((p.R + p.G + p.B) / 3);
                    if (gray < 128)
                        copy.pic[i, j] = Pixel.BLACK;
                    else
                        copy.pic[i, j] = Pixel.WHITE;
                }
            }

            return copy;

        }

        /// <summary>
        /// Crée le négatif d'une image
        /// </summary>
        /// <returns>image négative</returns>
        public MyImage Negatif()
        {

            MyImage copy = new MyImage(this.Height, this.Width);

            Pixel p;

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    p = this.pic[i, j];
                    if (p == null)
                        p = new Pixel(255, 255, 255);

                    copy.pic[i, j] = new Pixel((byte)(255-p.R), (byte)(255-p.G), (byte)(255-p.B));

                }
            }

            return copy;

        }


        /// <summary>
        /// Permet d'agrandir ou de rétrecir une image par un facteur
        /// </summary>
        /// <param name="facteur">facteur d'agrandissement/rétrecissement</param>
        /// <returns>Nouvelle instance filtrée</returns>
        public MyImage Agrandir(double facteur)
        {
            MyImage newimage = new MyImage((int)(this.Height * facteur), (int)(this.Width * facteur));

            for (int i = 0; i < newimage.Height; i++)
                for (int j = 0; j < newimage.Width; j++)
                    newimage.pic[i, j] = this.pic[(int)(i / facteur), (int)(j / facteur)];

            return newimage;
        }

        /// <summary>
        /// Permet d'inverser l'image sur le plan verticale
        /// </summary>
        /// <returns>Nouvelle instance filtrée</returns>
        public MyImage MirroirVertical()
        {
            MyImage newimage = new MyImage(this.Height, this.Width);

            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    newimage.pic[i, j] = this.pic[i, Width - 1 - j];

            return newimage;
        }

        /// <summary>
        /// Permet d'inverser l'image selon le plan horizontal
        /// </summary>
        /// <returns>Nouvelle instance filtrée</returns>
        public MyImage MirroirHorizontal()
        {
            MyImage newimage = new MyImage(this.Height, this.Width);

            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    newimage.pic[i, j] = this.pic[Height - 1 - i, j];

            return newimage;
        }

        /// <summary>
        /// Permet d'appliquer une matrice de convolution (Kernel) à une matrice d'instance (utilisela méthode SommeConvolution)
        /// </summary>
        /// <param name="Kernel">Matrice de Kernel utilisée</param>
        /// <returns>Nouvelle instance filtrée</returns>
        public MyImage Convolution(double[,] Kernel)
        {
            MyImage Convol = new MyImage(this.Height, this.Width);
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    Convol.pic[i, j] = SommeConvolution(i, j, Kernel);
                }
            }
            return Convol;
        }

        /// <summary>
        /// Permet de calculer la nouvelle valeur pour le Pixel à modifier
        /// </summary>
        /// <param name="x">ligne</param>
        /// <param name="y">colonne</param>
        /// <param name="Kernel">matrice de Kernel utilisée</param>
        /// <returns>Pixel modifié</returns>
        public Pixel SommeConvolution(int x, int y, double[,] Kernel)
        {
            double sommeR = 0;
            double sommeG = 0;
            double sommeB = 0;
            double diviseur = 0;
            
            for (int i = 0; i < Kernel.GetLength(0); i++)
            {
                for(int j = 0; j < Kernel.GetLength(1); j++)
                {
                    diviseur += Kernel[i, j];
                }
            } 
            if(diviseur==0)
            {
                diviseur = 1;
            }

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x == 0|| x == Height-1|| y == 0 || y == Width-1) { }//non appliqué aux bords de l'image pour éviter les ArgumentOutOfRange
                    else
                    {
                        sommeR += Kernel[i + 1, j + 1] * this.pic[x + i, y + j].R;
                        sommeG += Kernel[i + 1, j + 1] * this.pic[x + i, y + j].G;
                        sommeB += Kernel[i + 1, j + 1] * this.pic[x + i, y + j].B;
                    }
                    
                }
            }
            sommeR /= diviseur;
            sommeG /= diviseur;
            sommeB /= diviseur;


            if (sommeR > 255) sommeR = 255;
            if (sommeR < 0) sommeR = 0;
            if (sommeG > 255) sommeG = 255;
            if (sommeG < 0) sommeG = 0;
            if (sommeB > 255) sommeB = 255;
            if (sommeB < 0) sommeB = 0;
            return new Pixel((byte)((sommeR)), (byte)(sommeG), (byte)(sommeB));
        }

        /// <summary>
        /// Crée la matrice de Kernel pour la détection des contours et applique la méthode convolution 
        /// </summary>
        /// <returns>Nouvelle instance filtrée</returns>
        public MyImage DetectionContour()
        {
            double[,] KernelContour = { {0,1,0}, {1,-4,1}, {0,1,0} };
            MyImage Filtre = Convolution(KernelContour);
            return Filtre;
        }

        /// <summary>
        /// Crée la matrice de Kernel pour le flou et applique la méthode convolution
        /// </summary>
        /// <returns>Nouvelle insatnce filtrée</returns>
        public MyImage Flou()
        {
            double[,] KernelFlou = { { (double)1 / 16, (double)1 / 8, (double)1 / 16 }, { (double)1 / 8, (double)1 / 4, (double)1 / 8 }, { (double)1 / 16, (double)1 / 8, (double)1 / 16 } };
            MyImage Filtre = Convolution(KernelFlou);
            return Filtre;
        }

        /// <summary>
        /// Crée la matrice de Kernel pour le renforcement des bords et applique la méthode convolution
        /// </summary>
        /// <returns>nouvelle instance filtrée</returns>
        public MyImage RenforcementBords()
        {
            double[,] KernelBords = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            MyImage Filtre = Convolution(KernelBords);
            return Filtre;
        }

        /// <summary>
        /// Crée la matrice de Kernel pour le repoussage et applique la méthode convolution
        /// </summary>
        /// <returns>Nouvelle instance filtrée</returns>
        public MyImage Repoussage()
        {
            double[,] KernelRepoussage = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            MyImage Filtre = Convolution(KernelRepoussage);
            return Filtre;
        }

        /// <summary>
        /// Méthode test qui n'applique aucun filtre avec un matrice de Kernel neutre
        /// </summary>
        /// <returns></returns>
        public MyImage Null()//test aucun filtre
        {
            double[,] KernelNothing = { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
            MyImage Filtre = Convolution(KernelNothing);
            return Filtre;
        }

        /// <summary>
        /// Méthode qui permet une roattion de l'image
        /// </summary>
        /// <param name="degRot"></param>
        /// <returns></returns>
        public MyImage Rotation(double degRot)
        {
            double Rad = (degRot * Math.PI) / 180;
            double HeigthMiddle = this.Height / 2.0;
            double WidthMiddle = this.Width / 2.0;
            double maxSize = Math.Sqrt(HeigthMiddle * HeigthMiddle + WidthMiddle * WidthMiddle);           

            MyImage imageRot = new MyImage(this.Height, this.Width);

            //Rotating
            double module;
            double angle;
            double X;
            double Y;

            double x2;
            double y2;
            for (int i = 0; i < this.Height; i++)
                for (int j = 0; j < this.Width; j++)
                {
                    x2 = j - WidthMiddle;
                    y2 = i - HeigthMiddle;
                    module = Math.Sqrt(x2 * x2 + y2 * y2);

                    if (module > maxSize)//Optimisation
                        continue;

                    angle = Math.Atan2(y2, x2) + Rad;
                    X = Math.Cos(angle) * module;
                    Y = Math.Sin(angle) * module;

                    if (HeigthMiddle + Y >= 0 && HeigthMiddle + Y < Height && WidthMiddle + X >= 0 && WidthMiddle + X < Width)
                    {
                        imageRot.pic[i, j] = this.pic[(int)(HeigthMiddle + Y), (int)(WidthMiddle + X)];
                    }
                }

            return imageRot;
        }

        #endregion


        #region<Histogrammes>
        /// <summary>
        /// Crée une image représentant l'histogramme du bleu présent dans l'image à laquelle est appliqué la méthode
        /// </summary>
        /// <returns>Instance représentant l'histogramme du bleu</returns>
        public MyImage HistogrammeBleu()
        {
            MyImage Histo = new MyImage(100, 256);//Création histogramme vierge
            for(int i = 0; i < 100; i++)
            {
                for(int j = 0; j < 256; j++)
                {
                    Histo.pic[i, j] = Pixel.WHITE;
                }
            }
            
            int[] TabHisto = new int[256];
            for (int i = 0; i < Height; i++)//Compter les occurences pour chaque valeur de bleu
            {
                for (int j = 0; j < Width; j++)
                {
                    TabHisto[this.pic[i, j].B]++;

                }
            }
            
            int max = 0;
            for(int i = 0; i < TabHisto.Length; i++)
            {
                if (max < TabHisto[i]) max = TabHisto[i];
            } 
            for(int j = 0; j < 256; j++)
            {              

                for (int i = 0; i < (int)Math.Round(((double)TabHisto[j] / (double)max) * 100); i++)
                {
                    Histo.pic[99 - i, j] = new Pixel((byte)j, 0, 0);
                }
            }
            return Histo;
        }

        /// <summary>
        /// Crée une image représentant l'histogramme du rouge présent dans l'image à laquelle est appliqué la méthode
        /// </summary>
        /// <returns>Instance représentant l'histogramme du rouge</returns>
        public MyImage HistogrammeRouge()
        {
            MyImage Histo = new MyImage(100, 256);//Création histogramme vierge
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    Histo.pic[i, j] = Pixel.WHITE;
                }
            }

            int[] TabHisto = new int[256];
            for (int i = 0; i < Height; i++)//Compter les occurences pour chaque valeur de rouge
            {
                for (int j = 0; j < Width; j++)
                {
                    TabHisto[this.pic[i, j].R]++;

                }
            }

            int max = 0;
            for (int i = 0; i < TabHisto.Length; i++)
            {
                if (max < TabHisto[i]) max = TabHisto[i];
            }
            for (int j = 0; j < 256; j++)
            {

                for (int i = 0; i < (int)Math.Round(((double)TabHisto[j] / (double)max) * 100); i++)
                {
                    Histo.pic[99 - i, j] = new Pixel(0, 0, (byte)j);
                }
            }
            return Histo;
        }

        /// <summary>
        /// Crée une image représentant l'histogramme du Vert présent dans l'image à laquelle est appliqué la méthode
        /// </summary>
        /// <returns>Instance représentant l'histogramme du vert</returns>
        public MyImage HistogrammeVert()
        {
            MyImage Histo = new MyImage(100, 256);//Création histogramme vierge
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    Histo.pic[i, j] = Pixel.WHITE;
                }
            }

            int[] TabHisto = new int[256];
            for (int i = 0; i < Height; i++)//Compter les occurences pour chaque valeur de vert
            {
                for (int j = 0; j < Width; j++)
                {
                    TabHisto[this.pic[i, j].G]++;

                }
            }

            int max = 0;
            for (int i = 0; i < TabHisto.Length; i++)
            {
                if (max < TabHisto[i]) max = TabHisto[i];
            }
            for (int j = 0; j < 256; j++)
            {

                for (int i = 0; i < (int)Math.Round(((double)TabHisto[j] / (double)max) * 100); i++)
                {
                    Histo.pic[99 - i, j] = new Pixel(0, (byte)j, 0);
                }
            }
            return Histo;
        }

        /// <summary>
        /// Crée une image représentant l'histogramme d'exposition de l'image à laquelle est applqiuée la méthode
        /// </summary>
        /// <returns>Instance représentant l'histogramme d'exposition</returns>
        public MyImage HistogrammeBAndW()
        {
            MyImage Histo = new MyImage(100, 256);//Création histogramme vierge
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    Histo.pic[i, j] = Pixel.WHITE;
                }
            }

            int[] TabHisto = new int[256];
            for (int i = 0; i < Height; i++)//Compter les occurences pour chaque valeur de vert
            {
                for (int j = 0; j < Width; j++)
                {
                    int moyenne = (int)((double)(this.pic[i, j].R + this.pic[i, j].G + this.pic[i, j].B) / (double)3);
                    TabHisto[moyenne]++;

                }
            }

            int max = 0;
            for (int i = 0; i < TabHisto.Length; i++)
            {
                if (max < TabHisto[i]) max = TabHisto[i];
            }
            for (int j = 0; j < 256; j++)
            {

                for (int i = 0; i < (int)Math.Round(((double)TabHisto[j] / (double)max) * 100); i++)
                {
                    Histo.pic[99 - i, j] = new Pixel((byte)j, (byte)j, (byte)j);
                }
            }
            return Histo;
        }

        #endregion

        /// <summary>
        /// Appelle la fonction MyImage obj.Fractale
        /// </summary>
        /// <returns></returns>
        public static MyImage Fractale()
        {
            Console.WriteLine("Veuillez saisir les dimensions (3500x3500 recommandé) : ");
            Console.WriteLine("Hauteur : ");
            int Hauteur = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Largeur : ");
            int Largeur = Convert.ToInt32(Console.ReadLine());
            return Fractale(Hauteur, Largeur);
        }



        /// <summary>
        /// Crée une instance représentan les fractale de Mandelbrot
        /// </summary>
        /// <returns>instance de la fractale</returns>
        public static MyImage Fractale(int Hauteur, int Largeur)
        {
            
            Console.WriteLine("Veuillez patientez, cela peut prendre quelques instants...");
            MyImage Fractale = new MyImage(Hauteur, Largeur);
            for(int i = 0; i < Hauteur; i++)
            {
                for(int j = 0; j < Largeur; j++)
                {
                    Fractale.pic[i, j] = new Pixel(0, 0, 0);
                }
            }
            for(int i = 0; i < Hauteur; i++)
            {
                for(int j = 0; j < Largeur; j++)
                { 
                    int compteur = 0;
                    
                    double a = (double)(i - (Largeur/ 2)) / (double)(Largeur / 4); //mise à l'échelle
                    double b = (double)(j - (Hauteur / 2)) / (double)(Hauteur / 4);//mise à l'échelle
                    Complexe z = new Complexe(0, 0);
                    Complexe c = new Complexe(a, b);
                    while (compteur < 100)
                    {
                        z = (z.MultiplicationComplexe(z)).SommeComplexe(c);
                        compteur++;
                        if (z.Module > 2) break;
                    }
                    if (compteur < 100)
                    {
                        Fractale.pic[i, j] = new Pixel((byte)(compteur*2), (byte)(compteur), (byte)(compteur/2));
                    }
                    else
                    {
                        Fractale.pic[i, j] = new Pixel(128, 128, 128);
                    }
                }
            }
           
            
            return Fractale;
        }
        #endregion
    }
}
