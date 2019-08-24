using System.Collections.Generic;
using HumanRamen;
using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue.Systems
{
    public class ChoiceSystem : EntityProcessingSystem
    {
        private readonly GraphicsDevice _graphic;
        private readonly GameContent _content;

        private ComponentMapper<ChoiceComponent> _choiceMapper;

        private UIDialogComponent _uiDialogComponent;

        private Entity _currentEntity;
        private ChoiceComponent _currentChoiceComponent;
        private TextMenuComponent _currentTextMenuComponent;

        private int _currentSelected;

        public ChoiceSystem(GraphicsDevice graphic,
                            GameContent content) :
            base(Aspect.All(typeof(ChoiceComponent)))
        {
            _graphic = graphic;
            _content = content;

            _uiDialogComponent = new UIDialogComponent(new ColoredTexture(_graphic, Color.Black));
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _choiceMapper = mapperService.GetMapper<ChoiceComponent>();
            _currentEntity = CreateEntity();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            if (_currentChoiceComponent == null || _currentChoiceComponent.IsConstructed == false)
            {
                _currentChoiceComponent = _choiceMapper.Get(entityId);

                var menu = new List<TextMenuItemComponent>();

                var count = 0;
                foreach (var choice in _currentChoiceComponent.Choices)
                {
                    var item = new TextMenuItemComponent(_content.BrandFont, count, choice.Text);

                    if (item.Order == _currentChoiceComponent.Selected)
                    {
                        item.Color = Color.Yellow;
                        _currentSelected = count;
                    }

                    menu.Add(item);
                    count++;
                }

                _currentTextMenuComponent = new TextMenuComponent(menu);

                _currentEntity.Attach(_uiDialogComponent);
                _currentEntity.Attach(_currentTextMenuComponent);

                _currentChoiceComponent.IsConstructed = true;
            }

            if (_currentSelected != _currentChoiceComponent.Selected)
            {
                _currentSelected = _currentChoiceComponent.Selected;

                foreach (var item in _currentTextMenuComponent.Items)
                {
                    item.Color = Color.White;
                }

                _currentTextMenuComponent.Items[_currentSelected].Color = Color.Yellow;
            }

            _currentTextMenuComponent.IsVisible = _currentChoiceComponent.IsVisible;
        }
    }
}
