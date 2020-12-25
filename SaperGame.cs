using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Saper.Entites;
using Saper.Sprites;
using System;
using System.Diagnostics;

namespace Saper
{
	public class SaperGame : Game
    {
        private const int WINDOW_WIDTH = 400;
        private const int WINDOW_HEIGHT = 700;

        private const int ROW_SIZE = 8;
        private const int COL_SIZE = 10;

        private const int LAYER_HEIGHT = 200;
		private const int LAYER_WIDTH = 400;

		private const int RESET_BUTTON_WIDTH = 140;
		private const int RESET_BUTTON_HEIGHT = 110;

		private int _bombs;

        private GameState _gameState;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _layer;
        private Texture2D _buttonReset;

        private EntityManager _entityManager;

        private Cell[,] _cells = new Cell[ROW_SIZE + 2, COL_SIZE + 2];
        private Layer _layerSprite;
        private BombsCounter _counter;
        private Button _resetButton;

        private Random _random;

        private delegate void Anonym(int index);

        public SaperGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _entityManager = new EntityManager();
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _random = new Random();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Textures.textures["squere"] = Content.Load<Texture2D>("Textures/squere");
            Textures.textures["flag"] = Content.Load<Texture2D>("Textures/flag");
            Textures.textures["empty"] = Content.Load<Texture2D>("Textures/empty");
            Textures.textures["bomb"] = Content.Load<Texture2D>("Textures/bomb");
            _layer = Content.Load<Texture2D>("Textures/Layer");
            _buttonReset = Content.Load<Texture2D>("Textures/ResetButton");

            for(int i = 1; i <= 8; i++)
                Textures.textures[$"{ i }"] = Content.Load<Texture2D>($"Textures/{ i }");

            _layerSprite = new Layer(new Sprite(_layer, 0, 0, LAYER_WIDTH, LAYER_HEIGHT), new Vector2(0, 0));
            _layerSprite.DrawOrder = 1;

            _counter = new BombsCounter(Content.Load<SpriteFont>("Text/BombsCount"), new Vector2(50, 50), _bombs);
            _counter.DrawOrder = 3;

            _resetButton = new Button(new Sprite(_buttonReset, 0, 0, RESET_BUTTON_WIDTH, RESET_BUTTON_HEIGHT), new Vector2(227, 46));
            _resetButton.DrawOrder = 2;

            InitGame();

            _entityManager.AddEntity(_layerSprite);
            _entityManager.AddEntity(_counter);
            _entityManager.AddEntity(_resetButton);
        }

        protected override void Update(GameTime gameTime)
        {
            for(int i = 1; i <= COL_SIZE; i++)
			{
                for(int j = 1; j <= ROW_SIZE; j++)
				{
                    if(_gameState != GameState.GameOver)
                    {
                        if(_cells[j, i].Clicked_Left())
                        {
                            if(_cells[j, i].State == CellState.Reveal && _cells[j, i].isEmpty)
                                Floodfill(j, i);
                            else if(_cells[j, i].State == CellState.Reveal && _cells[j, i].isNumber)
                                _cells[j, i].State = CellState.Number;
                            else if(_cells[j, i].State == CellState.Reveal && _cells[j, i].isBomb)
                                GameOver();
                        }

                        if(_cells[j, i].Clicked_Right())
                        {
                            if(_cells[j, i].State == CellState.Reveal && _counter.Bombs > 0)
                            {
                                _cells[j, i].State = CellState.Flag;

                                _counter.Bombs -= 1;
                            }
                            else if(_cells[j, i].State == CellState.Flag)
                            {
                                _cells[j, i].State = CellState.Reveal;

                                _counter.Bombs++;
                            }
                        }
                    }
                }
			}

            _resetButton.Clicked += OnResetButtonClicked;

            _entityManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _entityManager.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void InitGame()
		{
            _bombs = 0;

            for(int i = 0; i < COL_SIZE + 2; i++)
            {
                for(int j = 0; j < ROW_SIZE + 2; j++)
                    _cells[j, i] = new Cell(new Sprite(Textures.textures["squere"], 50, 50), new Vector2((j * 50) - 50, (LAYER_HEIGHT + i * 50) - 50));
            }

            int bombCount = 0;

            while(bombCount < 8)
            {
                bombCount = 0;

                for(int i = 1; i <= COL_SIZE; i++)
                {
                    for(int j = 1; j <= ROW_SIZE; j++)
                    {
                        if(_random.Next(1, 6) < 2 &&
                            (i != 0 && i != 11) && (j != 0 && j != 11))
                        {
                            _cells[j, i].isBomb = true;
                            _cells[j, i].isEmpty = false;
                            _cells[j, i].isNumber = false;

                            _cells[j - 1, i - 1].AddBomb();
                            _cells[j - 1, i].AddBomb();
                            _cells[j - 1, i + 1].AddBomb();

                            _cells[j, i - 1].AddBomb();
                            _cells[j, i + 1].AddBomb();

                            _cells[j + 1, i - 1].AddBomb();
                            _cells[j + 1, i].AddBomb();
                            _cells[j + 1, i + 1].AddBomb();

                            bombCount++;
                        }
                    }
                }
            }

            Anonym SetToNone = (index) =>
            {
                for(int i = 0; i < COL_SIZE + 2; i++)
                    _cells[index, i].State = CellState.None;
            };

            SetToNone(0);

            for(int i = 0; i < ROW_SIZE + 2; i++)
            {
                _cells[i, 0].State = CellState.None;
                _cells[i, 11].State = CellState.None;
            }

            SetToNone(9);

            foreach(var bomb in _cells)
            {
                if(bomb.isBomb)
                    _bombs++;
            }

            foreach(var entity in _cells)
                _entityManager.AddEntity(entity);

            _counter.Bombs = _bombs;

            _gameState = GameState.Playing;
        }

		#region Mouse Clicked
		private void OnResetButtonClicked(object sender, EventArgs e)
		{
            foreach(var cell in _cells)
                _entityManager.RemoveEntity(cell);

            Array.Clear(_cells, 0, _cells.Length);

            InitGame();
		}
        
        private bool Floodfill(int row, int col)
		{
            if(_cells[row, col].State == CellState.Reveal)
			{
                if(_cells[row, col].isEmpty)
                    _cells[row, col].State = CellState.Empty;

                if(_cells[row, col].isNumber)
				{
                    _cells[row, col].State = CellState.Number;
                    return true;
                }

                Floodfill(row - 1, col - 1);
                Floodfill(row - 1, col);
                Floodfill(row - 1, col + 1);

                Floodfill(row, col - 1);
                Floodfill(row, col + 1);

                Floodfill(row + 1, col - 1);
                Floodfill(row + 1, col);
                Floodfill(row + 1, col + 1);
			}

            return false;
        }

        private void GameOver()
		{
            foreach(var cell in _cells)
                if(cell.isBomb)
                    cell.State = CellState.Bomb;

            _counter.Bombs = _bombs;

            _gameState = GameState.GameOver;
		}
		#endregion
	}
}
