using Tiggy.Pickups;
using UnityEngine;

namespace Tiggy.Modifiers
{
    [CreateAssetMenu(menuName = "Tiggy/Resources/Single Use")]
    public class SingleUseResource : Resource
    {
        public override void OnActivated(Pickup pickup) { }

        public override bool IsExhausted(Pickup pickup) => true;
    }
}
