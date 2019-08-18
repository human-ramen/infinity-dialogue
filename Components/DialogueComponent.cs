using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace InfinityDialogue.Components
{
    public class DialogueChoiceComponent
    {
        public string Text { get; set; }
        public Color Color { get; set; } = Color.Bisque;
        // TODO add events mb?
    }

    public class DialogueStateComponent
    {
        public string Text { get; set; }
        public Color Color { get; set; } = Color.Black;
        public List<DialogueChoiceComponent> Choices;

        // TODO more states for rendering

        public DialogueStateComponent(string text)
        {
            Text = text;
        }

        public DialogueStateComponent(string text, List<DialogueChoiceComponent> choices)
        {
            Text = text;
            Choices = choices;
        }
    }

    public class DialogueQueueComponent
    {
        private Queue<DialogueStateComponent> _queue = new Queue<DialogueStateComponent>();

        public void Add(DialogueStateComponent dsc)
        {
            _queue.Enqueue(dsc);
        }

        public DialogueStateComponent Pop()
        {
            return _queue.Dequeue();
        }

        public bool IsEmpty()
        {
            return _queue.Count == 0;
        }

        public void Clear()
        {
            _queue.Clear();
        }
    }
}
