using MoreMountains.CorgiEngine;
using Tiggy.Pickups;
using UnityEngine;

namespace Tiggy.Modifiers
{
    [CreateAssetMenu(menuName = "Tiggy/Usages/Instant")]
    public class InstantUsage : Usage
    {
        public override void OnPickedUp(Pickup pickup, Character picker)
        {
            pickup.Activate(picker);
        }
    }
}
