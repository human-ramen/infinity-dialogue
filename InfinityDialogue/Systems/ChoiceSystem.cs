using System.Collections.Generic;
using HumanRamen;
using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue.Systems
{
    public class ChoiceSystem : EntityProcessingSystem, ICommandHandler
    {
        private readonly GraphicsDevice _graphic;
        private readonly GameContent _content;
        private readonly Commander _commander;

        private ComponentMapper<ChoiceComponent> _choiceMapper;

        private UIDialogComponent _uiDialogComponent;

        private Entity _currentEntity;
        private ChoiceComponent _currentChoiceComponent;
        private TextMenuComponent _currentTextMenuComponent;
        private bool _isControlled = true;
        private int _currentChoiceNum;

        public ChoiceSystem(GraphicsDevice graphic,
                            GameContent content,
                            Commander commander) :
            base(Aspect.All(typeof(ChoiceComponent)))
        {
            _graphic = graphic;
            _content = content;
            _commander = commander;

            _uiDialogComponent = new UIDialogComponent(new ColoredTexture(_graphic, Color.Black));

            _commander.RegisterHandler("Control", this);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _choiceMapper = mapperService.GetMapper<ChoiceComponent>();
            _currentEntity = CreateEntity();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if (_currentTextMenuComponent == null)
            {
                _currentChoiceComponent = _choiceMapper.Get(entityId);

                var menu = new List<TextMenuItemComponent>();

                var order = 0;
                foreach (var choice in _currentChoiceComponent.Choices)
                {
                    var item = new TextMenuItemComponent(_content.BrandFont, order, choice.Text);

                    if (item.Order == _currentChoiceNum)
                    {
                        item.Color = Color.Yellow;
                    }

                    menu.Add(item);
                    order++;
                }

                _currentTextMenuComponent = new TextMenuComponent(menu);

                _currentEntity.Attach(_uiDialogComponent);
                _currentEntity.Attach(_currentTextMenuComponent);
            }

        }

        public void HandleCommand(string topic, string command)
        {
            if (!_isControlled) return;

            if (command == "Up")
            {
                if (_currentChoiceNum == 0) return;
                chooseIndex(--_currentChoiceNum);
            }
            else if (command == "Down")
            {
                if (_currentChoiceNum == 3) return;
                chooseIndex(++_currentChoiceNum);
            }
            else if (command == "Enter")
            {
                _isControlled = false;
                _commander.Command("Choice", _currentChoiceComponent.Choices[_currentChoiceNum].Id);
            }
        }

        private void chooseIndex(int index)
        {
            foreach (var item in _currentTextMenuComponent.Items)
            {
                item.Color = Color.White;
            }

            _currentTextMenuComponent.Items[index].Color = Color.Yellow;
        }
    }
}
