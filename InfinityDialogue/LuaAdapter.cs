using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using NLua;

namespace InfinityDialogue
{
    public class LuaAdapter
    {
        private readonly Lua _lua;

        public Dictionary<Keys, string> GetControlScheme()
        {
            var lua = new Lua();

            lua.NewTable("kbd");
            lua.DoFile("./Lua/controls.lua");

            var raw = lua.GetTableDict(lua.GetTable("kbd")) as Dictionary<object, object>;

            var res = new Dictionary<Keys, string>();
            foreach (var row in raw)
            {
                res.Add(Enum.Parse<Keys>(row.Key.ToString()), row.Value.ToString());
            }

            lua.Dispose();

            return res;
        }
    }
}
