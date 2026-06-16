using MoreMountains.CorgiEngine;
using UnityEngine;

namespace Tiggy.Characters
{
    public class AnimatorSync : MonoBehaviour
    {
        static readonly int Idle        = Animator.StringToHash("Idle");
        static readonly int Walking     = Animator.StringToHash("Walking");
        static readonly int Jumping     = Animator.StringToHash("Jumping");
        static readonly int Dashing     = Animator.StringToHash("Dashing");
        static readonly int WallClinging = Animator.StringToHash("WallClinging");
        static readonly int Dangling    = Animator.StringToHash("Dangling");

        Animator _source;
        Animator _other;

        bool _idle, _walking, _jumping, _dashing, _wallClinging, _dangling;

        void Start()
        {
            _source = GetComponentInParent<Character>().CharacterAnimator;
            _other = GetComponent<Animator>();
        }

        void LateUpdate()
        {
            SyncBool(Idle,         ref _idle);
            SyncBool(Walking,      ref _walking);
            SyncBool(Jumping,      ref _jumping);
            SyncBool(Dashing,      ref _dashing);
            SyncBool(WallClinging, ref _wallClinging);
            SyncBool(Dangling,     ref _dangling);
        }

        void SyncBool(int hash, ref bool cache)
        {
            bool value = _source.GetBool(hash);
            if (value == cache) return;
            cache = value;
            _other.SetBool(hash, value);
        }
    }
}