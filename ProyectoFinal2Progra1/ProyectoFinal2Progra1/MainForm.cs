using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ProyectoFinal2Progra1
{
    public partial class MainForm : Form
    {
        private GroqApiClient groqClient;
        private List<Pregunta> preguntasGeneradas;
        private Dictionary<int, string> respuestasEstudiante;
        private string gradoSeleccionado;
        private int preguntaActual = 0;

        public MainForm()
        {
            InitializeComponent();
            groqClient = new GroqApiClient();
            preguntasGeneradas = new List<Pregunta>();
            respuestasEstudiante = new Dictionary<int, string>();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Text = "Asistente Virtual para Maestros";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);

            // Panel principal
            var panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };
            this.Controls.Add(panelPrincipal);

            // Título
            var lblTitulo = new Label
            {
                Text = "Asistente Virtual para Evaluación Estudiantil",
                Font = new System.Drawing.Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(25, 118, 210),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            panelPrincipal.Controls.Add(lblTitulo);

            // Panel de selección de grado
            var panelGrado = new GroupBox
            {
                Text = "Selección de Grado",
                Location = new Point(20, 70),
                Size = new Size(750, 80),
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold)
            };
            panelPrincipal.Controls.Add(panelGrado);

            var lblGrado = new Label
            {
                Text = "Seleccione el grado:",
                Location = new Point(20, 35),
                AutoSize = false,
                Size = new Size(120, 20), 
                TextAlign = ContentAlignment.MiddleLeft
            };
            panelGrado.Controls.Add(lblGrado);

            var cmbGrado = new ComboBox
            {
                Name = "cmbGrado",
                Location = new Point(250, 27),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbGrado.Items.AddRange(new string[] { "1°", "2°", "3°", "4°", "5°", "6°" });
            panelGrado.Controls.Add(cmbGrado);

            var btnGenerarPreguntas = new Button
            {
                Name = "btnGenerarPreguntas",
                Text = "Generar Preguntas",
                Location = new Point(420, 25),
                Size = new Size(110, 28),
                BackColor = System.Drawing.Color.FromArgb(76, 175, 80),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnGenerarPreguntas.Click += BtnGenerarPreguntas_Click;
            panelGrado.Controls.Add(btnGenerarPreguntas);

          
            var panelPreguntas = new GroupBox
            {
                Name = "panelPreguntas",
                Text = "Evaluación",
                Location = new Point(20, 170),
                Size = new Size(750, 350),
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                Visible = false
            };
            panelPrincipal.Controls.Add(panelPreguntas);

         
            var lblPregunta = new Label
            {
                Name = "lblPregunta",
                Location = new Point(20, 30),
                Size = new Size(700, 120),
                Font = new System.Drawing.Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = System.Drawing.Color.White,
                Padding = new Padding(10)
            };
            panelPreguntas.Controls.Add(lblPregunta);

       
            var txtRespuesta = new RichTextBox
            {
                Name = "txtRespuesta",
                Location = new Point(20, 160),
                Size = new Size(700, 70),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            panelPreguntas.Controls.Add(txtRespuesta);

           

   
            var btnSiguiente = new Button
            {
                Name = "btnSiguiente",
                Text = "Siguiente",
                Location = new Point(470, 245),
                Size = new Size(110, 38), // Más grande
                BackColor = System.Drawing.Color.FromArgb(33, 150, 243),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSiguiente.Click += BtnSiguiente_Click;
            panelPreguntas.Controls.Add(btnSiguiente);

    
            var btnAnterior = new Button
            {
                Name = "btnAnterior",
                Text = "Anterior",
                Location = new Point(350, 245),
                Size = new Size(110, 38), // Más grande
                BackColor = System.Drawing.Color.FromArgb(158, 158, 158),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAnterior.Click += BtnAnterior_Click;
            panelPreguntas.Controls.Add(btnAnterior);

            var btnTerminar = new Button
            {
                Name = "btnTerminar",
                Text = "Terminar Evaluación",
                Location = new Point(560, 250),
                Size = new Size(130, 28),
                BackColor = System.Drawing.Color.FromArgb(255, 87, 34),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };
            btnTerminar.Click += BtnTerminar_Click;
            panelPreguntas.Controls.Add(btnTerminar);

            var lblProgreso = new Label
            {
                Name = "lblProgreso",
                Text = "Pregunta 1 de 9",
                Location = new Point(20, 260),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Italic)
            };
            panelPreguntas.Controls.Add(lblProgreso);

            // Status bar
            var statusStrip = new StatusStrip();
            var statusLabel = new ToolStripStatusLabel
            {
                Name = "statusLabel",
                Text = "Listo para comenzar"
            };
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private async void BtnGenerarPreguntas_Click(object sender, EventArgs e)
        {
            var cmbGrado = this.Controls.Find("cmbGrado", true)[0] as ComboBox;

          
            ToolStripStatusLabel statusLabel = null;
            var statusStrip = this.Controls.OfType<StatusStrip>().FirstOrDefault();
            if (statusStrip != null)
            {
                statusLabel = statusStrip.Items
                    .OfType<ToolStripStatusLabel>()
                    .FirstOrDefault(item => item.Name == "statusLabel");
            }

            if (cmbGrado.SelectedItem == null)
            {
                MessageBox.Show("Por favor seleccione un grado.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            gradoSeleccionado = cmbGrado.SelectedItem.ToString();
            if (statusLabel != null)
                statusLabel.Text = "Generando preguntas...";

            try
            {
                await GenerarPreguntasAsync();
                MostrarPregunta(0);

                var panelPreguntas = this.Controls.Find("panelPreguntas", true)[0];
                panelPreguntas.Visible = true;

                if (statusLabel != null)
                    statusLabel.Text = "Evaluación en progreso";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar preguntas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (statusLabel != null)
                    statusLabel.Text = "Error al generar preguntas";
            }
        }

        private async Task GenerarPreguntasAsync()
        {
            string prompt = $@"Genera exactamente 9 preguntas de evaluación para un estudiante de {gradoSeleccionado} grado de primaria en Guatemala.
Necesito 3 preguntas de cada materia:
- 3 preguntas de Matemáticas (apropiadas para el nivel)
- 3 preguntas de Comunicación y Lenguaje (gramática, comprensión lectora, escritura)
- 3 preguntas de Ciencias Naturales (apropiadas para el nivel)

Devuelve SOLO el siguiente formato JSON, sin explicaciones ni texto adicional:
{{
    ""preguntas"": [
        {{
            ""materia"": ""Matemáticas"",
            ""pregunta"": ""texto de la pregunta"",
            ""nivel"": ""{gradoSeleccionado}""
        }}
        // ... (8 preguntas más)
    ]
}}
";

            var respuesta = await groqClient.GenerarRespuestaAsync(prompt);

 

            PreguntasResponse preguntasJson = null;
            try
            {
                preguntasJson = JsonConvert.DeserializeObject<PreguntasResponse>(respuesta);
            }
            catch
            {
                MessageBox.Show("La respuesta de la API no es un JSON válido:\n\n" + respuesta, "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (preguntasJson == null || preguntasJson.preguntas == null || preguntasJson.preguntas.Count != 9)
            {
                MessageBox.Show("La API no devolvió 9 preguntas. Respuesta recibida:\n\n" + respuesta, "Error de cantidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            preguntasGeneradas.Clear();
            foreach (var p in preguntasJson.preguntas)
            {
                preguntasGeneradas.Add(new Pregunta
                {
                    Materia = p.materia,
                    TextoPregunta = p.pregunta,
                    Nivel = p.nivel
                });
            }
        }


        private void MostrarPregunta(int indice)
        {
            if (preguntasGeneradas == null || indice < 0 || indice >= preguntasGeneradas.Count)
                return;
            var lblPregunta = this.Controls.Find("lblPregunta", true)[0] as Label;
            var txtRespuesta = this.Controls.Find("txtRespuesta", true)[0] as RichTextBox;
            var lblProgreso = this.Controls.Find("lblProgreso", true)[0] as Label;
            var btnAnterior = this.Controls.Find("btnAnterior", true)[0] as Button;
            var btnSiguiente = this.Controls.Find("btnSiguiente", true)[0] as Button;
            var btnTerminar = this.Controls.Find("btnTerminar", true)[0] as Button;

            preguntaActual = indice;
            var pregunta = preguntasGeneradas[indice];

            lblPregunta.Text = $"{pregunta.Materia}\n\n{pregunta.TextoPregunta}";

            // Cargar respuesta guardada si existe
            if (respuestasEstudiante.ContainsKey(indice))
            {
                txtRespuesta.Text = respuestasEstudiante[indice];
            }
            else
            {
                txtRespuesta.Text = "";
            }

            lblProgreso.Text = $"Pregunta {indice + 1} de {preguntasGeneradas.Count}";

            btnAnterior.Enabled = indice > 0;
            btnSiguiente.Visible = indice < preguntasGeneradas.Count - 1;
            btnTerminar.Visible = indice == preguntasGeneradas.Count - 1;
        }

        private void BtnAnterior_Click(object sender, EventArgs e)
        {
            GuardarRespuestaActual();
            MostrarPregunta(preguntaActual - 1);
        }

        private void BtnSiguiente_Click(object sender, EventArgs e)
        {
            GuardarRespuestaActual();
            MostrarPregunta(preguntaActual + 1);
        }

        private void GuardarRespuestaActual()
        {
            var txtRespuesta = this.Controls.Find("txtRespuesta", true)[0] as RichTextBox;
            respuestasEstudiante[preguntaActual] = txtRespuesta.Text;
        }

        private async void BtnTerminar_Click(object sender, EventArgs e)
        {
            GuardarRespuestaActual();

            if (respuestasEstudiante.Count < preguntasGeneradas.Count)
            {
                var resultado = MessageBox.Show("Algunas preguntas no han sido respondidas. ¿Desea continuar con el análisis?",
                    "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.No) return;
            }

            // Buscar el statusLabel correctamente desde el StatusStrip
            ToolStripStatusLabel statusLabel = null;
            var statusStrip = this.Controls.OfType<StatusStrip>().FirstOrDefault();
            if (statusStrip != null)
            {
                statusLabel = statusStrip.Items
                    .OfType<ToolStripStatusLabel>()
                    .FirstOrDefault(item => item.Name == "statusLabel");
            }
            if (statusLabel != null)
                statusLabel.Text = "Analizando respuestas y generando planificación...";

            try
            {
                await GenerarAnalisisYPlanificacion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar análisis: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (statusLabel != null)
                    statusLabel.Text = "Error en el análisis";
            }
        }

        private async Task GenerarAnalisisYPlanificacion()
        {
            MessageBox.Show($"Preguntas generadas: {preguntasGeneradas.Count}\nRespuestas registradas: {respuestasEstudiante.Count}", "Depuración");

            if (preguntasGeneradas == null || preguntasGeneradas.Count == 0)
            {
                MessageBox.Show("No hay preguntas generadas para analizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StringBuilder respuestasTexto = new StringBuilder();
            respuestasTexto.AppendLine($"EVALUACIÓN ESTUDIANTE - GRADO {gradoSeleccionado}");
            respuestasTexto.AppendLine("".PadRight(50, '='));

         
            for (int i = 0; i < preguntasGeneradas.Count; i++)
            {
                Pregunta pregunta = null;
                try
                {
                    pregunta = preguntasGeneradas[i];
                }
                catch
                {
                    MessageBox.Show($"Error: El índice {i} está fuera del rango de preguntasGeneradas.", "Error de índice", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                var respuesta = respuestasEstudiante.ContainsKey(i) ? respuestasEstudiante[i] : "[Sin respuesta]";

                respuestasTexto.AppendLine($"\nMATERIA: {pregunta.Materia}");
                respuestasTexto.AppendLine($"PREGUNTA: {pregunta.TextoPregunta}");
                respuestasTexto.AppendLine($"RESPUESTA: {respuesta}");
                respuestasTexto.AppendLine("".PadRight(30, '-'));
            }

            string promptAnalisis = $@"Analiza las siguientes respuestas de un estudiante de {gradoSeleccionado} grado de primaria en Guatemala y genera una planificación educativa completa.

{respuestasTexto}

Por favor proporciona:

1. ANÁLISIS DEL NIVEL EDUCATIVO:
   - Fortalezas identificadas por materia
   - Áreas de mejora por materia
   - Nivel general del estudiante (Excelente/Bueno/Regular/Necesita refuerzo)

2. PLANIFICACIÓN EDUCATIVA:
   - Objetivos específicos para cada materia
   - Estrategias de enseñanza recomendadas
   - Actividades y ejercicios específicos
   - Cronograma sugerido (semanal)
   - Recursos didácticos recomendados

3. RECOMENDACIONES PARA EL MAESTRO:
   - Metodologías específicas
   - Adaptaciones curriculares si son necesarias
   - Formas de evaluación continua

El análisis debe ser detallado, profesional y basado en el currículo guatemalteco de primaria.";

            var analisis = await groqClient.GenerarRespuestaAsync(promptAnalisis);

        
            var formResultado = new ResultadoForm(analisis);
            formResultado.ShowDialog();
        }


    }

    public class Pregunta
    {
        public string Materia { get; set; }
        public string TextoPregunta { get; set; }
        public string Nivel { get; set; }
    }

    public class PreguntasResponse
    {
        public List<PreguntaJson> preguntas { get; set; }
    }

    public class PreguntaJson
    {
        public string materia { get; set; }
        public string pregunta { get; set; }
        public string nivel { get; set; }
    }

    public class GroqApiClient
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey = ""; 

        public GroqApiClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        // ...

        public async Task<string> GenerarRespuestaAsync(string prompt)
        {
            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                model = "llama3-8b-8192"
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            string responseString;
#if NET8_0_OR_GREATER
            responseString = await response.Content.ReadAsStringAsync();
#else
            responseString = await response.Content.ReadAsStringAsync();
#endif

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error de API: {response.StatusCode} - {responseString}");
            }

            dynamic result = JsonConvert.DeserializeObject(responseString);
            return result.choices[0].message.content.ToString();
        }
    }
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
     
        }
    }
}