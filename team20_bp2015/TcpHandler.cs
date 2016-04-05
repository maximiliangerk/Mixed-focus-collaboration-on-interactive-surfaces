using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Net;
using System.Net.Sockets;
using Microsoft.Kinect;

namespace team20_bp2015
{
    class TcpHandler
    {

        //Parameterliste
            public string data = null;

            public string test;

            private string type;

            public string serverIP = "127.0.0.1";

            public int serverPORT = 8080;

            public int activeUsers = 0;

            private char interruption = '|';

            public User[] users;

            public User[] activeClientUsers;

            private string clientUserdata = "";


        public TcpHandler()
        {

        }

        /// <summary>
        /// Handling der SendenEmpfangen-Funktion und Aufruf 
        /// </summary>
        /// <param name="type0">Typ: "server" oder "client"</param>
        public void tcpHandling(object type0)
        {

            //Übergabe type0 an type, damit type global verfügbar ist
            type = (string)type0;

            //startet Server-Anwendung
            if (type.Equals("server"))
            {

                StartListening(this.serverIP, this.serverPORT);

            }

            //startet Client-Anwendung
            if (type.Equals("client"))
            {
                //Übergabe der Dtaen vom Client in einen String
                //1. Stelle Anzahl aktiver Nutzer
                clientUserdata += "" + activeUsers + interruption;

                mappingHandler krtt = new mappingHandler();

                //linke untere Ecke des Bildschirms im CameraSpace
                CameraSpacePoint cspOrigin = new CameraSpacePoint();
                cspOrigin.X = (float)0.4216754;
                cspOrigin.Y = (float)-0.4185504;
                cspOrigin.Z = (float)3.742178;
                //rechte obere Ecke des Bildschirms im CamerSpace
                CameraSpacePoint cspCorner = new CameraSpacePoint();
                cspCorner.X = (float)-1.017629;
                cspCorner.Y = (float)0.4573232;
                cspCorner.Z = (float)2.621581;
                users = krtt.convertCoordinatesKinectToScreen(users, cspOrigin, cspCorner);

                foreach(User user in users)
                {

                    if(user != null)
                    {


                      

                        //Farbe des Nutzers
                        clientUserdata += "" + user.getColor() + interruption;

                        Vector3 rightHand = user.RightHandRelativeToScreen;

                        //Rechte Hand samt Position
                        clientUserdata += "R" + interruption + rightHand.X + interruption + rightHand.Y + interruption + rightHand.Z + interruption;

                        clientUserdata += "V" + user.Body.HandRightConfidence + interruption;

                        Vector3 leftHand = user.LeftHandRelativeToScreen;

                        //Linke Hand samt Position
                        clientUserdata += "L" + interruption + leftHand.X + interruption + leftHand.Y + interruption + leftHand.Z + interruption;


                        clientUserdata += "W" + user.Body.HandLeftConfidence + interruption;
                    }
                    
                }

                StartClient(this.serverIP, this.serverPORT, clientUserdata);

            }

        }

        /// <summary>
        /// Dekodieren der Daten aus dem String
        /// </summary>
        /// <param name="input">String vom Client</param>
        private void decodeInput(string input)
        {
            //String in Abschnitte teilen
            string[] data = input.Split(interruption);

            users = new User[3];
            
            activeUsers = Int32.Parse(data[0]);

            for (int i = 0; i < activeUsers; i++)
            {
                //Farbebehandlung
                Color colorUser = new Color();
                colorUser.A = System.Drawing.ColorTranslator.FromHtml(data[i * 13 + 1]).A;
                colorUser.R = System.Drawing.ColorTranslator.FromHtml(data[i * 13 + 1]).R;
                colorUser.G = System.Drawing.ColorTranslator.FromHtml(data[i * 13 + 1]).G;
                colorUser.B = System.Drawing.ColorTranslator.FromHtml(data[i * 13 + 1]).B;

                //neuer Nutzer erstellen
                users[i] = new User(null, colorUser, i + "", i);

                Vector3 rightHand = new Vector3();

                rightHand.X = float.Parse(data[i * 13 + 3]);
                rightHand.Y = float.Parse(data[i * 13 + 4]);
                rightHand.Z = float.Parse(data[i * 13 + 5]);

                //cspRightHand des Nutzers legen
                users[i].RightHandRelativeToScreen = rightHand;

                users[i].CertaintyRightHand_Client = float.Parse(data[i * 13 + 7]);

                Vector3 leftHand = new Vector3();

                leftHand.X = float.Parse(data[i * 13 + 9]);
                leftHand.Y = float.Parse(data[i * 13 + 10]);
                leftHand.Z = float.Parse(data[i * 13 + 11]);

                //cspLeftHand des Nutzers legen
                users[i].LeftHandRelativeToScreen = leftHand;

                users[i].CertaintyLeftHand_Client = float.Parse(data[i * 13 + 13]); ;
            }
            activeClientUsers = users;
            test = this.data;
            
        }

        public /* static */void StartClient(string serverIP, int serverPORT, string output)
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            try
            {
                //Einrichten eines Remote Endpunkt für den Socket.
                IPHostEntry ipHostInfo = Dns.GetHostByAddress(serverIP);

                IPAddress ipAddress = ipHostInfo.AddressList[0];

                IPEndPoint remoteEP = new IPEndPoint(ipAddress, serverPORT);

                //Einen TCP/IP Socket erstellen.
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //Den Socket zum Remote Endpunkt verbinden und jedliche Fehler abfangen.
                try
                {

                    sender.Connect(remoteEP);
                    
                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array. Den Data String in ein byte attay encodieren.
                    //TODO das mit dem counter ändern, Umsetzung ohne counter
                    int counter = 0;

                    while (counter != 1)
                    {

                        //Die Daten über den Socket versenden.

                        byte[] msg = Encoding.ASCII.GetBytes(output);

                        int bytesSent = sender.Send(msg);
                        
                        //Die Rückmeldung des RemoteDevice erhalten.
                        int bytesRec = sender.Receive(bytes);

                        Console.WriteLine("Echoed test = {0}",

                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    }


                    //Den Socket freigeben.

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                         Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                        Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                         Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                   Console.WriteLine(e.ToString());
            }
        }


        public /*static*/ void StartListening(string serverIP, int serverPORT)
        {
            //Data Buffer für reinkommende Daten.
            byte[] bytes = new Byte[1024];

            //Einrichten eines lokalen Endpunkt für den Socket.
            // Dns.GetHostName gibt den Namen des Hosts wieder, bei dem die App läuft.

            IPHostEntry ipHostInfo = Dns.GetHostByAddress(serverIP);

            IPAddress ipAddress = ipHostInfo.AddressList[0];

            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, serverPORT);

            // Erstellt einen TCP/IP Socket. 
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            //Binde den Socket an den lokalen endpunkt und warte auf reinkommende Verbindungen

            try
            {

                listener.Bind(localEndPoint);

                listener.Listen(10);

                //Fange an auf Verbindungen zu reagieren.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");

                    //Program ist eingestellt solange es auf reinkommende Verbindungen wartet.
                    Socket handler = listener.Accept();

                    data = null;

                    //Eine reinkommende Verbindung muss verarbeitet werden.
                    if (true)
                    {

                        bytes = new byte[1024];

                        int bytesRec = handler.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        //TODO ausgabe
                        //break;

                        Console.WriteLine("Text received : {0}", data);

                        

                    }

                    //Zeige die Daten auf der Konsole
                    Console.WriteLine("Text received : {0}", data);

                    //Wiederhole die Daten dem Client.
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    decodeInput(data);
                    
                    handler.Send(msg);

                    handler.Shutdown(SocketShutdown.Both);

                    handler.Close();

                }

            }
            catch (Exception e)
            {
                  Console.WriteLine(e.ToString());
            }

             Console.WriteLine("\nPress ENTER to continue...");

             Console.Read();

        }
    }
}
