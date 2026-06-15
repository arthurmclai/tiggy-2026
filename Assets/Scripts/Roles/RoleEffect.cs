using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Roles
{
    public abstract class RoleEffect : ScriptableObject
    {
        public abstract void Apply(Character character);
        public abstract void Revert(Character character);
    }
}
