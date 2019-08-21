using System.Collections.Generic;
using System.Threading.Tasks;
using HumanRamen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;

namespace InfinityDialogue.Systems
{
    public class ControlSystem : UpdateSystem
    {
        private readonly string _topic = "Control";

        private Commander _commander;
        private LuaAdapter _luaAdapter;
        private Dictionary<Keys, string> _kbdmap;

        public ControlSystem(LuaAdapter luaAdapter, Commander commander)
        {
            _luaAdapter = luaAdapter;
            _commander = commander;

            _kbdmap = _luaAdapter.GetControlScheme();

            // TODO parse lua config
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
