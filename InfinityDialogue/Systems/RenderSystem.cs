using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue.Systems
{
    public class RenderSystem : EntityDrawSystem
    {
        private SpriteBatch _spriteBatch;
        private GameContent _content;

        private ComponentMapper<SpriteComponent> _spriteMapper;
        private ComponentMapper<SpriteFontComponent> _spriteFontMapper;

        public RenderSystem(SpriteBatch spriteBatch, GameContent content) :
            base(Aspect.One(typeof(SpriteComponent), typeof(SpriteFontComponent)))
        {
            _spriteBatch = spriteBatch;
            _content = content;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.GetMapper<SpriteComponent>();
            _spriteFontMapper = mapperService.GetMapper<SpriteFontComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack,
                               blendState: BlendState.AlphaBlend);

            foreach (var entity in ActiveEntities)
            {
                drawSprite(_spriteMapper.Get(entity));
                drawSpriteFont(_spriteFontMapper.Get(entity));
            }

            _spriteBatch.End();
        }

        private void drawSprite(SpriteComponent sprite)
        {
            if (sprite == null || !sprite.IsVisible) return;

            // TODO: refactor
            if (sprite.IsBackground)
            {
                _spriteBatch.Draw(sprite.Texture,
                                  new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y,
                                                _spriteBatch.GraphicsDevice.Viewport.Width,
                                                _spriteBatch.GraphicsDevice.Viewport.Height),
                                  null, sprite.Mask, 0.0f, Vector2.Zero, SpriteEffects.None, 0);

                return;
            }

            _spriteBatch.Draw(sprite.Texture, sprite.Position, null,
                              sprite.Mask, 0.0f, Vector2.Zero, SpriteEffects.None, sprite.Depth);
        }

        private void drawSpriteFont(SpriteFontComponent spriteFont)
        {
            if (spriteFont == null || !spriteFont.IsVisible) return;

            _spriteBatch.DrawString(spriteFont.SpriteFont, spriteFont.Text, spriteFont.Position, spriteFont.Color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, spriteFont.Depth);
        }
    }
}
