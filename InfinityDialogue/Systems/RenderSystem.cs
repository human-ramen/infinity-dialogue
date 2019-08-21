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

            _spriteBatch.Begin();

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

            if (sprite.IsBackground)
            {
                _spriteBatch.Draw(_content.BGKitchen,
                                  new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y,
                                                _spriteBatch.GraphicsDevice.Viewport.Width,
                                                _spriteBatch.GraphicsDevice.Viewport.Height), sprite.Mask);

                return;
            }

            _spriteBatch.Draw(_content.BGKitchen, sprite.Position, sprite.Mask);
        }

        private void drawSpriteFont(SpriteFontComponent spriteFont)
        {
            if (spriteFont == null || !spriteFont.IsVisible) return;

            _spriteBatch.DrawString(spriteFont.SpriteFont, spriteFont.Text, spriteFont.Position, spriteFont.Color);
        }
    }
}
