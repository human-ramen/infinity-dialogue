using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HumanRamen
{
    public interface ICommandHandler
    {
        void HandleCommand(string topic, string command);
    }

    public class Commander
    {
        private readonly Dictionary<string, List<ICommandHandler>> _topicHandlers = new Dictionary<string, List<ICommandHandler>>();

        public void RegisterHandler(string topic, ICommandHandler handler)
        {
            if (!_topicHandlers.ContainsKey(topic))
            {
                _topicHandlers.Add(topic, new List<ICommandHandler>());
            }

            _topicHandlers[topic].Add(handler);

        }

        public void Command(string topic, string command)
        {
            if (!_topicHandlers.ContainsKey(topic))
            {
                throw new ExceptionNoSuchTopic(topic);
            }

            Parallel.ForEach(_topicHandlers[topic],
                             handler => handler.HandleCommand(topic, command));
        }
    }

    public class CommanderLogger
    {
        private readonly Logger _l = new Logger("Commander");
        private readonly Commander _c = new Commander();

        public void RegisterHandler(string topic, ICommandHandler handler)
        {
            _c.RegisterHandler(topic, handler);

            _l.Debug(String.Format("Registered {0} with topic {1}", handler.GetType(), topic));
        }

        public void Command(string topic, string command)
        {
            _c.Command(topic, command);

            _l.Debug(String.Format("Topic: {0}, Command {1}", topic, command));
        }
    }

    public class ExceptionNoSuchTopic : Exception
    {
        public ExceptionNoSuchTopic(string topic) : base(String.Format("No such topic {0}", topic))
        {
        }
    }
}
