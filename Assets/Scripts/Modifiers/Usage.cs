using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Modifiers
{
    public abstract class Usage : ScriptableObject
    {
        public abstract void OnPickedUp(Tiggy.Pickups.Pickup pickup, Character picker);
    }
}
