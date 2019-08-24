using System.Collections.Generic;
using HumanRamen;
using InfinityDialogue.Systems;
using InfinityDialogue.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using static InfinityDialogue.UI.Choices;

namespace InfinityDialogue
{
    public class Game1 : Game, ICommandHandler
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Commander _commander;

        private SpriteBatch _spriteBatch;
        private World _world;
        private GameContent _content;
        private UISystem _ui;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _commander = new Commander();
            _commander.RegisterHandler("Control", this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
            _content = new GameContent(Content);
            _ui = new UISystem(_spriteBatch, _content, _commander);

            _world = new WorldBuilder()
                .AddSystem(new ScenarioSystem(_content, _commander))
                .AddSystem(new ControlSystem(_commander))
                .AddSystem(new RenderSystem(_spriteBatch, _content))
                // .AddSystem(new DialogSystem(_graphics.GraphicsDevice, _content))
                // .AddSystem(new ChoiceSystem(_graphics.GraphicsDevice, _content))
                .AddSystem(new DebugSystem(_content, _commander))
                .Build();

            _ui.IsDialogBackgroundVisible = true;
            // _ui.UpdateDialog(new Dialog("Karen", "Hello yyyyy uuuuu WOOOORLD long text hey\nhey hey hey hey"));
            var choicesList = new List<Choice>();
            choicesList.Add(new Choice("keyone", "heyhoa"));
            choicesList.Add(new Choice("keytwo", "heyhoaaa"));
            choicesList.Add(new Choice("keythree", "wow"));
            _ui.UpdateChoices(new Choices(choicesList));
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack,
                               blendState: BlendState.AlphaBlend);

            _world.Draw(gameTime);
            _ui.Draw(gameTime);
            base.Draw(gameTime);

            _spriteBatch.End();
        }

        public void HandleCommand(string topic, string command)
        {
            if (topic == "Control" && command == "Fullscreen")
            {
                _graphics.ToggleFullScreen();
            }

            if (topic == "Control" && command == "Exit")
            {
                Exit();
            }
        }
    }
}
