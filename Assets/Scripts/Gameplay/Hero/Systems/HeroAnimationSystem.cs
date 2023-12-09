using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<HeroAnimationData> _animPool;
        private EcsPool<PhysicsBodyData> _bodyPool;
        private EcsPool<InputData> _inputPool;
        private EcsPool<GroundTag> _groundPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _data = systems.GetShared<SharedData>();

            _filter = world
                .Filter<HeroAnimationData>()
                .Inc<PhysicsBodyData>()
                .Inc<InputData>()
                .End();
            
            _animPool = world.GetPool<HeroAnimationData>();
            _bodyPool = world.GetPool<PhysicsBodyData>();
            _inputPool = world.GetPool<InputData>();
            _groundPool = world.GetPool<GroundTag>();
        }

        public void Run(IEcsSystems systems)
        {            
            foreach (var ent in _filter)
            {
                ref var anim = ref _animPool.Get(ent);
                ref var body = ref _bodyPool.Get(ent);
                ref var input = ref _inputPool.Get(ent);
                var isGrounded = _groundPool.Has(ent);

                //attack
                if (input.IsAttackPressed)
                {
                    anim.AnimatorRef.SetTrigger(ConstPrm.Animation.ATTACK);
                }

                //speed
                var normalSpeed = (isGrounded) ? GetBodyNormalizeSpeed(_data, ref body) : 0f;
                anim.AnimatorRef.SetFloat(ConstPrm.Animation.MOVE_SPEED_PRM, normalSpeed, 0.1f, Time.deltaTime);

                //falling
                anim.AnimatorRef.SetBool(ConstPrm.Animation.GROUND_PRM, isGrounded);
            }
        }

        private float GetBodyNormalizeSpeed(SharedData data, ref PhysicsBodyData body)
        {
            var speed = data.Config.Hero.MovementConfig.Speed;
            var vel = body.Rb.velocity;
            vel.y = 0f;

            return vel.magnitude / speed;
        }
    }
}