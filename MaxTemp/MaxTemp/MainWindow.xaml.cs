using System;
using System.Globalization;
using System.IO;
using System.Windows;
using Path = System.IO.Path;

namespace MaxTemp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Diese Routine (EventHandler des Buttons Auswerten) liest die Werte
        /// zeilenweise aus der Datei temps.csv aus, merkt sich den höchsten Wert
        /// und gibt diesen auf der Oberfläche aus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            //Zugriff auf Datei erstellen.
            string relativePath = Path.Combine("MaxTemp", "temps.csv");
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temps.csv");
            
            //In einer Schleife die Werte holen und auswerten. Den größten Wert "merken".
            double maxValue = double.MinValue;
            string maxValueFormatted = string.Empty;
            string line;
            
            int currentLine = 0;      // zählt alle gelesenen Zeilen
            int lineOfMax = -1;       // Zeilennummer, an der der aktuelle Max gefunden wurde

            // Öffne die Datei zum Lesen in einem FileStream und lade sie mit einem StreamReader
            using (FileStream datenStrom = new FileStream(fullPath, FileMode.Open))
            using (StreamReader reader = new StreamReader(datenStrom))

                // Lese Zeile für Zeile, bis alle Zeilen der Datei verarbeitet sind
            
            while ((line = reader.ReadLine()) != null)
            {
                 currentLine++; // jede gelesene Zeile hochzählen
                 string[] parts = line.Split(',');

                // Prüfe, ob mindestens drei Elemente vorhanden sind (Index 0,1,2)
                // Versuche den dritten Wert als Zahl (double) zu parsen
                // NumberStyles.Any bedeutet, dass sämtliche üblichen Zahlenformatierungen erlaubt sind
                // verwende InvariantCulture für Punkt als Dezimaltrennzeichen
                if (parts.Length >= 3 && double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double wert))
                {
                    if (wert > maxValue)
                    {
                        maxValue = wert;
                        // Formatiere den gefundenen Höchstwert mit 2 Nachkommastellen zur Anzeige
                        // (Punkt als Dezimaltrennzeichen)
                        maxValueFormatted = wert.ToString("F1", CultureInfo.InvariantCulture);
                        lineOfMax = currentLine;
                    }
                }
            }

            //Höchstwert auf Oberfläche ausgeben.
            lblAusgabe.Content = "Höchsttemperatur: " + maxValueFormatted + "°C";
            lblZeile.Content = "gefunden in Zeile: " + lineOfMax;
        }
    }
}
