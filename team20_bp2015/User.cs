using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace team20_bp2015
{
    public class User
    {
        //Parameterliste

        /// <summary>
        /// vom System zugewiesene Nummer des Users
        /// </summary>
        private int userID;

        /// <summary>
        /// [unbenutzt] Name des Users
        /// </summary>
        private string userName;

        /// <summary>
        /// Farbe des Shirts
        /// </summary>
        private Color userColor;

        /// <summary>
        /// Body-Daten des Users
        /// </summary>
        private Body _Body;
        public Body Body
        {
            get { return _Body; }
            set { _Body = value; }
        }

        /// <summary>
        /// String, welcher verrät, welche Kameras den User erkennen
        /// </summary>
        private string _CameraDetection = "";
        public string CameraDetection
        {
            get { return _CameraDetection; }
            set { _CameraDetection = value; }
        }

        /// <summary>
        /// Server
        /// Wahrscheinlichkeit, wie sehr sich die Client_KinectKamera über die Position des Users ist
        /// </summary>
        private float _CertaintyRightHand_Server = 0.0f;
        public float CertaintyRightHand_Server
        {
            get { return _CertaintyRightHand_Server; }
            set { _CertaintyRightHand_Server = value; }
        }

        /// <summary>
        /// Wahrscheinlichkeit, wie sehr sich die Client_KinectKamera über die Position des Users ist
        /// </summary>
        private float _CertaintyLeftHand_Server = 0.0f;
        public float CertaintyLeftHand_Server
        {
            get { return _CertaintyLeftHand_Server; }
            set { _CertaintyLeftHand_Server = value; }
        }


        /// <summary>
        /// Client
        /// Wahrscheinlichkeit, wie sehr sich die Client_KinectKamera über die Position des Users ist
        /// </summary>
        private float _CertaintyRightHand_Client = 0.0f;
        public float CertaintyRightHand_Client
        {
            get { return _CertaintyRightHand_Client; }
            set { _CertaintyRightHand_Client = value; }
        }

        /// <summary>
        /// Wahrscheinlichkeit, wie sehr sich die Client_KinectKamera über die Position des Users ist
        /// </summary>
        private float _CertaintyLeftHand_Client = 0.0f;
        public float CertaintyLeftHand_Client
        {
            get { return _CertaintyLeftHand_Client; }
            set { _CertaintyLeftHand_Client = value; }
        }

        /// <summary>
        /// Koordinaten in Relation zum Touchscreen und dem Blickwinkel der Kamera
        ///  - von der linken Hand
        /// </summary>
        private Vector3 _LeftHandRelativeToScreen;
        public Vector3 LeftHandRelativeToScreen
        {
            get { return _LeftHandRelativeToScreen; }
            set { _LeftHandRelativeToScreen = value; }
        }

        /// <summary>
        /// Koordinaten in Relation zum Touchscreen und dem Blickwinkel der Kamera
        ///  - von der rechten Hand
        /// </summary>
        private Vector3 _RightHandRelativeToScreen;
        public Vector3 RightHandRelativeToScreen
        {
            get { return _RightHandRelativeToScreen; }
            set { _RightHandRelativeToScreen = value; }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="userBody">Körperdaten des Nutzers</param>
        /// <param name="userColor">Farbe des Nutzers</param>
        /// <param name="userName">Name des Nutzers(vorläufig)</param>
        /// <param name="userID">ID des Nutzers</param>
        public User(Body userBody, Color userColor, string userName, int userID)
        {
            Body = userBody;
            this.userColor = userColor;
            this.userName = userName;
            this.userID = userID;
            CameraDetection = "";
            
        }

        /// <summary>
        /// TODO überarbeiten
        /// getter und setter für userID
        /// </summary>
        /// <returns></returns>
        public int getID()
        {
            return this.userID;
        }

        public void setID(int userID)
        {
            this.userID = userID;
        }

        /// <summary>
        /// TODO überarbeiten
        /// getter und setter für userName
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            return this.userName;
        }

        public void setName(string userName)
        {
            this.userName = userName;
        }


        /// <summary>
        /// TODO überarbeiten
        /// getter und setter für userColor
        /// </summary>
        /// <returns></returns>
        public Color getColor()
        {
            return this.userColor;
        }

        public void setColor(Color userColor)
        {
            this.userColor = userColor;
        }




    }
}
