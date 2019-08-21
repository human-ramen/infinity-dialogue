using System.Collections.Generic;

namespace InfinityDialogue.Components
{
    public class Choice
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public Choice(string id, string text)
        {
            Id = id;
            Text = text;
        }

        // TODO: events changing game state
    }

    public class ChoiceComponent
    {
        public List<Choice> Choices { get; set; }

        public ChoiceComponent(List<Choice> choices)
        {
            Choices = choices;
        }
    }
}
