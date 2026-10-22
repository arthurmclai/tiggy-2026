using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Modifiers
{
    public abstract class Power : ScriptableObject
    {
        public abstract void Apply(Character character);
        public abstract void Revert(Character character);
    }
}
