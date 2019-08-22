using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InfinityDialogue
{
    public class GameContent
    {
        public Effect FxBlank { get; set; }

        public SpriteFont BrandFont { get; set; }

        public Texture2D BgKitchen { get; set; }

        public Texture2D ChrKaren { get; set; }

        public GameContent(ContentManager Content)
        {
            FxBlank = Content.Load<Effect>("Effects/Blank");

            BrandFont = Content.Load<SpriteFont>("LeagueGothic16");

            BgKitchen = Content.Load<Texture2D>("BgKitchen");

            ChrKaren = Content.Load<Texture2D>("ChrKaren");
        }
    }
}
