using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Saper.Entites;
using System;
using System.Diagnostics;

namespace Saper.Sprites
{
	public class Button : IGameEntity
	{
		private Sprite _sprite;

		private MouseState _previousMouseState;

		public int DrawOrder { get; set; }

		public event EventHandler Clicked;

		public Vector2 Position { get; private set; }
		public Rectangle Rectangle { get => new Rectangle(_sprite.X, _sprite.Y, _sprite.Width, _sprite.Height); }

		public Button(Sprite sprite, Vector2 position)
		{
			_sprite = sprite;
			Position = position;
		}

		#region Render Methods
		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			_sprite.Draw(spriteBatch, Position);
		}

		public void Update(GameTime gameTime)
		{
			OnClicked();
		}
		#endregion

		#region Events
		protected virtual void OnClicked()
		{
			EventHandler handler = Clicked;
			MouseState currentMouseState = Mouse.GetState();

			bool success = false;

			if(_previousMouseState.LeftButton != ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed)
				if(Position.X <= currentMouseState.X && Position.X + _sprite.Width >= currentMouseState.X)
					if(Position.Y <= currentMouseState.Y && Position.Y + _sprite.Height >= currentMouseState.Y)
						success = true;

			_previousMouseState = currentMouseState;

			if(success)
				handler?.Invoke(this, EventArgs.Empty);
		}
		#endregion
	}
}
