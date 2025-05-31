namespace ProyectoFinal2Progra1
{
    partial class ResultadoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.RichTextBox rtbAnalisis;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtbAnalisis = new System.Windows.Forms.RichTextBox();
            this.rtbAnalisis.Name = "rtbAnalisis";
            this.rtbAnalisis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbAnalisis.ReadOnly = true;
            this.rtbAnalisis.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Controls.Add(this.rtbAnalisis);

        }

        private void ResultadoForm_Load(object sender, EventArgs e)
        {
            
        }

        #endregion
    }
}
