using System;
using System.Collections.Generic;
using TetrisC;

namespace TetrisAIProject
{
    public class TetrisAI
    {
        private GameState gameState;
        private BlockQueue blockQueue;

        public TetrisC(GameState gameState, BlockQueue blockQueue)
        {
            this.gameState = gameState;
            this.blockQueue = blockQueue;
        }

        public void MakeMove()
        {
            // Exemple : Évalue les meilleurs mouvements possibles
            int bestScore = int.MinValue;
            string bestMove = string.Empty;

            foreach (var action in GetPossibleActions())
            {
                GameState simulationState = SimulateMove(action);

                // Calculez le score après le mouvement
                int score = EvaluateGameState(simulationState);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = action;
                }
            }

            // Applique le meilleur mouvement
            ApplyMove(bestMove);
        }

        private List<string> GetPossibleActions()
        {
            // Retourne une liste de tous les mouvements possibles : 
            // déplacement gauche, droite, rotation, et hard drop
            return new List<string> { "Left", "Right", "RotateCW", "RotateCCW", "Drop" };
        }

        private GameState SimulateMove(string move)
        {
            // Crée une simulation du jeu avec le mouvement donné appliqué
            GameState simulatedState = new GameState(gameState);
            simulatedState.ApplyMove(move);
            return simulatedState;
        }

        private int EvaluateGameState(GameState state)
        {
            // Fonction heuristique qui évalue l'état du jeu. 
            // Plus ce score est élevé, plus l'état est favorable pour l'IA.
            int score = 0;

            // Évalue la hauteur de la grille
            score -= CalculateHeightPenalty(state);

            // Évalue les lignes complètes
            score += CalculateLineClearBonus(state);

            // Évalue le nombre de trous
            score -= CalculateHolePenalty(state);

            return score;
        }

        private void ApplyMove(string move)
        {
            // Applique le mouvement au jeu (déplacement, rotation, etc.)
            switch (move)
            {
                case "Left":
                    gameState.MoveBlockLeft();
                    break;
                case "Right":
                    gameState.MoveBlockRight();
                    break;
                case "RotateCW":
                    gameState.RotateBlockCW();
                    break;
                case "RotateCCW":
                    gameState.RotateBlockCCW();
                    break;
                case "Drop":
                    gameState.DropBlock();
                    break;
            }
        }

        // Pénalité basée sur la hauteur de la grille
        private int CalculateHeightPenalty(GameState state)
        {
            int height = 0;
            for (int c = 0; c < state.GameGrid.Columns; c++)
            {
                for (int r = state.GameGrid.Rows - 1; r >= 0; r--)
                {
                    if (state.GameGrid[r, c] != 0)
                    {
                        height = state.GameGrid.Rows - r;
                        break;
                    }
                }
            }
            return height;
        }

        // Bonus pour les lignes complètes
        private int CalculateLineClearBonus(GameState state)
        {
            int linesCleared = state.LinesCleared;
            return linesCleared * 100; // Exemple de bonus
        }

        // Pénalité basée sur les trous dans la grille
        private int CalculateHolePenalty(GameState state)
        {
            int holes = 0;
            for (int c = 0; c < state.GameGrid.Columns; c++)
            {
                bool blockFound = false;
                for (int r = 0; r < state.GameGrid.Rows; r++)
                {
                    if (state.GameGrid[r, c] != 0)
                    {
                        blockFound = true;
                    }
                    else if (blockFound)
                    {
                        holes++; // Trou trouvé sous un bloc
                    }
                }
            }
            return holes * 50; // Exemple de pénalité
        }
    }
}
