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
using System.Windows.Shapes;
using team20_bp2015.Properties;

namespace team20_bp2015
{
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        //untere linke Ecke
        private string _CspOriginX;

        private string _CspOriginY;

        private string _CspOriginZ;

        //obere rechte Ecke
        private string _CspCornerUpX;

        private string _CspCornerUpY;

        private string _CspCornerUpZ;
        
        /// <summary>
        /// Konstruktor
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button, um die Daten in die Settings-Datei zu speichern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)
        {


            Settings.Default.cspOriginX = _CspOriginX;
            Settings.Default.cspOriginY = _CspOriginY;
            Settings.Default.cspOriginZ = _CspOriginZ;
            Settings.Default.cspCornerUpX = _CspCornerUpX;
            Settings.Default.cspCornerUpY = _CspCornerUpY;
            Settings.Default.cspCornerUpZ = _CspCornerUpZ;
            Settings.Default.Save();
            
            
            
            this.Close();
        }

        /// <summary>
        /// X-Koordinate der linken unteren Ecke wurde verändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_csp1X_TextChanged(object sender, TextChangedEventArgs e)
        {
            _CspOriginX = Box_csp1X.Text;
        }

        /// <summary>
        /// Y-Koordinate der linken unteren Ecke wurde verändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_csp1Y_TextChanged(object sender, TextChangedEventArgs e)
        {
            _CspOriginY = Box_csp1Y.Text;
        }

        /// <summary>
        /// Z-Koordinate der linken unteren Ecke wurde verändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_csp1Z_TextChanged(object sender, TextChangedEventArgs e)
        {
            _CspOriginZ = Box_csp1Z.Text;
        }

        /// <summary>
        /// X-Koordinate der oberen rechten Ecke wurde verändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_csp2X_TextChanged(object sender, TextChangedEventArgs e)
        {
            _CspCornerUpX = Box_csp2X.Text;
        }

        /// <summary>
        /// Y-Koordinate der oberen rechten Ecke wurde verändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_csp2Y_TextChanged(object sender, TextChangedEventArgs e)
        {
            _CspCornerUpY = Box_csp2Y.Text;
        }

        /// <summary>
        /// Z-Koordinate der oberen rechten Ecke wurde verändert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box_csp2Z_TextChanged(object sender, TextChangedEventArgs e)
        {
            _CspCornerUpZ = Box_csp2Z.Text;
        }

    }
}
