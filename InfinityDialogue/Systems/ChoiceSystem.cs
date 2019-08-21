using HumanRamen;
using InfinityDialogue.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace InfinityDialogue.Systems
{
    public class ChoiceSystem : EntityProcessingSystem, ICommandHandler
    {
        public ChoiceSystem() : base(Aspect.All(typeof(ChoiceComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            throw new System.NotImplementedException();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            throw new System.NotImplementedException();
        }

        public void HandleCommand(string topic, string command)
        {
            throw new System.NotImplementedException();
        }
    }
}
