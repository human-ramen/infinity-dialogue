using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue.Systems
{
    public interface IVisible
    {
        bool IsVisible { get; set; }
    }

    public class RenderSystem : EntityDrawSystem
    {
        private SpriteBatch _spriteBatch;
        private GameContent _content;

        private int _w;
        private int _h;

        private ComponentMapper<SpriteComponent> _spriteMapper;
        private ComponentMapper<SpriteFontComponent> _spriteFontMapper;
        private ComponentMapper<TextNameComponent> _textNameMapper;
        private ComponentMapper<TextDialogComponent> _textDialogMapper;
        private ComponentMapper<TextMenuComponent> _textMenuMapper;
        private ComponentMapper<UIDialogComponent> _uiDialogMapper;

        // TODO: Maybe refactor all this mess?
        public RenderSystem(SpriteBatch spriteBatch, GameContent content) :
            base(Aspect.One(typeof(SpriteComponent), typeof(SpriteFontComponent),
                            typeof(TextNameComponent), typeof(TextDialogComponent),
                            typeof(TextMenuComponent),
                            typeof(UIDialogComponent)))
        {
            _spriteBatch = spriteBatch;
            _content = content;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _spriteMapper = mapperService.GetMapper<SpriteComponent>();
            _spriteFontMapper = mapperService.GetMapper<SpriteFontComponent>();
            _textNameMapper = mapperService.GetMapper<TextNameComponent>();
            _textDialogMapper = mapperService.GetMapper<TextDialogComponent>();
            _textMenuMapper = mapperService.GetMapper<TextMenuComponent>();
            _uiDialogMapper = mapperService.GetMapper<UIDialogComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            _w = _spriteBatch.GraphicsDevice.Viewport.Width;
            _h = _spriteBatch.GraphicsDevice.Viewport.Height;

            _spriteBatch.GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack,
                               blendState: BlendState.AlphaBlend);

            foreach (var entity in ActiveEntities)
            {
                // TODO: I will hate myself when see this code after a while, but woah! This is beautiful.
                // Consider switching to F#;

                var textMenu = _textMenuMapper.Get(entity);
                if (textMenu != null && textMenu.IsVisible)
                {
                    // TODO: Things getting absolute crazy
                    foreach (var item in textMenu.Items)
                    {
                        drawSpriteFont(calculatePosition<TextMenuItemComponent>(item));
                    }
                }

                drawSprite(_spriteMapper.Get(entity));
                drawSprite(calculatePosition<UIDialogComponent>(_uiDialogMapper.Get(entity)));

                drawSpriteFont(_spriteFontMapper.Get(entity));
                drawSpriteFont(calculatePosition<TextNameComponent>(_textNameMapper.Get(entity)));
                drawSpriteFont(calculatePosition<TextDialogComponent>(_textDialogMapper.Get(entity)));
            }

            _spriteBatch.End();
        }

        private T calculatePosition<T>(ICalculatePosition sprite)
        {
            if (sprite != null)
            {
                sprite.CalculatePosition(_w, _h);
            }

            return (T)sprite;
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
