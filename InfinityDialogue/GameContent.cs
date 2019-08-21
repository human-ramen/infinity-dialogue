using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityDialogue
{
    public class GameContent
    {
        public SpriteFont BrandFont { get; set; }

        public Texture2D BgKitchen { get; set; }

        public Texture2D ChrKaren { get; set; }

        public GameContent(ContentManager Content)
        {
            BrandFont = Content.Load<SpriteFont>("LeagueGothic16");

            BgKitchen = Content.Load<Texture2D>("BgKitchen");

            ChrKaren = Content.Load<Texture2D>("ChrKaren");
        }
    }
}
