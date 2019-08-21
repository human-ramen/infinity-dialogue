using HumanRamen;
using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue.Systems
{
    public class DebugSystem : UpdateSystem, ICommandHandler
    {
        private readonly Commander _commander;
        private SpriteComponent _spriteComponent;

        public DebugSystem(GameContent content, Commander commander)
        {
            _commander = commander;
            _spriteComponent = new SpriteComponent(content.BgKitchen);
            _spriteComponent.IsVisible = false;

            _commander.RegisterHandler("Control", this);
            _commander.RegisterHandler("Choice", this);
        }

        public override void Initialize(World world)
        {
            var entity = world.CreateEntity();
            entity.Attach(_spriteComponent);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO add data to sprite components
        }

        public void HandleCommand(string topic, string command)
        {
            switch (command)
            {
                case "ToggleDebugConsole":
                    _spriteComponent.IsVisible ^= true;
                    break;
            }
        }
    }
}
