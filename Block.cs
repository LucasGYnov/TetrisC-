using Sytem.Collections.Generic;

namespace Tetris
{
    public class Block
    {
        protected Position[][] Tiles { get;}
        protected Position[][] StartOffset { get;}
        public abstract int Id { get; }
        private int rotationState;
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public Ienumerble<Position> TilePositions()
        {
            foreach (Position pos in Tile[rotationState])
            {
                yield return new Position(pos.Row + offset.Row, pos.Column + offset.Column);
            }
        }

        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

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

        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset(){
            rotationState =0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}