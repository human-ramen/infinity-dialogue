using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityDialogue.Components
{
    public class SpriteComponent
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
        }
    }

    public class SpriteFontComponent
    {
        public SpriteFont SpriteFont { get; set; }
        public StringBuilder Text { get; set; } = new StringBuilder();
        public Color Color { get; set; } = Color.Black;
        public Vector2 Position { get; set; }
        public float Depth { get; set; } = 1;
        public bool IsVisible { get; set; } = true;

        public SpriteFontComponent(SpriteFont spriteFont)
        {
            SpriteFont = spriteFont;
        }
    }
}
