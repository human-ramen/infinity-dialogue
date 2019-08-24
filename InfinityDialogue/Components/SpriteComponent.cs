using System;
using System.Collections.Generic;
using System.Text;
using InfinityDialogue.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityDialogue.Components
{
    public interface ICalculatePosition
    {
        void CalculatePosition(int width, int height);
    }

    public class SpriteComponent : IVisible
    {
        public Texture2D Texture { get; set; }
        public Rectangle Position { get; set; }
        public Color Mask { get; set; } = Color.White;
        public float Depth { get; set; } = 0.5f;
        public bool IsVisible { get; set; } = true;
        public bool IsBackground { get; set; }

        public SpriteComponent(Texture2D texture)
        {
            Texture = texture;
            Position = new Rectangle(0, 0, texture.Width, texture.Height);
        }
    }

    public class UIDialogComponent : SpriteComponent, ICalculatePosition
    {
        public UIDialogComponent(Texture2D texture) : base(texture)
        {
            Mask = Color.Black;
        }

        public void CalculatePosition(int width, int height)
        {
            Position = new Rectangle(width / 50, height - height / 4, width - width / 25, height / 4 - 15);
        }
    }

    public class SpriteFontComponent : IVisible
    {
        public SpriteFont SpriteFont { get; set; }
        public StringBuilder Text { get; set; } = new StringBuilder();
        public Color Color { get; set; } = Color.Black;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public float Depth { get; set; } = 1;
        public bool IsVisible { get; set; } = true;

        public SpriteFontComponent(SpriteFont spriteFont)
        {
            SpriteFont = spriteFont;
        }
    }

    public class TextNameComponent : SpriteFontComponent, ICalculatePosition
    {
        public TextNameComponent(SpriteFont spriteFont) : base(spriteFont)
        {
            Color = Color.Blue;
        }

        public void CalculatePosition(int width, int height)
        {
            Position = new Vector2(width / 50 + 15, height - height / 4 + 10);
        }
    }

    public class TextDialogComponent : SpriteFontComponent, ICalculatePosition
    {
        public TextDialogComponent(SpriteFont spriteFont) : base(spriteFont)
        {
            Color = Color.White;
        }

        public void CalculatePosition(int width, int height)
        {
            Position = new Vector2(width / 50 + 15, height - height / 4 + 35);
        }
    }

    public class TextMenuItemComponent : SpriteFontComponent, ICalculatePosition, IVisible
    {
        public int Order { get; set; }

        public TextMenuItemComponent(SpriteFont spriteFont, int order, string text) : base(spriteFont)
        {
            Color = Color.White;
            Order = order;
            Text = new StringBuilder(text);
        }

        // TODO: refactor
        public void CalculatePosition(int width, int height)
        {
            // Good start;
            var x = width / 50 + 30;
            var y = height - height / 4 + 15;

            switch (Order)
            {
                case 0:
                    Position = new Vector2(x, y);
                    break;
                case 1:
                    Position = new Vector2(x + 300, y);
                    break;
                case 2:
                    Position = new Vector2(x, y + 40);
                    break;
                case 3:
                    Position = new Vector2(x + 300, y + 40);
                    break;
                default:
                    throw new Exception("Max 4 items, needs refactoring");
            }
        }
    }

    public class TextMenuComponent : IVisible
    {
        public List<TextMenuItemComponent> Items;

        public bool IsVisible { get; set; } = true;

        public TextMenuComponent(List<TextMenuItemComponent> items)
        {
            Items = items;
        }
    }
}
