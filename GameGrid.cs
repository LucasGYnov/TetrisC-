namespace Tetris
{
    public class GameGrid
    {
        // A 2D array to store the state of the grid
        private readonly int[,] grid;

        // Properties to define the dimensions of the grid
        public int Rows { get; }
        public int Columns { get; }

        // Allows accessing or modifying grid values using [row, column]
        public int this[int row, int column]
        {
            get => grid[row, column];
            set => grid[row, column] = value;
        }

        // Constructor to initialize the grid with specified rows and columns
        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns]; // Creates a grid filled with zeros by default
        }

        // Checks if a given cell (row, column) is within the grid's boundaries
        public bool IsInside(int row, int column)
        {
            return row >= 0 && row < Rows && column >= 0 && column < Columns;
        }

        // Checks if a cell is both inside the grid and empty
        public bool IsEmpty(int row, int column)
        {
            return IsInside(row, column) && grid[row, column] == 0;
        }

        // Checks if an entire row is full
        public bool IsRowFull(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        // Checks if an entire row is empty
        public bool IsRowEmpty(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        // Clears all cells in a specific row
        private void ClearRow(int row)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row, column] = 0;
            }
        }

        // Moves a row's content down
        private void MoveRowDown(int row, int numRows)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row + numRows, column] = grid[row, column]; // Copy to the new position
                grid[row, column] = 0; // Clear the original position
            }
        }

        // Clears all full rows
        public int ClearFullRows()
        {
            int cleared = 0; // Counter for cleared rows

            // Iterate from the bottom row upwards
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (IsRowFull(row))
                {
                    ClearRow(row); // Clear it
                    cleared++; // Increment the cleared rows count
                }
                else if (cleared > 0) // If rows below were cleared
                {
                    MoveRowDown(row, cleared); // Move the current row down
                }
            }

            return cleared; // Return the total number of rows cleared
        }
    }
}
