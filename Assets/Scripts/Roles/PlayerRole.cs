using System;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Roles
{
    public enum Role { None, Free, It, Assist }

    public class PlayerRole : MonoBehaviour
    {
        public Role CurrentRole { get; private set; } = Role.None;

        public event Action<Role, Role> OnRoleChanged;

        private List<RoleEffect> _activeEffects = new List<RoleEffect>();
        private Character _character;

        protected virtual void Awake()
        {
            _character = GetComponent<Character>();
        }

        public void SetRole(Role role, IEnumerable<RoleEffect> effects = null)
        {
            Role previous = CurrentRole;

            foreach (var effect in _activeEffects)
                effect.Revert(_character);
            _activeEffects.Clear();

            CurrentRole = role;

            if (effects != null)
            {
                foreach (var effect in effects)
                {
                    effect.Apply(_character);
                    _activeEffects.Add(effect);
                }
            }

            OnRoleChanged?.Invoke(previous, CurrentRole);
        }
    }
}
