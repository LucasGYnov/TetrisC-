using System;

namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new OBlock(),
            new TBlock(),
            new SBlock(),
            new ZBlock(),
            new JBlock(),
            new LBlock(),
        };

        private readonly Random rand = new Random();

        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            return blocks[rand.Next(blocks.Length)];
        }

        public Block GetAndUpdate()
        {
            Block current = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (NextBlock.Id == current.Id);

            return current;
        }
    }
}
