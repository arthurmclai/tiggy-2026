using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Roles
{
    [CreateAssetMenu(menuName = "Tiggy/Role Effects/Set Hat")]
    public class SetHatEffect : RoleEffect
    {
        [Tooltip("Prefab instantiated above the player GO. Null removes any existing hat.")]
        public GameObject HatPrefab;

        private const string HatAnchorName = "HatAnchor";

        public override void Apply(Character character)
        {
            Transform anchor = GetOrCreateAnchor(character.transform);
            foreach (Transform child in anchor)
                Destroy(child.gameObject);
            if (HatPrefab != null)
                Instantiate(HatPrefab, anchor);
        }

        public override void Revert(Character character)
        {
            Transform anchor = character.transform.Find(HatAnchorName);
            if (anchor == null) return;
            foreach (Transform child in anchor)
                Destroy(child.gameObject);
        }

        private Transform GetOrCreateAnchor(Transform parent)
        {
            Transform anchor = parent.Find(HatAnchorName);
            if (anchor != null) return anchor;
            var go = new GameObject(HatAnchorName);
            go.transform.SetParent(parent, false);
            return go.transform;
        }
    }
}
