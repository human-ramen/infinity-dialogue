using System.Collections.Generic;
using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using HumanRamen;

namespace InfinityDialogue.Systems
{
    public class DialogueSystem : EntityDrawSystem
    {
        private Logger _l = new Logger("DialogueSystem");

        private ComponentMapper<DialogueQueueComponent> _dialogueComponent;

        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private int _w;
        private int _h;

        private enum state
        {
            Unknown,
            Dialogue,
            Choice,
            Locked,
        }
        private state _state;
        private DialogueStateComponent _current;
        private List<string> _prev = new List<string>();
        private List<DialogueChoiceComponent> _choices;

        private bool _isWaitForInput;

        public DialogueSystem(SpriteBatch spriteBatch, SpriteFont font) : base(Aspect.All(typeof(DialogueQueueComponent)))
        {
            _spriteBatch = spriteBatch;
            _font = font;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _dialogueComponent = mapperService.GetMapper<DialogueQueueComponent>();

            _l.Info("Initialized");
        }

        public override void Draw(GameTime gameTime)
        {
            _w = _spriteBatch.GraphicsDevice.Viewport.Width;
            _h = _spriteBatch.GraphicsDevice.Viewport.Height;

            if (ActiveEntities.Count == 0) { return; }
            var queue = _dialogueComponent.Get(ActiveEntities[0]);

            if (queue.IsEmpty() && _current == null)
            {
                _state = state.Unknown;
                if (_prev.Count > 0) locked();
                return;
            }

            if (_state == state.Unknown)
            {
                _state = state.Dialogue;
            }

            if (_current == null)
            {
                _current = queue.Pop();
            }

            switch (_state)
            {
                case state.Dialogue:
                    _l.Debug("Dialogue state");
                    dialogue();
                    break;
                case state.Choice:
                    _l.Debug("Choice state");
                    choice();
                    break;
                case state.Locked:
                    _l.Debug("Locked state");
                    locked();
                    break;
            }
        }

        private void dialogue()
        {
            // TODO: fancy pancy animation
            // TODO: add sound
            // TODO: add cancelation
            _spriteBatch.DrawString(_font, _current.Text, pos(), _current.Color);

            var kbd = Keyboard.GetState();

            if (kbd.IsKeyDown(Keys.Space))
            {
                _isWaitForInput = true;
            }

            if (_isWaitForInput && kbd.IsKeyUp(Keys.Space))
            {
                _isWaitForInput = false;

                _l.Debug("Next dialogue");
                _prev.Add(_current.Text);

                _current = null;
            }
        }

        private void choice()
        {
            var line = 0;

            foreach (var choice in _choices)
            {
                var cur = pos();
                cur.Y += line * 50;

                _spriteBatch.DrawString(_font, choice.Text, cur, choice.Color);

                line++;
            }

        }

        private void locked()
        {
            _spriteBatch.DrawString(_font, _prev[_prev.Count - 1], pos(), Color.Gray);
        }

        private Vector2 pos()
        {
            return new Vector2(50, _h - _h / 4);
        }
    }
}
