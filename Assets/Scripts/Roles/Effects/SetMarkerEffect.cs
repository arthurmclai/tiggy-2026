using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace Tiggy.Roles
{
    [CreateAssetMenu(menuName = "Tiggy/Role Effects/Set Marker")]
    public class SetMarkerEffect : RoleEffect
    {
        public override void Apply(Character character)
        {
            character.GetComponent<MarkerConfiguration>()?.ShowMarker();
        }

        public override void Revert(Character character)
        {
            character.GetComponent<MarkerConfiguration>()?.HideMarker();
        }
    }
}
