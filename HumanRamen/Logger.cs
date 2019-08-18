using System;

namespace HumanRamen
{
    public class Logger
    {
        private string _from;
        private string _prev = "";

        public Logger(string from)
        {
            _from = from;
        }

        public void Debug(string log)
        {
            if (isAlreadyLogged(log)) return;

            Console.WriteLine("DEBUG: {0} - {1}", _from, log);
        }

        public void Info(string log)
        {
            if (isAlreadyLogged(log)) return;

            Console.WriteLine("INFO: {0} - {1}", _from, log);
        }

        public void Warn(string log)
        {
            if (isAlreadyLogged(log)) return;

            Console.WriteLine("WARN: {0} - {1}", _from, log);
        }

        private bool isAlreadyLogged(string log)
        {
            if (_prev.Equals(log)) return true;

            _prev = log;

            return false;
        }
    }
}
