using System;

namespace Tetris
{
    public class BlockQueue
    {
        private readonly Block[] blocks;
        private readonly Random rand = new Random();

        public Block NextBlock { get; private set; }

        public BlockQueue()
        {
            blocks = new Block[]
            {
                new IBlock(),
                new OBlock(),
                new TBlock(),
                new SBlock(),
                new ZBlock(),
                new JBlock(),
                new LBlock(),
            };
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            return blocks[rand.Next(blocks.Length)];
        }

        public Block GetAnUpdate()
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
