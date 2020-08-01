using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DnDSekai.Data.Types
{
    public class Effect
    {
        public Dictionary<string, int> effects;
        public List<string> special;

        public Effect()
        {
            effects = new Dictionary<string, int>();
            special = new List<string>();
        }

        public void Merge(Effect effect)
        {
            foreach (KeyValuePair<string, int> e in effect.effects)
            {
                effects.TryGetValue(e.Key, out int value);
                effects[e.Key] = value + e.Value;
            }
            foreach (string s in effect.special)
            {
                if (!special.Contains(s))
                {
                    special.Add(s);
                }
            }
        }
    }
}