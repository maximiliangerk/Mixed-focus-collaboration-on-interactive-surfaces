﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.Timers;
using System.Threading;
using team20_bp2015.Properties;


namespace team20_bp2015
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// -> andere Demos an den Stellen von Demo1 aufrufen (vorallem in Touch_FrameReported für Touch-Events und Reader_OnBodyFrameArrived für Änderungen der aktiven Nutzer)
    /// </summary>
    public partial class MainWindow : Window
    {

        //Parameterliste

        /// <summary>
        /// KinectSensor für die BodyDaten
        /// </summary>
        KinectSensor sensor = null;

        /// <summary>
        /// das stetig aktalisierende BodyArray
        /// </summary>
        Body[] bodies = null;

        /// <summary>
        /// EventVariable für das Lesen der neuen Body-Daten
        /// </summary>
        BodyFrameReader bodyFrameReader = null;

        /// <summary>
        /// Anzahl aktiver Bodies
        /// </summary>
        private int activeBodies = 0;

        /// <summary>
        /// Boolean, ob die Kamera aktiv ist
        /// </summary>
        private bool activ = false;

        /// <summary>
        /// IterationsVariable für die filterActiveBodies()-Funktionalität
        /// </summary>
        private int iteration = 0;

        /// <summary>
        /// Anzahl der User, benutzt in der filterActiveBodies()-Funktionalität
        /// </summary>
        private int userNumber = 0;

        /// <summary>
        /// writeableBitmap, um die Farbinformationen bekommen zu können
        /// </summary>
        private WriteableBitmap colorBitmap;

        /// <summary>
        /// Event-Variable für das Erkennen der Farbe
        /// </summary>
        private ColorFrameReader colorFrameReader = null;

        /// <summary>
        /// CameraSpacePoint für userTracking
        /// </summary>
        ColorSpacePoint csp = new ColorSpacePoint();

        /// <summary>
        /// User-Array, welches alle (seit dem Start) User enthält, die vom System erkannt wurden
        /// </summary>
        User[] users = new User[100];

        /// <summary>
        /// User-Array, welches alle aktiven User enthält
        /// </summary>
        User[] activeUsers = new User[10];

        /// <summary>
        /// Index, um die nächste Stelle des users-Array beschreiben zu können
        /// </summary>
        int nextUserID = 0;

        /// <summary>
        /// Index, um die nächste Stelle des activeUsers-Array beschreiben zu können
        /// </summary>
        int nextUserIDactive = 0;

        /// <summary>
        /// Initialisierung des colorHandlers
        /// </summary>
        colorHandler ch = new colorHandler();

        /// <summary>
        /// Anzahl der User bei der letzten Überprüfung
        /// </summary>
        int oldbodynumber = 0;

        /// <summary>
        /// Anzahl der User beim Client bei der letzten Überprüfung
        /// </summary>
        int oldActiveClientUsernumber = 0;

        /// <summary>
        /// Array mit den aktiven Usern des Client
        /// </summary>
        User[] activeClientUsers = new User[100];

        /// <summary>
        /// String-Array mit den Informationen, welche User von welchen Kameras erkannt werden.
        /// </summary>
        string[] cameraSource = new string[100];

        /// <summary>
        /// enthält die Stelle der ClientUser, an denen sie in activeUsers gespeichert wurden.
        /// </summary>
        int[] activeClientPosition = new int[3];

        /// <summary>
        /// Timer, um Client-Daten öfter abzurufen
        /// </summary>
        System.Timers.Timer timer = new System.Timers.Timer();

        /// <summary>
        /// Array mit verbundenen Touch-Daten und User-Daten
        /// </summary>
        team20_bp2015.TouchInput.TouchPoint.TouchPoint[] mergedTouchPoints = new TouchInput.TouchPoint.TouchPoint[100];

        //Send and Receive - Parameterliste

        /// <summary>
        /// Thread, auf dem der tcpHandler laufen wird
        /// </summary>
        System.Threading.Thread send_receiveThread;

        /// <summary>
        /// tcpHandler, über den das Senden und Empfangen behandelt wird
        /// </summary>
        TcpHandler tcpHandler = new TcpHandler();

        /// <summary>
        /// Mappinghandler, durch den Zusammenführen der Daten von Client und Server gemanaged wird
        /// </summary>
        mappingHandler _mappinghandler = new mappingHandler();

        /// <summary>
        /// host ist der Hauptrechner, der die Daten bekommt
        /// client ist der Nebenrechner, der die Daten versendet
        /// </summary>
        public string serverOrClient = "client"; //"server" oder "client" oder "offline"

        /// <summary>
        /// Ip des Server, auf den zugegriffen wird
        /// </summary>
        public string serverIP = "130.83.221.194";

        /// <summary>
        /// Port des Servers, auf den zugegriffen wird
        /// </summary>
        public int serverPORT = 8080;


        //TestParameterListe
        int oldAllActiveUsersCounter = 0;

        /// <summary>
        /// Demo1
        /// </summary>
        Demo1 demo1;

        /// <summary>
        /// Mainklasse zur Initialisierung des Hauptfensters
        /// </summary>
        public MainWindow()
        {

            //Initialisierungen

            sensor = KinectSensor.GetDefault();

            sensor.Open();

            activ = sensor.IsAvailable;

            this.bodies = new Body[sensor.BodyFrameSource.BodyCount];

            //Demo1 Aufruf
            demo1 = new Demo1();

            //Radius für die Farberkennung änderen
            ch.setRadius(15);

            //Body Daten über die bodyFrames abgreifen
            bodyFrameReader = sensor.BodyFrameSource.OpenReader();

            //Den Reader für ColorFrames öffnen
            this.colorFrameReader = this.sensor.ColorFrameSource.OpenReader();

            //Erstellung der ColorFrameDescription vom ColorFrameSource (Nutzung des bgra Formats)
            FrameDescription colorFrameDescription = this.sensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);

            //Erstellung einer Bitmap des Displays
            this.colorBitmap = new WriteableBitmap(colorFrameDescription.Width, colorFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);

            //momentan nicht genutzt; TODO eventuelle Nutzungsmöglichkeiten betrachten
            InitializeComponent();

            //Touchklasse starten
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);

            //Thread-erstellung für Daten-Senden/Empfangen
            if (serverOrClient.Equals("server"))
            {
                tcpHandler = new TcpHandler();
                tcpHandler.serverIP = serverIP;
                tcpHandler.serverPORT = serverPORT;
                send_receiveThread = new System.Threading.Thread(tcpHandler.tcpHandling);

                send_receiveThread.Start(serverOrClient);
            } 

            //BodyFrameReader öffnen
            if (this.bodyFrameReader != null)
            {

                this.bodyFrameReader.FrameArrived += this.Reader_OnBodyFrameArrived;

            }

            //ColorFrameReader öffnen
            if (colorFrameReader != null)
            {

                this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;

            }
            //Timer-Event auskommentiert, da bei kleinen Intervallen Performance-Probleme auftreten.
            //timer.Elapsed += new ElapsedEventHandler(timerelapsed);
            timer.Interval = 300000;
            timer.Enabled = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerelapsed(object sender, ElapsedEventArgs e)
        {
            //bei zu großen Zeitabständen sind Berechnungen mit den Wahrscheinlíchkeiten des Client voraussichtlich falsch
            if(timer.Interval > 5000)
            {
                foreach(User user in activeUsers)
                {
                    if(user == null)
                    {
                        break;
                    }
                  user.CertaintyLeftHand_Client = 0;
                    user.CertaintyRightHand_Client = 0;
                }
            }

            //auf der Seite des Servers werden die Wahrscheinlichkeiten von Host und Client abgeglichen und die relativen Koordinaten dem entsprechend gesetzt werden
            if (serverOrClient.Equals("server"))
            {
                activeClientUsers = tcpHandler.activeClientUsers;

                for (int i = 0; i < oldActiveClientUsernumber; i++)
                {
                    if (activeUsers[activeClientPosition[i]] != null && activeClientUsers[i] != null)
                    {
                        activeUsers[activeClientPosition[i]].CertaintyLeftHand_Client = activeClientUsers[i].CertaintyLeftHand_Client;
                        activeUsers[activeClientPosition[i]].CertaintyRightHand_Client = activeClientUsers[i].CertaintyRightHand_Client;
                        if (activeUsers[activeClientPosition[i]].CertaintyLeftHand_Client > activeUsers[activeClientPosition[i]].CertaintyLeftHand_Server)
                        {

                            activeUsers[activeClientPosition[i]].RightHandRelativeToScreen = activeClientUsers[i].RightHandRelativeToScreen;
                        }

                        if (activeClientUsers[i].CertaintyLeftHand_Client > activeUsers[activeClientPosition[i]].CertaintyLeftHand_Server)
                        {
                            activeUsers[activeClientPosition[i]].LeftHandRelativeToScreen = activeClientUsers[i].LeftHandRelativeToScreen;
                        }


                    }


                }


            }
            //auf der Seite des Clients wird der tcpHandler aufgerufen und User-Daten werden zum Host gesendet
            else if ( serverOrClient.Equals("client"))
            {
                //TODO schließen des Threads besser behandeln
                if (send_receiveThread != null)
                {
                    send_receiveThread.Abort();
                }

                //Öffen des Tcp-Prozess
                tcpHandler = new TcpHandler();

                //Übertragung wichtiger Daten zum Handler
                tcpHandler.users = activeUsers;
                tcpHandler.activeUsers = countActiveBodies();
                tcpHandler.serverIP = serverIP;
                tcpHandler.serverPORT = serverPORT;

                //Thread erstellen
                send_receiveThread = new System.Threading.Thread(tcpHandler.tcpHandling);

                send_receiveThread.Start(serverOrClient);

            }
            //der timer wird für den offline-Modus nicht gebraucht
            else if (serverOrClient.Equals("offline"))
            {

            }
        }

        /// <summary>
        /// Event für die TouchInputs über das Canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            TouchInput.TouchPoint.TouchPoint[] tempOutput = new TouchInput.TouchPoint.TouchPoint[100];

            tempOutput = mergedTouchPoints;

            if (this.canvas1 != null)
            {

                //jeden TouchPoint vom Canvas in einer Schleife erhalten
                foreach (TouchPoint _touchPoint in e.GetTouchPoints(this.canvas1))
                {
                    User tpUser;



                    //Den Nutzer von _touchPoint finden
                    tpUser = findUserOfTouchPoint(_touchPoint);

                    if (tpUser == null)
                    {
                        break;
                    }

                    //tpUser = activeUsers[0];

                    if (tpUser != null && tempOutput[tpUser.getID()] == null)
                    {
                        tempOutput[tpUser.getID()] = new TouchInput.TouchPoint.TouchPoint();

                    }


                    //Berechnungen, wenn der TouchPoint gerade entstanden ist
                    if (_touchPoint.Action == TouchAction.Down)
                    {
                        //wenn der TouchP0int null ist, wird er hier initialisiert
                        if (tpUser != null && tempOutput[tpUser.getID()].TouchP0int == null)
                        {
                            Console.Write("x" + tpUser.Body.Joints[JointType.HandLeft].Position.X + "y" + tpUser.Body.Joints[JointType.HandLeft].Position.Y + "Z" + tpUser.Body.Joints[JointType.HandLeft].Position.Z);
                            tempOutput[tpUser.getID()] = new TouchInput.TouchPoint.TouchPoint(_touchPoint, _touchPoint.TouchDevice.Id, tpUser);

                        }
                        //ansonsten wird der TouchPoint  in die TouchPointCollection gespeichert
                        else if (tempOutput[tpUser.getID()].TouchP0int != _touchPoint && tempOutput[tpUser.getID()].TouchP0int.TouchDevice.Id != _touchPoint.TouchDevice.Id)
                        {
                            Boolean tempb = false;

                            //wird geprüft, ob dieser TouchPoint schon enthalten wird
                            foreach (TouchPoint tp in tempOutput[tpUser.getID()].TouchPointCollection)
                            {
                                if (tp != null && tp.TouchDevice.Id == _touchPoint.TouchDevice.Id)
                                {
                                    tempb = true;
                                    break;
                                }

                            }
                            //Wenn der TouchPoint noch nicht in der Collection existiert, wird dieser in die Collection an der Stelle NextTouchPoint gespeichert.
                            if (!tempb)
                            {
                                tempOutput[tpUser.getID()].TouchPointCollection[tempOutput[tpUser.getID()].NextTouchPoint] = new TouchPoint(_touchPoint.TouchDevice, _touchPoint.Position, _touchPoint.Bounds, _touchPoint.Action);

                                tempOutput[tpUser.getID()].NextTouchPoint++;
                            }


                        }
                        else
                        {

                        }

                    }

                    //Berechnungen, wenn sich der TouchPoint bewegt
                    else if (_touchPoint.Action == TouchAction.Move)
                    {
                        //TouchPoint wird aktualisiert
                        //entweder als TouchP0int

                        if (tempOutput[tpUser.getID()].TouchP0int != null && tempOutput[tpUser.getID()].TouchP0int.TouchDevice.Id == _touchPoint.TouchDevice.Id)
                        {
                            tempOutput[tpUser.getID()].TouchP0int = _touchPoint;

                          
                        }
                        //oder in der TouchPointCollection
                        else
                        {
                            for (int i = 0; i < tempOutput[tpUser.getID()].NextTouchPoint; i++)
                            {
                                if (tempOutput[tpUser.getID()].TouchPointCollection[i].TouchDevice.Id == _touchPoint.TouchDevice.Id)
                                {
                                    tempOutput[tpUser.getID()].TouchPointCollection[i] = _touchPoint;

                                }
                            }

                        }




                    }

                    //Berechnungen, wenn der TouchPoint aufgelöst wird
                    else if (_touchPoint.Action == TouchAction.Up)
                    {
                        //prüfen, ob der zu löschende TouchPoint dem HauptTouchPoint TouchP0int entspricht
                        if (tempOutput[tpUser.getID()].TouchP0int != null && tempOutput[tpUser.getID()].TouchP0int.TouchDevice.Id.Equals(_touchPoint.TouchDevice.Id))
                        {
                            if (tempOutput[tpUser.getID()].TouchPointCollection == null || tempOutput[tpUser.getID()].NextTouchPoint == 0)
                            {
                                tempOutput[tpUser.getID()].TouchP0int = null;
                            }
                            else
                            {

                                tempOutput[tpUser.getID()].TouchP0int = tempOutput[tpUser.getID()].TouchPointCollection[0];

                                tempOutput[tpUser.getID()].TouchPointCollection = shift(tempOutput[tpUser.getID()].TouchPointCollection, 0);


                                tempOutput[tpUser.getID()].NextTouchPoint--;


                            }
                           
                        }
                        //ansonsten ist der TouchPoint in der TouchPointCollection und muss dort gelöscht werden. 
                        //Weiterhin müssen die restlichen TouchPoints in der Collection neu angeordnet werden.
                        else
                        {
                            for (int i = 0; i < tempOutput[tpUser.getID()].NextTouchPoint; i++)
                            {
                                if (tempOutput[tpUser.getID()].TouchPointCollection[i].TouchDevice.Id == _touchPoint.TouchDevice.Id)
                                {

                                    tempOutput[tpUser.getID()].TouchPointCollection = shift(tempOutput[tpUser.getID()].TouchPointCollection, i);


                                    tempOutput[tpUser.getID()].NextTouchPoint--;
                                    break;

                                }
                            }
                        }

                    }




                }

            }

            mergedTouchPoints = tempOutput;

            //Test für die TouchPoints
            AllTests allTests = new AllTests();

            User user1 = activeUsers[0];

            //Demo1

            demo1.mergedTPs = mergedTouchPoints;
            demo1.TouchFrameDrawingDemo(canvas1,button1, activeUsers);


        }

        /// <summary>
        /// Logik, um einem Touchpoint einen User zuzuweisen
        /// </summary>
        User findUserOfTouchPoint(TouchPoint _touchPoint)
        {
            mappingHandler krttTemp = new mappingHandler();
            try
            {
                //ScreenWidth und ScreenHeight an das canvas anpassen
                //auskommentiert, da Übergabe von canvas1.Width und canvas1.Height falsche Werte übergibt
                // ->Initialisierung der Höhe und Weite im  mappingHandler unter screenWidth und screenHeigth einfügen
                // krttTemp.screenWidth = canvas1.Width;
                //krttTemp.screenHeigth = canvas1.Height;

                //linke untere Ecke des Bildschirms im CameraSpace
                CameraSpacePoint cspOrigin = new CameraSpacePoint();
                //Daten werden von der zentralen Settings-Datei genommen
                cspOrigin.X = float.Parse(Settings.Default.cspOriginX);
                cspOrigin.Y = float.Parse(Settings.Default.cspOriginY);
                cspOrigin.Z = float.Parse(Settings.Default.cspOriginZ);
                //rechte obere Ecke des Bildschirms im CamerSpace
                CameraSpacePoint cspCorner = new CameraSpacePoint();
                //Daten werden von der zentralen Settings-Datei genommen
                cspCorner.X = float.Parse(Settings.Default.cspCornerUpX);
                cspCorner.Y = float.Parse(Settings.Default.cspCornerUpY);
                cspCorner.Z = float.Parse(Settings.Default.cspCornerUpZ);

                //Konventierung der aktiven User
                activeUsers = krttTemp.convertCoordinatesKinectToScreen(activeUsers, cspOrigin, cspCorner);

            }
            catch
            {

            }
            //Daten werden von der zentralen Settings-Datei genommen
            return krttTemp.compareCoordinatesKinectToScreen(_touchPoint, activeUsers);
        }

        /// <summary>
        /// Ein TouchPoint-Array um index Einheiten nach links verschieben.
        /// Einträge an der äußersten rechten Stelle werden aber nicht befüllt.
        /// </summary>
        /// <param name="tp">TouchPoint-Array</param>
        /// <param name="index">Menge der zu shiftenen Stellen</param>
        /// <returns></returns>
        public TouchPoint[] shift(TouchPoint[] tp, int index)
        {

            for (int i = index; i < tp.Length; i++)
            {
                if (tp[i + 1] != null)
                {
                    tp[i] = tp[i + 1];
                }
                else
                {
                    tp[i] = null;
                    break;
                }
            }
            return tp;
        }


        /// <summary>
        /// getätigte Aktionen,  wenn das MainWindow geladen wird
        /// </summary>
        /// <param name="sender">object "sender" sendet das Event</param>
        /// <param name="e">Event Argumente</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {


        }

        /// <summary>
        /// getätigte Aktionen beim Schließen des Fensters
        /// </summary>
        /// <param name="sender">object "sender" sendet das Event</param>
        /// <param name="e">Event Argumente</param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Schließen des BodyFrameReaders
            if (this.bodyFrameReader != null)
            {

                this.bodyFrameReader.Dispose();

                this.bodyFrameReader = null;

            }

            //leeren von Körperdaten
            if (this.bodies != null)
            {

                bodies = null;

            }

            //Schließen des Sensors
            if (this.sensor != null)
            {

                this.sensor.Close();

                this.sensor = null;

            }

            //Threads schließen
            if (send_receiveThread != null)
            {

                send_receiveThread.Abort();

            }

        }









        /// <summary>
        /// Hilfsfunktion für das Erhalten der BodyDaten aus einem Frame
        /// </summary>
        /// <param name="sender">object "sender" sendet das Event</param>
        /// <param name="e">Event Argumente</param>
        private void Reader_OnBodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            BodyFrameReference bodyframeref = e.FrameReference;
            Boolean activeDifferent = false;

            try
            {
                //Abfangen der BodyFrames
                using (BodyFrame frame = bodyframeref.AcquireFrame())
                {

                    if (frame != null)
                    {
                        //Körperdaten aus dem Frame heraus aktualisieren
                        frame.GetAndRefreshBodyData(this.bodies);





                        //client
                        //Wenn sich die Anzahl aktiver User ändert
                        if (oldbodynumber != countActiveBodies() && serverOrClient.Equals("client"))
                        {
                            activeDifferent = true;
                            //Aufrufen der userTracking()-Funktion zur Erkennung aktiver Nutzer
                            userTracking();
                            demo1.updatecanvas(canvas1, activeUsers, button1);
                            demo1.updatecanvas2(canvas1, activeUsers, button1);

                            //client-seitig die Daten schicken, wenn sich die Anzahl aktiver Nutzer ändert
                            if (serverOrClient.Equals("client"))
                            {
                                //TODO schließen des Threads besser behandeln
                                if (send_receiveThread != null)
                                {
                                    send_receiveThread.Abort();
                                }

                                //Öffen des Tcp-Prozess
                                tcpHandler = new TcpHandler();

                                //Übertragung wichtiger Daten zum Handler
                                tcpHandler.users = activeUsers;
                                tcpHandler.activeUsers = countActiveBodies();
                                tcpHandler.serverIP = serverIP;
                                tcpHandler.serverPORT = serverPORT;

                                //Thread erstellen
                                send_receiveThread = new System.Threading.Thread(tcpHandler.tcpHandling);

                                send_receiveThread.Start(serverOrClient);

                            }

                        }

                        //server
                        //Wenn sich die Anzahl aktiver User ändert
                        if (serverOrClient.Equals("server") && (oldbodynumber != countActiveBodies() || tcpHandler.activeUsers != oldActiveClientUsernumber))
                        {
                            activeDifferent = true;
                            //Aufrufen der userTracking()-Funktion zur Erkennung aktiver Nutzer
                            userTracking();
                        }


                        //TODO erweitern und verbessern
                        //server-seitig abfragen, ob sich die Anzahl der Nutzer beim Client verändert hat
                        if (serverOrClient.Equals("server"))
                        {

                            if (activeDifferent)
                            {

                                if (tcpHandler.activeUsers == 0 && countActiveBodies() == 0)
                                {
                                    oldbodynumber = 0;
                                    oldActiveClientUsernumber = 0;
                                }
                                activeClientUsers = tcpHandler.activeClientUsers;
                                oldActiveClientUsernumber = tcpHandler.activeUsers;

                                _mappinghandler.mappingHandling(users, activeUsers, activeClientUsers, cameraSource, nextUserID, oldbodynumber);
                                cameraSource = _mappinghandler.cameraSource;

                                users = _mappinghandler.allUsers;
                                activeUsers = _mappinghandler.allActiveUsers;
                                nextUserID = _mappinghandler.nextUserID;



                                oldAllActiveUsersCounter = _mappinghandler.nextAllActiveUserID;
                                //TODO: rect4.Fill = new SolidColorBrush(activeClientUsers[0].getColor());


                                demo1.updatecanvas(canvas1, activeUsers, button1);
                                demo1.updatecanvas2(canvas1, activeUsers, button1);

                            }

                        }

                        if (serverOrClient.Equals("offline"))
                        {
                            userTracking();
                            demo1.updatecanvas(canvas1, activeUsers, button1);
                            demo1.updatecanvas2(canvas1, activeUsers, button1);
                        }

                        //Wenn der Rechner der Client ist, nur lokale Daten beachten
                        if (serverOrClient.Equals("client"))
                        {
                            oldAllActiveUsersCounter = nextUserIDactive;
                        }

                        

   

                       
                            

                    }
                }



            }

            catch (Exception)
            {
                //TODO behandlen von eventuellen Exceptions
            }


        }

        /// <summary>
        /// Eintragen neuer Nutzer und Aktualisierung bereits bestehender Nutzer
        /// </summary>
        private void userTracking()
        {
            //erstellt ein temporäres Array für die Körperdaten aktiver User
            Body[] userBodies = filterActiveBodies();

            //Boolean zur Bestimmung, ob ein User existiert, der mit der Farbe des aktuellen Körpers übereinstimmt
            Boolean exists = false;

            //Neuzuordnung des Arrays activeUsers (maximal 3 Personen)
            activeUsers = new User[3];

            //nächste UserID, die vergeben wird
            nextUserIDactive = 0;

            foreach (Body body in userBodies)
            {
                //Zurücksetzung von exists
                exists = false;

                if (body != null)
                {
                    //Körpermittelpunkt heraussuchen
                    csp = sensor.CoordinateMapper.MapCameraPointToColorSpace(body.Joints[JointType.SpineMid].Position);

                    //mit der FarbBehandlung Farbe vom Körper (Punkt um den Körpermittelpunkt) herausfinden
                    Color tempColor = ch.colorHandling(colorBitmap, (int)csp.X, (int)csp.Y, 0);

                    foreach (User user in users)
                    {

                        if (user != null && nextUserIDactive < 3)
                        {

                            //ob User die gleiche Farbe besitzt
                            if (ch.compareColor(user.getColor(), tempColor))
                            {
                                //Boolean auf wahr setzen, ein solcher Kröper existiert 
                                exists = true;

                                //Körperdaten aktualisieren
                                user.Body = body;

                                //Wahrscheinlichkeit, wie gut die Kamera die Person erkennt
                                user.CertaintyRightHand_Server = (float)body.HandRightConfidence;
                                user.CertaintyLeftHand_Server = (float)body.HandLeftConfidence;

                                //Userdaten ins Array der aktiven Nutzer eintragen
                                activeUsers[nextUserIDactive] = user;

                                //
                                cameraSource[nextUserIDactive] = "The host-camera recognizes this person. ";
                                //
                                user.CameraDetection = cameraSource[nextUserIDactive];
                                //ID für die aktiven User hoch setzen
                                nextUserIDactive++;
                                break;

                            }
                        }
                    }

                    //wenn keine Übereinstimmung der Farben existiert
                    if (!exists)
                    {

                        if (users[nextUserID] == null)
                        {

                            //neuen User mit Hilfe des Konstruktors erstllen
                            users[nextUserID] = new User(body, ch.colorHandling(colorBitmap, (int)csp.X, (int)csp.Y, 0), "" + nextUserID, nextUserID);

                            //allgemeine UserID für den nächsten neuen Nutzer hochsetzen
                            nextUserID++;

                            //user in das Array für die aktiven Nutzer eintragen
                            activeUsers[nextUserIDactive] = users[nextUserID - 1];

                            //ID für den nächsten aktiven Nutzer hochsetzen
                            nextUserIDactive++;
                        }
                        else
                        {
                            //sollte nicht erreicht werden
                        }
                    }

                }
            }

            //oldbodynumber auf den aktuellen Stand an aktiven Nutzer setzen
            oldbodynumber = nextUserIDactive;

        }



        /// <summary>
        /// Zählt die momentan aktiven Bodies vor der Kamera
        /// </summary>
        /// <returns>Anzahl aktiver Bodies</returns>
        public int countActiveBodies()
        {
            //den BodyCounter zurücksetzen
            this.activeBodies = 0;

            if (activ)
            {

                if (bodies != null)
                {

                    foreach (Body body in this.bodies)
                    {
                        //falls der Body body erfasst wird, wird der Counter inkrementiert
                        if (body != null)
                        {
                            if (body.IsTracked)
                            {
                                this.activeBodies++;


                            }
                        }

                    }
                }



            }

            activ = sensor.IsAvailable;

            return activeBodies;
        }



        /// <summary>
        /// Funktion zur Filterung der Positionen aktiver Nutzer
        /// </summary>
        /// <returns>Array an aktiver Körper</returns>
        public Body[] filterActiveBodies()
        {

            //Anpassung der Positionen an die Anzahl der aktiven Users
            userNumber = countActiveBodies();

            Body[] outputbodies = new Body[sensor.BodyFrameSource.BodyCount];


            //Tracking der Positionen der PersonenMittelpunkte mithilfe der Hilfsklasse Coordinates
            //dabei werden nur aktive Körper berücksichtigt
            if (activ)
            {

                if (bodies != null)
                {

                    iteration = 0;
                    foreach (Body body in this.bodies)
                    {
                        if (body != null)
                        {

                            if (body.IsTracked)
                            {

                                outputbodies[iteration] = body;

                                iteration++;

                            }
                        }
                    }
                }
            }
            return outputbodies;
        }


        /// <summary>
        /// Behandlung der ColorFrames, die vom Sensor ankommen
        /// </summary>
        /// <param name="sender">object "sender" sendet das Event</param>
        /// <param name="e">Event Argumente</param>
        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            // ColorFrame ist IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    FrameDescription colorFrameDescription = colorFrame.FrameDescription;

                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        this.colorBitmap.Lock();

                        // Prüft Daten und schreibt die ColorFrame Daten auf die DisplayBitmap
                        if ((colorFrameDescription.Width == this.colorBitmap.PixelWidth) && (colorFrameDescription.Height == this.colorBitmap.PixelHeight))
                        {
                            colorFrame.CopyConvertedFrameDataToIntPtr(
                                this.colorBitmap.BackBuffer,
                                (uint)(colorFrameDescription.Width * colorFrameDescription.Height * 4),
                                ColorImageFormat.Bgra);

                            this.colorBitmap.AddDirtyRect(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight));
                        }

                        this.colorBitmap.Unlock();

                    }
                }
            }
        }



    }
}