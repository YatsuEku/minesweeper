using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Saper.Entites;

namespace Saper.Sprites
{
	public class BombsCounter : IGameEntity
	{
		private SpriteFont _spriteFont;

		public int DrawOrder { get; set; }

		public int Bombs { get; set; } = 0;
		public Vector2 Position { get; private set; }

		public BombsCounter(SpriteFont spriteFont, Vector2 position, int bombs)
		{
			_spriteFont = spriteFont;
			Position = position;
			Bombs = bombs;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.DrawString(_spriteFont, Bombs.ToString(), Position, Color.Black);
		}

		public void Update(GameTime gameTime)
		{
			
		}
	}
}
