using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

namespace Tiggy.Roles
{
    public struct TagEvent
    {
        public Character Tagger;
        public Character Tagged;

        public TagEvent(Character tagger, Character tagged)
        {
            Tagger = tagger;
            Tagged = tagged;
        }

        public static void Trigger(Character tagger, Character tagged)
        {
            MMEventManager.TriggerEvent(new TagEvent(tagger, tagged));
        }
    }
}
