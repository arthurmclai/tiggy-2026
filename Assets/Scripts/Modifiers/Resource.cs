using UnityEngine;

namespace Tiggy.Modifiers
{
    public abstract class Resource : ScriptableObject
    {
        public abstract void Used();
    }
}
