using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue
{
    public class RenderSystem : DrawSystem
    {
        private SpriteBatch _sb;
        private Texture2D _bg;

        public RenderSystem(SpriteBatch sb, Texture2D bg)
        {
            _sb = sb;
            _bg = bg;
        }

        public override void Draw(GameTime gameTime)
        {
            _sb.Draw(_bg, new Rectangle(0, 0, _sb.GraphicsDevice.Viewport.Width, _sb.GraphicsDevice.Viewport.Height), Color.White);
        }
    }
}
