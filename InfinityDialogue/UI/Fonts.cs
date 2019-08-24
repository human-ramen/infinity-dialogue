using Microsoft.Xna.Framework.Graphics;

namespace InfinityDialogue.UI
{
    public class Fonts
    {
        public SpriteFont Brand { get; private set; }
        // TODO etc

        public Fonts(GameContent _content)
        {
            Brand = _content.BrandFont;
        }
    }
}
