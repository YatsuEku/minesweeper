using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Saper.Entites;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Saper.Sprites
{
    public class Cell : IGameEntity
    {
        private MouseState _previousMouseState;
        private MouseState _previousMouseStateRight;

        private Sprite _sprite;

        public Vector2 Position { get; }

        public int DrawOrder { get; set; }

        public CellState State { get; set; } = CellState.Reveal;
        public bool isBomb { get; set; }
        public bool isNumber { get; set; }
        public bool isEmpty { get; set; } = true;
        public int BombsAround { get; set; }

        public Vector2 Size { get => new Vector2(_sprite.Width, _sprite.Height); }
        public Rectangle Rectangle { get => new Rectangle(_sprite.Rectangle.Location, _sprite.Rectangle.Size); }
        public Texture2D Texture
		{
            get => _sprite.Texture;
            set { _sprite.Texture = value; }
		}

        public Cell(Sprite sprite, Vector2 position)
        {
            _sprite = sprite;
            Position = position;
        }

        public void AddBomb()
        {
            if(!isBomb)
			{
                isNumber = true;
                isEmpty = false;
                isBomb = false;

                BombsAround++;
            }
        }

        public void Update(GameTime gameTime)
        {
            switch(State)
			{
                case CellState.Reveal:
                    Texture = Textures.textures["squere"];
                    break;
				case CellState.Flag:
                    Texture = Textures.textures["flag"];
					break;
				case CellState.Number:
                    Texture = Textures.textures[BombsAround.ToString()];
					break;
				case CellState.Empty:
                    Texture = Textures.textures["empty"];
					break;
				case CellState.Bomb:
                    Texture = Textures.textures["bomb"];
					break;
                case CellState.None:
                    isBomb = false;
                    isNumber = false;
                    isEmpty = false;

                    BombsAround = 0;
                    break;
			}
        }

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
            _sprite.Draw(spriteBatch, Position);
		}

		#region Mouse Manager
		public bool Clicked_Left()
		{
            MouseState currentMouseState = Mouse.GetState();

            bool success = false;

            if(_previousMouseState.LeftButton != ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
                if(Position.X <= currentMouseState.X && Position.X + Size.X >= currentMouseState.X)
                    if(Position.Y <= currentMouseState.Y && Position.Y + Size.Y >= currentMouseState.Y)
                        success = true;

            _previousMouseState = currentMouseState;
            return success;
        }

        public bool Clicked_Right()
        {
            MouseState currentMouseState = Mouse.GetState();

            bool success = false;

            if(_previousMouseStateRight.RightButton != ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Pressed)
                if(Position.X <= currentMouseState.X && Position.X + Size.X >= currentMouseState.X)
                    if(Position.Y <= currentMouseState.Y && Position.Y + Size.Y >= currentMouseState.Y)
                        success = true;

            _previousMouseStateRight = currentMouseState;
            return success;
        }
		#endregion
	}
}
