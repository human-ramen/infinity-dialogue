using InfinityDialogue.Systems;

namespace InfinityDialogue.Components
{
    public class DialogComponent : IVisible
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public bool IsDone { get; set; }

        public bool IsVisible { get; set; } = true;
        public bool IsConstructed { get; set; }

        // TODO: sprite, emotion, sound, etc
    }
}
