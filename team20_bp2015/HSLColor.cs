using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;


namespace team20_bp2015
{
    public class HSLColor
    {

        public float Hue;
        //Farbwert

        public float Saturation;
        //Sättigung

        public float Luminosity;
        //Beleuchtung

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="H">Farbwert</param>
        /// <param name="S">Sättigung</param>
        /// <param name="L">Beleuchtung</param>
        public HSLColor(float H, float S, float L)
        {
            Hue = H;
            Saturation = S;
            Luminosity = L;
        }

        /// <summary>
        /// Umwanldung einer RGB-Farbe in eine HSL-Farbe
        /// </summary>
        public static HSLColor FromRGB(Color Clr)
        {
            return FromRGB(Clr.R, Clr.G, Clr.B);
        }

        /// <summary>
        /// Berechnung der Umwanldung
        /// </summary>
        public static HSLColor FromRGB(Byte R, Byte G, Byte B)
        {
            //prozentualer Anteil von Rot, Grün und Blau berechnen
            float _R = (R / 255f);
            float _G = (G / 255f);
            float _B = (B / 255f);

            //Mininum, Maximum und Delta von Min/Max der Farben ermitteln
            float _Min = Math.Min(Math.Min(_R, _G), _B);
            float _Max = Math.Max(Math.Max(_R, _G), _B);
            float _Delta = _Max - _Min;

            //Initialisierung von H, S und L
            float H = 0;
            float S = 0;
            //Berechnung der Beleuchtung
            float L = (float)((_Max + _Min) / 2.0f);


            if (_Delta != 0)
            {
                //Berechnung der Sättigung
                if (L < 0.5f)
                {
                    S = (float)(_Delta / (_Max + _Min));
                }
                else
                {
                    S = (float)(_Delta / (2.0f - _Max - _Min));
                }

                //Berechnung des Farbwerts
                float _Delta_R = (float)(((_Max - _R) / 6.0f + (_Delta / 2.0f)) / _Delta);
                float _Delta_G = (float)(((_Max - _G) / 6.0f + (_Delta / 2.0f)) / _Delta);
                float _Delta_B = (float)(((_Max - _B) / 6.0f + (_Delta / 2.0f)) / _Delta);

                if (_R == _Max)
                {
                    H = _Delta_B - _Delta_G;
                }
                else if (_G == _Max)
                {
                    H = (1.0f / 3.0f) + _Delta_R - _Delta_B;
                }
                else if (_B == _Max)
                {
                    H = (2.0f / 3.0f) + _Delta_G - _Delta_R;
                }

                if (H < 0) H += 1.0f;
                if (H > 1) H -= 1.0f;
            }
            //Ausgabe der neuen HSL-Farbe
            return new HSLColor(H, S, L);
        }

    }
}
