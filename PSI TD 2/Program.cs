using System;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace PSI_TD_2
{
    public class Program
    {
        static void Main(string[] args)
        {

            MyImage Image = Choix_Fichier();
            bool leave = false;
            bool choice = false;
            int n = 0;
            do
            {
                Menu(Image);
                Console.Clear();
                
                do
                {
                    Console.WriteLine("1- Quitter le programme\n" +
                    "2- Faire une autre action");
                    n = Convert.ToInt32(Console.ReadLine());
                    if (n == null) n = 0;
                    choice = Test_Bool_Selections(n, 2);
                    Console.Clear();
                    if(!choice)Console.WriteLine("Veuillez saisir un choix valide"); 
                } while (!choice);
                leave = Fin_Action(n);
                
            } while (!leave);

            /*QRCode qr = new QRCode(2);
            qr.toStringQR();
            Console.WriteLine();
            qr.toStringModify();
            qr.Convert_QR_To_Image();
            string msg = "HELLO world";
            string a = QRCode.Encode_Message_In_Byte(msg,1);
            string b = "00100000010110110000101101111000110100010111001011011100010011010100001101000000 111011000001000111101100000100011110110000010001111011000001000111101100";
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(String.Equals(a,b));
            string msg = "HELLO WORLD";
            Encoding u8 = Encoding.UTF8;
            byte[] msgByte = u8.GetBytes(msg);
            byte[] result = ReedSolomon.ReedSolomonAlgorithm.Encode(msgByte, 7);
            foreach (byte val in result) Console.Write(val + " ");*/






        }

        #region<Méthodes de convertion>
        
        /// <summary>
        /// Convertis le n en endian dans le tableau d'octet à l'index choisi du header
        /// </summary>
        /// <param name="n">nombre à convertir</param>
        /// <param name="index">index du tableau</param>
        /// <param name="tab">tableau d'octet</param>
        public static void ConvertIntToEndian(int n, int index, byte[] tab)
        {
            //byte[] IntInEndian = BitConverter.GetBytes(val);
            //return IntInEndian;
            for (int i = 0; i < 4; i++)
            {
                tab[index + i] = (byte)(n % 256);
                n /= 256;
            }
        }

        /// <summary>
        /// Convertis un nombre du format endian en int
        /// </summary>
        /// <param name="data"></param>
        /// <returns>entier</returns>
        public static int ConvertEndianToInt(byte[] data)
        {
            //Array.Reverse(data);            //Inverse car little endian donne le plus faible en premier

            int EndianInInt = BitConverter.ToInt32(data, 0);
            Console.WriteLine("int: " + EndianInInt);
            return EndianInInt;
        }

        /// <summary>
        /// Permet d'écrire un nombre sur un fichier
        /// </summary>
        /// <param name="n">nombre</param>
        /// <param name="s">stream</param>
        public static void WriteInt(int n, Stream s)
        {
            for (int i = 0; i < 4; i++)
            {
                s.WriteByte((byte)(n % 256));
                n /= 256;
            }
        }


        /// <summary>
        /// Permet de lire un nombre en byte et de le donner en int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ReadInt(Stream s)
        {
            int n = 0;
            int exp = 1;
            for (int i = 0; i < 4; i++)
            {
                n += s.ReadByte() * exp;
                exp *= 256;
            }
            return n;
        }
        #endregion

        #region<Méthodes pour le Menu>

        /// <summary>
        /// Menu de selection principal
        /// </summary>
        /// <param name="Image">Instance utilisée</param>
        public static void Menu(MyImage Image)
        {
            int n = 0;
            bool choice = false;
            do
            {
                Console.WriteLine("Veuillez choisir l'action désirée : \n" +
                    "1- Application de Filtres couleurs\n" +
                    "2- Application de Filtres effets\n" +
                    "3- Rotation\n" +
                    "4- Miroir\n" +
                    "5- Agrandir/Rétrécir\n" +
                    "6- Histogrammes\n" +
                    "7- Fractales\n" +
                    "8- Qr Code");
                n = Convert.ToInt32(Console.ReadLine());
                if (n == null) n = 0;
                choice = Test_Bool_Selections(n,8);
                if (!choice)
                {
                    Console.Clear();
                    Console.WriteLine("Veuillez faire un choix valide");
                }
                
            } while(!choice);
            switch (n)
            {
                case 1: Sous_Choix_Filtres_Couleurs(Image);
                    break;
                case 2:Sous_Choix_Filtres_Effets(Image);
                    break;
                case 3:Console.Clear();
                    Console.WriteLine("Effet Indisponible");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 4: Miroir(Image);
                    break;
                case 5: Zoom(Image);
                    break;
                case 6: Sous_Choix_Histogrammes(Image);
                    break;
                case 7:Fractales_Menu();
                    break;
                case 8:Console.WriteLine("Indisponible pour le moment");
                    Console.ReadKey();
                    break;

            }
            
        }

        /// <summary>
        /// Sous-menu pour les filtres de couleurs
        /// </summary>
        /// <param name="Image">Instance utilisée</param>
        public static void Sous_Choix_Filtres_Couleurs(MyImage Image)
        {
            Console.Clear();
            int n = 0;
            bool choice = false;
            do
            {
                Console.WriteLine("Veuillez choisir le Filtre Couleur désiré : \n" +
                    "1- Niveaux de Gris\n" +
                    "2- Noir et Blanc");
                n = Convert.ToInt32(Console.ReadLine());
                if (n == null) n = 0;
                choice = Test_Bool_Selections(n, 2);
                if (!choice)
                {
                    Console.Clear();
                    Console.WriteLine("Veuillez faire un choix valide");
                }
                

            } while (!choice);
            switch (n)
            {
                case 1:
                    MyImage Image_Grey = Image.Grey();
                    Console.Clear();
                    string newFileNameG = "Grey_Filter.bmp";
                    Image_Grey.Save("Grey_Filter.bmp");
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameG);
                    Console.ReadKey();
                    break;
                case 2:
                    MyImage Image_B_and_W = Image.ToBlackAndWhite();
                    Console.Clear();
                    string newFileNameBW = "B_and_W_Filter.bmp";
                    Image_B_and_W.Save("B_and_W_Filter.bmp");
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameBW + "\n" +
                        "Veuillez appuyer sur une touche pour continuer");
                    Console.ReadKey();
                    break;
            }
           

        }

        /// <summary>
        /// Sous-Menu pour les filtres d'effet (convolution)
        /// </summary>
        /// <param name="Image">Instance utilisée</param>
        public static void Sous_Choix_Filtres_Effets(MyImage Image)
        {
            Console.Clear();
            int n = 0;
            bool choice = false;
            do
            {
                Console.WriteLine("Veuillez choisir le Filtre Couleur désiré : \n" +
                    "1- Flou\n" +
                    "2- Détection des Contours\n" +
                    "3- Renforcement des Bords\n" +
                    "4- Repoussage");
                n = Convert.ToInt32(Console.ReadLine());
                if (n == null) n = 0;
                choice = Test_Bool_Selections(n, 4);
                if (!choice)
                {
                    Console.Clear();
                    Console.WriteLine("Veuillez faire un choix valide");
                }
               

            } while (!choice);
            switch (n)
            {
                case 1:
                    MyImage Image_Flou = Image.Flou();
                    Console.Clear();
                    string newFileNameBlur = "Flou_Filtre.bmp";
                    Image_Flou.Save(newFileNameBlur);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameBlur);
                    Console.ReadKey();
                    break;
                case 2:
                    MyImage Image_Contours = Image.DetectionContour();
                    Console.Clear();
                    string newFileNameContours = "Contours_Filtre.bmp";
                    Image_Contours.Save(newFileNameContours);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameContours + "\n" +
                        "Veuillez appuyer sur une touche pour continuer");
                    Console.ReadKey();
                    break;
                case 3:
                    MyImage Image_Bords = Image.RenforcementBords();
                    Console.Clear();
                    string newFileNameBords = "Bords_Filtre.bmp";
                    Image_Bords.Save(newFileNameBords);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameBords + "\n" +
                        "Veuillez appuyer sur une touche pour continuer");
                    Console.ReadKey();
                    break;
                case 4:
                    MyImage Image_Repoussage = Image.Repoussage();
                    Console.Clear();
                    string newFileNameRepoussage = "Repoussage_Filtre.bmp";
                    Image_Repoussage.Save(newFileNameRepoussage);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameRepoussage + "\n" +
                        "Veuillez appuyer sur une touche pour continuer");
                    Console.ReadKey();
                    break;
            }
            
            
        }

        /// <summary>
        /// Choix du fichier à utiliser et ouverture d'une instance associée
        /// </summary>
        /// <returns>instance</returns>
        public static MyImage Choix_Fichier()
        {
            int n=0;
            bool choice = false;
            do
            {
                Console.WriteLine("Choisir un fichier à utiliser :\n"
                                   + "1- coco.bmp\n"
                                   + "2- lac.bmp\n"
                                   + "3- lena.bmp\n"
                                   + "4- Image téléchargée dans le fichier de la solution");
                n = Convert.ToInt32(Console.ReadLine());
                if (n == null) n = 0;
                choice = Test_Bool_Selections(n, 4);
                if(!choice)
                {
                    Console.Clear();
                    Console.WriteLine("Veuillez faire un choix valide"); 
                }

            } while (!choice);

            if (n == 1)
            {
                Console.Clear();
                return new MyImage("coco.bmp");
                
            }
            if (n == 2)
            {
                Console.Clear();
                return new MyImage("lac.bmp");
            }
            if (n == 3)
            {
                Console.Clear();
                return new MyImage("lena.bmp");
            }
            if (n == 4)
            {
                Console.Clear();
                Console.WriteLine("Veuillez écrire le nom complet du fichier (.bmp) : ");
                string fileName = Console.ReadLine();
                return new MyImage(fileName);
            }
            Console.Clear();
            return null;
        }

        /// <summary>
        /// Méthode pour savoir si un nombre est compris entre 1 et le nombre de choix existant
        /// </summary>
        /// <param name="n">numéro entré</param>
        /// <param name="selectRange">nombre de possibilités</param>
        /// <returns>true si n est compris, false sinon</returns>
        public static bool Test_Bool_Selections(int n,int selectRange)
        {
            bool test = false;
            if (n == null) return false;
            for(int i = 1; i <= selectRange; i++)
            {
                if (n == i) test = true;
            }

            return test;

        }
       
        /// <summary>
        /// Méthode appelé après une action pour sortir du programme ou continuer
        /// </summary>
        /// <param name="select">Choix</param>
        /// <returns></returns>
        public static bool Fin_Action(int select)
        {
            if (select == 1) return true;
            if (select == 2) return false;
            return false;
        }

        /// <summary>
        /// Méthode appelée quand on choisit l'effet miroir, elle applqiue l'effet à l'image utilisée
        /// </summary>
        /// <param name="Image">Instance utilisée</param>
        public static void Miroir(MyImage Image)
        {
            Console.Clear();
            int n = 0;
            bool choice = false;
            do
            {
                Console.WriteLine("Veuillez choisir le sens désiré : \n" +
                    "1- Horizontal\n" +
                    "2- Vertical");
                n = Convert.ToInt32(Console.ReadLine());
                choice = Test_Bool_Selections(n, 2);
                if (!choice)
                {
                    Console.Clear();
                    Console.WriteLine("Veuillez faire un choix valide");
                }


            } while (!choice);
            switch (n)
            {
                case 1:
                    MyImage Image_Horizontale = Image.MirroirHorizontal();
                    Console.Clear();
                    string newFileNameHoriz = "Horizontal_Filtre.bmp";
                    Image_Horizontale.Save(newFileNameHoriz);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameHoriz);
                    Console.ReadKey();
                    break;
                case 2:
                    MyImage Image_Verticale = Image.MirroirVertical();
                    Console.Clear();
                    string newFileNameVerti = "Vertical_Filtre.bmp";
                    Image_Verticale.Save(newFileNameVerti);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameVerti + "\n" +
                        "Veuillez appuyer sur une touche pour continuer");
                    Console.ReadKey();
                    break;
            }
            Console.Clear();
            
        }

        /// <summary>
        /// Méthode appelée quand on choisit l'effet agrandir/retrecir, elle applqiue l'effet à l'image utilisée
        /// </summary>
        /// <param name="Image">instance utilisée</param>
        public static void Zoom(MyImage Image)
        {
            Console.Clear();
            Console.WriteLine("Veuillez saisir le facteur d'agrandissement/de rétrecissement : ");           
            MyImage Image_Zoomed = Image.Agrandir(Convert.ToDouble(Console.ReadLine()));
            Image_Zoomed.Save("Zoom_Filtre.bmp");
            Console.Clear();
            Console.WriteLine("L'image a été enregistrée sous le nom : \n" +
                "Zoom_Filtre.bmp\n" +
                "Veuillez appuyer sur une touche pour continuer...");
            Console.ReadKey();
            Console.Clear();
            
        }

        /// <summary>
        /// Sous-menu permettant de choisir l'histogramme voulu
        /// </summary>
        /// <param name="Image">instance utilisée</param>
        public static void Sous_Choix_Histogrammes(MyImage Image)
        {
            Console.Clear();
            int n = 0;
            bool choice = false;
            do
            {
                Console.WriteLine("Veuillez choisir l'Histogramme désiré : \n" +
                    "1- Histogramme des Rouges\n" +
                    "2- Histogramme des Verts\n" +
                    "3- Histogramme des Bleus\n" +
                    "4- Histogramme d'exposition\n" +
                    "5- Tous les Histogrammes");
                n = Convert.ToInt32(Console.ReadLine());
                if (n == null) n = 0;
                choice = Test_Bool_Selections(n, 5);
                if (!choice)
                {
                    Console.Clear();
                    Console.WriteLine("Veuillez faire un choix valide");
                }


            } while (!choice);
            switch (n)
            {
                case 1:
                    MyImage Histogramme_Rouge = Image.HistogrammeRouge();
                    Console.Clear();
                    string newFileNameHistoR = "Histogramme_Rouge.bmp";
                    Histogramme_Rouge.Save(newFileNameHistoR);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameHistoR+"\n"+
                        "Veuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    break;
                case 2:
                    MyImage Histogramme_Vert = Image.HistogrammeVert();
                    Console.Clear();
                    string newFileNameHistoV = "Histogramme_Vert.bmp";
                    Histogramme_Vert.Save(newFileNameHistoV);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameHistoV + "\n" +
                        "Veuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    break;
                case 3:
                    MyImage Histogramme_Bleu = Image.HistogrammeBleu();
                    Console.Clear();
                    string newFileNameHistoB = "Histogramme_Bleu.bmp";
                    Histogramme_Bleu.Save(newFileNameHistoB);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameHistoB + "\n" +
                        "Veuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    break;
                case 4:
                    MyImage Histogramme_Exposition = Image.HistogrammeBAndW();
                    Console.Clear();
                    string newFileNameExpo = "Histogramme_Exposition.bmp";
                    Histogramme_Exposition.Save(newFileNameExpo);
                    Console.WriteLine("L'image a été sauvergardée sous le nom : \n" +
                        newFileNameExpo + "\n" +
                        "Veuillez appuyer sur une touche pour continuer...");
                    Console.ReadKey();
                    break;
                case 5:
                    MyImage Histogramme_Rouge_Complet = Image.HistogrammeRouge();
                    Console.Clear();
                    string newFileNameHistoRComp = "Histogramme_Rouge.bmp";
                    Histogramme_Rouge_Complet.Save(newFileNameHistoRComp);
                    MyImage Histogramme_Vert_Complet = Image.HistogrammeVert();
                    Console.Clear();
                    string newFileNameHistoVComp = "Histogramme_Vert.bmp";
                    Histogramme_Vert_Complet.Save(newFileNameHistoVComp);
                    MyImage Histogramme_Bleu_Complet = Image.HistogrammeBleu();
                    Console.Clear();
                    string newFileNameHistoBComp = "Histogramme_Bleu.bmp";
                    Histogramme_Bleu_Complet.Save(newFileNameHistoBComp);
                    MyImage Histogramme_Exposition_Complet = Image.HistogrammeBAndW();
                    Console.Clear();
                    string newFileNameExpoComp = "Histogramme_Exposition.bmp";
                    Histogramme_Exposition_Complet.Save(newFileNameExpoComp);
                    Console.WriteLine("Les fichier ont été enregistrésous les noms :\n" +
                        newFileNameHistoRComp+"\n"+
                        newFileNameHistoVComp+"\n" +
                        newFileNameHistoBComp+"\n" +
                        newFileNameExpoComp+"\n" +
                        "Veuillez saisir une touche pour continuer...");
                    Console.ReadKey();
                    break;
            }
        }

        /// <summary>
        /// Permet d'appelé la méthode d'instance MyImage.Fractale
        /// </summary>
        public static void Fractales_Menu()
        {
            Console.Clear();
            MyImage Fractale = MyImage.Fractale();
            Fractale.Save("Fractale.bmp");
            Console.Clear();
            Console.WriteLine("Le fichier a été enregistré sous le nom : \n" +
                "Fractale.bmp");
            Console.WriteLine("Veuillez appuyer sur une touche pour continuer...");
            Console.ReadKey();
            Console.Clear();
        }
        #endregion

    }
}
