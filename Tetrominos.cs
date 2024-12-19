namespace Tetris
{
    public abstract class Block
    {
        public abstract int Id { get; }
        public abstract Position[][] Tiles { get; }
        protected abstract Position StartOffset { get; }
    }

    public class IBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(1, 0), new(1, 1), new(1, 2), new(1, 3) },
            new Position[] { new(0, 2), new(1, 2), new(2, 2), new(3, 2) },
            new Position[] { new(2, 0), new(2, 1), new(2, 2), new(2, 3) },
            new Position[] { new(0, 1), new(1, 1), new(2, 1), new(3, 1) },
        };

        public override int Id => 1;
        protected override Position StartOffset => new(-1, 3);
        public override Position[][] Tiles => tiles;
    }

    public class OBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0, 0), new(0, 1), new(1, 0), new(1, 1) }
        };

        public override int Id => 2;
        protected override Position StartOffset => new(0, 4);
        public override Position[][] Tiles => tiles;
    }

    public class TBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0, 1), new(1, 0), new(1, 1), new(1, 2) },
            new Position[] { new(0, 1), new(1, 1), new(1, 2), new(2, 1) },
            new Position[] { new(1, 0), new(1, 1), new(1, 2), new(2, 1) },
            new Position[] { new(0, 1), new(1, 0), new(1, 1), new(2, 1) },
        };

        public override int Id => 3;
        protected override Position StartOffset => new(0, 3);
        public override Position[][] Tiles => tiles;
    }

    public class SBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(1, 0), new(1, 1), new(0, 1), new(0, 2) },
            new Position[] { new(0, 1), new(1, 1), new(1, 2), new(2, 2) },
            new Position[] { new(1, 0), new(1, 1), new(0, 1), new(0, 2) },
            new Position[] { new(0, 1), new(1, 1), new(1, 2), new(2, 2) },
        };

        public override int Id => 4;
        protected override Position StartOffset => new(0, 3);
        public override Position[][] Tiles => tiles;
    }

    public class ZBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0, 0), new(0, 1), new(1, 1), new(1, 2) },
            new Position[] { new(0, 2), new(1, 1), new(1, 2), new(2, 1) },
            new Position[] { new(0, 0), new(0, 1), new(1, 1), new(1, 2) },
            new Position[] { new(0, 2), new(1, 1), new(1, 2), new(2, 1) },
        };

        public override int Id => 5;
        protected override Position StartOffset => new(0, 3);
        public override Position[][] Tiles => tiles;
    }

    public class JBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0, 0), new(1, 0), new(1, 1), new(1, 2) },
            new Position[] { new(0, 1), new(0, 2), new(1, 1), new(2, 1) },
            new Position[] { new(1, 0), new(1, 1), new(1, 2), new(2, 2) },
            new Position[] { new(0, 1), new(1, 1), new(2, 0), new(2, 1) },
        };

        public override int Id => 6;
        protected override Position StartOffset => new(0, 3);
        public override Position[][] Tiles => tiles;
    }

    public class LBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new(0, 2), new(1, 0), new(1, 1), new(1, 2) },
            new Position[] { new(0, 1), new(1, 1), new(2, 1), new(2, 2) },
            new Position[] { new(1, 0), new(1, 1), new(1, 2), new(2, 0) },
            new Position[] { new(0, 1), new(0, 2), new(1, 1), new(2, 1) },
        };

        public override int Id => 7;
        protected override Position StartOffset => new(0, 3);
        public override Position[][] Tiles => tiles;
    }
}
