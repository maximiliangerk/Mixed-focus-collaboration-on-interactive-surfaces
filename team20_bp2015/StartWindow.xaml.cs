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
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        /// <summary>
        /// Information, ob der User Server oder Client ist
        /// </summary>
        private string _ServerOrClient = "";

        /// <summary>
        /// eingegebene ServerIP
        /// </summary>
        private string _ServerIP = "";

        /// <summary>
        /// eingegebener ServerPort
        /// </summary>
        private int _ServerPort = 8080;

        //Boolean-Werte, ob client/Server gesetzt sind und die Werte aus dem Settingswindow gesetzt sind
        private bool clientServerSet = false;
        private bool settingsValuesRight = false;

        //Settingswindow
        SettingsWindow sw = new SettingsWindow();

        /// <summary>
        /// Main des Startfenster, welche die grundlegenden Komponenten initialisiert
        /// </summary>
        public StartWindow()
        {
            InitializeComponent();
            //Button des InitializeWindow muss unsichtbar sein wegen dem unten erwähnten Fehler!
            this.button_init.Visibility = Visibility.Hidden;
            this.button_Start.Visibility = Visibility.Hidden;
            
        }

        /// <summary>
        /// Button, bei dessen Klick sich der User/Rechner als Server identifiziert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_server_Click(object sender, RoutedEventArgs e)
        {
            _ServerOrClient = "server";
            clientServerSet = true;
            if (settingsValuesRight)
            {
                this.button_Start.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Button, bei dessen Klick sich der User/Rechner als Client identifiziert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_client_Click(object sender, RoutedEventArgs e)
        {
            _ServerOrClient = "client";
            clientServerSet = true;
            if (settingsValuesRight)
            {
                this.button_Start.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Klick, bei dem die Hauptanwendung (samt Demo1) gestartet wird und das Startfenster automatisch geschlossen wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Start_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            
            MainWindow mw = new MainWindow();
            mw.serverOrClient = _ServerOrClient;
            mw.serverIP = _ServerIP;
            mw.serverPORT = _ServerPort;
            mw.Show();
            this.Close();
        }

        /// <summary>
        /// automatische Änderung der serverIp bei Eingabe im Textfeld
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            _ServerIP = textBoxIpInput.Text;
        }

        /// <summary>
        /// automatische Änderung des serverPorts bei Eingabe im Textfeld
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _ServerPort = int.Parse(textBox.Text);
        }

        //Bei Klick wird das Settingsfenster für die Eingabe de rzu initialisierenden Daten geöffnet
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            
            sw.Box_csp1X.Text = Settings.Default.cspOriginX;
            sw.Box_csp1Y.Text = Settings.Default.cspOriginY;
            sw.Box_csp1Z.Text = Settings.Default.cspOriginZ;
            sw.Box_csp2X.Text = Settings.Default.cspCornerUpX;
            sw.Box_csp2Y.Text = Settings.Default.cspCornerUpY;
            sw.Box_csp2Z.Text = Settings.Default.cspCornerUpZ;

            if(sw.Box_csp1X.Text != "" && sw.Box_csp1Y.Text != "" && sw.Box_csp1Z.Text != "" && sw.Box_csp2X.Text != "" && sw.Box_csp2Y.Text != "" && sw.Box_csp2Z.Text != "")
            {
                settingsValuesRight = true;
            }

            sw.Show();
        }

        //Check-Button, der der Start-Button verfügbar macht, wenn Server/Client-Information verfügbar ist(Buttons) und Settings-Daten nicht leer sind
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (sw.Box_csp1X.Text != "" && sw.Box_csp1Y.Text != "" && sw.Box_csp1Z.Text != "" && sw.Box_csp2X.Text != "" && sw.Box_csp2Y.Text != "" && sw.Box_csp2Z.Text != "")
            {
                settingsValuesRight = true;
            }
            if (clientServerSet)
            {
                this.button_Start.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Aufruf zum Initialiseren
        /// 
        /// auskommentiert, da InitializeWIndow eine unlösbare Nullreference in einer von VisualStudio generierten Datei erzeugt
        /// WICHTIG Ausführung nur mit vorherigen seperater Sicherung des Projects
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_init_Click(object sender, RoutedEventArgs e)
        {
            //auskommentiert, da InitializeWIndow eine unlösbare Nullreference in einer von VisualStudio generierten Datei erzeugt
            //WICHTIG Ausführung nur mit vorherigen seperater Sicherung des Projects
            //InitializeWindow iw = new InitializeWindow();
            //iw.Show();
        }
    }
}
