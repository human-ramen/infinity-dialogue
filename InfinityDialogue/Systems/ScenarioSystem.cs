using System;
using System.Collections.Generic;
using HumanRamen;
using InfinityDialogue.UI;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using NLua;
using static InfinityDialogue.UI.Choices;

namespace InfinityDialogue.Systems
{
    public class ScenarioSystem : UpdateSystem, ICommandHandler
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
        private readonly UISystem _ui;
        private readonly Commander _commander;

        private readonly string _startScript = "Lua/test_scenario.lua";

        private State _state;
        private Entity _scenarioEntity;

        private Scenario.Node _currentNode;
        private bool _currentNodeInited;

        public ScenarioSystem(GameContent content, UISystem ui, Commander commander)
        {
            _content = content;
            _ui = ui;
            _commander = commander;

            _commander.RegisterHandler("UI", this);
        }

        public override void Initialize(World world)
        {
            _scenarioEntity = world.CreateEntity();

            _lua.LoadCLRPackage();
            var scenario = new Scenario();
            _lua["Scenario"] = scenario;

            _lua.DoFile(_startScript);

            _currentNode = scenario.Start;
            // TODO: check node type
            _state = State.Dialog;
            _ui.IsDialogBackgroundVisible = true;

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
                _ui.UpdateDialog(new Dialog(_currentNode.DialogueName, _currentNode.DialogueText));

                _currentNodeInited = true;
            }

            if (!_currentNodeInited && _state == State.Choice)
            {
                var choicesList = new List<Choice>();

                foreach (var choice in _currentNode.Responses)
                {
                    choicesList.Add(new Choice(choice.Key, choice.Key));
                }

                var choices = new Choices(choicesList);
                _ui.UpdateChoices(choices);

                _currentNodeInited = true;
            }
        }

        public override void Dispose()
        {
            _lua.Dispose();
            base.Dispose();
        }

        public void HandleCommand(string topic, string command)
        {
            if (_state == State.Dialog && topic == "UI" && command == "Continue")
            {
                _state = State.Choice;
                _currentNodeInited = false;
                return;
            }

            if (_state == State.Choice && topic == "UI")
            {
                if (!_currentNode.Responses.ContainsKey(command))
                {
                    throw new ExceptionNullNode(command);
                }

                _currentNode = _currentNode.Responses[command];

                _state = State.Dialog;
                _currentNodeInited = false;
                return;
            }
        }
    }

    public class ExceptionNullNode : Exception
    {
        public ExceptionNullNode(string command) : base(String.Format("Node with key {0} is NULL", command))
        {
        }
    }
}
