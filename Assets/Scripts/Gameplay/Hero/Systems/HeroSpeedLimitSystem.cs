using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroSpeedLimitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<PhysicsBodyData> _bodyPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _data = systems.GetShared<SharedData>();

            _filter = world
                .Filter<HeroTag>()
                .Inc<PhysicsBodyData>()
                .End();
            
            _bodyPool = world.GetPool<PhysicsBodyData>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var body = ref _bodyPool.Get(ent);

                var curVelocity = body.Rb.velocity;
                var horVelocity = new Vector3(curVelocity.x, 0f, curVelocity.z);
                var speed = _data.Config.Hero.MovementConfig.Speed;

                if (horVelocity.sqrMagnitude <= speed * speed) continue;

                var limitedVelocity = horVelocity.normalized * speed;
                body.Rb.velocity = new Vector3(limitedVelocity.x, curVelocity.y, limitedVelocity.z);            
            }
        }
    }
}