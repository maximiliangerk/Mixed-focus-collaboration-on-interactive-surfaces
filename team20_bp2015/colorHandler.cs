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
    class colorHandler
    {
        //Parameterliste
        
            /// <summary>
            /// Radius, in dem die Punkte für die Farberkennung einbezogen werden.
            /// </summary>
            int radius = 5;

            /// <summary>
            /// Anzahl der Punkte im Radius
            /// </summary>
            int range = 121;

            /// <summary>
            /// Array mit den Farb-Daten mit range-vielen Stellen
            /// </summary>
            Color[] colorArray = new Color[121];

            /// <summary>
            /// Index für das colorArray und das counterArray
            /// </summary>
            int colorcount = 0;
            
            /// <summary>
            /// Array mit Einträgen, wie oft eine bestimmte Farbe gesehen wurde
            /// </summary>
            int[] counterArray = new int[121];

            /// <summary>
            /// veränderbare Bitmap mit den Farb-Daten
            /// </summary>
            WriteableBitmap writeableBitmap;

            /// <summary>
            /// TestString für das Testen der Farberkennung
            /// </summary>
            string tempColor = "";
            
            /// <summary>
            /// Boolean für Modus 0
            /// </summary>
            Boolean colorFound = false;

            /// <summary>
            /// FarbAusgabe für Modi 0 und 3
            /// </summary>
            Color outputColor;

            /// <summary>
            /// Counter für alle Punkte für Modus 3
            /// </summary>
            int counterTotal = 0;

            /// <summary>
            /// temprärer Int-Wert für Rot für Modi 0, 1 und 3
            /// </summary>
            int tempR = 0;

            /// <summary>
            /// temprärer Int-Wert für Grün für Modi 0, 1 und 3
            /// </summary>
            int tempG = 0;

            /// <summary>
            /// temprärer Int-Wert für Blau für Modi 0, 1 und 3
            /// </summary>
            int tempB = 0;

            /// <summary>
            /// Farbe, die aus der writeableBitmap gezogen werden
            /// </summary>
            Color c;

            /// <summary>
            /// Farbe, die mit c verglichen werden
            /// </summary>
            Color c2;

            /// <summary>
            /// Farbe c im HSL-Farbraum
            /// </summary>
            HSLColor h1;

            /// <summary>
            /// Farbe c2 im HSL-Farbraum
            /// </summary>
            HSLColor h2;

        /// <summary>
        /// Radius für den Bereich um den Punkt x/y setzen.
        /// </summary>
        /// <param name="radius">Radius vom Punkt x/y</param>
        public void setRadius(int radius)
        {

            this.radius = radius;

            this.range = ((radius * 2) + 1) ^ 2;

            colorArray = new Color[this.range];

            counterArray = new int[this.range];
            
        }

        /// <summary>
        /// Behandlung der Farberkennung. Modi 0 und 3 empfohlen. TODO hue implementieren
        /// </summary>
        /// <param name="wb">Übergabe der writeableBitmap</param>
        /// <param name="x">X-Koordinate des Mittelpunkts</param>
        /// <param name="y">Y-Koordinate des Mittelpunkts</param>
        /// <param name="mode">Modus, welche Art der Farberkennung benuzt wird. 0: meist vorhandene Farbe; 1: Durchschnitt nach jedem neuen Farbwert; 2: Erfassung von vordefinierten Farben (Rot,Grün,Blau,Schwarz,Weiß); 3: Durschnitt aller Farbwerte; 4: hue TODO implementieren</param>
        /// <returns></returns>
        public Color colorHandling(WriteableBitmap wb, int x, int y, int mode)
        {
            //Übergabe der Bitmap
            this.writeableBitmap = wb;

            //Abtastung der Punkte im Radius-Bereich um den Punkt x/y (Beachte: der Bereich ist kein Kreis)
            for (int i = 0; i < 2 * radius + 1; i++)
            {

                for (int j = 0; i < 2 * radius + 1; j++)
                {

                    if (mode == 0)
                    {
                        //Punkt darf nicht außerhalb des Bildschirms liegen
                        if (x - (radius + 1) + i >= 0 && x - (radius + 1) + i + 10 <= 1920 && y - (radius + 1) + j >= 0 && y - (radius + 1) + j + 10 <= 1080)
                        {
                            //weitere Behandlung von Modus 0; Beschreibung von c und c2 in Sonderfällen; Behanldung vom letzten Punkt uninteressant
                            if (i == 0 && j == 0)
                                c = writeableBitmap.GetPixel(x - (radius + 1) + i, y - (radius + 1) + j);

                            if (j == 2 * radius + 1 && i == radius * 2 + 1)
                            {

                                break;

                            }
                            if (j == 2 * radius + 1)
                            {

                                c2 = writeableBitmap.GetPixel(x - (radius + 1) + i + 1, y - (radius + 1) + j);

                            }
                            else
                            {

                                c2 = writeableBitmap.GetPixel(x - (radius + 1) + i, y - (radius + 1) + j + 1);

                            }
                        }
                        //ansonsten betrachte diesen Punkt nicht weiter
                        else
                            break;

                    }
                    //Punkt darf nicht außerhalb des Bildschirms liegen
                    if (mode == 1)
                    {
                        if (x - (radius + 1) + i >= 0 && x - (radius + 1) + i <= 1920 && y - (radius + 1) + j >= 0 && y - (radius + 1) + j <= 1080)
                            c = writeableBitmap.GetPixel(x - (radius + 1) + i, y - (radius + 1) + j);
                        else
                            break;

                    }
                    //Punkt darf nicht außerhalb des Bildschirms liegen
                    if (mode == 2)
                    {
                        if (x - (radius + 1) + i >= 0 && x - (radius + 1) + i +1<= 1920 && y - (radius + 1) + j >= 0 && y - (radius + 1) + j +1<= 1080)
                            c = writeableBitmap.GetPixel(x - (radius + 1) + i, y - (radius + 1) + j);
                        else
                            break;

                    }

                    //switch-case für die verschiedenen Modi
                    switch (mode)
                    {
                        case 0:
                            {
                                //Betrachtung des ersten Punktes im Bereich
                                if (i == 0 && j == 0)
                                {

                                    colorArray[0] = c;
                                    

                                    colorcount = 0;

                                }
                                else
                                {

                                    colorFound = false;

                                    //Suche nach einer ähnlichen Farbe
                                    for (int k = 0; k < colorcount + 1; k++)
                                    {

                                        if (k < range && compareColorZ(colorArray[k], c2))
                                        {

                                            counterArray[k]++;

                                            colorFound = true;

                                        }

                                    }

                                    //wenn keine ähnliche Farbe gefunden wurde, wird die Farbe c2 ins FarbArray gespeichert.
                                    if (!colorFound)
                                    {
                                        if (colorcount + 1 < range)
                                        {

                                            colorArray[colorcount + 1] = c2;

                                            colorcount++;

                                        }
                                    }


                                }

                                break;

                            }
                        case 1:
                            {
                                //Betrachtung des ersten Punktes im Bereich
                                if (i == 0 && j == 0)
                                {

                                    tempR = c.R;

                                    tempG = c.G;

                                    tempB = c.B;

                                }
                                else
                                {
                                    //Fabrwert hinzuaddieren und gesamter Wert halbieren
                                    tempR = (tempR + c.R) / 2;

                                    tempG = (tempG + c.G) / 2;

                                    tempB = (tempB + c.B) / 2;

                                }
                                break;
                            }
                        case 2:
                            {
                                //Farbwert wird hinzuaddiert und Anzahl der gesamten Punkte inkrementiert
                                tempR += c.R;

                                tempG += c.G;

                                tempB += c.B;

                                counterTotal++;

                                break;
                            }
                    }



                }
            }


            switch (mode)
            {
                case 0:
                    {
                        //Betrachtung welche Farbe im ColorArray am meisten Einträge im gleichen index in counterArray hat
                        int id = 0;
                        for (int m = 0; m < colorcount + 1; m++)
                        {
                            if (m == colorcount)
                                break;
                            if (id < range && m + 1 < range && counterArray[m + 1] > counterArray[id])
                            {
                                id = m + 1;
                            }
                        }

                        tempR = colorArray[id].R;

                        tempG = colorArray[id].G;

                        tempB = colorArray[id].B;

                        //Ausgabe(Farbe)
                        outputColor = colorArray[id];

                        //Teststring
                        tempColor = "Red= " + tempR + "Green= " + tempG + "Blue= " + tempB + ".";

                        break;
                    }
                case 1:
                    {
                        //Testausgabe
                        tempColor = "Red= " + tempR + "Green= " + tempG + "Blue= " + tempB + ".";
                        break;
                    }
                
                case 2:
                    {
                        //zusammengerechnete Farbwerte werden durch die Anzahl der betrachteten Punkt geteilt
                        tempR = tempR / counterTotal;
                        tempG = tempG / counterTotal;
                        tempB = tempB / counterTotal;

                        //Umsetzung in eine ausgebbare Farbe
                        outputColor.R = (byte)tempR;
                        outputColor.G = (byte)tempG;
                        outputColor.B = (byte)tempB;

                        //Testausgabe
                        tempColor = "Red= " + tempR + "Green= " + tempG + "Blue= " + tempB + ".";

                        break;
                    }
            }

            return outputColor;

        }

        /// <summary>
        /// Vergleich zweier Farben, ob sie ähnlich sind.
        /// </summary>
        /// <param name="h1">Farbe 1</param>
        /// <param name="h2">Farbe 2</param>
        /// <returns></returns>
        public Boolean compareColor(Color c1, Color c2)
        {   //Transformation HSL Raum für den Vergleich
            h1 = HSLColor.FromRGB(c1);
            h2 = HSLColor.FromRGB(c2);

            if(h1 != null && h2 != null)
            {
                //Wenn der Abstand der Farbanteile klein genug ist, sind die Farben ähnlich
                if (Math.Abs(h2.Hue - h1.Hue) <= 0.25 && Math.Abs(h2.Luminosity-h1.Luminosity) <= 0.25 && Math.Abs(h2.Saturation - h1.Saturation) <= 0.25)
                {

                    return true;

                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Vergleich zweier Farben, ob sie ähnlich sind. Diese Funktion ist nur für die interne Verarbeiung des Modus 0 gedacht.
        /// </summary>
        /// <param name="h1">Farbe 1</param>
        /// <param name="h2">Farbe 2</param>
        /// <returns></returns>
        private Boolean compareColorZ(Color c1, Color c2)
        {   //Transformation HSL Raum für den Vergleich
            h1 = HSLColor.FromRGB(c1);
            h2 = HSLColor.FromRGB(c2);
            

            if (h1 != null && h2 != null)
            {
               // Console.WriteLine("Hue 1:" + h1.Hue + "-Hue2:"+ h2.Hue + "-Lum1:"+ h1.Luminosity + "-Lum2:" + h2.Luminosity + "-Sat1:"+ h1.Saturation + "-Sat2:" + h2.Saturation+ "|");
                //Wenn der Abstand der Farbanteile klein genug ist, sind die Farben ähnlich
                if (Math.Abs(h2.Hue - h1.Hue) <= 0.09 && Math.Abs(h2.Luminosity - h1.Luminosity) <= 0.09 && Math.Abs(h2.Saturation - h1.Saturation) <= 0.09)
                {

                    return true;

                }
                else
                    return false;
            }
            else
                return false;
        }

    }
}
