using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PACMAN_TIMAO
{
    public partial class Form1 : Form
    {
        // Configurações do Tabuleiro
        string[] tabuleiro = new string[9];
        int lintabuleiro = 9;
        int coltabuleiro = 40;
        int tamanhoCelula = 24;

        // Posição e Estado do Pacman
        int pacmanLin = 1;
        int pacmanCol = 1;

        // Posição do Fantasma
        int fantasmaLin = 5;
        int fantasmaCol = 20;

        // Controle do Jogo
        int pontos = 0;
        int tempo = 100;
        bool jogoIniciado = false;
        bool pacmanBocaAberta = true;
        string direcaoPacman = "HD";

        private readonly string pastaAplicacao = Application.StartupPath;

        public Form1()
        {
            InitializeComponent();
            ConfigurarInicio();
        }

        private void ConfigurarInicio()
        {
            // Ativa o KeyPreview para o Form capturar as teclas WASD
            this.KeyPreview = true;

            TrocarImagemDeFundo(CarregarImagem("abertura.jpg", "abertura.png", "inicio.jpg", "inicio.png", "corinthians.jpg"));

            pbPACMAN.Visible = false;
            pbFantasma.Visible = false;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            // 1. Criar o Mapa (* = Parede, . = Ponto, Espaço = Vazio)
            tabuleiro[0] = "****************************************";
            tabuleiro[1] = "*......................................*";
            tabuleiro[2] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[3] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[4] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[5] = "*......................................*";
            tabuleiro[6] = "*.****.***.*******.****.********.***.***";
            tabuleiro[7] = "*......***.........****..............***";
            tabuleiro[8] = "****************************************";

            // 2. Resetar Posições
            pacmanCol = 1;
            pacmanLin = 1;
            fantasmaLin = 5;
            fantasmaCol = 20;
            pontos = 0;
            tempo = 100;
            jogoIniciado = true;
            pacmanBocaAberta = true;
            direcaoPacman = "HD";

            // 3. Remove a imagem de abertura para exibir o labirinto desenhado
            TrocarImagemDeFundo(null);

            // 4. Carrega Imagens dos Personagens se existirem
            pbPACMAN.Image = CarregarImagem(ObterNomesPacman());
            pbFantasma.Image = CarregarImagem("fantasma.jpg", "fantasma.png", "ghost.png");
            pbPACMAN.BackColor = Color.Black;
            pbFantasma.BackColor = Color.Black;

            pbPACMAN.Visible = true;
            pbFantasma.Visible = true;
            AtualizarPosicoesPersonagens();
            pbPACMAN.BringToFront();
            pbFantasma.BringToFront();

            // 5. Inicia o Timer do jogo
            timer1.Interval = 200;
            timer1.Enabled = true;

            // 6. Força o redesenho do mapa
            pbTabuleiro.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tempo--;
            lblTempo.Text = "TEMPO: " + tempo;
            lblPontos.Text = "PONTOS: " + pontos;

            // Mover Fantasma perseguindo o Pacman
            MoverFantasma();
            AlternarImagemPacman();

            // Atualizar Gráficos do Mapa
            pbTabuleiro.Invalidate();

            // Verificação de Game Over (Tempo esgotado ou Fantasma pegou Pacman)
            if (tempo <= 0 || (pacmanLin == fantasmaLin && pacmanCol == fantasmaCol))
            {
                timer1.Enabled = false;
                jogoIniciado = false;
                lblTempo.Text = "GAME OVER!";

                TrocarImagemDeFundo(CarregarImagem("gameover.jpg", "game-over.jpg", "fim.jpg", "fimdejogo.jpg", "gameover.png"));

                pbPACMAN.Visible = false;
                pbFantasma.Visible = false;
            }
        }

        private void pbTabuleiro_Paint(object sender, PaintEventArgs e)
        {
            // Só desenha o labirinto se o jogo estiver rolando
            if (!jogoIniciado) return;

            // Cores do Corinthians (Preto e Branco)
            using (Pen penParede = new Pen(Color.White, 2))
            using (SolidBrush brushParede = new SolidBrush(Color.Black))
            using (SolidBrush brushPonto = new SolidBrush(Color.White))
            using (SolidBrush brushFundo = new SolidBrush(Color.Black))
            {
                for (int lin = 0; lin < lintabuleiro; lin++)
                {
                    for (int col = 0; col < coltabuleiro; col++)
                    {
                        int x = col * tamanhoCelula;
                        int y = lin * tamanhoCelula;

                        if (tabuleiro[lin][col] == '*')
                        {
                            e.Graphics.FillRectangle(brushParede, x, y, tamanhoCelula, tamanhoCelula);
                            e.Graphics.DrawRectangle(penParede, x, y, tamanhoCelula, tamanhoCelula);
                        }
                        else if (tabuleiro[lin][col] == '.')
                        {
                            e.Graphics.FillRectangle(brushFundo, x, y, tamanhoCelula, tamanhoCelula);
                            e.Graphics.FillEllipse(brushPonto, x + 9, y + 9, 6, 6);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(brushFundo, x, y, tamanhoCelula, tamanhoCelula);
                        }
                    }
                }
            }

            AtualizarPosicoesPersonagens();
        }

        private void AtualizarPosicoesPersonagens()
        {
            pbPACMAN.Location = new Point(
                pbTabuleiro.Left + (pacmanCol * tamanhoCelula),
                pbTabuleiro.Top + (pacmanLin * tamanhoCelula));

            pbFantasma.Location = new Point(
                pbTabuleiro.Left + (fantasmaCol * tamanhoCelula),
                pbTabuleiro.Top + (fantasmaLin * tamanhoCelula));

            pbPACMAN.BringToFront();
            pbFantasma.BringToFront();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!timer1.Enabled || !jogoIniciado) return;

            int proximaLin = pacmanLin;
            int proximaCol = pacmanCol;

            // Controle WASD
            if (e.KeyCode == Keys.W) { proximaLin--; direcaoPacman = "VU"; }
            else if (e.KeyCode == Keys.S) { proximaLin++; direcaoPacman = "VD"; }
            else if (e.KeyCode == Keys.A) { proximaCol--; direcaoPacman = "HE"; }
            else if (e.KeyCode == Keys.D) { proximaCol++; direcaoPacman = "HD"; }

            // Colisão com as paredes do mapa
            if (proximaLin >= 0 && proximaLin < lintabuleiro &&
                proximaCol >= 0 && proximaCol < coltabuleiro &&
                tabuleiro[proximaLin][proximaCol] != '*')
            {
                pacmanLin = proximaLin;
                pacmanCol = proximaCol;

                // Coletar pastilha / somar pontos
                if (tabuleiro[pacmanLin][pacmanCol] == '.')
                {
                    pontos += 10;
                    StringBuilder sb = new StringBuilder(tabuleiro[pacmanLin]);
                    sb[pacmanCol] = ' ';
                    tabuleiro[pacmanLin] = sb.ToString();
                }
            }

            pbTabuleiro.Invalidate();
        }

        private void MoverFantasma()
        {
            int novaLin = fantasmaLin;
            int novaCol = fantasmaCol;

            if (fantasmaCol < pacmanCol && PodeMover(fantasmaLin, fantasmaCol + 1)) novaCol++;
            else if (fantasmaCol > pacmanCol && PodeMover(fantasmaLin, fantasmaCol - 1)) novaCol--;
            else if (fantasmaLin < pacmanLin && PodeMover(fantasmaLin + 1, fantasmaCol)) novaLin++;
            else if (fantasmaLin > pacmanLin && PodeMover(fantasmaLin - 1, fantasmaCol)) novaLin--;

            fantasmaLin = novaLin;
            fantasmaCol = novaCol;
        }

        private bool PodeMover(int linha, int coluna)
        {
            return linha >= 0 && linha < lintabuleiro && coluna >= 0 && coluna < coltabuleiro &&
                tabuleiro[linha][coluna] != '*';
        }

        private void AlternarImagemPacman()
        {
            pacmanBocaAberta = !pacmanBocaAberta;
            Image imagem = CarregarImagem(ObterNomesPacman());
            if (imagem != null)
            {
                Image imagemAnterior = pbPACMAN.Image;
                pbPACMAN.Image = imagem;
                if (imagemAnterior != null) imagemAnterior.Dispose();
            }
        }

        private string[] ObterNomesPacman()
        {
            string numeroImagem = pacmanBocaAberta ? "001" : "002";
            return new[]
            {
                "PM" + direcaoPacman + numeroImagem + ".jpg",
                "PM" + direcaoPacman + numeroImagem + ".png",
                pacmanBocaAberta ? "pacman-aberto.png" : "pacman-fechado.png",
                pacmanBocaAberta ? "pacman1.png" : "pacman2.png",
                "pacman.png"
            };
        }

        private Image CarregarImagem(params string[] nomes)
        {
            DirectoryInfo pasta = new DirectoryInfo(pastaAplicacao);
            for (int nivel = 0; nivel < 4 && pasta != null; nivel++, pasta = pasta.Parent)
            {
                foreach (string nome in nomes)
                {
                    string caminho = Path.Combine(pasta.FullName, nome);
                    if (File.Exists(caminho))
                    {
                        using (Image original = Image.FromFile(caminho))
                        {
                            return new Bitmap(original);
                        }
                    }
                }
            }
            return null;
        }

        private void TrocarImagemDeFundo(Image imagem)
        {
            Image anterior = pbTabuleiro.BackgroundImage;
            pbTabuleiro.BackgroundImage = imagem;
            pbTabuleiro.BackgroundImageLayout = ImageLayout.Tile;
            BackgroundImage = null;
            if (anterior != null) anterior.Dispose();
        }

        private void btnSair_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}