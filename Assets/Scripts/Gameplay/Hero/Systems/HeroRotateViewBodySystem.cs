using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroRotateViewBodySystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<HeroViewData> _viewPool;
        private EcsPool<PhysicsBodyData> _bodyPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _data = systems.GetShared<SharedData>();

            _filter = world
                .Filter<HeroTag>()
                .Inc<HeroViewData>()
                .Inc<PhysicsBodyData>()
                .End();

            _viewPool = world.GetPool<HeroViewData>();
            _bodyPool = world.GetPool<PhysicsBodyData>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var view = ref _viewPool.Get(ent);
                ref var body = ref _bodyPool.Get(ent);
                
                var horVelocity = new Vector3(body.Rb.linearVelocity.x, 0f, body.Rb.linearVelocity.z);

                var sqVelThreshold = _data.Config.Hero.View.VelocityThreshold * _data.Config.Hero.View.VelocityThreshold;
                if (horVelocity.sqrMagnitude < sqVelThreshold) continue;

                view.ViewTransform.rotation = Quaternion.RotateTowards
                (
                    view.ViewTransform.rotation,
                    Util.Vector3Math.DirToQuaternion(horVelocity),
                    _data.Config.Hero.View.RotateSpeed * Time.deltaTime
                );
            }
        }
    }
}