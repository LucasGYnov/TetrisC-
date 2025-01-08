using System.Collections.Generic;

namespace TetrisC
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }

        private int rotationState;
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        /// <summary>
        /// Retourne les positions actuelles des tuiles du bloc sur la grille.
        /// </summary>
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        /// <summary>
        /// Effectue une rotation horaire (CW).
        /// </summary>
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        /// <summary>
        /// Effectue une rotation antihoraire (CCW).
        /// </summary>
        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        /// <summary>
        /// Déplace le bloc et retourne la distance parcourue verticalement (en lignes).
        /// </summary>
        /// <param name="rows">Nombre de lignes à déplacer.</param>
        /// <param name="columns">Nombre de colonnes à déplacer.</param>
        /// <returns>Nombre de lignes parcourues verticalement.</returns>
        public int Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
            return rows; // Retourne la distance verticale parcourue.
        }

        /// <summary>
        /// Réinitialise la position et la rotation du bloc.
        /// </summary>
        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
