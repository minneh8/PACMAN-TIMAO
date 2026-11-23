using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PACMAN_TIMAO
{
    public partial class Form1 : Form
    {
        // Configurações da matriz do tabuleiro (9 linhas por 40 colunas)
        string[] tabuleiro = new string[9];
        int lintabuleiro = 9;
        int coltabuleiro = 40;
        int tamanhoCelula = 24; // Cada quadrado do mapa tem 24x24 pixels

        // Posição inicial do Pacman na matriz
        int pacmanLin = 1;
        int pacmanCol = 1;

        // Posição inicial do Fantasma na matriz
        int fantasmaLin = 5;
        int fantasmaCol = 20;

        // Variáveis de controle da partida
        int pontos = 0;
        int tempo = 100;
        bool jogoIniciado = false;
        bool pacmanBocaAberta = true;
        string direcaoPacman = "HD"; // Direção atual: HD (Direita), HE (Esquerda), VU (Cima), VD (Baixo)

        // Pega o caminho da pasta onde o .exe está rodando
        private readonly string pastaAplicacao = Application.StartupPath;

        public Form1()
        {
            InitializeComponent();
            ConfigurarInicio();
        }

        // Prepara a tela inicial antes de começar o jogo
        private void ConfigurarInicio()
        {
            // Ativa o KeyPreview para o Form conseguir ler as teclas WASD mesmo com botões na tela
            this.KeyPreview = true;

            // Carrega a imagem de capa no fundo do tabuleiro
            TrocarImagemDeFundo(CarregarImagem("abertura.jpg", "abertura.png", "inicio.jpg", "inicio.png", "corinthians.jpg"));

            // Deixa os personagens escondidos na tela inicial
            pbPACMAN.Visible = false;
            pbFantasma.Visible = false;
        }

        // Botão para iniciar ou reiniciar a partida
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            // Desenho do mapa (* = parede, . = ponto, espaço = vazio)
            tabuleiro[0] = "****************************************";
            tabuleiro[1] = "*......................................*";
            tabuleiro[2] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[3] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[4] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[5] = "*......................................*";
            tabuleiro[6] = "*.****.***.*******.****.********.***.***";
            tabuleiro[7] = "*......***.........****..............***";
            tabuleiro[8] = "****************************************";

            // Reseta posições, pontos e tempo para o início
            pacmanCol = 1;
            pacmanLin = 1;
            fantasmaLin = 5;
            fantasmaCol = 20;
            pontos = 0;
            tempo = 100;
            jogoIniciado = true;
            pacmanBocaAberta = true;
            direcaoPacman = "HD";

            // Tira a imagem de abertura para mostrar o labirinto
            TrocarImagemDeFundo(null);

            // Carrega as imagens dos personagens e ajusta a cor de fundo
            pbPACMAN.Image = CarregarImagem(ObterNomesPacman());
            pbFantasma.Image = CarregarImagem("fantasma.jpg", "fantasma.png", "ghost.png");
            pbPACMAN.BackColor = Color.Black;
            pbFantasma.BackColor = Color.Black;

            // Mostra os personagens e coloca eles na frente do tabuleiro
            pbPACMAN.Visible = true;
            pbFantasma.Visible = true;
            AtualizarPosicoesPersonagens();
            pbPACMAN.BringToFront();
            pbFantasma.BringToFront();

            // Liga o timer do jogo (roda a cada 200ms)
            timer1.Interval = 200;
            timer1.Enabled = true;

            // Força o tabuleiro a se redesenhar na tela
            pbTabuleiro.Invalidate();
        }

        // Loop principal do jogo (executa a cada tick do timer)
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Diminui o tempo e atualiza os textos do placar
            tempo--;
            lblTempo.Text = "TEMPO: " + tempo;
            lblPontos.Text = "PONTOS: " + pontos;

            // Faz o fantasma perseguir o Pacman e anima a boca do Pacman
            MoverFantasma();
            AlternarImagemPacman();

            // Redesenha o mapa
            pbTabuleiro.Invalidate();

            // Verifica se deu Game Over (acabou o tempo ou o fantasma encostou no Pacman)
            if (tempo <= 0 || (pacmanLin == fantasmaLin && pacmanCol == fantasmaCol))
            {
                timer1.Enabled = false;
                jogoIniciado = false;
                lblTempo.Text = "GAME OVER!";

                // Coloca a imagem de fim de jogo e esconde os personagens
                TrocarImagemDeFundo(CarregarImagem("gameover.jpg", "game-over.jpg", "fim.jpg", "fimdejogo.jpg", "gameover.png"));

                pbPACMAN.Visible = false;
                pbFantasma.Visible = false;
            }
        }

        // Desenha as paredes e os pontinhos dentro do PictureBox do tabuleiro
        private void pbTabuleiro_Paint(object sender, PaintEventArgs e)
        {
            // Se o jogo não começou, não desenha o labirinto
            if (!jogoIniciado) return;

            // Define as cores (preto e branco do Corinthians) para desenhar o mapa
            using (Pen penParede = new Pen(Color.White, 2))
            using (SolidBrush brushParede = new SolidBrush(Color.Black))
            using (SolidBrush brushPonto = new SolidBrush(Color.White))
            using (SolidBrush brushFundo = new SolidBrush(Color.Black))
            {
                // Percorre cada linha e coluna da matriz do tabuleiro
                for (int lin = 0; lin < lintabuleiro; lin++)
                {
                    for (int col = 0; col < coltabuleiro; col++)
                    {
                        // Calcula a posição X e Y em pixels na tela
                        int x = col * tamanhoCelula;
                        int y = lin * tamanhoCelula;

                        if (tabuleiro[lin][col] == '*')
                        {
                            // Desenha a parede com borda branca
                            e.Graphics.FillRectangle(brushParede, x, y, tamanhoCelula, tamanhoCelula);
                            e.Graphics.DrawRectangle(penParede, x, y, tamanhoCelula, tamanhoCelula);
                        }
                        else if (tabuleiro[lin][col] == '.')
                        {
                            // Desenha o fundo preto e a bolinha branca no meio da célula
                            e.Graphics.FillRectangle(brushFundo, x, y, tamanhoCelula, tamanhoCelula);
                            e.Graphics.FillEllipse(brushPonto, x + 9, y + 9, 6, 6);
                        }
                        else
                        {
                            // Espaço vazio (caminho já comido)
                            e.Graphics.FillRectangle(brushFundo, x, y, tamanhoCelula, tamanhoCelula);
                        }
                    }
                }
            }

            AtualizarPosicoesPersonagens();
        }

        // Atualiza a posição (X, Y) das PictureBoxes do Pacman e do Fantasma na tela
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

        // Lê as teclas WASD para movimentar o Pacman
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!timer1.Enabled || !jogoIniciado) return;

            int proximaLin = pacmanLin;
            int proximaCol = pacmanCol;

            // Define a próxima posição e a sigla da direção conforme a tecla apertada
            if (e.KeyCode == Keys.W) { proximaLin--; direcaoPacman = "VU"; }
            else if (e.KeyCode == Keys.S) { proximaLin++; direcaoPacman = "VD"; }
            else if (e.KeyCode == Keys.A) { proximaCol--; direcaoPacman = "HE"; }
            else if (e.KeyCode == Keys.D) { proximaCol++; direcaoPacman = "HD"; }

            // Só move se estiver dentro dos limites do tabuleiro e não bater na parede (*)
            if (proximaLin >= 0 && proximaLin < lintabuleiro &&
                proximaCol >= 0 && proximaCol < coltabuleiro &&
                tabuleiro[proximaLin][proximaCol] != '*')
            {
                pacmanLin = proximaLin;
                pacmanCol = proximaCol;

                // Se passar por cima de um ponto (.), soma 10 pontos e apaga o ponto do mapa
                if (tabuleiro[pacmanLin][pacmanCol] == '.')
                {
                    pontos += 10;
                    StringBuilder sb = new StringBuilder(tabuleiro[pacmanLin]);
                    sb[pacmanCol] = ' ';
                    tabuleiro[pacmanLin] = sb.ToString();
                }
            }

            // Redesenha o mapa para mostrar o movimento na hora
            pbTabuleiro.Invalidate();
        }

        // Faz o fantasma andar na direção do Pacman
        private void MoverFantasma()
        {
            int novaLin = fantasmaLin;
            int novaCol = fantasmaCol;

            // Tenta aproximar primeiro na horizontal (colunas) e depois na vertical (linhas)
            if (fantasmaCol < pacmanCol && PodeMover(fantasmaLin, fantasmaCol + 1)) novaCol++;
            else if (fantasmaCol > pacmanCol && PodeMover(fantasmaLin, fantasmaCol - 1)) novaCol--;
            else if (fantasmaLin < pacmanLin && PodeMover(fantasmaLin + 1, fantasmaCol)) novaLin++;
            else if (fantasmaLin > pacmanLin && PodeMover(fantasmaLin - 1, fantasmaCol)) novaLin--;

            fantasmaLin = novaLin;
            fantasmaCol = novaCol;
        }

        // Verifica se a posição está dentro do mapa e não é uma parede
        private bool PodeMover(int linha, int coluna)
        {
            return linha >= 0 && linha < lintabuleiro && coluna >= 0 && coluna < coltabuleiro &&
                tabuleiro[linha][coluna] != '*';
        }

        // Alterna entre boca aberta e fechada para dar efeito de animação
        private void AlternarImagemPacman()
        {
            pacmanBocaAberta = !pacmanBocaAberta;
            Image imagem = CarregarImagem(ObterNomesPacman());
            if (imagem != null)
            {
                Image imagemAnterior = pbPACMAN.Image;
                pbPACMAN.Image = imagem;
                if (imagemAnterior != null) imagemAnterior.Dispose(); // Libera a imagem antiga da memória
            }
        }

        // Monta os nomes possíveis dos arquivos de imagem do Pacman de acordo com a direção e boca
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

        // Procura a imagem na pasta do executável e até 3 pastas acima (raiz do projeto)
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

        // Troca a imagem de fundo do tabuleiro (abertura / game over) e limpa a anterior
        private void TrocarImagemDeFundo(Image imagem)
        {
            Image anterior = pbTabuleiro.BackgroundImage;
            pbTabuleiro.BackgroundImage = imagem;
            pbTabuleiro.BackgroundImageLayout = ImageLayout.Tile;
            BackgroundImage = null;
            if (anterior != null) anterior.Dispose();
        }

        // Botão para fechar o jogo
        private void btnSair_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
