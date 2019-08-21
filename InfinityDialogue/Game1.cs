using System.Text;
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
                // .AddSystem(new DialogueSystem(_spriteBatch, _content.BrandFont))
                .AddSystem(new DebugSystem(_content, _commander))
                .Build();

            var env = _world.CreateEntity();
            var bg = new SpriteComponent(_content.BGKitchen);
            bg.IsBackground = true;
            var title = new SpriteFontComponent(_content.BrandFont);
            title.Text = new StringBuilder("Hey it's titile");
            title.Position = new Vector2(100, 100);
            env.Attach(bg);
            env.Attach(title);
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
