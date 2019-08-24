using System.Collections.Generic;

namespace InfinityDialogue.Components
{
    public class Choice
    {
        public string Key { get; set; }
        public string Text { get; set; }

        public Choice(string key, string text)
        {
            Key = key;
            Text = text;
        }

        // TODO: events changing game state
    }

    public class ChoiceComponent
    {
        public List<Choice> Choices { get; set; }
        public int Selected { get; set; }

        public bool IsVisible { get; set; } = true;
        public bool IsConstructed { get; set; }

        public ChoiceComponent(List<Choice> choices)
        {
            Choices = choices;
        }
    }
}
