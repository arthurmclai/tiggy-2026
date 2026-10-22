using UnityEngine;

namespace Tiggy.Modifiers
{
    public abstract class Resource : ScriptableObject
    {
        public abstract void OnActivated(Tiggy.Pickups.Pickup pickup);
        public abstract bool IsExhausted(Tiggy.Pickups.Pickup pickup);
    }
}
