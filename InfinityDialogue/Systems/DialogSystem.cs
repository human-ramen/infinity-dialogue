using System.Text;
using HumanRamen;
using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue.Systems
{
    public class DialogSystem : EntityProcessingSystem
    {
        private readonly Logger _l = new Logger("DialogSystem");

        private readonly GraphicsDevice _graphic;
        private readonly GameContent _content;

        private ComponentMapper<DialogComponent> _dialogMapper;

        private Entity _currentEntity;
        private DialogComponent _currentDialogComponent;

        private UIDialogComponent _backgroundSpriteComponent;
        private TextNameComponent _textNameComponent;
        private TextDialogComponent _textDialogComponent;

        private StringBuilder _buff;
        private int _timeout;

        public DialogSystem(GraphicsDevice graphic, GameContent content) : base(Aspect.All(typeof(DialogComponent)))
        {
            _graphic = graphic;
            _content = content;

            // TODO: move colored texture to palette or something?
            // TODO: move dialog bg from dialog system to scenario?
            _backgroundSpriteComponent = new UIDialogComponent(new ColoredTexture(_graphic, Color.Black));
            _textNameComponent = new TextNameComponent(content.BrandFont);
            _textDialogComponent = new TextDialogComponent(content.BrandFont);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _dialogMapper = mapperService.GetMapper<DialogComponent>();
            _currentEntity = CreateEntity();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if (_currentDialogComponent == null || _currentDialogComponent.IsConstructed == false)
            {
                // TODO: dialog inited
                _currentDialogComponent = _dialogMapper.Get(entityId);

                _currentEntity.Attach(_backgroundSpriteComponent);

                _buff = _textDialogComponent.Text;

                _currentEntity.Attach(_textDialogComponent);

                _textNameComponent.Text.Append(_currentDialogComponent.Name);
                _currentEntity.Attach(_textNameComponent);

                // TODO: do something with name;

                _currentDialogComponent.IsConstructed = true;
            }

            // TODO: timeout from settings
            if (_buff.Length < _currentDialogComponent.Text.Length && _timeout == 3)
            {
                _buff.Append(_currentDialogComponent.Text[_buff.Length]);
                // _commander.Command("Sound", "HighPitchVoice");

                _timeout = 0;
            }
            else
            {
                _currentDialogComponent.IsDone = true;
            }

            _timeout++;

            // TODO: questionable
            if (!_currentDialogComponent.IsVisible)
            {
                _textDialogComponent.IsVisible = false;
                _textNameComponent.IsVisible = false;
            }
            else
            {
                _textDialogComponent.IsVisible = true;
                _textNameComponent.IsVisible = true;
            }
        }
    }
}
