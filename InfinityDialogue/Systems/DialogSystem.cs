using System.Text;
using HumanRamen;
using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue.Systems
{
    public class DialogSystem : EntityProcessingSystem, ICommandHandler
    {
        private readonly Logger _l = new Logger("DialogSystem");

        private readonly GraphicsDevice _graphic;
        private readonly GameContent _content;
        private readonly Commander _commander;

        private ComponentMapper<DialogComponent> _dialogMapper;

        private int _w;
        private int _h;
        private Entity _currentEntity;
        private DialogComponent _currentDialogComponent;
        private SpriteComponent _backgroundSpriteComponent;
        private SpriteFontComponent _nameSpriteFontComponent;
        private SpriteFontComponent _dialogSpriteFontComponent;

        private StringBuilder _buff;
        private int _timeout;

        public DialogSystem(GraphicsDevice graphic, GameContent content, Commander commander) : base(Aspect.All(typeof(DialogComponent)))
        {
            _graphic = graphic;
            _content = content;
            _commander = commander;

            _backgroundSpriteComponent = new SpriteComponent(new ColoredTexture(_graphic, Color.Black));
            _nameSpriteFontComponent = new SpriteFontComponent(content.BrandFont);
            _dialogSpriteFontComponent = new SpriteFontComponent(content.BrandFont);
            _dialogSpriteFontComponent.Color = Color.White;

            _commander.RegisterHandler("Control", this);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _dialogMapper = mapperService.GetMapper<DialogComponent>();
            _currentEntity = CreateEntity();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            _w = _graphic.Viewport.Width;
            _h = _graphic.Viewport.Height;

            if (_currentDialogComponent == null)
            {
                // TODO: dialog inited
                _currentDialogComponent = _dialogMapper.Get(entityId);

                _backgroundSpriteComponent.Position = new Rectangle(_w / 50, _h - _h / 4, _w - _w / 25, _h / 4 - 15);
                _currentEntity.Attach(_backgroundSpriteComponent);

                _dialogSpriteFontComponent.Position = new Vector2(_w / 50 + 15, _h - _h / 4 + 15);
                _buff = _dialogSpriteFontComponent.Text;

                _currentEntity.Attach(_dialogSpriteFontComponent);

                // TODO: do something with name;
            }

            // TODO: timeout from settings
            if (_buff.Length < _currentDialogComponent.Text.Length && _timeout == 3)
            {
                _buff.Append(_currentDialogComponent.Text[_buff.Length]);
                // _commander.Command("Sound", "HighPitchVoice");

                _timeout = 0;
            }

            _timeout++;

        }

        public void HandleCommand(string topic, string command)
        {
            if (topic == "Control" && command == "Continue")
            {
                _buff.Clear();
                _buff.Append(_currentDialogComponent.Text);
            }
        }
    }
}
