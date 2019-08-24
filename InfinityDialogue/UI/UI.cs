using System;
using System.Collections.Generic;
using System.Text;
using HumanRamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static InfinityDialogue.UI.Choices;

namespace InfinityDialogue.UI
{
    public class UISystem : ICommandHandler
    {
        public bool IsDialogBackgroundVisible { get; set; }

        private enum state
        {
            Unknown,
            Dialog,
            Choice,
            Wait,
        }
        private state _state;

        private readonly Logger _l = new Logger("UI");
        private readonly Palette _palette;
        private readonly Fonts _fonts;
        private readonly SpriteBatch _spriteBatch;
        private readonly Commander _commander;

        private int _w;
        private int _h;

        private bool _isDialogAnimationDone;
        private string _currentDialogName;
        private string _currentDialogText;

        private List<Choice> _currentChoices;
        private int _currentSelected;

        private StringBuilder _dialogBuff = new StringBuilder();
        private int _dialogTimeout;


        public UISystem(SpriteBatch spriteBatch, GameContent content, Commander commander)
        {
            _palette = new Palette(spriteBatch.GraphicsDevice);
            _fonts = new Fonts(content);
            _spriteBatch = spriteBatch;
            _commander = commander;

            _commander.RegisterHandler("Control", this);
        }

        public void Draw(GameTime delta)
        {
            _w = _spriteBatch.GraphicsDevice.Viewport.Width;
            _h = _spriteBatch.GraphicsDevice.Viewport.Height;

            if (IsDialogBackgroundVisible) drawDialogBackground();
            if (_state == state.Dialog) drawDialog();
            if (_state == state.Choice) drawChoices();
        }

        public void HandleCommand(string topic, string command)
        {
            if (_state == state.Dialog && _dialogBuff.Length < _currentDialogText.Length
                && topic == "Control" && command == "Enter")
            {
                _isDialogAnimationDone = true;
                _dialogBuff.Clear();
                _dialogBuff.Append(_currentDialogText);
            }

            if (_state == state.Dialog && _isDialogAnimationDone
                && topic == "Control" && command == "Enter")
            {
                _l.Debug("EVENT: Next dialog");
            }

            if (_state == state.Choice
                && topic == "Control" && command == "Enter")
            {
                _l.Debug(String.Format("EVENT: Selected ID: {0}", _currentChoices[_currentSelected].Key));
            }

            if (_state == state.Choice
                && topic == "Control" && command == "Up")
            {
                if (_currentSelected != _currentChoices.Count - 1)
                {
                    _currentSelected++;
                }
                else
                {
                    _currentSelected = 0;
                }
            }

            if (_state == state.Choice
                && topic == "Control" && command == "Down")
            {
                if (_currentSelected != 0)
                {
                    _currentSelected--;
                }
                else
                {
                    _currentSelected = _currentChoices.Count - 1;
                }
            }
        }

        public void UpdateDialog(Dialog dialog)
        {
            _currentDialogName = dialog.Name;
            _currentDialogText = dialog.Text;

            _state = state.Dialog;
        }

        public void UpdateChoices(Choices choices)
        {
            _currentChoices = choices.ChoicesList;

            _state = state.Choice;
        }

        private void drawDialogBackground()
        {
            _spriteBatch.Draw(_palette.Dark, new Rectangle(_w / 50, _h - _h / 4, _w - _w / 25, _h / 4 - 15),
                              null, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void drawDialog()
        {
            if (_dialogBuff.Length < _currentDialogText.Length && _dialogTimeout == 3)
            {
                _isDialogAnimationDone = false;

                _dialogBuff.Append(_currentDialogText[_dialogBuff.Length]);
                // _commander.Command("Sound", "HighPitchVoice");

                _dialogTimeout = 0;
            }

            if (_isDialogAnimationDone == false)
            {
                _dialogTimeout++;
            }

            if (_dialogBuff.Length == _currentDialogText.Length)
            {
                _isDialogAnimationDone = true;
            }

            if (_currentDialogName != "")
            {
                _spriteBatch.DrawString(_fonts.Brand, _currentDialogName, new Vector2(_w / 50 + 15, _h - _h / 4 + 10),
                                  Color.Cyan, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1);
            }

            if (_currentDialogText != "")
            {
                _spriteBatch.DrawString(_fonts.Brand, _dialogBuff.ToString(), new Vector2(_w / 50 + 15, _h - _h / 4 + 35),
                                      Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1);
            }
        }

        public void drawChoices()
        {
            var x = _w / 50 + 30;
            var y = _h - _h / 4 + 15;

            var pos = Vector2.Zero;
            var count = 0;
            foreach (var choice in _currentChoices)
            {
                switch (count)
                {
                    case 0:
                        pos = new Vector2(x, y);
                        break;
                    case 1:
                        pos = new Vector2(x + 300, y);
                        break;
                    case 2:
                        pos = new Vector2(x, y + 40);
                        break;
                    case 3:
                        pos = new Vector2(x + 300, y + 40);
                        break;
                    default:
                        throw new Exception("Max 4 items, needs refactoring");
                }

                Color color;

                if (count != _currentSelected)
                {
                    color = Color.White;
                }
                else
                {
                    color = Color.Yellow;
                }

                _spriteBatch.DrawString(_fonts.Brand, choice.Text, pos,
                                      color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 1);

                count++;
            }
        }
    }
}
