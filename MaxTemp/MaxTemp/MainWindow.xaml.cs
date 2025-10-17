using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaxTemp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
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
            string filePath = SelectCSVFile();
            if (string.IsNullOrEmpty(filePath))
            {
                lblAusgabe.Content = "Keine Datei ausgewählt";
                return;
            }

            float maxValue = float.MinValue;
            string maxLine = null;
            int maxLineNumber = -1;

            try
            {
                int currentLineNumber = 0;
                foreach (string line in File.ReadLines(filePath))
                {
                    currentLineNumber++;
                    string[] parts = line.Split(',');
                    for(int i = 2; i < parts.Length; i++)
                    {
                        if (float.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                        {
                            if(value > maxValue)
                            {
                                maxValue = value;
                                maxLine = line;
                                maxLineNumber = currentLineNumber;
                            }
                        }
                    }
                }

                if (maxLine != null)
                {
                    lblAusgabe.Content = $"Größter Wert: {maxValue.ToString("F2", CultureInfo.InvariantCulture)} in der CSV Datei bei Zeile: {maxLineNumber}";
                    lblAusgabeInfo.Content = $"Weitere Informationen: {maxLine}";
                }
                else
                    lblAusgabe.Content = "Fehler: Keine Zahlenwerte gefunden.";
            }
            catch (Exception ex)
            {
                lblAusgabe.Content = "Fehler beim lesen der csv Datei: " + ex.Message;
            }
        }

        public string SelectCSVFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            MessageBox.Show("Bitte wähle eine .csv Datei aus.");
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();

            if (result == true) return openFileDialog.FileName;
            else return null;

        }
    }
}
