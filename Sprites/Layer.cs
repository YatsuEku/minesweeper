using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Saper.Entites;
using Saper.Sprites;

namespace Saper
{
	public class Layer : IGameEntity
	{
        private Sprite _sprite;

		public int DrawOrder { get; set; }

		public Vector2 Position { get; }

		public Layer(Sprite sprite, Vector2 position)
		{
            _sprite = sprite;
            Position = position;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
            _sprite.Draw(spriteBatch, Position);
		}

		public void Update(GameTime gameTime)
		{
			
		}
	}
}
