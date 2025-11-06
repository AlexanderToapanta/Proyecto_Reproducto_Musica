namespace Reproducto_Musica
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_CargarMusica = new System.Windows.Forms.Button();
            this.btn_Play = new System.Windows.Forms.Button();
            this.btn_Parar = new System.Windows.Forms.Button();
            this.btn_Anterior = new System.Windows.Forms.Button();
            this.btn_Siguiente = new System.Windows.Forms.Button();
            this.lstw_Canciones = new System.Windows.Forms.ListView();
            this.prb_Progreso = new System.Windows.Forms.ProgressBar();
            this.lbl_Tiempo = new System.Windows.Forms.Label();
            this.lbl_Nom_Cancion = new System.Windows.Forms.Label();
            this.txt_texto = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btn_CargarMusica
            // 
            this.btn_CargarMusica.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_CargarMusica.Location = new System.Drawing.Point(27, 107);
            this.btn_CargarMusica.Name = "btn_CargarMusica";
            this.btn_CargarMusica.Size = new System.Drawing.Size(43, 23);
            this.btn_CargarMusica.TabIndex = 0;
            this.btn_CargarMusica.Text = "button1";
            this.btn_CargarMusica.UseVisualStyleBackColor = true;
            this.btn_CargarMusica.Click += new System.EventHandler(this.btn_CargarMusica_Click);
            // 
            // btn_Play
            // 
            this.btn_Play.Location = new System.Drawing.Point(390, 386);
            this.btn_Play.Name = "btn_Play";
            this.btn_Play.Size = new System.Drawing.Size(75, 23);
            this.btn_Play.TabIndex = 1;
            this.btn_Play.Text = "button1";
            this.btn_Play.UseVisualStyleBackColor = true;
            this.btn_Play.Click += new System.EventHandler(this.btn_Play_Click);
            // 
            // btn_Parar
            // 
            this.btn_Parar.Location = new System.Drawing.Point(284, 386);
            this.btn_Parar.Name = "btn_Parar";
            this.btn_Parar.Size = new System.Drawing.Size(75, 23);
            this.btn_Parar.TabIndex = 2;
            this.btn_Parar.Text = "button1";
            this.btn_Parar.UseVisualStyleBackColor = true;
            this.btn_Parar.Click += new System.EventHandler(this.btn_Parar_Click);
            // 
            // btn_Anterior
            // 
            this.btn_Anterior.Location = new System.Drawing.Point(192, 386);
            this.btn_Anterior.Name = "btn_Anterior";
            this.btn_Anterior.Size = new System.Drawing.Size(75, 23);
            this.btn_Anterior.TabIndex = 3;
            this.btn_Anterior.Text = "button1";
            this.btn_Anterior.UseVisualStyleBackColor = true;
            this.btn_Anterior.Click += new System.EventHandler(this.btn_Anterior_Click);
            // 
            // btn_Siguiente
            // 
            this.btn_Siguiente.Location = new System.Drawing.Point(508, 386);
            this.btn_Siguiente.Name = "btn_Siguiente";
            this.btn_Siguiente.Size = new System.Drawing.Size(75, 23);
            this.btn_Siguiente.TabIndex = 4;
            this.btn_Siguiente.Text = "button1";
            this.btn_Siguiente.UseVisualStyleBackColor = true;
            this.btn_Siguiente.Click += new System.EventHandler(this.btn_Siguiente_Click);
            // 
            // lstw_Canciones
            // 
            this.lstw_Canciones.HideSelection = false;
            this.lstw_Canciones.Location = new System.Drawing.Point(115, 46);
            this.lstw_Canciones.Name = "lstw_Canciones";
            this.lstw_Canciones.Size = new System.Drawing.Size(616, 300);
            this.lstw_Canciones.TabIndex = 5;
            this.lstw_Canciones.UseCompatibleStateImageBehavior = false;
            this.lstw_Canciones.SelectedIndexChanged += new System.EventHandler(this.lstw_Canciones_SelectedIndexChanged);
            // 
            // prb_Progreso
            // 
            this.prb_Progreso.Location = new System.Drawing.Point(114, 352);
            this.prb_Progreso.Name = "prb_Progreso";
            this.prb_Progreso.Size = new System.Drawing.Size(617, 23);
            this.prb_Progreso.TabIndex = 6;
            this.prb_Progreso.Click += new System.EventHandler(this.prb_Progreso_Click);
            // 
            // lbl_Tiempo
            // 
            this.lbl_Tiempo.AutoSize = true;
            this.lbl_Tiempo.Location = new System.Drawing.Point(129, 391);
            this.lbl_Tiempo.Name = "lbl_Tiempo";
            this.lbl_Tiempo.Size = new System.Drawing.Size(42, 13);
            this.lbl_Tiempo.TabIndex = 7;
            this.lbl_Tiempo.Text = "Tiempo";
            this.lbl_Tiempo.Click += new System.EventHandler(this.lbl_Tiempo_Click);
            // 
            // lbl_Nom_Cancion
            // 
            this.lbl_Nom_Cancion.AutoSize = true;
            this.lbl_Nom_Cancion.Location = new System.Drawing.Point(52, 391);
            this.lbl_Nom_Cancion.Name = "lbl_Nom_Cancion";
            this.lbl_Nom_Cancion.Size = new System.Drawing.Size(44, 13);
            this.lbl_Nom_Cancion.TabIndex = 8;
            this.lbl_Nom_Cancion.Text = "Nombre";
            this.lbl_Nom_Cancion.Click += new System.EventHandler(this.lbl_Nom_Cancion_Click);
            // 
            // txt_texto
            // 
            this.txt_texto.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txt_texto.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_texto.ForeColor = System.Drawing.Color.Crimson;
            this.txt_texto.Location = new System.Drawing.Point(12, 153);
            this.txt_texto.Multiline = true;
            this.txt_texto.Name = "txt_texto";
            this.txt_texto.ReadOnly = true;
            this.txt_texto.Size = new System.Drawing.Size(75, 71);
            this.txt_texto.TabIndex = 9;
            this.txt_texto.Text = "Presione el boton para agregar musicas";
            this.txt_texto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txt_texto);
            this.Controls.Add(this.lbl_Nom_Cancion);
            this.Controls.Add(this.lbl_Tiempo);
            this.Controls.Add(this.prb_Progreso);
            this.Controls.Add(this.lstw_Canciones);
            this.Controls.Add(this.btn_Siguiente);
            this.Controls.Add(this.btn_Anterior);
            this.Controls.Add(this.btn_Parar);
            this.Controls.Add(this.btn_Play);
            this.Controls.Add(this.btn_CargarMusica);
            this.Name = "MainForm";
            this.Text = "Reprodcutor de musica";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_CargarMusica;
        private System.Windows.Forms.Button btn_Play;
        private System.Windows.Forms.Button btn_Parar;
        private System.Windows.Forms.Button btn_Anterior;
        private System.Windows.Forms.Button btn_Siguiente;
        private System.Windows.Forms.ListView lstw_Canciones;
        private System.Windows.Forms.ProgressBar prb_Progreso;
        private System.Windows.Forms.Label lbl_Tiempo;
        private System.Windows.Forms.Label lbl_Nom_Cancion;
        private System.Windows.Forms.TextBox txt_texto;
        private System.Windows.Forms.Timer timer1;
    }
}

