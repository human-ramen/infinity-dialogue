using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HumanRamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;
using NLua;

namespace InfinityDialogue.Systems
{
    public class ControlSystem : UpdateSystem
    {
        private readonly string _topic = "Control";

        private Commander _commander;
        private Dictionary<Keys, string> _kbdmap;

        public ControlSystem(Commander commander)
        {
            _commander = commander;

            var lua = new Lua();

            lua.NewTable("kbd");
            lua.DoFile("./Lua/controls.lua");

            var raw = lua.GetTableDict(lua.GetTable("kbd")) as Dictionary<object, object>;

            _kbdmap = new Dictionary<Keys, string>();
            foreach (var row in raw)
            {
                _kbdmap.Add(Enum.Parse<Keys>(row.Key.ToString()), row.Value.ToString());
            }

            lua.Dispose();

            // _kbdmap.Add(Keys.Q, "Exit");
            // _kbdmap.Add(Keys.Space, "Continue");
            // _kbdmap.Add(Keys.F, "Fullscreen");
            // _kbdmap.Add(Keys.D, "ToggleDebugConsole");
        }

        public override void Update(GameTime gameTime)
        {
            var kbd = KeyboardExtended.GetState();

            Parallel.ForEach(_kbdmap, (m) =>
            {
                if (kbd.WasKeyJustDown(m.Key))
                {
                    _commander.Command(_topic, m.Value);
                }
            });
        }
    }
}
