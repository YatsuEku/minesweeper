using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Saper.Sprites
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Width { get; set; }
        public int Height { get; set; }

        public Color TintColor { get; set; } = Color.White;

        public Rectangle Rectangle { get => new Rectangle(X, Y, Width, Height); }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        public Sprite(Texture2D texture, int width, int height)
        {
            Texture = texture;
            Width = width;
            Height = height;
        }

        public Sprite(Texture2D texture, int x, int y, int width, int height)
        {
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Sprite(Sprite sprite, Vector2 position)
        {
            Texture = sprite.Texture;
            X = sprite.X;
            Y = sprite.Y;
            Width = sprite.Width;
            Height = sprite.Height;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, Rectangle, TintColor);
        }
    }
}
