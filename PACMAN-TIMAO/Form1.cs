// Importa bibliotecas essenciais do sistema, gráficos (cores, desenhos), manipulação de arquivos, texto e janelas do Windows
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

// Define o agrupamento (namespace) do projeto
namespace PACMAN_TIMAO
{
    // Declaração da classe principal da janela (Form1), que herda comportamentos padrão de janelas do Windows (Form)
    public partial class Form1 : Form
    {

        // Vetor de textos que guarda o desenho lógico do labirinto (9 linhas)
        string[] tabuleiro = new string[9];
        // Quantidade total de linhas da matriz do mapa
        int lintabuleiro = 9;
        // Quantidade total de colunas da matriz do mapa
        int coltabuleiro = 40;
        // Tamanho em pixels de cada "quadrado" (célula) do tabuleiro (24x24 pixels)
        int tamanhoCelula = 24;

        // Coordenadas iniciais do Pac-Man na matriz (Linha 1, Coluna 1)
        int pacmanLin = 1;
        int pacmanCol = 1;

        // Coordenadas iniciais do Fantasma na matriz (Linha 5, Coluna 20)
        int fantasmaLin = 5;
        int fantasmaCol = 20;

        // Pontuação atual do jogador
        int pontos = 0;
        // Tempo limite da partida em ciclos/segundos
        int tempo = 100;
        // Bandeira (flag) que indica se a partida está ativa ou parada
        bool jogoIniciado = false;
        // Controla a animação da boca (true = aberta, false = fechada)
        bool pacmanBocaAberta = true;
        // Guarda a direção atual do Pac-Man para carregar a imagem virada para o lado certo ("HD" = Horizontal Direita)
        string direcaoPacman = "HD";

        // Guarda o caminho da pasta onde o programa (.exe) está sendo executado no computador
        private readonly string pastaAplicacao = Application.StartupPath;


        // Construtor da janela: executado automaticamente assim que o programa abre
        public Form1()
        {
            // Inicializa todos os componentes visuais desenhados no Visual Studio (botões, labels, pictureboxes)
            InitializeComponent();
            // Chama a configuração inicial personalizada do jogo
            ConfigurarInicio();
        }

        // Prepara a tela antes de o jogador clicar em "Iniciar"
        private void ConfigurarInicio()
        {
            // Faz com que o formulário capture as teclas pressionadas (WASD) antes de qualquer botão focado
            this.KeyPreview = true;

            // Tenta carregar e exibir a imagem de abertura no fundo do tabuleiro
            TrocarImagemDeFundo(CarregarImagem("abertura.jpg", "abertura.png", "inicio.jpg", "inicio.png", "corinthians.jpg"));

            // Esconde os personagens na tela inicial
            pbPACMAN.Visible = false;
            pbFantasma.Visible = false;
        }


        // Evento disparado quando o jogador clica no botão "Iniciar"
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            // 1. Monta o desenho lógico do labirinto (* = Parede, . = Pastilha/Ponto, ' ' = Vazio)
            tabuleiro[0] = "****************************************";
            tabuleiro[1] = "*......................................*";
            tabuleiro[2] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[3] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[4] = "*.*.******.****.****.***.******.***.**.*";
            tabuleiro[5] = "*......................................*";
            tabuleiro[6] = "*.****.***.*******.****.********.***.***";
            tabuleiro[7] = "*......***.........****..............***";
            tabuleiro[8] = "****************************************";

            // 2. Restaura todas as posições e variáveis para o início de uma nova partida
            pacmanCol = 1;
            pacmanLin = 1;
            fantasmaLin = 5;
            fantasmaCol = 20;
            pontos = 0;
            tempo = 100;
            jogoIniciado = true;
            pacmanBocaAberta = true;
            direcaoPacman = "HD";

            // 3. Remove a imagem de abertura para deixar o fundo limpo para o desenho do labirinto
            TrocarImagemDeFundo(null);

            // 4. Carrega as imagens (sprites) do Pac-Man e do Fantasma da pasta do projeto
            pbPACMAN.Image = CarregarImagem(ObterNomesPacman());
            pbFantasma.Image = CarregarImagem("fantasma.jpg", "fantasma.png", "ghost.png");
            // Define o fundo das caixas de imagem como preto para combinar com o tabuleiro
            pbPACMAN.BackColor = Color.Black;
            pbFantasma.BackColor = Color.Black;

            // Torna os personagens visíveis, posiciona-os no mapa e traz para a camada da frente
            pbPACMAN.Visible = true;
            pbFantasma.Visible = true;
            AtualizarPosicoesPersonagens();
            pbPACMAN.BringToFront();
            pbFantasma.BringToFront();

            // 5. Configura o relógio do jogo para rodar a cada 200 milissegundos (5 vezes por segundo) e o liga
            timer1.Interval = 200;
            timer1.Enabled = true;

            // 6. Obriga o PictureBox do tabuleiro a se redesenhar imediatamente (chama o pbTabuleiro_Paint)
            pbTabuleiro.Invalidate();
        }

        // Evento executado repetidamente a cada 200ms enquanto o timer1 estiver ligado
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Diminui 1 unidade do tempo restante
            tempo--;
            // Atualiza os textos do placar na tela
            lblTempo.Text = "TEMPO: " + tempo;
            lblPontos.Text = "PONTOS: " + pontos;

            // Faz o fantasma andar um passo em direção ao Pac-Man
            MoverFantasma();
            // Alterna a imagem do Pac-Man (abre e fecha a boca)
            AlternarImagemPacman();

            // Atualiza a tela redesenhando o mapa e os personagens
            pbTabuleiro.Invalidate();

            // Verifica condição de derrota (Game Over): se o tempo zerou OU se o fantasma encostou no Pac-Man
            if (tempo <= 0 || (pacmanLin == fantasmaLin && pacmanCol == fantasmaCol))
            {
                // Desliga o relógio do jogo e marca a partida como encerrada
                timer1.Enabled = false;
                jogoIniciado = false;
                lblTempo.Text = "GAME OVER!";

                // Exibe a imagem de fim de jogo no fundo
                TrocarImagemDeFundo(CarregarImagem("gameover.jpg", "game-over.jpg", "fim.jpg", "fimdejogo.jpg", "gameover.png"));

                // Esconde os personagens
                pbPACMAN.Visible = false;
                pbFantasma.Visible = false;
            }
        }


        // Evento responsável por pintar as paredes, pastilhas e fundo dentro do pbTabuleiro
        private void pbTabuleiro_Paint(object sender, PaintEventArgs e)
        {
            // Se o jogo ainda não começou, não desenha o labirinto (mantém a imagem de abertura/gameover)
            if (!jogoIniciado) return;

            // Cria as "canetas" e "pincéis" nas cores do Corinthians (Preto e Branco) com descarte automático de memória (using)
            using (Pen penParede = new Pen(Color.White, 2)) // Borda branca de 2px para as paredes
            using (SolidBrush brushParede = new SolidBrush(Color.Black)) // Preenchimento preto da parede
            using (SolidBrush brushPonto = new SolidBrush(Color.White)) // Cor branca para as pastilhas
            using (SolidBrush brushFundo = new SolidBrush(Color.Black)) // Cor preta para o corredor vazio
            {
                // Percorre todas as linhas da matriz (0 a 8)
                for (int lin = 0; lin < lintabuleiro; lin++)
                {
                    // Percorre todas as colunas da matriz (0 a 39)
                    for (int col = 0; col < coltabuleiro; col++)
                    {
                        // Converte a posição da matriz (linha/coluna) para coordenadas em pixels (X, Y) na tela
                        int x = col * tamanhoCelula;
                        int y = lin * tamanhoCelula;

                        // Se o caractere for '*', desenha um bloco de parede com borda branca
                        if (tabuleiro[lin][col] == '*')
                        {
                            e.Graphics.FillRectangle(brushParede, x, y, tamanhoCelula, tamanhoCelula);
                            e.Graphics.DrawRectangle(penParede, x, y, tamanhoCelula, tamanhoCelula);
                        }
                        // Se o caractere for '.', desenha o fundo preto e uma bolinha branca (pastilha) no centro da célula
                        else if (tabuleiro[lin][col] == '.')
                        {
                            e.Graphics.FillRectangle(brushFundo, x, y, tamanhoCelula, tamanhoCelula);
                            e.Graphics.FillEllipse(brushPonto, x + 9, y + 9, 6, 6);
                        }
                        // Se for espaço vazio ' ', pinta apenas o fundo preto
                        else
                        {
                            e.Graphics.FillRectangle(brushFundo, x, y, tamanhoCelula, tamanhoCelula);
                        }
                    }
                }
            }

            // Sincroniza a posição visual das PictureBoxes com as coordenadas lógicas atualizadas
            AtualizarPosicoesPersonagens();
        }

        // Converte linha/coluna do Pac-Man e do Fantasma em posição (X, Y) na tela
        private void AtualizarPosicoesPersonagens()
        {
            // Calcula a posição exata somando a margem do tabuleiro + (coluna/linha * tamanho da célula)
            pbPACMAN.Location = new Point(
                pbTabuleiro.Left + (pacmanCol * tamanhoCelula),
                pbTabuleiro.Top + (pacmanLin * tamanhoCelula));

            pbFantasma.Location = new Point(
                pbTabuleiro.Left + (fantasmaCol * tamanhoCelula),
                pbTabuleiro.Top + (fantasmaLin * tamanhoCelula));

            // Garante que os personagens fiquem sempre visíveis por cima do tabuleiro
            pbPACMAN.BringToFront();
            pbFantasma.BringToFront();
        }


        // Evento disparado toda vez que o jogador aperta uma tecla no teclado
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Ignora o teclado se o jogo estiver pausado ou não tiver sido iniciado
            if (!timer1.Enabled || !jogoIniciado) return;

            // Variáveis temporárias para testar para onde o Pac-Man pretende ir
            int proximaLin = pacmanLin;
            int proximaCol = pacmanCol;

            // Verifica qual tecla WASD foi pressionada, ajusta a posição futura e atualiza a direção do sprite
            if (e.KeyCode == Keys.W) { proximaLin--; direcaoPacman = "VU"; }      // Cima (Vertical Up)
            else if (e.KeyCode == Keys.S) { proximaLin++; direcaoPacman = "VD"; } // Baixo (Vertical Down)
            else if (e.KeyCode == Keys.A) { proximaCol--; direcaoPacman = "HE"; } // Esquerda (Horizontal Esquerda)
            else if (e.KeyCode == Keys.D) { proximaCol++; direcaoPacman = "HD"; } // Direita (Horizontal Direita)

            // Verifica se a posição desejada está dentro dos limites do mapa E não é uma parede ('*')
            if (proximaLin >= 0 && proximaLin < lintabuleiro &&
                proximaCol >= 0 && proximaCol < coltabuleiro &&
                tabuleiro[proximaLin][proximaCol] != '*')
            {
                // Confirma o movimento do Pac-Man
                pacmanLin = proximaLin;
                pacmanCol = proximaCol;

                // Verifica se há uma pastilha ('.') na nova posição
                if (tabuleiro[pacmanLin][pacmanCol] == '.')
                {
                    // Soma 10 pontos ao placar
                    pontos += 10;
                    // Usa StringBuilder para substituir o '.' por espaço vazio ' ' na linha do tabuleiro
                    StringBuilder sb = new StringBuilder(tabuleiro[pacmanLin]);
                    sb[pacmanCol] = ' ';
                    tabuleiro[pacmanLin] = sb.ToString();
                }
            }

            // Solicita o redesenho imediato da tela para refletir o movimento sem esperar o próximo tick do timer
            pbTabuleiro.Invalidate();
        }

        // Lógica simples de perseguição do fantasma
        private void MoverFantasma()
        {
            int novaLin = fantasmaLin;
            int novaCol = fantasmaCol;

            // Compara a posição do fantasma com a do Pac-Man e tenta se aproximar horizontalmente ou verticalmente sem atravessar paredes
            if (fantasmaCol < pacmanCol && PodeMover(fantasmaLin, fantasmaCol + 1)) novaCol++;
            else if (fantasmaCol > pacmanCol && PodeMover(fantasmaLin, fantasmaCol - 1)) novaCol--;
            else if (fantasmaLin < pacmanLin && PodeMover(fantasmaLin + 1, fantasmaCol)) novaLin++;
            else if (fantasmaLin > pacmanLin && PodeMover(fantasmaLin - 1, fantasmaCol)) novaLin--;

            // Atualiza a posição do fantasma
            fantasmaLin = novaLin;
            fantasmaCol = novaCol;
        }

        // Método auxiliar que retorna true se a célula de destino existir no mapa e não for parede
        private bool PodeMover(int linha, int coluna)
        {
            return linha >= 0 && linha < lintabuleiro && coluna >= 0 && coluna < coltabuleiro &&
                tabuleiro[linha][coluna] != '*';
        }


        // Faz o efeito de abrir e fechar a boca do Pac-Man trocando a imagem
        private void AlternarImagemPacman()
        {
            // Inverte o estado atual (se estava aberta, fecha; se estava fechada, abre)
            pacmanBocaAberta = !pacmanBocaAberta;
            // Busca a imagem correspondente à nova direção e estado da boca
            Image imagem = CarregarImagem(ObterNomesPacman());
            if (imagem != null)
            {
                // Guarda a referência da imagem antiga, aplica a nova e libera a antiga da memória RAM
                Image imagemAnterior = pbPACMAN.Image;
                pbPACMAN.Image = imagem;
                if (imagemAnterior != null) imagemAnterior.Dispose();
            }
        }

        // Monta uma lista de possíveis nomes de arquivo para o sprite atual do Pac-Man (ex: PMHD001.jpg)
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

        // Procura os arquivos de imagem na pasta do executável e sobe até 4 pastas acima (ex: bin/Debug -> raiz do projeto)
        private Image CarregarImagem(params string[] nomes)
        {
            DirectoryInfo pasta = new DirectoryInfo(pastaAplicacao);
            // Loop que verifica a pasta atual e até 3 pastas pai
            for (int nivel = 0; nivel < 4 && pasta != null; nivel++, pasta = pasta.Parent)
            {
                // Testa cada nome de arquivo passado por parâmetro
                foreach (string nome in nomes)
                {
                    string caminho = Path.Combine(pasta.FullName, nome);
                    // Se o arquivo existir no disco, carrega uma cópia em memória (Bitmap) para não travar o arquivo
                    if (File.Exists(caminho))
                    {
                        using (Image original = Image.FromFile(caminho))
                        {
                            return new Bitmap(original);
                        }
                    }
                }
            }
            // Retorna null caso nenhuma das imagens seja encontrada
            return null;
        }

        // Troca a imagem de fundo do tabuleiro (usado para tela de abertura e game over) limpando a memória da anterior
        private void TrocarImagemDeFundo(Image imagem)
        {
            Image anterior = pbTabuleiro.BackgroundImage;
            pbTabuleiro.BackgroundImage = imagem;
            pbTabuleiro.BackgroundImageLayout = ImageLayout.Tile;
            BackgroundImage = null;
            if (anterior != null) anterior.Dispose();
        }


        // Evento disparado ao clicar no botão "Sair"
        private void btnSair_Click_1(object sender, EventArgs e)
        {
            // Encerra completamente o aplicativo
            Application.Exit();
        }
    }
}
