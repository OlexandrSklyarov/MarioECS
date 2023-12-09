using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroJumpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<JumpData> _jumpPool;
        private EcsPool<PhysicsBodyData> _bodyPool;
        private EcsPool<GroundTag> _groundPool;
        private EcsPool<InputData> _inputPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _data = systems.GetShared<SharedData>();

            _filter = world
                .Filter<HeroTag>()
                .Inc<JumpData>()
                .Inc<PhysicsBodyData>()
                .Inc<InputData>()
                .End();

            _jumpPool = world.GetPool<JumpData>();
            _bodyPool = world.GetPool<PhysicsBodyData>();
            _groundPool = world.GetPool<GroundTag>();
            _inputPool = world.GetPool<InputData>();
        }

        public void Run(IEcsSystems systems)
        {            
            foreach (var ent   in _filter)
            {
                ref var jump = ref _jumpPool.Get(ent);
                ref var body = ref _bodyPool.Get(ent);
                ref var input = ref _inputPool.Get(ent);

                if (jump.JumpUnlockTimer > 0f)
                {
                    jump.JumpUnlockTimer -= Time.deltaTime;
                    continue;
                }
                
                if (!input.IsJumpPressed) continue;
                if (!_groundPool.Has(ent)) continue;

                Jump(ref body, ref jump, _data);
            }
        }
        

        private void Jump(ref PhysicsBodyData body, ref JumpData jump, SharedData data)
        {
            var vel = body.Rb.velocity;
            body.Rb.velocity = new Vector3(vel.x, 0f, vel.z);
            
            body.Rb.AddForce
            (
                body.Rb.transform.up * data.Config.Hero.Jump.Force, 
                ForceMode.Acceleration
            );

            jump.JumpUnlockTimer = data.Config.Hero.Jump.Cooldown;
        }
    }
}