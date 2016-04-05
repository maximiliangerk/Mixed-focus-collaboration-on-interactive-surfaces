using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
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

namespace team20_bp2015
{
    class AllTests
    {
        /// <summary>
        /// leerer Konstruktor zur Initialiserung
        /// </summary>
        public AllTests()
        {

        }


        public void testColorDetection_WithDifferentPerspectives(User[] allActiveUsers)
        {
            foreach(User user in allActiveUsers)
            {
                if(user != null && user.CameraDetection != "")
                {
                    if (user.CameraDetection.Equals("The host-camera recognizes this person. "))
                    {
                        Console.Write("Only Host recognizes this Person : Color - " + user.getColor() + "|");
                    }
                    else if (user.CameraDetection.Equals("The client-camera recognizes this person. "))
                    {
                        Console.Write("Both recognizes this Person : Color - " + user.getColor() + "|");
                    }
                    else if (user.CameraDetection.Equals("Both cameras recognize this person. "))
                    {
                        Console.Write("Only Client recognizes this Person : Color - " + user.getColor() + "|");
                    }
                }
               
            }
        }

        /// <summary>
        /// Test, um die Touch-Koordinaten abzufangen und anzuzeigen
        /// </summary>
        /// <param name="textBlock">übergebener TextBlock für die Ausgabe</param>
        /// <param name="touchPoints">Array an TouchPoints, die angezeigt werden sollen</param>
        public void testTouchPointConnector(TextBlock textBlock, TouchInput.TouchPoint.TouchPoint[] touchPoints)
        {
            textBlock.Text = "";
            if(touchPoints != null)
            {
                foreach(TouchInput.TouchPoint.TouchPoint touchPoint in touchPoints)
                {
                    if(touchPoint != null && touchPoint.TouchP0int != null)
                    {
                        textBlock.Text += "The touchPoint with the coordinates X=" + touchPoint.TouchP0int.Position.X + " and Y=" 
                            + touchPoint.TouchP0int.Position.Y + " is registered from the user " + touchPoint.UserID + " with the color " + touchPoint.User.getColor() + ".\n\n";

                        foreach(TouchPoint touchPoint2 in touchPoint.TouchPointCollection)
                        {
                            if(touchPoint2 != null)
                            {
                                textBlock.Text += "The touchPoint with the coordinates X=" + touchPoint2.Position.X 
                                    + " and Y=" + touchPoint2.Position.Y + " is registered from the user " + touchPoint.UserID + " with the color " + touchPoint.User.getColor() + ".\n\n";
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Test, um die umgewandelten relativen Hand-Koordinaten anzuzeigen
        /// Anzeige von bis zu 3 aktiven Usern
        /// </summary>
        /// <param name="tb1">erster Textblock für den ersten User</param>
        /// <param name="tb2">zweiter Textblock für den zweiten User</param>
        /// <param name="tb3">dritter Textblock für den dritten User</param>
        /// <param name="activeUsers">Array der aktiven User</param>
        /// <param name="tpa">TouchPointArray mit verbunden User/TouchPoint-Daten</param>
        public void testHandPositions(TextBlock tb1, TextBlock tb2, TextBlock tb3, User[] activeUsers, TouchInput.TouchPoint.TouchPoint[] tpa)
        {
            int tbIndex = 0;

            TouchPoint[] currentTPs = new TouchPoint[3];
            User[] currentUsers = new User[3];

            foreach(User user in activeUsers)
            {
                
                if (user != null)
                {
                    int currentID = user.getID();
                    tb3.Text = "ID=" + currentID + " Farbe=" + user.getColor();
                    if (tpa != null &&  tpa[currentID] != null && tpa[currentID].TouchP0int != null)
                    {
                        TouchPoint currentTP = tpa[currentID].TouchP0int;
                        
                        currentTPs[tbIndex] = currentTP;
                        currentUsers[tbIndex] = user;
                        tbIndex++;

                        if(false)
                        switch (currentID)
                        {
                            case 0:
                                {
                                    tb1.Text = "Der User= " + user.getID() + " gehört der TouchPoint " + currentTP.Position + " \nund leftX " + user.LeftHandRelativeToScreen.X + " und leftY " + user.LeftHandRelativeToScreen.Y + "\nFarbe ist: " + user.getColor();
                                    tbIndex++;
                                    break;
                                }
                            case 1:
                                {
                                    tb2.Text = "Der User= " + user.getID() + " gehört der TouchPoint " + currentTP.Position + " \nund leftX " + user.LeftHandRelativeToScreen.X + " und leftY " + user.LeftHandRelativeToScreen.Y + "\nFarbe ist: " + user.getColor();
                                    tbIndex++;
                                    break;
                                }
                            case 2:
                                {
                                    tb3.Text = "Der User= " + user.getID() + " gehört der TouchPoint " + currentTP.Position + " \nund leftX " + user.LeftHandRelativeToScreen.X + " und leftY " + user.LeftHandRelativeToScreen.Y + "\nFarbe ist: " + user.getColor();

                                    break;
                                }
                        }
                    }
                    

                    
                }

            }

            if(currentTPs[0] != null)
            {
                User user = currentUsers[0];
                TouchPoint currentTP = currentTPs[0];
                tb1.Text = "Der User= " + currentUsers[0].getID() + " gehört der TouchPoint " + currentTPs[0].Position + " \nund leftX " + currentUsers[0].LeftHandRelativeToScreen.X + " und leftY " + currentUsers[0].LeftHandRelativeToScreen.Y + "\nFarbe ist: " + currentUsers[0].getColor();

            }
            
            if (currentTPs[1] != null)
            {
                User user2 = currentUsers[1];
                TouchPoint currentTP2 = currentTPs[1];
                tb2.Text = "Der User= " + currentUsers[1].getID() + " gehört der TouchPoint " + currentTPs[1].Position + " \nund leftX " + currentUsers[1].LeftHandRelativeToScreen.X + " und leftY " + currentUsers[1].LeftHandRelativeToScreen.Y + "\nFarbe ist: " + currentUsers[1].getColor();

            }
            
            if (currentTPs[2] != null)
            {
                User user3 = currentUsers[2];
                TouchPoint currentTP3 = currentTPs[2];
                tb3.Text = "Der User= " + currentUsers[2].getID() + " gehört der TouchPoint " + currentTPs[2].Position + " \nund leftX " + currentUsers[2].LeftHandRelativeToScreen.X + " und leftY " + currentUsers[2].LeftHandRelativeToScreen.Y + "\nFarbe ist: " + currentUsers[2].getColor();

            }
            
        }

    }
}
