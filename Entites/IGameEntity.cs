using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Saper.Entites
{
    public interface IGameEntity
    {
        public int DrawOrder { get; set; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
