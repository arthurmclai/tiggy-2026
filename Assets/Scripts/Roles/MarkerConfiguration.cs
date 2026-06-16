using System;
using System.Collections.Generic;
using Tiggy.Utilities;
using UnityEngine;

namespace Tiggy.Roles
{
    [Serializable]
    public class MarkerSlot
    {
        public string AnchorName;
        public GameObject MarkerPrefab;
    }

    public class MarkerConfiguration : MonoBehaviour
    {
        [Tooltip("Available marker slots on this character (anchor name + prefab pairs).")]
        public MarkerSlot[] Slots;

        [Tooltip("Index into Slots for the currently selected marker.")]
        public int SelectedSlotIndex = 0;

        [HideInInspector] public Color PlayerColor = Color.white;

        private Dictionary<string, GameObject> _markerInstances = new Dictionary<string, GameObject>();

        protected virtual void Awake()
        {
            if (Slots == null) return;
            foreach (var slot in Slots)
            {
                Transform anchor = transform.FindDeep(slot.AnchorName);
                if (anchor == null)
                {
                    Debug.LogWarning($"[MarkerConfiguration] No '{slot.AnchorName}' found anywhere under {name}. Add it to the prefab at the desired attachment position.", this);
                    continue;
                }
                if (slot.MarkerPrefab == null) continue;

                var instance = Instantiate(slot.MarkerPrefab, anchor);
                instance.SetActive(false);
                _markerInstances[slot.AnchorName] = instance;
            }
        }

        protected virtual void Start()
        {
            if (Slots == null || Slots.Length == 0) return;
            int index = Mathf.Clamp(SelectedSlotIndex, 0, Slots.Length - 1);
            if (_markerInstances.TryGetValue(Slots[index].AnchorName, out var marker))
                foreach (var sr in marker.GetComponentsInChildren<SpriteRenderer>())
                    sr.color = PlayerColor;
        }

        public void ShowMarker()
        {
            if (Slots == null || Slots.Length == 0) return;
            int index = Mathf.Clamp(SelectedSlotIndex, 0, Slots.Length - 1);
            if (_markerInstances.TryGetValue(Slots[index].AnchorName, out var marker))
                marker.SetActive(true);
        }

        public void HideMarker()
        {
            if (Slots == null || Slots.Length == 0) return;
            int index = Mathf.Clamp(SelectedSlotIndex, 0, Slots.Length - 1);
            if (_markerInstances.TryGetValue(Slots[index].AnchorName, out var marker))
                marker.SetActive(false);
        }
    }
}
