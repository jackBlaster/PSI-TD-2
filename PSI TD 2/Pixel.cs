using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSI_TD_2
{
    public class Pixel
    {
        public static Pixel WHITE = new Pixel(255, 255, 255);
        public static Pixel BLACK = new Pixel(0, 0, 0);

        //un pixel = 3 octets (1 par couleur)
        byte r;
        byte g;
        byte b;

        public byte R => r;   // équivalent au get
        public byte G => g;
        public byte B => b;

        /// <summary>
        /// Créer un pixel avec les valeurs pour le rouge, le bleu et le vert.
        /// </summary>
        /// <param name="r"> octet pour le rouge </param>
        /// <param name="g"> octet pour le vert </param>
        /// <param name="b"> octet pour le bleu </param>
        public Pixel(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}
