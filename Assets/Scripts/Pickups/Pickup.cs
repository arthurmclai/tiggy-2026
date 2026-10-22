using MoreMountains.CorgiEngine;
using Tiggy.Modifiers;
using UnityEngine;

namespace Tiggy.Pickups
{
    public class Pickup : PickableItem
    {
        [SerializeField] private Power _power;
        [SerializeField] private Usage _usage;
        [SerializeField] private Resource _resource;
        [SerializeField] private Area _area;

        protected override void Pick(GameObject picker)
        {
            base.Pick(picker);
            Character character = picker.GetComponent<Character>();
            if (character == null) return;
            _usage.OnPickedUp(this, character);
        }

        public void Activate(Character picker)
        {
            foreach (var target in _area.GetAffected(picker))
                _power.Apply(target);

            _resource.OnActivated(this);
            if (_resource.IsExhausted(this))
                gameObject.SetActive(false);
        }
    }
}
