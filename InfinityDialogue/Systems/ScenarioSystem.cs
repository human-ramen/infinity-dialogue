using System.Collections.Generic;
using HumanRamen;
using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using NLua;

namespace InfinityDialogue.Systems
{
    public class ScenarioSystem : UpdateSystem
    {
        public enum State
        {
            Wait,
            Dialog,
            Choice,
            Battle,
        }

        private readonly Logger _l = new Logger("GameStateSystem");
        private readonly Lua _lua = new Lua();

        private readonly GameContent _content;
        private readonly Commander _commander;

        private readonly string _startScript = "Lua/test_scenario.lua";

        private State _state;
        private Entity _scenarioEntity;

        private Scenario.Node _currentNode;
        private bool _currentNodeInited;
        private DialogComponent _currentDialogComponent;
        private ChoiceComponent _currentChoiceComponent;

        public ScenarioSystem(GameContent content, Commander commander)
        {
            _content = content;
            _commander = commander;

            // _commander.RegisterHandler("Control", this);
        }

        public override void Initialize(World world)
        {
            _scenarioEntity = world.CreateEntity();

            _lua.LoadCLRPackage();
            var scenario = new Scenario();
            _lua["Scenario"] = scenario;

            _lua.DoFile(_startScript);

            _currentNode = scenario.Start;
            _state = State.Dialog;

            // var karen = _world.CreateEntity();
            // var sprite = new SpriteComponent(_content.ChrKaren);
            // sprite.Depth = 0.4f;
            // sprite.Position = new Rectangle(150, 30, _content.ChrKaren.Width / 2, _content.ChrKaren.Height / 2);
            // karen.Attach(sprite);

            // var gameState = _world.CreateEntity();
            // // TODO: Render layers
            // var bg = new SpriteComponent(_content.BgKitchen);
            // bg.IsBackground = true;
            // // var dialog = new DialogComponent();
            // // dialog.Name = "Karen";
            // // dialog.Text = "Hello, Sunshine. Maybe some violent rape saves your morning mood?\nI like to jerk off in coffee when nobody watching. Like it?";

            // var listChoices = new List<Choice>();
            // listChoices.Add(new Choice("satans_call", "Heil Satan!"));
            // listChoices.Add(new Choice("masterbate", "<Starting wildly masturbate>"));
            // listChoices.Add(new Choice("coffee", "Hey... eehhh... Coffee??"));
            // listChoices.Add(new Choice("suck", "You suck fuck you fucking fuck ////"));
            // var choices = new ChoiceComponent(listChoices);
            // gameState.Attach(bg);
            // // gameState.Attach(dialog);
            // gameState.Attach(choices);
        }


        public override void Update(GameTime gameTime)
        {
            if (!_currentNodeInited && _state == State.Dialog)
            {
                if (_currentDialogComponent == null)
                {
                    _currentDialogComponent = new DialogComponent();
                }
                else
                {
                    _currentDialogComponent.IsVisible = true;
                    _currentDialogComponent.IsConstructed = false;
                }
                _currentDialogComponent.Name = _currentNode.DialogueName;
                _currentDialogComponent.Text = _currentNode.DialogueText;

                if (_currentChoiceComponent != null) _currentChoiceComponent.IsVisible = false;

                _scenarioEntity.Attach(_currentDialogComponent);

                _currentNodeInited = true;
            }

            if (!_currentNodeInited && _state == State.Choice)
            {
                var choices = new List<Choice>();

                foreach (var c in _currentNode.Responses)
                {
                    choices.Add(new Choice(c.Key, c.Key));
                }

                if (_currentChoiceComponent == null)
                {

                    _currentChoiceComponent = new ChoiceComponent(choices);
                }
                else
                {
                    _currentChoiceComponent.IsVisible = true;
                    _currentChoiceComponent.IsConstructed = true;
                }

                if (_currentDialogComponent != null) _currentDialogComponent.IsVisible = false;

                _scenarioEntity.Attach(_currentChoiceComponent);

                _currentNodeInited = true;
            }
        }

        // public void HandleCommand(string topic, string command)
        // {
        //     if (!_currentNodeInited) return;

        //     if (topic == "Control" && command == "Continue" && _state == State.Dialog)
        //     {
        //         _state = State.Choice;
        //         _currentNodeInited = false;
        //     }

        //     if (topic == "Control" && command == "Enter" && _state == State.Choice)
        //     {
        //         var id = _currentChoiceComponent.Choices[_currentChoiceComponent.Selected].Key;
        //         if (_currentNode.Responses[id] == null)
        //         {
        //             _l.Debug(id);
        //             foreach (var resp in _currentNode.Responses)
        //             {
        //                 _l.Debug(resp.ToString());
        //             }
        //             throw new Exception("Node is null");
        //         }
        //         _currentNode = _currentNode.Responses[id];

        //         _state = State.Dialog;
        //         _currentNodeInited = false;
        //     }

        //     if (topic == "Control" && command == "Down" && _state == State.Choice)
        //     {
        //         if (_currentChoiceComponent.Selected != _currentChoiceComponent.Choices.Count - 1)
        //         {
        //             _currentChoiceComponent.Selected++;
        //         }
        //         else
        //         {
        //             _currentChoiceComponent.Selected = 0;
        //         }
        //     }

        //     if (topic == "Control" && command == "Up" && _state == State.Choice)
        //     {
        //         if (_currentChoiceComponent.Selected != 0)
        //         {
        //             _currentChoiceComponent.Selected--;
        //         }
        //         else
        //         {
        //             _currentChoiceComponent.Selected = _currentChoiceComponent.Choices.Count - 1;
        //         }
        //     }
        // }

        public override void Dispose()
        {
            _lua.Dispose();
            base.Dispose();
        }
    }
}
