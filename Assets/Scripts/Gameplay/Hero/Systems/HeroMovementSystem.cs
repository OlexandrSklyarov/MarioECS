using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<PhysicsBodyData> _bodyPool;
        private EcsPool<OrientationData> _orientationPool;
        private EcsPool<InputData> _inputPool;
        private EcsPool<GroundTag> _groundPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _data = systems.GetShared<SharedData>();

            _filter = world
                .Filter<PhysicsBodyData>()
                .Inc<OrientationData>()
                .Inc<InputData>()
                .End();
            
            _bodyPool = world.GetPool<PhysicsBodyData>();
            _orientationPool = world.GetPool<OrientationData>();
            _inputPool = world.GetPool<InputData>();
            _groundPool = world.GetPool<GroundTag>();
        }

        public void Run(IEcsSystems systems)
        {          
            foreach (var ent in _filter)
            {
                ref var body = ref _bodyPool.Get(ent);
                ref var orientation = ref _orientationPool.Get(ent);
                ref var input = ref _inputPool.Get(ent);

                var viewDir = orientation.OrientationLook.forward * input.Direction.y + 
                    orientation.OrientationLook.right * input.Direction.x;

                var moveDirection = new Vector3(viewDir.x, 0f, viewDir.z).normalized;

                var isGrounded = _groundPool.Has(ent);
                
                body.Rb.linearDamping = (isGrounded) ? _data.Config.Hero.MovementConfig.GroundDrag: 0f;                

                var speed = (isGrounded)
                    ? _data.Config.Hero.MovementConfig.Speed
                    : _data.Config.Hero.MovementConfig.Speed * _data.Config.Hero.Jump.AirMultiplier;

                var curSpeed = speed * _data.Config.Hero.MovementConfig.SpeedMultiplier;
                body.Rb.AddForce(moveDirection * curSpeed, ForceMode.Acceleration);        
            }
        }
    }
}