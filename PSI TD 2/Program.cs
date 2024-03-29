﻿using System;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace PSI_TD_2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Intro();
            Console.ReadKey();
            Console.Clear();
            MyImage Image = Choix_Fichier();
            bool leave = false;
            bool choice = false;
            int n = 0;
            do
            {
                Menu(Image,choice);
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

            Conclu();
        }

        #region<Méthodes de conversion>
        
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
        /// Convert en string un int en binaire
        /// </summary>
        /// <param name="val">valeur decimale</param>
        /// <returns>string représentant va en binaire</returns>
        public static string TestIntToEndian(int val) 
        {
            string result = "";
            int value = val;
            while (value != 0)
            {
                result += Convert.ToString(Convert.ToByte(value % 256)) + " ";

                value /= 256;
            }
            return result;
        }

        /// <summary>
        /// Permet d'écrire un nombre sur un fichier
        /// </summary>
        /// <param name="n">nombre</param>
        /// <param name="s">stream</param>
        public static void WriteInt(int n, Stream s) //Stream car + simple et utilise moins de mémoire
        {
            for (int i = 0; i < 4; i++)
            {
                s.WriteByte((byte)(n % 256));  // Ecrit int en endian dans le stream
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
        /// Introduction du programme
        /// </summary>
        public static void Intro()
        {
            for(int i = 0; i < 6; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine("                                      -----|| PROJET SCIENTIFIQUE INFORMATIQUE ||-----");
            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine("                              ---| Présenté par Jade BETTOYA et Thibault BIVILLE ESILV A2 |---");
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine("                                          Cliquez sur une touche pour continuer...");
        }

        /// <summary>
        /// Crédits
        /// </summary>
        public static void Conclu()
        {
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine("                           --Ce Projet vous a été proposé par Jade BETTOYA et Thibault BIVILLE--");
            Console.ReadKey();
        }

        /// <summary>
        /// Menu de selection principal
        /// </summary>
        /// <param name="Image">Instance utilisée</param>
        public static void Menu(MyImage Image,bool b)
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
                    "8- Qr Code\n" +
                    "9- Négatif\n" +
                    "10- Quitter le programme");
                n = Convert.ToInt32(Console.ReadLine());
                if (n == null) n = 0;
                choice = Test_Bool_Selections(n,10);
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
                case 3:Rotate(Image);
                    break;
                case 4: Miroir(Image);
                    break;
                case 5: Zoom(Image);
                    break;
                case 6: Sous_Choix_Histogrammes(Image);
                    break;
                case 7:Fractales_Menu();
                    break;
                case 8:Init_QR();
                    break;
                case 9:Negatif(Image);
                    break;
                case 10: b = true; break;

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

        public static void Rotate(MyImage Image)
        {
            Console.Clear();
            Console.WriteLine("Veuillez choisir l'angle de rotation : ");
            double angle = Convert.ToDouble(Console.ReadLine());
            MyImage img = Image.Rotation(angle);
            img.Save("Filtre_Rotation.bmp");
            Console.Clear();
            Console.WriteLine("L'image a été enregistrée sous le nom : \n" +
                "Filtre_Rotation.bmp\n" +
                "Veuillez saisir une touche pour continuer...");
            Console.ReadKey();
            Console.Clear();

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
        /// Permet d'appeler la méthode d'instance MyImage.Fractale
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

        /// <summary>
        /// Méthode qui lance la création et la sauvegarde en .bmp d'un qr code
        /// </summary>
        public static void Init_QR()
        {
            Console.Clear();
            string msg = "11111111111111111111111111111111111";
            bool choice = false;
            do
            {
                Console.WriteLine("Veuillez sasir votre message à encoder (moins de 34 caractères):");
                msg = Console.ReadLine();
                if (msg.Length < 34) choice = true;
                if (!choice) Console.WriteLine("Message trop long");
            } while (!choice);
            QRCode myQR = new QRCode(msg);
            MyImage QR = myQR.Convert_QR_To_Image();
            Console.Clear();
            Console.WriteLine("Votre fichier à été enregistré sous le nom :\n" +
                "QR_Code.bmp\n" +
                "Veuillez appuyer sur une touche pour continuer...");
            Console.ReadKey();
        }

        /// <summary>
        /// Permet d'appeler la fonction prr appliquer le filtre négatif
        /// </summary>
        /// <param name="Image">image de base</param>
        public static void Negatif(MyImage Image)
        {
            Console.Clear();
            MyImage im = Image.Negatif();
            im.Save("Filtre_Negatif.bmp");
            Console.WriteLine("L'image a été enregistrée sous le nom :\n" +
                "Filtre_Negatif.bmp\n" +
                "Veuillez saisir une touche pour continuer...");
            Console.ReadKey();
            Console.Clear();
        }
        #endregion

    }
}
