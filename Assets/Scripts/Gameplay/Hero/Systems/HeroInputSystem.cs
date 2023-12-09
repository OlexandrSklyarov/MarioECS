using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroInputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private InputHandleProvider _provider;
        private EcsFilter _filter;
        private EcsPool<InputData> _inputPool;

        public void Init(IEcsSystems systems)
        {
            systems.GetShared<SharedData>().InputProvider.Enable();

            var world = systems.GetWorld();
            
            _provider = systems.GetShared<SharedData>().InputProvider;

            _filter = world.Filter<InputData>().End();
            
            _inputPool = world.GetPool<InputData>();
        }
        

        public void Run(IEcsSystems systems)
        {           
            foreach (var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent);

                input.Direction = _provider.Direction;
                input.LookDirection = _provider.LookDirection;
                input.IsJumpPressed = _provider.IsJump;
                input.IsRunPressed = _provider.IsRunning;
                input.IsMouseControl = _provider.IsCurrentDeviceMouse;
                input.IsAttackPressed = _provider.IsAttack;
            }
            
            _provider.ResetInput();
        }
        

        public void Destroy(IEcsSystems systems)
        {
            systems.GetShared<SharedData>().InputProvider.Disable();
        }
    }
}