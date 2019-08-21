using HumanRamen;
using InfinityDialogue.Components;
using InfinityDialogue.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;

namespace InfinityDialogue
{
    public class Game1 : Game, ICommandHandler
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Commander _commander;

        private SpriteBatch _spriteBatch;
        private World _world;
        private GameContent _content;
        private LuaAdapter _luaAdapter;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _commander = new Commander();
            _commander.RegisterHandler("Control", this);

            _luaAdapter = new LuaAdapter();

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


            _world = new WorldBuilder()
                .AddSystem(new ControlSystem(_luaAdapter, _commander))
                .AddSystem(new RenderSystem(_spriteBatch, _content))
                .AddSystem(new DialogSystem(_graphics.GraphicsDevice, _content, _commander))
                .AddSystem(new DebugSystem(_content, _commander))
                .Build();

            var karen = _world.CreateEntity();
            var sprite = new SpriteComponent(_content.ChrKaren);
            sprite.Depth = 0.4f;
            sprite.Position = new Rectangle(150, 30, _content.ChrKaren.Width / 2, _content.ChrKaren.Height / 2);
            karen.Attach(sprite);

            var gameState = _world.CreateEntity();
            // TODO: Render layers
            var bg = new SpriteComponent(_content.BgKitchen);
            bg.IsBackground = true;
            var dialog = new DialogComponent();
            dialog.Name = "Goosebumps";
            dialog.Text = "Hello, Sunshine. Maybe some violent rape saves your morning mood?\nI like to jerk off in coffee when nobody watching. Like it?";
            gameState.Attach(bg);
            gameState.Attach(dialog);
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
            base.Draw(gameTime);
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
