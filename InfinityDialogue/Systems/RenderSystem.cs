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

        public RenderSystem(SpriteBatch spriteBatch, GameContent content) : base(Aspect.All(typeof(SpriteComponent)))
        {
            _spriteBatch = spriteBatch;
            _content = content;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.GetMapper<SpriteComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            foreach (var entity in ActiveEntities)
            {
                var sprite = _spriteMapper.Get(entity);

                if (!sprite.IsVisible) continue;

                if (sprite.IsBackground)
                {
                    _spriteBatch.Draw(_content.BGKitchen,
                                      new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y,
                                                    _spriteBatch.GraphicsDevice.Viewport.Width,
                                                    _spriteBatch.GraphicsDevice.Viewport.Height), sprite.Mask);

                    continue;
                }

                _spriteBatch.Draw(_content.BGKitchen, sprite.Position, sprite.Mask);
            }

            _spriteBatch.End();
        }
    }
}
