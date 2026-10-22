using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Modifiers
{
    public abstract class Area : ScriptableObject
    {
        public abstract IEnumerable<Character> GetAffected(Character picker);
    }
}
