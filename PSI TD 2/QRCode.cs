using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PSI_TD_2
{
    public class QRCode
    {

        #region<Attributs>

        int type;
        bool[,] modify;
        int[,] QR;  //255 => case blanche | 0 => case noire

        #endregion

        #region<Constructeur>
        /// <summary>
        /// Construit un QR code à partir d'un string
        /// </summary>
        /// <param name="mot">mot à placer dans le qr code</param>
        public QRCode(string mot)
        {
            this.type = ChoixVersion(mot);
            this.QR = new int[((type - 1) * 4) + 21, ((type - 1) * 4) + 21];
            this.modify = new bool[((type - 1) * 4) + 21, ((type - 1) * 4) + 21];
            //placement des modules et du masque
            if (type == 1)
            {
                Placement_Modules_V1();
                Apply_EC_V1();
            }
            if (type == 2)
            {
                Placement_Modules_V2();
                Apply_EC_V2();
            }
            if (type == 0) QR = null;
            string binaryMsg = Encode_Message_In_Byte(mot, type);
            Write_On_QR(binaryMsg);

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  }
        #endregion

        #region<Méthodes>

        /// <summary>
        /// Méthodes appelée par le constructeur permettant de placer tout les modules du QR Code pour une version 1
        /// </summary>
        public void Placement_Modules_V1()
        {
            //initialisation des matrices du QRcode

            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    this.QR[i, j] = 255;
                    this.modify[i, j] = true;//true = modifiable
                }
            }

            //application des repères
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    //Rendre les cases non modifiables : false = non-modifiable
                    this.modify[i, j] = false; //Repère haut gauche
                    this.modify[i, 20 - j] = false;//repère haut droite
                    this.modify[20 - i, j] = false; //repère bas droite

                    if (i == 0 || i == 6)
                    {
                        //Repère haut gauche
                        this.QR[i, j] = 0;


                        //repère haut droite
                        this.QR[i, 20 - j] = 0;


                        //repère bas droite
                        this.QR[20 - i, j] = 0;

                    }
                    else
                    {
                        if (i == 1 || i == 5)
                        {
                            if (j == 0 || j == 6)
                            {
                                this.QR[i, j] = 0;
                                this.QR[i, 20 - j] = 0;
                                this.QR[20 - i, j] = 0;
                            }
                        }
                        else
                        {
                            if (j == 0 || j == 2 || j == 3 || j == 4 || j == 6)
                            {
                                this.QR[i, j] = 0;
                                this.QR[i, 20 - j] = 0;
                                this.QR[20 - i, j] = 0;
                            }
                        }
                    }

                    //Application des separators
                    if (i != 6 && j == 6)
                    {
                        this.modify[i, j + 1] = false;
                        this.modify[i, 20 - j - 1] = false;
                        this.modify[20 - i, j + 1] = false;
                    }
                    if (i == 6)
                    {

                        this.modify[i + 1, j] = false;
                        this.modify[i + 1, 20 - j] = false;
                        this.modify[20 - i - 1, j] = false;
                        if (j == 6)
                        {
                            this.modify[i, j + 1] = false;
                            this.modify[i, 20 - j - 1] = false;
                            this.modify[20 - i, j + 1] = false;
                            this.modify[i + 1, j + 1] = false;
                            this.modify[i + 1, 20 - j - 1] = false;
                            this.modify[20 - i - 1, j + 1] = false;
                        }
                    }
                }
            }



            //application des traits discontinus
            for (int k = 8; k < 13; k++)
            {
                this.modify[6, k] = false;
                this.modify[k, 6] = false;
                if (k % 2 == 0)
                {
                    this.QR[6, k] = 0;
                    this.QR[k, 6] = 0;
                }
            }

            //application du dark module
            this.modify[13, 8] = false;
            this.QR[13, 8] = 0;
        }

        /// <summary>
        /// Méthodes appelée par le constructeur permettant de placer tout les modules du QR Code pour une version 2
        /// </summary>
        public void Placement_Modules_V2()
        {
            //initialisation des matrices du QRcode

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    this.QR[i, j] = 255;
                    this.modify[i, j] = true;//true = modifiable
                }
            }

            //application des repères
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    //Rendre les cases non modifiables : false = non-modifiable
                    this.modify[i, j] = false; //Repère haut gauche
                    this.modify[i, 24 - j] = false;//repère haut droite
                    this.modify[24 - i, j] = false; //repère bas droite

                    if (i == 0 || i == 6)
                    {
                        //Repère haut gauche
                        this.QR[i, j] = 0;


                        //repère haut droite
                        this.QR[i, 24 - j] = 0;


                        //repère bas droite
                        this.QR[24 - i, j] = 0;

                    }
                    else
                    {
                        if (i == 1 || i == 5)
                        {
                            if (j == 0 || j == 6)
                            {
                                this.QR[i, j] = 0;
                                this.QR[i, 24 - j] = 0;
                                this.QR[24 - i, j] = 0;
                            }
                        }
                        else
                        {
                            if (j == 0 || j == 2 || j == 3 || j == 4 || j == 6)
                            {
                                this.QR[i, j] = 0;
                                this.QR[i, 24 - j] = 0;
                                this.QR[24 - i, j] = 0;
                            }
                        }
                    }

                    //Application des separators
                    if (i != 6 && j == 6)
                    {
                        this.modify[i, j + 1] = false;
                        this.modify[i, 24 - j - 1] = false;
                        this.modify[24 - i, j + 1] = false;
                    }
                    if (i == 6)
                    {

                        this.modify[i + 1, j] = false;
                        this.modify[i + 1, 24 - j] = false;
                        this.modify[24 - i - 1, j] = false;
                        if (j == 6)
                        {
                            this.modify[i, j + 1] = false;
                            this.modify[i, 24 - j - 1] = false;
                            this.modify[24 - i, j + 1] = false;
                            this.modify[i + 1, j + 1] = false;
                            this.modify[i + 1, 24 - j - 1] = false;
                            this.modify[24 - i - 1, j + 1] = false;
                        }
                    }
                }
            }



            //application des traits discontinus
            for (int k = 8; k < 17; k++)
            {
                this.modify[6, k] = false;
                this.modify[k, 6] = false;
                if (k % 2 == 0)
                {
                    this.QR[6, k] = 0;
                    this.QR[k, 6] = 0;
                }
            }

            //application du dark module
            this.modify[17, 8] = false;
            this.QR[17, 8] = 0;

            //Alignment pattern
            for(int i = 16; i < 21; i++)
            {
                for(int j = 16; j < 21; j++)
                {
                    this.modify[i, j] = false;
                    if (i == 16 || i==20)
                    {
                        this.QR[i, j] = 0;
                        
                    }
                    else
                    {
                        if(j==16 || j == 20)
                        {
                            this.QR[i, j] = 0;
                            
                        }
                    }
                    
                }
            }
            this.QR[18, 18] = 0;
            this.modify[18, 18] = false;
        }

        /// <summary>
        /// Méthode qui choisit la version la version du qr code à générer à partir de la taille du string
        /// </summary>
        /// <param name="mot"></param>
        /// <returns>1 pour version 1, 2 pour version 2, "Choix Invalide sinon"</returns>
        public static int ChoixVersion(string mot)
        {

            int version = 0;
            if (mot.Length <= 19)
            {
                version = 1;

            }
            if (mot.Length > 19 && mot.Length <= 34)
            {
                version = 2;
            }
            if(mot.Length>34)
            {
                Console.WriteLine("Version invalide");
            }
            return version;
        }

        /// <summary>
        /// Méthode permettant d'fficher sur la console une visuation du QR Code ('H' ->pixel noir | "." ->pixel blanc)
        /// </summary>
        public void toStringQR()
        {
            for(int i = 0; i < this.modify.GetLength(0); i++)
            {
                for(int j = 0; j < this.modify.GetLength(1); j++)
                {
                    if (this.QR[i,j]==255)
                    {
                        Console.Write(". ");
                    }
                    else
                    {
                        Console.Write("H ");
                    }
                    
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Méthode permettant de visualiser les cases modifiable dans la matrice QR ('H'->non-modifiable | '.'->modifiable
        /// </summary>
        public void toStringModify()
        {
            for (int i = 0; i < this.modify.GetLength(0); i++)
            {
                for (int j = 0; j < this.modify.GetLength(1); j++)
                {
                    if (this.modify[i,j])
                    {
                        Console.Write(". ");
                    }
                    else
                    {
                        Console.Write("H ");
                    }

                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Méthode permmetant de transformer le QR Code en instance MyImage afin de le sauvegarder sous format .bmp
        /// </summary>
        public MyImage Convert_QR_To_Image()
        {
            MyImage QR = new MyImage(((type - 1) * 4) + 21, ((type - 1) * 4) + 21);
            for(int i = 0; i < QR.pic.GetLength(0); i++)
            {
                for(int j = 0; j < QR.pic.GetLength(1); j++)
                {
                    if (this.QR[i, j] == 255) QR.pic[i, j] = Pixel.WHITE;
                    else QR.pic[i, j] = Pixel.BLACK;
                }
            }
            QR = QR.Agrandir(10);
           QR.Save("QR_Code.bmp");
            return QR;
        }

        /// <summary>
        /// Permet d'encoder la suite de bits à écrire sur le QR code
        /// </summary>
        /// <param name="msg">message à encoder</param>
        /// <returns>string représenter la chaine de bits</returns>
        public static string Encode_Message_In_Byte(string msg,int type)
        {
            //Mise en capitales
            string msg_In_Cap = msg.ToUpper();

            //début données (code alpha numérique et taille msg
            string Données = "0010"; //initialisation avec l'alphanumérique

            //encodage de la taille du message sur 9 bits
            int[] DonnéesCodeAlphaNum = new int[msg.Length];
            string msgSizeString = Convert.ToString(Convert.ToByte(msg.Length),2);//convertit on byte en string sous forme binaire
            while (msgSizeString.Length % 9 != 0) 
            {
                msgSizeString = "0" + msgSizeString;
            }
            Données += msgSizeString;

            if(msg.Length % 2 == 0)
            {
                for (int i = 0; i < msg.Length - 1; i += 2)
                {
                    string cutData = Convert.ToString(msg_In_Cap[i]) + Convert.ToString(msg_In_Cap[i + 1]);
                    Données += Convert_To_QR_Format(cutData,11);
                }
            }
            else
            {
                for (int i = 0; i < msg.Length ; i += 2)
                {
                    if (i == msg.Length - 1)
                    {
                        string cutData = Convert.ToString(msg_In_Cap[i]);
                        Données += Convert_To_QR_Format(cutData,6);
                    }
                    else
                    {
                        string cutData = Convert.ToString(msg_In_Cap[i]) + Convert.ToString(msg_In_Cap[i + 1]);
                        Données += Convert_To_QR_Format(cutData,11);
                    }
                }
            }
            if (type == 1)
            {
                int pad = 152 - Données.Length;
                if (pad != 0)
                {
                    if (pad < 4)
                    {
                        while (pad != 0)
                        {
                            Données += "0";
                        }

                    }
                    else
                    {
                        Données += "0000"; //terminaison
                    }
                }
                while (Données.Length % 8 != 0)
                {
                    Données += "0";
                }
            }
            if (type == 2)
            {
                int pad = 272 - Données.Length;
                if (pad != 0)
                {
                    if (pad < 4)
                    {
                        while (pad != 0)
                        {
                            Données += "0";
                        }

                    }
                    else
                    {
                        Données += "0000"; //terminaison
                    }
                }
                while (Données.Length % 8 != 0)
                {
                    Données += "0";
                }
            }

            //ajout des pavés 236 17
            int nbPavés;
            if (type == 1)
            {
                nbPavés = (152 - Données.Length) / 8;
            }
            else
            {
                nbPavés = (272 - Données.Length) / 8;
            }
            int compteur = 0;
            while (nbPavés != 0)
            {
                if (compteur % 2 == 0)
                {
                    Données += "11101100";//236
                }
                else
                {
                    Données += "00010001";//17
                }
                nbPavés -= 1;
                compteur++;
            }
            //Application du correcteur
            if(type == 1) Données += Apply_Correction_V1(Données,msg_In_Cap,type);
            if(type == 2) Données += Apply_Correction_V2(Données, msg_In_Cap, type);
            return Données;           
        }    

        /// <summary>
        /// Convertit un string écrit en string représentant le message en binaire
        /// </summary>
        /// <param name="data">string à convertir</param>
        /// <param name="taille">taille de la chaine de bits en sortie</param>
        /// <returns>chaine de bits en string</returns>
        public static string Convert_To_QR_Format(string data,int taille)
        {
            string result = "";

            //tableau alphanumérique
            string AlphaNum = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            int[] dataAlphanumCode = new int[data.Length];
            for(int i = 0; i < data.Length; i++)
            {
                for(int j = 0; j < AlphaNum.Length; j++)
                {
                    if (data[i] == AlphaNum[j])
                    {
                        dataAlphanumCode[i] = j;//ex : si data ="HE" -> AlphanumCode = {17,14}
                    }
                }
            }
            int nbEqData;
            if (data.Length == 2)
            {
                nbEqData = (45 * dataAlphanumCode[0]) + dataAlphanumCode[1];

            }
            else
            {
                nbEqData = ( dataAlphanumCode[0]);
            }
            result = Convert.ToString(nbEqData, 2);
            while (result.Length % taille != 0)
            {
                result = "0" + result;
            }

            return result;
        }

        /// <summary>
        /// Génére les bits de corrections à l'aide de Reed-Solomon et les ajoute au code binaire du qr code version 1
        /// </summary>
        /// <param name="data">string binaire</param>
        /// <param name="msg">message initial</param>
        /// <returns></returns>
        public static string Apply_Correction_V1(string data, string msg,int type)
        {

            byte[] msgByte = Convert_Text_To_AlphaNum_Byte(msg);
            byte[] result = ReedSolomon.ReedSolomonAlgorithm.Encode(msgByte, 7, ReedSolomon.ErrorCorrectionCodeType.QRCode); 
            
            Console.WriteLine(result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                if(Convert.ToString(result[i], 2).Length != 8) 
                {
                    string ajout =  Convert.ToString(result[i], 2);
                    while(ajout.Length != 8)
                    {
                        ajout = "0" + ajout;
                    }
                    data += ajout;
                    
                }
                
            }
            return data;
        }

        /// <summary>
        /// Génére les bits de corrections à l'aide de Reed-Solomon et les ajoute au code binaire du qr code version 2
        /// </summary>
        /// <param name="data">string binaire</param>
        /// <param name="msg">message initial</param>
        /// <returns></returns>
        public static string Apply_Correction_V2(string data, string msg, int type)
        {

            byte[] msgByte = Convert_Text_To_AlphaNum_Byte(msg);
            byte[] result = ReedSolomon.ReedSolomonAlgorithm.Encode(msgByte, 10, ReedSolomon.ErrorCorrectionCodeType.QRCode);

            Console.WriteLine(result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                if (Convert.ToString(result[i], 2).Length != 8)
                {
                    string ajout = Convert.ToString(result[i], 2);
                    while (ajout.Length != 8)
                    {
                        ajout = "0" + ajout;
                    }
                    data += ajout;

                }

            }
            return data;
        }

        /// <summary>
        /// Associe à un string un tableau de byte avec le code alpha numérique de chaque caractère 
        /// </summary>
        /// <param name="msg">message</param>
        /// <returns>tableau avec le message en code alphanumérique</returns>
        public static byte[] Convert_Text_To_AlphaNum_Byte(string msg)
        {
            byte[] msgEncoding = new byte[msg.Length];
            string AlphaNum = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ $%*+-./:";
            int[] dataAlphanumCode = new int[msg.Length];
            for (int i = 0; i < msg.Length; i++)
            {
                for (int j = 0; j < AlphaNum.Length; j++)
                {
                    if (msg[i] == AlphaNum[j])
                    {
                        dataAlphanumCode[i] = j;//ex : si data ="HE" -> AlphanumCode = {17,14}
                    }
                }
            }
            for(int i = 0; i < dataAlphanumCode.Length; i++)
            {
                msgEncoding[i] = (byte)dataAlphanumCode[i];
            }
            return msgEncoding;
        }

        /// <summary>
        /// Applique les bits de corrections et de donnée sur la matrice de base (sans le message) sur une version 1
        /// </summary>
        public void Apply_EC_V2()
        {
            string mask = "111011111000100";
            #region<QR>
            this.QR[8, 0] = Return_value_on_Bit_Mask(mask[0]);
            this.QR[8, 1] = Return_value_on_Bit_Mask(mask[1]);
            this.QR[8, 2] = Return_value_on_Bit_Mask(mask[2]);
            this.QR[8, 3] = Return_value_on_Bit_Mask(mask[3]);
            this.QR[8, 4] = Return_value_on_Bit_Mask(mask[4]);
            this.QR[8, 5] = Return_value_on_Bit_Mask(mask[5]);
            this.QR[8, 7] = Return_value_on_Bit_Mask(mask[6]);
            this.QR[8, 8] = Return_value_on_Bit_Mask(mask[7]);

            this.QR[7, 8] = Return_value_on_Bit_Mask(mask[8]);
            this.QR[5, 8] = Return_value_on_Bit_Mask(mask[9]);
            this.QR[4, 8] = Return_value_on_Bit_Mask(mask[10]);
            this.QR[3, 8] = Return_value_on_Bit_Mask(mask[11]);
            this.QR[2, 8] = Return_value_on_Bit_Mask(mask[12]);
            this.QR[1, 8] = Return_value_on_Bit_Mask(mask[13]);
            this.QR[0, 8] = Return_value_on_Bit_Mask(mask[14]);

            this.QR[24, 8] = Return_value_on_Bit_Mask(mask[0]);
            this.QR[23, 8] = Return_value_on_Bit_Mask(mask[1]);
            this.QR[22, 8] = Return_value_on_Bit_Mask(mask[2]);
            this.QR[21, 8] = Return_value_on_Bit_Mask(mask[3]);
            this.QR[20, 8] = Return_value_on_Bit_Mask(mask[4]);
            this.QR[19, 8] = Return_value_on_Bit_Mask(mask[5]);
            this.QR[18, 8] = Return_value_on_Bit_Mask(mask[6]);

            this.QR[8, 17] = Return_value_on_Bit_Mask(mask[7]);
            this.QR[8, 18] = Return_value_on_Bit_Mask(mask[8]);
            this.QR[8, 19] = Return_value_on_Bit_Mask(mask[9]);
            this.QR[8, 20] = Return_value_on_Bit_Mask(mask[10]);
            this.QR[8, 21] = Return_value_on_Bit_Mask(mask[11]);
            this.QR[8, 22] = Return_value_on_Bit_Mask(mask[12]);
            this.QR[8, 23] = Return_value_on_Bit_Mask(mask[13]);
            this.QR[8, 24] = Return_value_on_Bit_Mask(mask[14]);
            #endregion

            #region<Modify>
            this.modify[8, 0] = false;
            this.modify[8, 1] = false;
            this.modify[8, 2] = false;
            this.modify[8, 3] = false;
            this.modify[8, 4] = false;
            this.modify[8, 5] = false;
            this.modify[8, 7] = false;
            this.modify[8, 8] = false;

            this.modify[7, 8] = false;
            this.modify[5, 8] = false;
            this.modify[4, 8] = false;
            this.modify[3, 8] = false;
            this.modify[2, 8] = false;
            this.modify[1, 8] = false;
            this.modify[0, 8] = false;

            this.modify[24, 8] = false;
            this.modify[23, 8] = false;
            this.modify[22, 8] = false;
            this.modify[21, 8] = false;
            this.modify[20, 8] = false;
            this.modify[19, 8] = false; 
            this.modify[18, 8] = false;

            this.modify[8, 17] = false;
            this.modify[8, 18] = false;
            this.modify[8, 19] = false;
            this.modify[8, 20] = false;
            this.modify[8, 21] = false;
            this.modify[8, 22] = false;
            this.modify[8, 23] = false;
            this.modify[8, 24] = false;
            #endregion

        }

        /// <summary>
        /// Applique les bits de corrections et de donnée sur la matrice de base (sans le message) sur une version 2
        /// </summary>
        public void Apply_EC_V1()
        {
            string mask = "111011111000100";
            #region<QR>
            this.QR[8, 0] = Return_value_on_Bit_Mask(mask[0]);
            this.QR[8, 1] = Return_value_on_Bit_Mask(mask[1]);
            this.QR[8, 2] = Return_value_on_Bit_Mask(mask[2]);
            this.QR[8, 3] = Return_value_on_Bit_Mask(mask[3]);
            this.QR[8, 4] = Return_value_on_Bit_Mask(mask[4]);
            this.QR[8, 5] = Return_value_on_Bit_Mask(mask[5]);
            this.QR[8, 7] = Return_value_on_Bit_Mask(mask[6]);
            this.QR[8, 8] = Return_value_on_Bit_Mask(mask[7]);

            this.QR[7, 8] = Return_value_on_Bit_Mask(mask[8]);
            this.QR[5, 8] = Return_value_on_Bit_Mask(mask[9]);
            this.QR[4, 8] = Return_value_on_Bit_Mask(mask[10]);
            this.QR[3, 8] = Return_value_on_Bit_Mask(mask[11]);
            this.QR[2, 8] = Return_value_on_Bit_Mask(mask[12]);
            this.QR[1, 8] = Return_value_on_Bit_Mask(mask[13]);
            this.QR[0, 8] = Return_value_on_Bit_Mask(mask[14]);

            this.QR[20, 8] = Return_value_on_Bit_Mask(mask[0]);
            this.QR[19, 8] = Return_value_on_Bit_Mask(mask[1]);
            this.QR[18, 8] = Return_value_on_Bit_Mask(mask[2]);
            this.QR[17, 8] = Return_value_on_Bit_Mask(mask[3]);
            this.QR[16, 8] = Return_value_on_Bit_Mask(mask[4]);
            this.QR[15, 8] = Return_value_on_Bit_Mask(mask[5]);
            this.QR[14, 8] = Return_value_on_Bit_Mask(mask[6]);

            this.QR[8, 13] = Return_value_on_Bit_Mask(mask[7]);
            this.QR[8, 14] = Return_value_on_Bit_Mask(mask[8]);
            this.QR[8, 15] = Return_value_on_Bit_Mask(mask[9]);
            this.QR[8, 16] = Return_value_on_Bit_Mask(mask[10]);
            this.QR[8, 17] = Return_value_on_Bit_Mask(mask[11]);
            this.QR[8, 18] = Return_value_on_Bit_Mask(mask[12]);
            this.QR[8, 19] = Return_value_on_Bit_Mask(mask[13]);
            this.QR[8, 20] = Return_value_on_Bit_Mask(mask[14]);
            #endregion

            #region<Modify>
            this.modify[8, 0] = false;
            this.modify[8, 1] = false;
            this.modify[8, 2] = false;
            this.modify[8, 3] = false;
            this.modify[8, 4] = false;
            this.modify[8, 5] = false;
            this.modify[8, 7] = false;
            this.modify[8, 8] = false;

            this.modify[7, 8] = false;
            this.modify[5, 8] = false;
            this.modify[4, 8] = false;
            this.modify[3, 8] = false;
            this.modify[2, 8] = false;
            this.modify[1, 8] = false;
            this.modify[0, 8] = false;

            this.modify[20, 8] = false;
            this.modify[19, 8] = false;
            this.modify[18, 8] = false;
            this.modify[17, 8] = false;
            this.modify[16, 8] = false;
            this.modify[15, 8] = false;
            this.modify[14, 8] = false;

            this.modify[8, 13] = false;
            this.modify[8, 14] = false;
            this.modify[8, 15] = false;
            this.modify[8, 16] = false;
            this.modify[8, 17] = false;
            this.modify[8, 18] = false;
            this.modify[8, 19] = false;
            this.modify[8, 20] = false;
            #endregion

        }

        /// <summary>
        /// Méthode qui retourne 255 si le char est '1', 0 sinon
        /// </summary>
        /// <param name="a">char à analyser</param>
        /// <returns>255 ou 0</returns>
        public static int Return_value_on_Bit_Mask(char a)
        {
            if (a == '1') return 0;
            return 255;
        }

        /// <summary>
        /// Ecrit le message binaire directement sur la matrice du qr code
        /// </summary>
        /// <param name="BinaryMsg">string message en binaire</param>
        public void Write_On_QR(string BinaryMsg)
        {
            bool up = true;//true si l'écriture est montante, false sinon
            int binaryCount = BinaryMsg.Length-1;
            for(int j = QR.GetLength(0)-1; j > 0; j-=2)
            {
                for(int i = QR.GetLength(0)-1; i > 0; i --)
                {
                    if (up) {
                        if (modify[i, j])
                        {
                            
                            if (BinaryMsg[BinaryMsg.Length - binaryCount] == '1') QR[i, j] = 0;
                            else QR[i, j] = 255;
                            binaryCount--;
                            if (BinaryMsg[BinaryMsg.Length - binaryCount] == '1') QR[i, j - 1] = 0;
                            else QR[i, j - 1] = 255;
                            binaryCount--;                           
                        }
                    }
                    else
                    {
                        if (modify[QR.GetLength(0) - i, j])
                        {
                            if (BinaryMsg[BinaryMsg.Length-1 - binaryCount] == '1') QR[QR.GetLength(0) - i, j] = 0;
                            else QR[QR.GetLength(0) - i, j] = 255;
                            binaryCount--;
                            if (BinaryMsg[BinaryMsg.Length-1 - binaryCount] == '1') QR[QR.GetLength(0) - i, j - 1] = 0;
                            else QR[QR.GetLength(0) - i, j - 1] = 255;
                            binaryCount--;
                        }
                    }
                    
                }
                if (up) up = false;
                else up = true;
            }
            int countMask = 0;
            for(int i = 0; i < QR.GetLength(0); i++)
            {
                for(int j = 0; j < QR.GetLength(1); j++)
                {
                    if(modify[i,j])
                    {
                        if (countMask % 2 == 0)
                        {
                            if (QR[i, j] == 255) QR[i, j] = 0;
                            else QR[i, j] = 255;
                        }
                    }
                    countMask++;
                }
            }
        }
        #endregion
    }
}
