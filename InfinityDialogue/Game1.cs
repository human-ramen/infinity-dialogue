using InfinityDialogue.Components;
using InfinityDialogue.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;

namespace InfinityDialogue
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private World _world;
        private GameContent _content;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _content = new GameContent(Content);

            _world = new WorldBuilder()
                .AddSystem(new RenderSystem(_spriteBatch, _content.BGKitchen))
                .AddSystem(new DialogueSystem(_spriteBatch, _content.BrandFont))
                .Build();

            var entity = _world.CreateEntity();
            var queue = new DialogueQueueComponent();
            queue.Add(new DialogueStateComponent("Hello world!1"));
            queue.Add(new DialogueStateComponent("What a dialog!"));
            queue.Add(new DialogueStateComponent("Byeea"));
            entity.Attach(queue);
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);

            _spriteBatch.Begin();
            _world.Draw(gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
