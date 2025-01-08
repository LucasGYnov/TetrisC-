namespace TetrisC
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Ajoute deux positions pour produire une nouvelle position.
        /// </summary>
        public static Position operator +(Position a, Position b)
        {
            return new Position(a.Row + b.Row, a.Column + b.Column);
        }

        /// <summary>
        /// Soustrait deux positions pour produire une nouvelle position.
        /// </summary>
        public static Position operator -(Position a, Position b)
        {
            return new Position(a.Row - b.Row, a.Column - b.Column);
        }
    }
}
