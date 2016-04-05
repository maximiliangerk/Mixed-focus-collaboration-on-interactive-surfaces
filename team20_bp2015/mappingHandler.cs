using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace team20_bp2015
{
    class mappingHandler
    {

        //Parameterliste 

        public double screenWidth = 1900; //1920 alternativ
        public double screenHeigth = 1000; // 1080 alternativ


        //Parameterliste

        /// <summary>
        /// Array mit allen bisher gesehenen Usern
        /// </summary>
        public User[] allUsers;

        /// <summary>
        /// Array mit allen momentan aktiven Usern
        /// </summary>
        public User[] allActiveUsers;

        /// <summary>
        /// nächste zu füllende Stelle in allUsers
        /// </summary>
        public int nextUserID;

        /// <summary>
        /// nächste zu füllende Stelle in allActiveUsers
        /// </summary>
        public int nextAllActiveUserID;

        /// <summary>
        /// Array mit Informationen, welche Kamera den User an Stelle der ID sehen
        /// </summary>
        public string[] cameraSource;

        /// <summary>
        /// User auf HostSeite
        /// </summary>
        User userHost;

        /// <summary>
        /// User auf ClientSeite
        /// </summary>
        User userClient;

        /// <summary>
        /// aktiver User
        /// </summary>
        User userActive;

        /// <summary>
        /// Farbe des ClientUsers
        /// </summary>
        Color userClientColor;

        //User[] allUsers;

        /// <summary>
        /// Initialisierung des ColorHandlers für die Farberkennung
        /// </summary>
        colorHandler ch = new colorHandler();

        /// <summary>
        /// enthält Stelle der User auf HostSeite im activeUser-Array
        /// </summary>
        public int[] serverindexe = new int[3];

        /// <summary>
        /// leerer Konstruktor zum Initialisieren
        /// </summary>
        public mappingHandler()
        {

        }


        /// <summary>
        /// verbindet User-Daten von dem Client-Rechner mit denen des Host-Rechners
        /// </summary>
        /// <param name="allUsers">alle bisher gesehenen User</param>
        /// <param name="activeUsers">alle aktiven User die bisher vom Host-Rechner erkannt wurden (auch durch vorheriges Aufrufen des MappingHandlers)</param>
        /// <param name="activeClientUsers">die neusten aktiven User vom Client-Rechner</param>
        /// <param name="cameraSource">String-Array, in dem die Informationen gespeichert sind, welche Kamera User an der Stelle i erkennen.</param>
        /// <param name="nextID">der Index für den nächsten Eintrag in allUsers</param>
        /// <param name="nextActiveID">der Index für den nächsten Eintrag in activeUsers</param>
        public void mappingHandling(User[] allUsers, User[] activeUsers, User[] activeClientUsers, string[] cameraSource, int nextID, int nextActiveID)
        {
            //Initialisierungen

            allActiveUsers = activeUsers;

            this.cameraSource = cameraSource;

            if (activeClientUsers != null)
                //alle aktiven User des Clients untersuchen
                for (int j = 0; j < 3; j++)
                {
                    User userclient = activeClientUsers[j];

                    if (userClient == null)
                    {
                        break;
                    }

                    userClientColor = userClient.getColor();
                    Boolean colorSeen = false;
                    Boolean activeSeen = false;

                    if (activeUsers != null)
                        for (int i = 0; i < activeUsers.Length; i++)
                        {
                            User userActive = activeUsers[i];
                            //Wenn beide Rechner die Person erkennen
                            if (userActive != null && ch.compareColor(userActive.getColor(), userClientColor))
                            {
                                this.userActive = userActive;
                                activeSeen = true;
                                serverindexe[j] = i;
                                //
                                if (userClient.CertaintyRightHand_Client > userActive.CertaintyRightHand_Server)
                                {
                                    allActiveUsers[i].RightHandRelativeToScreen = userClient.RightHandRelativeToScreen;
                                }
                                else
                                {
                                    allActiveUsers[i].RightHandRelativeToScreen = userActive.RightHandRelativeToScreen;
                                }
                                if (userClient.CertaintyLeftHand_Client > userActive.CertaintyLeftHand_Server)
                                {
                                    allActiveUsers[i].LeftHandRelativeToScreen = userClient.LeftHandRelativeToScreen;
                                }
                                else
                                {
                                    allActiveUsers[i].LeftHandRelativeToScreen = userActive.RightHandRelativeToScreen;
                                }

                                this.cameraSource[i] = "Both cameras recognize this person. ";
                                allActiveUsers[i].CameraDetection = this.cameraSource[i];
                                break;
                            }
                        }
                    //Berechnungen, falls die Person von beiden Kameras erkannt wurde
                    if (activeSeen)
                    {

                        //keine Veränderungen nötig

                        break;
                    }
                    if (allUsers != null)
                        foreach (User userHost in allUsers)
                        {
                            //wenn der Client-User überhaupt schon einmal im System registriert wurde
                            if (userHost != null && ch.compareColor(userHost.getColor(), userClientColor))
                            {
                                this.userHost = userHost;
                                colorSeen = true;


                                break;
                            }
                        }
                    //Berechnungen, falls die Person schon einmal im System registriert wurde
                    if (colorSeen)
                    {
                        serverindexe[j] = nextActiveID;
                        //Behandlung, wenn userHost und userClient gleich sind
                        allActiveUsers[nextActiveID] = userHost;
                        allActiveUsers[nextActiveID].RightHandRelativeToScreen = userClient.RightHandRelativeToScreen;
                        allActiveUsers[nextActiveID].LeftHandRelativeToScreen = userClient.LeftHandRelativeToScreen;
                        allActiveUsers[nextActiveID].CertaintyRightHand_Client = userClient.CertaintyRightHand_Client;
                        allActiveUsers[nextActiveID].CertaintyRightHand_Client = userClient.CertaintyLeftHand_Client;
                        allActiveUsers[nextActiveID].CertaintyRightHand_Server = 0;
                        allActiveUsers[nextActiveID].CertaintyLeftHand_Server = 0;

                        this.cameraSource[nextActiveID] = "The client-camera recognizes this person. ";
                        allActiveUsers[nextActiveID].CameraDetection = this.cameraSource[nextActiveID];
                        nextActiveID++;
                    }
                    else
                    {
                        serverindexe[j] = nextActiveID;
                        //Behandlung, wenn userHost und userClient unterschiedlich sind
                        allActiveUsers[nextActiveID] = userClient;
                        allActiveUsers[nextActiveID].setID(nextID);
                        this.cameraSource[nextActiveID] = "The client-camera recognizes this person. ";
                        allActiveUsers[nextActiveID].CameraDetection = this.cameraSource[nextActiveID];
                        allUsers[nextID] = allActiveUsers[nextActiveID];

                        allActiveUsers[nextActiveID].CertaintyRightHand_Server = 0;
                        allActiveUsers[nextActiveID].CertaintyLeftHand_Server = 0;


                        nextID++;
                        nextActiveID++;
                       
                    }
                }
            //Vorbereitung der Ausgabe
            nextUserID = nextID;
            this.allUsers = allUsers;
            nextAllActiveUserID = nextActiveID;
        }


        /// <summary>
        /// Von den Hand-Daten der User die Koordinaten relativ zum Bildschirm und des Blickwinkels der Kamera umwandeln.
        /// Wird genutzt, um TouchPoints besser mit HandPositionen vergleichen zu können.
        /// </summary>
        /// <param name="activeUsers"></param>
        /// <param name="cspOriginTouchScreen"></param>
        /// <param name="cspTopRightCorner"></param>
        /// <returns></returns>
        public User[] convertCoordinatesKinectToScreen(User[] activeUsers, CameraSpacePoint cspOriginTouchScreen, CameraSpacePoint cspTopRightCorner)
        {
            //TODO Einstellen auf welcher seite die Kinect steht (Z-Koordinate!)

            //Delta der X-Koordinate, Abstand zwischen der zwei Ecken des Bildschirm
            double tcDeltaX = (cspTopRightCorner.X - cspOriginTouchScreen.X);

            //Delta der Y-Koordinate, Abstand zwischen der zwei Ecken des Bildschirm
            double tcDeltaY = (cspTopRightCorner.Y - cspOriginTouchScreen.Y);

            //Pixel-Weite des Bildschirms
            int pixelWidth = 1900;//1920;

            //Pixel-Höhe des Bildschims
            int pixelHeight = 1000;// 1080;

            //Ausgabe-Array für die aktiven User-Dateien
            User[] output = activeUsers;

            foreach (User user in activeUsers)
            {
                if (user != null)
                {
                    //Initialisierung

                    //Aus Performance-Gründen ohne Entfernung zum Bildschirm
                    //Alternativ (mit Entfernung) Code mit dem Code der if nach dem return Aufruf austauschen

                    user.LeftHandRelativeToScreen = new Vector3();

                    user.RightHandRelativeToScreen = new Vector3();

                    CameraSpacePoint leftHand = user.Body.Joints[JointType.HandLeft].Position;

                    //double proportionDxDy;

                    //prozentuale X-Koordinate in Relation des Bildschirms und dem Blickwinkel der Kamera
                    double ptDeltaX = (leftHand.X - cspOriginTouchScreen.X) / tcDeltaX;

                    //prozentuale Y-Koordinate in Relation des Bildschirms und dem Blickwinkel der Kamera
                    double ptDeltaY = (leftHand.Y - cspOriginTouchScreen.Y) / tcDeltaY;

                    //Umrechnung der X-Koordinate in Pixel
                    ptDeltaX *= pixelWidth;

                    //Umrechnung der Y-Koordinate in Pixel
                    ptDeltaY *= pixelHeight;

                    //Schreiben der neuen User-Daten
                    user.LeftHandRelativeToScreen.X = ptDeltaX;
                    user.LeftHandRelativeToScreen.Y = ptDeltaY;

                    //RightHand-Berechnung wie bei der linken Hand
                    CameraSpacePoint rightHand = user.Body.Joints[JointType.HandRight].Position;

                    ptDeltaX = (rightHand.X - cspOriginTouchScreen.X) / tcDeltaX;

                    ptDeltaY = (rightHand.Y - cspOriginTouchScreen.Y) / tcDeltaY;

                    ptDeltaX *= pixelWidth;

                    ptDeltaY *= pixelHeight;

                    user.RightHandRelativeToScreen.X = ptDeltaX;

                    user.RightHandRelativeToScreen.Y = ptDeltaY;
                }


            }

            //Ausgabe der neuen aktivenUser-Daten
            return activeUsers;

            //Mit Betrachtung einer Entfernung des users zum Bildschirms
            if (false)
            {


                //Delta der X-Koordinate, Abstand zwischen der zwei Ecken des Bildschirm
               // double tcDeltaX = (cspTopRightCorner.X - cspOriginTouchScreen.X);

                //Delta der Y-Koordinate, Abstand zwischen der zwei Ecken des Bildschirm
                //double tcDeltaY = (cspTopRightCorner.Y - cspOriginTouchScreen.Y);

                //Delta der Z-Koordinate, zum Schätzen, ob der Nutzer nah genug am Bildschirm ist, sodass er relevant ist
                double tcDeltaZ = (cspTopRightCorner.Z - cspOriginTouchScreen.Z);

                //Pixel-Weite des Bildschirms
                //int pixelWidth = (int)screenWidth;//1920;

                //Pixel-Höhe des Bildschims
                //int pixelHeight = (int)screenHeigth;// 1080;


                foreach (User user in activeUsers)
                {
                    if (user != null)
                    {

                        //Initialisierung
                        if (user.LeftHandRelativeToScreen == null)
                            user.LeftHandRelativeToScreen = new Vector3();
                        if (user.RightHandRelativeToScreen == null)
                            user.RightHandRelativeToScreen = new Vector3();

                        double distanceBetweenCorners = Math.Sqrt(tcDeltaX * tcDeltaX + tcDeltaY * tcDeltaY + tcDeltaZ * tcDeltaZ);

                        CameraSpacePoint leftHand = user.Body.Joints[JointType.HandLeft].Position;

                        //double proportionDxDy;

                        //prozentuale X-Koordinate in Relation des Bildschirms und dem Blickwinkel der Kamera
                        double ptDeltaX = (leftHand.X - cspOriginTouchScreen.X) / tcDeltaX;

                        //prozentuale Y-Koordinate in Relation des Bildschirms und dem Blickwinkel der Kamera
                        double ptDeltaY = (leftHand.Y - cspOriginTouchScreen.Y) / tcDeltaY;





                        //Behandlung der Entfernung zum Nutzer 
                        //hier prüfen, ob die linke Hand zu weit von einer der 2 primären eckpunkte entfernt ist(genauer Distanz zwischen den Eckpunkten)
                        double zTolerance = 0.1;
                        double distanceHandCorner1 = Math.Sqrt((leftHand.X - cspOriginTouchScreen.X) * (leftHand.X - cspOriginTouchScreen.X) + (leftHand.Y - cspOriginTouchScreen.Y) * (leftHand.Y - cspOriginTouchScreen.Y) + (leftHand.Z - cspOriginTouchScreen.Z) * (leftHand.Z - cspOriginTouchScreen.Z));
                        double distanceHandCorner2 = 0;

                        if (distanceHandCorner1 > distanceBetweenCorners + zTolerance)
                        {
                            user.LeftHandRelativeToScreen.Z = 1000;

                        }
                        else
                        {
                            distanceHandCorner2 = Math.Sqrt((leftHand.X - cspTopRightCorner.X) * (leftHand.X - cspTopRightCorner.X) + (leftHand.Y - cspTopRightCorner.Y) * (leftHand.Y - cspTopRightCorner.Y) + (leftHand.Z - cspTopRightCorner.Z) * (leftHand.Z - cspTopRightCorner.Z));
                            if (distanceHandCorner2 > distanceBetweenCorners + zTolerance)
                            {
                                user.LeftHandRelativeToScreen.Z = 1000;

                            }
                        }

                        //Umrechnung der X-Koordinate in Pixel
                        ptDeltaX *= pixelWidth;

                        //Umrechnung der Y-Koordinate in Pixel
                        ptDeltaY *= pixelHeight;

                        //Schreiben der neuen User-Daten
                        if (user.CertaintyLeftHand_Client <= user.CertaintyLeftHand_Server)
                        {
                            user.LeftHandRelativeToScreen.X = ptDeltaX;
                            user.LeftHandRelativeToScreen.Y = ptDeltaY;


                        }

                        //RightHand-Berechnung wie bei der linken Hand
                        CameraSpacePoint rightHand = user.Body.Joints[JointType.HandRight].Position;

                        ptDeltaX = (rightHand.X - cspOriginTouchScreen.X) / tcDeltaX;

                        ptDeltaY = (rightHand.Y - cspOriginTouchScreen.Y) / tcDeltaY;


                        //Behandlung der Entfernung zum Nutzer 
                        //hier prüfen, ob die rechte Hand zu weit von einer der 2 primären eckpunkte entfernt ist(genauer Distanz zwischen den Eckpunkten)
                        distanceHandCorner1 = Math.Sqrt((rightHand.X - cspOriginTouchScreen.X) * (rightHand.X - cspOriginTouchScreen.X) + (rightHand.Y - cspOriginTouchScreen.Y) * (rightHand.Y - cspOriginTouchScreen.Y) + (rightHand.Z - cspOriginTouchScreen.Z) * (rightHand.Z - cspOriginTouchScreen.Z));

                        if (distanceHandCorner1 > distanceBetweenCorners + zTolerance)
                        {
                            user.RightHandRelativeToScreen.Z = 1000;
                        }
                        else
                        {
                            distanceHandCorner2 = Math.Sqrt((rightHand.X - cspTopRightCorner.X) * (rightHand.X - cspTopRightCorner.X) + (rightHand.Y - cspTopRightCorner.Y) * (rightHand.Y - cspTopRightCorner.Y) + (rightHand.Z - cspTopRightCorner.Z) * (rightHand.Z - cspTopRightCorner.Z));
                            if (distanceHandCorner2 > distanceBetweenCorners + zTolerance)
                            {
                                user.RightHandRelativeToScreen.Z = 1000;
                            }
                        }

                        ptDeltaX *= pixelWidth;

                        ptDeltaY *= pixelHeight;

                        if (user.CertaintyLeftHand_Client <= user.CertaintyLeftHand_Server)
                        {
                            user.RightHandRelativeToScreen.X = ptDeltaX;

                            user.RightHandRelativeToScreen.Y = ptDeltaY;

                        }



                        //Ende der Iteration des Users user
                    }


                }

                //Ausgabe der neuen aktivenUser-Daten
                return activeUsers;
            }
           
        }

        /// <summary>
        /// Vergleich des TouchPoints mit den aktivenUser, deren Daten in convertCoordinatesKinectToScreen angepasst wurden
        /// </summary>
        /// <param name="tp">Touchpoint, der eine Zuordnung zu einem aktiven Nutzer benötigt</param>
        /// <param name="activeUsers">alle aktiven User</param>
        /// <returns></returns>
        public User compareCoordinatesKinectToScreen(TouchPoint tp, User[] activeUsers)
        {

            double maxDistanceZ = 2500;


            User bestUser = null;

            double bestDistance = -1;





            foreach (User user in activeUsers)
            {
                //Linke Hand:
                //Berechnung, ob die relativen Koordinaten der linken Hand von User user dem TouchPoint tp zuzuordnen ist
                if (user != null && user.LeftHandRelativeToScreen.Z <= maxDistanceZ)
                {
                    if (user.LeftHandRelativeToScreen != null)
                    {
                        double localDistance = Math.Sqrt(Math.Abs(tp.Position.X - user.LeftHandRelativeToScreen.X) * Math.Abs(tp.Position.X - user.LeftHandRelativeToScreen.X) + Math.Abs((screenHeigth - tp.Position.Y) - user.LeftHandRelativeToScreen.Y) * Math.Abs((screenHeigth - tp.Position.Y) - user.LeftHandRelativeToScreen.Y));

                        if (bestDistance == -1 || localDistance < bestDistance)
                        {
                            bestDistance = localDistance;
                            bestUser = user;
                        }
                    }

                }
                //Rechte Hand 
                //gleiche Berechnung wie bei der linken Hand
                else if (user != null && user.RightHandRelativeToScreen.Z <= maxDistanceZ)
                {
                    if (user.RightHandRelativeToScreen != null)
                    {
                        double localDistance = Math.Sqrt(Math.Abs(tp.Position.X - user.RightHandRelativeToScreen.X) * Math.Abs(tp.Position.X - user.RightHandRelativeToScreen.X) + Math.Abs((screenHeigth - tp.Position.Y) - user.RightHandRelativeToScreen.Y) * Math.Abs((screenHeigth - tp.Position.Y) - user.RightHandRelativeToScreen.Y));

                        if (bestDistance == -1 || localDistance < bestDistance)
                        {
                            bestDistance = localDistance;
                            bestUser = user;
                        }
                    }

                }


            }


            return bestUser;
        }



    }
}
