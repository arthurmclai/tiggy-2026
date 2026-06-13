using MoreMountains.Tools;
using Tiggy.Modifiers;
using UnityEngine;

namespace Tiggy.Pickups
{
    public abstract class Pickup : MMPoolableObject
    {
        [SerializeField] private Usage usage;
        [SerializeField] private Area area; // this might not work well
        [SerializeField] private Power power;
        [SerializeField] private Resource resource;
    }
}
