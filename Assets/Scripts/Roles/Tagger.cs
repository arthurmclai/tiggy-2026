using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine;

namespace Tiggy.Roles
{
    public class Tagger : MonoBehaviour
    {
        private Character _character;
        private PlayerRole _role;
        private Dictionary<Character, float> _gracePeriodExpiry = new Dictionary<Character, float>();

        protected virtual void Awake()
        {
            _character = GetComponent<Character>();
            _role = GetComponent<PlayerRole>();
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (_role.CurrentRole != Role.It) return;
            if ((LayerManager.PlayerLayerMask & (1 << other.gameObject.layer)) == 0) return;

            var otherRole = other.GetComponentInParent<PlayerRole>();
            if (otherRole == null || otherRole.CurrentRole != Role.Free) return;

            var otherCharacter = other.GetComponentInParent<Character>();
            if (otherCharacter == null) return;

            if (IsInGracePeriod(otherCharacter)) return;
            TagEvent.Trigger(_character, otherCharacter);
        }

        public void StartGracePeriod(Character character, float duration)
        {
            _gracePeriodExpiry[character] = Time.time + duration;
        }

        private bool IsInGracePeriod(Character character)
        {
            return _gracePeriodExpiry.TryGetValue(character, out float expiry) && Time.time < expiry;
        }
    }
}
