using UnityEngine;

namespace Tiggy.Utilities
{
    public static class TransformExtensions
    {
        public static Transform FindDeep(this Transform root, string childName)
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                if (t.name == childName) return t;
            return null;
        }
    }
}
