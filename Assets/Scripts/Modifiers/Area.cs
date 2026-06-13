using UnityEngine;

namespace Tiggy.Modifiers
{
    public abstract class Area : ScriptableObject
    {
        public abstract void Affected(); // instead of void, return players to be affected by power
    }
}
