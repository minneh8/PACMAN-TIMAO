namespace PACMAN_TIMAO
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbTabuleiro = new System.Windows.Forms.PictureBox();
            this.pbPACMAN = new System.Windows.Forms.PictureBox();
            this.pbFantasma = new System.Windows.Forms.PictureBox();
            this.lblTempo = new System.Windows.Forms.Label();
            this.lblPontos = new System.Windows.Forms.Label();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnSair = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbTabuleiro)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPACMAN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTabuleiro
            // 
            this.pbTabuleiro.BackColor = System.Drawing.Color.Black;
            this.pbTabuleiro.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Tile;
            this.pbTabuleiro.Location = new System.Drawing.Point(24, 24);
            this.pbTabuleiro.Name = "pbTabuleiro";
            this.pbTabuleiro.Size = new System.Drawing.Size(960, 216);
            this.pbTabuleiro.TabIndex = 0;
            this.pbTabuleiro.TabStop = false;
            this.pbTabuleiro.Paint += new System.Windows.Forms.PaintEventHandler(this.pbTabuleiro_Paint);
            // 
            // pbPACMAN
            // 
            this.pbPACMAN.BackColor = System.Drawing.Color.Black;
            this.pbPACMAN.Location = new System.Drawing.Point(48, 48);
            this.pbPACMAN.Name = "pbPACMAN";
            this.pbPACMAN.Size = new System.Drawing.Size(24, 24);
            this.pbPACMAN.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPACMAN.TabIndex = 1;
            this.pbPACMAN.TabStop = false;
            // 
            // pbFantasma
            // 
            this.pbFantasma.BackColor = System.Drawing.Color.Black;
            this.pbFantasma.Location = new System.Drawing.Point(504, 144);
            this.pbFantasma.Name = "pbFantasma";
            this.pbFantasma.Size = new System.Drawing.Size(24, 24);
            this.pbFantasma.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbFantasma.TabIndex = 2;
            this.pbFantasma.TabStop = false;
            // 
            // lblTempo
            // 
            this.lblTempo.AutoSize = true;
            this.lblTempo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTempo.ForeColor = System.Drawing.Color.White;
            this.lblTempo.Location = new System.Drawing.Point(24, 256);
            this.lblTempo.Name = "lblTempo";
            this.lblTempo.Size = new System.Drawing.Size(99, 17);
            this.lblTempo.TabIndex = 3;
            this.lblTempo.Text = "TEMPO: 100";
            // 
            // lblPontos
            // 
            this.lblPontos.AutoSize = true;
            this.lblPontos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPontos.ForeColor = System.Drawing.Color.White;
            this.lblPontos.Location = new System.Drawing.Point(180, 256);
            this.lblPontos.Name = "lblPontos";
            this.lblPontos.Size = new System.Drawing.Size(92, 17);
            this.lblPontos.TabIndex = 4;
            this.lblPontos.Text = "PONTOS: 0";
            // 
            // btnIniciar
            // 
            this.btnIniciar.BackColor = System.Drawing.Color.White;
            this.btnIniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIniciar.ForeColor = System.Drawing.Color.Black;
            this.btnIniciar.Location = new System.Drawing.Point(700, 248);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(100, 30);
            this.btnIniciar.TabIndex = 5;
            this.btnIniciar.Text = "JOGAR";
            this.btnIniciar.UseVisualStyleBackColor = false;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.White;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.ForeColor = System.Drawing.Color.Black;
            this.btnSair.Location = new System.Drawing.Point(820, 248);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(100, 30);
            this.btnSair.TabIndex = 6;
            this.btnSair.Text = "SAIR";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click_1);
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1008, 304);
            this.Controls.Add(this.pbTabuleiro);
            this.Controls.Add(this.pbPACMAN);
            this.Controls.Add(this.pbFantasma);
            this.Controls.Add(this.lblTempo);
            this.Controls.Add(this.lblPontos);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.btnSair);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1008, 343);
            this.Name = "Form1";
            this.Text = "PACMAN TIMAO";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbTabuleiro)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPACMAN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFantasma)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTabuleiro;
        private System.Windows.Forms.PictureBox pbPACMAN;
        private System.Windows.Forms.PictureBox pbFantasma;
        private System.Windows.Forms.Label lblTempo;
        private System.Windows.Forms.Label lblPontos;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Timer timer1;
    }
}

