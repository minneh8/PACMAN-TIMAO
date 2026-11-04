# 🦅 PAC-MAN: Corinthians ⚽

![C#](https://img.shields.io/badge/C%23-.NET%20Framework-blueviolet)
![Platform](https://img.shields.io/badge/Plataforma-Windows%20Forms-blue)
![Curso](https://img.shields.io/badge/Engenharia%20de%20Software-2º%20Semestre-brightgreen)

Projeto acadêmico de uma versão funcional e customizada do jogo clássico **PAC-MAN**, tematizada com a identidade visual do **Corinthians** contra o seu rival. Desenvolvido para a disciplina de *Algoritmos e Linguagem de Programação* (2º Semestre do curso de Engenharia de Software).

---

## 📌 1. Macrovisão do Jogo

O objetivo principal do projeto foi construir um jogo funcional em C# no ambiente **Windows Forms**. 
* O mapa/labirinto é estruturado dinamicamente através de uma matriz sobre um componente `PictureBox`.
* Um componente `Timer` controla o ritmo da partida a cada ciclo (*tick*), atualizando a movimentação do Pac-Man, a inteligência de perseguição do fantasma e a verificação de colisões em tempo real.

---

## 🎮 2. Características do Jogo

- **Dimensões do Mapa:** Matriz lógica com 40 colunas e 9 linhas (40x9).
- **Tamanho dos Tiles:** Cada bloco/célula do labirinto possui 20x20 pixels.
- **Controles do Jogador:** Movimentação direta pelas teclas **W, A, S, D**.
- **Sistema de Pontuação:** Cada pastilha comida pelo Pac-Man soma **10 pontos** ao placar.
- **Física e Colisão:** 
  - Impedimento de travessia em paredes.
  - Colisão letal caso o fantasma alcance o Pac-Man.
- **Condições de Fim de Jogo (Game Over):** Ocorre ao zerar o tempo limite ou ser interceptado pelo rival.

---

## 🛠️ 3. Elementos Funcionais do Jogo

| Componente | Função no Sistema |
| :--- | :--- |
| **`pbTabuleiro`** | `PictureBox` principal onde são desenhadas as paredes, caminhos e pontos. |
| **`pbPACMAN`** | `PictureBox` representando o jogador (com sprite ou cor temática). |
| **`pbFantasma`** | `PictureBox` representando o inimigo rival que persegue o jogador. |
| **`lblTempo` & `lblPontos`** | Labels para exibição e atualização da pontuação e tempo restante. |
| **`btnIniciar` & `btnSair`** | Botões de controle para iniciar a partida e fechar o aplicativo. |
| **`timer1`** | Temporizador com intervalo de 200ms que dita o ciclo de atualização. |

---

## 📋 4. Requisitos Funcionais

- **RF-01 (Inicialização do Grid):** Renderização inicial da matriz 40x9 com paredes (`*`), pastilhas (`.`) e caminhos livres.
- **RF-02 (Entrada e Direcionamento):** Leitura contínua dos comandos do teclado (teclas WASD) com a propriedade `KeyPreview = true`.
- **RF-03 (Validação de Colisão com Paredes):** Restrição de movimento em coordenadas mapeadas como parece.
- **RF-04 (Coleta de Itens e Pontuação):** Remoção de pastilhas consumidas da matriz e adição de pontos.
- **RF-05 (Perseguição Automatizada):** Movimentação contínua do fantasma em direção à posição atual do Pac-Man.
- **RF-06 (Encerramento por Interceptação):** Interrupção do `Timer` e exibição de tela de Game Over quando Pac-Man e Fantasma ocupam a mesma célula.
- **RF-07 (Controle de Cadência):** Atualização do mapa e verificação de regras a cada *tick* do `Timer`.
- **RF-08 (Gerenciamento de Partida):** Capacidade de reiniciar o jogo e redefinir valores por botões da interface.

---

## 📂 5. Como Executar o Projeto

1. Clone este repositório para sua máquina local:
   ```bash
   git clone [https://github.com/seu-usuario/pacman-corinthians.git](https://github.com/seu-usuario/PACMAN-TIMAO.git)
