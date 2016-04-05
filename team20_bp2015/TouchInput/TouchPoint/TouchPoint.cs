using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace team20_bp2015.TouchInput.TouchPoint
{
    public class TouchPoint
    {
        /// <summary>
        /// get/set für die ID des TouchPoints
        /// </summary>
        private int _TouchPointID;
        public int TouchPointID
        {
            get { return _TouchPointID; }
            set { _TouchPointID = value; }
        }

        /// <summary>
        /// get/set für die ID des dem TouchPoints zugeordneten Users
        /// </summary>
        public int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        /// <summary>
        /// get/set für des dem TouchPoints zugeordneten Users
        /// </summary>
        private User _User;
        public User User
        {
            get { return _User; }
            set { _User = value; }
        }

        /// <summary>
        /// get/set für das TouchDevice
        /// </summary>
        private TouchDevice _TouchDevice;
        public TouchDevice TouchDevice
        {
            get { return _TouchDevice; }
            set { _TouchDevice = value; }
        }

        /// <summary>
        /// get/set für die TouchEventArgs
        /// </summary>
        private TouchEventArgs _TouchEventArgs;
        public TouchEventArgs TouchEventArgs
        {
            get { return _TouchEventArgs; }
            set { _TouchEventArgs = value; }
        }

        /// <summary>
        /// get/set für den TouchPoint
        /// </summary>
        private System.Windows.Input.TouchPoint _TouchPoint;
        public System.Windows.Input.TouchPoint TouchP0int
        {
            get { return _TouchPoint; }
            set { _TouchPoint = value; }
        }

        /// <summary>
        /// get/set für ein Array der TouchPoints, die neuer als TouchP0int sind und dem gleichen User zugeordnet werden
        /// </summary>
        private System.Windows.Input.TouchPoint[] _TouchPointCollection;
        public System.Windows.Input.TouchPoint[] TouchPointCollection
        {
            get { return _TouchPointCollection; }
            set { _TouchPointCollection = value; }
        }

        /// <summary>
        /// get/set index für den nächsten TouchPoint in der Collection
        /// </summary>
        private int _nextTouchPoint;
        public int NextTouchPoint
        {
            get { return _nextTouchPoint; }
            set { _nextTouchPoint = value; }
        }

        /// <summary>
        /// Konstruktor mit Initalisierung der TouchPointCollection und dem Index NextTouchPoint
        /// </summary>
        public TouchPoint()
        {
            TouchPointCollection = new System.Windows.Input.TouchPoint[100];
            NextTouchPoint = 0;
        }

        /// <summary>
        /// Konstruktor mit vorgegeben Daten
        /// </summary>
        /// <param name="touchPoint">ein initialiserender TouchPoint</param>
        /// <param name="touchPointID">die ID vom TouchPoint/TouchDevice</param>
        /// <param name="user">der zugeordnete User</param>
        public TouchPoint(System.Windows.Input.TouchPoint touchPoint, int touchPointID ,User user)
        {
            User = user;

            UserID = user.getID();

            TouchP0int = touchPoint;

            TouchPointID = touchPointID;

            TouchPointCollection = new System.Windows.Input.TouchPoint[100];
            NextTouchPoint = 0;


        }


    }
}
