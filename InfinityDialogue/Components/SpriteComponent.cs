using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityDialogue.Components
{
    public class SpriteComponent
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public int Scale { get; set; } = 1;
        public Color Mask { get; set; } = Color.White;
        public bool IsVisible { get; set; } = true;
        public bool IsBackground { get; set; }

        public SpriteComponent(Texture2D texture)
        {
            Texture = texture;
        }
    }
}
