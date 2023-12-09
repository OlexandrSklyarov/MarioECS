using UnityEngine;

namespace MarioECS
{
    public struct HeroAttackEvent
    {
        public Collider Attacker;
        public Collider[] Collisions;
        public Vector3 Position;
    }
}