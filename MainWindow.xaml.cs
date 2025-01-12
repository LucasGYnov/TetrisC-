using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TetrisAIProject;

namespace TetrisC
{
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int[] dropDelays =
        {
            1000, 793, 618, 473, 355, 262, 190, 135, 94, 64, 43, 28, 18, 11, 7, 5, 3, 2, 1, 1
        }; // Temps en millisecondes par niveau (Tetris guideline)

        private GameState gameState = new GameState();
        private TetrisAI ai;

        private Key moveLeftKey = Key.Left;
        private Key moveRightKey = Key.Right;
        private Key rotateCWKey = Key.Up;
        private Key rotateCCWKey = Key.Z;
        private Key dropKey = Key.Space;
        private Key holdKey = Key.C;
        private Key softDropKey = Key.Down;
        private Button? currentKeyChangeButton;

        private MediaPlayer backgroundMusicPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
            ai = new TetrisAI(gameState, gameState.BlockQueue);

            PlayBackgroundMusic();
            LoadKeyBindings();
            ShowMenu();
        }

        private void LoadKeyBindings()
        {
            moveLeftKey = Key.Left;
            moveRightKey = Key.Right;
            rotateCWKey = Key.Up;
            rotateCCWKey = Key.Z;
            dropKey = Key.Space;
            holdKey = Key.C;
            softDropKey = Key.Down;

            LeftKeyText.Text = moveLeftKey.ToString();
            RightKeyText.Text = moveRightKey.ToString();
            RotateCWKeyText.Text = rotateCWKey.ToString();
            RotateCCWKeyText.Text = rotateCCWKey.ToString();
            DropKeyText.Text = dropKey.ToString();
            HoldKeyText.Text = holdKey.ToString();
            SoftDropKeyText.Text = softDropKey.ToString();
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block? heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Position position in block.TilePositions())
            {
                imageControls[position.Row + dropDistance, position.Column].Opacity = 0.25;
                imageControls[position.Row + dropDistance, position.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);

            ScoreText.Text = $"Score: {gameState.Score}";
            LevelText.Text = $"Level: {gameState.Level}";
            LinesText.Text = $"Lines: {gameState.LinesCleared}";
        }

        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                int level = Math.Min(gameState.Level, dropDelays.Length - 1);
                int delay = dropDelays[level];
                await Task.Delay(delay);


                gameState.MoveBlockDown();
                ai.MakeMove(); // L'IA fait un mouvement
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            if (currentKeyChangeButton != null)
            {
                AssignKey(e.Key);
                currentKeyChangeButton.Content = "Changer";
                currentKeyChangeButton = null;
                return;
            }

            if (e.Key == moveLeftKey)
            {
                gameState.MoveBlockLeft();
            }
            else if (e.Key == moveRightKey)
            {
                gameState.MoveBlockRight();
            }
            else if (e.Key == rotateCWKey)
            {
                gameState.RotateBlockCW();
            }
            else if (e.Key == rotateCCWKey)
            {
                gameState.RotateBlockCCW();
            }
            else if (e.Key == dropKey)
            {
                gameState.DropBlock();
            }
            else if (e.Key == holdKey)
            {
                gameState.HoldBlock();
            }
            else if (e.Key == softDropKey)
            {
                gameState.MoveBlockDown();
            }

            Draw(gameState);
        }

        private void ChangeKey_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                currentKeyChangeButton = button;
                button.Content = "Appuyez sur une touche...";
            }
        }

        private void AssignKey(Key newKey)
        {
            if (currentKeyChangeButton == ChangeLeftKeyButton)
            {
                moveLeftKey = newKey;
                LeftKeyText.Text = newKey.ToString();
            }
            else if (currentKeyChangeButton == ChangeRightKeyButton)
            {
                moveRightKey = newKey;
                RightKeyText.Text = newKey.ToString();
            }
            else if (currentKeyChangeButton == ChangeRotateCWKeyButton)
            {
                rotateCWKey = newKey;
                RotateCWKeyText.Text = newKey.ToString();
            }
            else if (currentKeyChangeButton == ChangeRotateCCWKeyButton)
            {
                rotateCCWKey = newKey;
                RotateCCWKeyText.Text = newKey.ToString();
            }
            else if (currentKeyChangeButton == ChangeDropKeyButton)
            {
                dropKey = newKey;
                DropKeyText.Text = newKey.ToString();
            }
            else if (currentKeyChangeButton == ChangeHoldKeyButton)
            {
                holdKey = newKey;
                HoldKeyText.Text = newKey.ToString();
            }
            else if (currentKeyChangeButton == ChangeSoftDropKeyButton)
            {
                softDropKey = newKey;
                SoftDropKeyText.Text = newKey.ToString();
            }
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (MainMenu.Visibility == Visibility.Collapsed)
            {
                await GameLoop();
            }
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        public void ShowMenu()
        {
            MainMenu.Visibility = Visibility.Visible;
            GameArea.Visibility = Visibility.Collapsed;
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            GameArea.Visibility = Visibility.Visible;

            InitializeGame();
        }

        private void ShowOptions_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            OptionsMenu.Visibility = Visibility.Visible;
        }

        private void QuitGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            OptionsMenu.Visibility = Visibility.Collapsed;
            GameOverMenu.Visibility = Visibility.Hidden;
            GameArea.Visibility = Visibility.Collapsed;

            gameState = new GameState();

            MainMenu.Visibility = Visibility.Visible;
        }

        private async void InitializeGame()
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;

            await GameLoop();
        }
        private void PlayBackgroundMusic()
        {
            string musicPath = "Assets/LofiLion-TameTheBeast.mp3";

            backgroundMusicPlayer.Open(new Uri(musicPath, UriKind.Relative));
            backgroundMusicPlayer.MediaEnded += (sender, e) => backgroundMusicPlayer.Position = TimeSpan.Zero;
            backgroundMusicPlayer.Play();
        }

        private void MusicToggle_Checked(object sender, RoutedEventArgs e)
        {
            backgroundMusicPlayer.Volume = 1.0;
            backgroundMusicPlayer.Play();
        }

        private void MusicToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            backgroundMusicPlayer.Volume = 0.0;
        }
    }
}
