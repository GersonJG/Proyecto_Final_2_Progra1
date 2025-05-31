using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Reflection.Metadata;

namespace ProyectoFinal2Progra1
{

    public partial class ResultadoForm : Form
    {
        private string contenidoAnalisis;

        // El constructor queda así, sin ambigüedad:
        public ResultadoForm(string analisis)
        {
            contenidoAnalisis = analisis;
            InitializeComponent();
            MostrarAnalisis();
        }

        private void MostrarAnalisis()
        {
            var rtbAnalisis = this.Controls.Find("rtbAnalisis", true)[0] as RichTextBox;

            if (rtbAnalisis == null)
                return;

            // Formatear el texto del análisis
            rtbAnalisis.Clear();

            string[] lineas = contenidoAnalisis.Split('\n');
            foreach (string linea in lineas)
            {
                if (linea.Contains("ANÁLISIS DEL NIVEL EDUCATIVO") ||
                    linea.Contains("PLANIFICACIÓN EDUCATIVA") ||
                    linea.Contains("RECOMENDACIONES PARA EL MAESTRO"))
                {
                    // Títulos principales en negrita y azul
                    int startIndex = rtbAnalisis.TextLength;
                    rtbAnalisis.AppendText(linea + "\n");
                    rtbAnalisis.Select(startIndex, linea.Length);
                    rtbAnalisis.SelectionFont = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold);
                    rtbAnalisis.SelectionColor = System.Drawing.Color.FromArgb(25, 118, 210);
                }
                else if (linea.StartsWith("- ") || linea.Contains(":"))
                {
                    // Subtítulos y puntos importantes
                    int startIndex = rtbAnalisis.TextLength;
                    rtbAnalisis.AppendText(linea + "\n");
                    rtbAnalisis.Select(startIndex, linea.Length);
                    rtbAnalisis.SelectionFont = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold);
                    rtbAnalisis.SelectionColor = System.Drawing.Color.FromArgb(51, 51, 51);
                }
                else
                {
                    // Texto normal
                    int startIndex = rtbAnalisis.TextLength;
                    rtbAnalisis.AppendText(linea + "\n");
                    rtbAnalisis.Select(startIndex, linea.Length);
                    rtbAnalisis.SelectionFont = new System.Drawing.Font("Segoe UI", 10, FontStyle.Regular);
                    rtbAnalisis.SelectionColor = System.Drawing.Color.FromArgb(68, 68, 68);
                }
            }

            rtbAnalisis.Select(0, 0); // Deseleccionar todo
        }

        // ... (resto del código de la clase)
    }
}
