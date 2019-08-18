using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityDialogue
{
    public class GameContent
    {
        public SpriteFont BrandFont { get; set; }

        public Texture2D BGKitchen { get; set; }

        public GameContent(ContentManager Content)
        {
            BrandFont = Content.Load<SpriteFont>("Arial14");

            BGKitchen = Content.Load<Texture2D>("kitchen");
        }
    }
}
