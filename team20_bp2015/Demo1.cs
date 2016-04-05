using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Navigation;

namespace team20_bp2015
{
    class Demo1
    {

        //Initialisierung
        
        //Arrays für den ersten Punkt einer Linie
        double[] x = new double[50];
        double[] y = new double[50];
        //Arrays für den zweiten Punkt einer Linie
        double[] x2 = new double[50];
        double[] y2 = new double[50];
        //Array für die Übergabe der verbundenen TouchPoints mit User-Daten vom MainWindow aus
        public TouchInput.TouchPoint.TouchPoint[] mergedTPs;
        //Speicherung der Linien, damit diese beim Gehen eines Users temporär gelöscht und beim Wiederkommen des Users wieder eingefügt werden können
        Line[,] userlines = new Line[50, 15555];
        int[] userlineindex = new int[50];
        //vordefinierte Linienfarben für eine bessere Darstellung
        Color[] colors = new Color[25];
        Color[] usercolors = new Color[25];



        User[] lastactiveusers = new User[3]; // letzte active user

        //Boolean, wenn ein User noch nie die Demo benutzt hat
        Boolean newuser = true;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Demo1()
        {
            colors[0] = Colors.Purple;
            colors[1] = Colors.Red;
            colors[2] = Colors.Black;
            colors[3] = Colors.Green;
            colors[4] = Colors.Black;
            colors[5] = Colors.Orange;
            colors[6] = Colors.Blue;
            colors[7] = Colors.Pink;
            colors[8] = Colors.Silver;
            colors[9] = Colors.Gold;
            colors[10] = Colors.Azure;
            colors[11] = Colors.Magenta;
            colors[12] = Colors.Olive;
            colors[13] = Colors.Brown;
            colors[14] = Colors.Violet;
            colors[15] = Colors.Crimson;
            colors[16] = Colors.Navy;
            colors[17] = Colors.Moccasin;
            colors[18] = Colors.SkyBlue;
            colors[19] = Colors.Lime;
            colors[20] = Colors.Tomato;
            colors[21] = Colors.Wheat;
        }

        /// <summary>
        /// Zum Löschen der Linien-Daten des Users, der den Button gedrückt hat
        /// </summary>
        /// <param name="number">Stelle des Users in lastactiveusers[]</param>
        /// <param name="canvas">canvas, auf dem die Linien-Daten liegen</param>
        public void deletebutton(int number, Canvas canvas)
        {
            //forschleife, um jedes Element des Users aus dem canvas zu löschen
            for (int j = 0; j < userlineindex[lastactiveusers[number].getID()]; j++)
            {
                canvas.Children.Remove(userlines[lastactiveusers[number].getID(), j]);
                userlines[lastactiveusers[number].getID(), j] = null;

            }
            userlineindex[lastactiveusers[number].getID()] = 0;

        }

        /// <summary>
        /// Funktion, um das canvas zu aktualisieren
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="users"></param>
        /// <param name="button1"></param>
        public void updatecanvas(Canvas canvas, User[] users, Button button1)
        {
            try
            {
                Boolean userstillactive = false;
                //activeUser untersuchen, wer noch aktiv ist
                for (int i = 0; i < 3; i++)
                {
                    userstillactive = false;

                    foreach (User user in users)
                    {
                        if (user != null && lastactiveusers[i] != null)
                        {
                            if (lastactiveusers[i].getID() == user.getID())
                            {
                                userstillactive = true;
                                break;
                            }

                        }

                    }
                    //wenn ein User nicht mehr aktiv ist, werden seine Daten auf dem canvas nicht mehr angezeigt
                    if (!userstillactive && lastactiveusers[i] != null)
                    {


                        for (int j = 0; j < userlineindex[lastactiveusers[i].getID()]; j++)
                        {
                            canvas.Children.Remove(userlines[lastactiveusers[i].getID(), j]);

                        }

                        lastactiveusers[i] = null;



                    }

                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// UpdateFunktion für neue User
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="users"></param>
        /// <param name="button1"></param>
        public void updatecanvas2(Canvas canvas, User[] users, Button button1)
        {
            try
            {
                foreach (User currentuser in users)
                {
                    newuser = true;


                    if (currentuser != null)
                    {


                        //es wird gesucht, ob ein aktiver User ein neuer User ist
                        foreach (User user in lastactiveusers)
                        {
                            if (user != null)
                            {
                                if (user.getID() == currentuser.getID())
                                {
                                    newuser = false;
                                    break;
                                }
                            }
                        }
                        //Ein neuer User werden entsprechend Daten(u.a. Farbe etc) zu geordnet
                        if (newuser)
                        {


                            for (int j = 0; j < userlineindex[currentuser.getID()]; j++)
                            { if(userlines[currentuser.getID(), j] != null && !canvas.Children.Contains(userlines[currentuser.getID(), j]))
                                canvas.Children.Add(userlines[currentuser.getID(), j]);

                            }

                            //Anordnung der momentanen aktiven User in lastactiveusers
                            if (lastactiveusers[0] == null)
                            {
                                lastactiveusers[0] = currentuser;


                            }
                            else if (lastactiveusers[1] == null)
                            {
                                lastactiveusers[1] = currentuser;

                            }
                            else if (lastactiveusers[2] == null)
                            {
                                lastactiveusers[2] = currentuser;

                            }

                        }

                    }
                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// Funktion zum Zeichnen des Canvas mit neuen Linien-Daten
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="button1"></param>
        /// <param name="users"></param>
        public void TouchFrameDrawingDemo(Canvas canvas, Button button1, User[] users)
        {
            try
            {
                //stetig updaten der momentan Daten
                updatecanvas(canvas, users, button1);
                updatecanvas2(canvas, users, button1);


                //alle mit User-Daten verbundene Daten werden abgesucht
                foreach (TouchInput.TouchPoint.TouchPoint tp in mergedTPs)
                {



                    if (tp != null && tp.TouchP0int != null)
                    {
                        //Bereich des Buttons, in dem ein Touch zum Löschen der Daten führt (Aufruf von deletbutton)
                        if (tp.TouchP0int.Position.X >= 837.2 && tp.TouchP0int.Position.X <= 1056.4 && tp.TouchP0int.Position.Y >= 40.8 && tp.TouchP0int.Position.Y <= 173.6)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (lastactiveusers[i] != null && lastactiveusers[i].getID() == tp.User.getID())
                                {
                                    deletebutton(i, canvas);
                                    break;
                                }
                            }
                        }
                        //Behandlung, wenn der User noch keinen Anfangspunkt besitzt
                        if (x[tp.User.getID()] == 0 || Math.Abs(x[tp.User.getID()] - tp.TouchP0int.Position.X) > 50 || Math.Abs(y[tp.User.getID()] - tp.TouchP0int.Position.Y) > 50)
                        {
                            x[tp.User.getID()] = tp.TouchP0int.Position.X;
                            y[tp.User.getID()] = tp.TouchP0int.Position.Y;
                        }
                        //Zeichnen der Linie, sowie Fortsetzung der PunkteKette als Array
                        else
                        {
                            Line line = new Line();


                            line.Stroke = new SolidColorBrush(colors[tp.User.getID()]);
                            line.X1 = tp.TouchP0int.Position.X;
                            line.Y1 = tp.TouchP0int.Position.Y;
                            line.X2 = x[tp.User.getID()];
                            line.Y2 = y[tp.User.getID()];
                            line.StrokeThickness = 5;
                            canvas.Children.Add(line);
                            userlines[tp.User.getID(), userlineindex[tp.User.getID()]] = line;
                            userlineindex[tp.User.getID()] = userlineindex[tp.User.getID()] + 1;




                            x[tp.User.getID()] = tp.TouchP0int.Position.X;
                            y[tp.User.getID()] = tp.TouchP0int.Position.Y;
                            
                        }
                        //sowie Absuche der restlichen Touchpoints des Users
                        //momentan nur für zwei Finger ausgelegt, ansonsten durch for-schleife für mehrere erweiterbar
                        //for(int i = 0; i < tp.TouchPointCollection.Length; i++){ } etc.
                        if (tp.TouchPointCollection[0] != null)
                        {

                            if (x2[tp.User.getID()] == 0 || Math.Abs(x2[tp.User.getID()] - tp.TouchPointCollection[0].Position.X) > 50 || Math.Abs(y2[tp.User.getID()] - tp.TouchPointCollection[0].Position.Y) > 50)
                            {
                                x2[tp.User.getID()] = tp.TouchPointCollection[0].Position.X;
                                y2[tp.User.getID()] = tp.TouchPointCollection[0].Position.Y;
                            }
                            else
                            {
                                Line line = new Line();


                                line.Stroke = new SolidColorBrush(colors[tp.User.getID()]);
                                line.X1 = tp.TouchPointCollection[0].Position.X;
                                line.Y1 = tp.TouchPointCollection[0].Position.Y;
                                line.X2 = x2[tp.User.getID()];
                                line.Y2 = y2[tp.User.getID()];
                                line.StrokeThickness = 5;
                                canvas.Children.Add(line);
                                userlines[tp.User.getID(), userlineindex[tp.User.getID()]] = line;
                                userlineindex[tp.User.getID()] = userlineindex[tp.User.getID()] + 1;




                                x2[tp.User.getID()] = tp.TouchPointCollection[0].Position.X;
                                y2[tp.User.getID()] = tp.TouchPointCollection[0].Position.Y;






                            }
                        }



                    }

                }
            }
            catch
            {

            }

        }

    }
}