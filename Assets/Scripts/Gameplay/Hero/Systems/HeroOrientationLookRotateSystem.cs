using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroOrientationLookRotateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<OrientationData> _orientationPool;
        private EcsPool<InputData> _inputPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _data = systems.GetShared<SharedData>();

            _filter = world
                .Filter<OrientationData>()
                .Inc<InputData>()
                .End();
            
            _orientationPool = world.GetPool<OrientationData>();
            _inputPool = world.GetPool<InputData>();
        }

        public void Run(IEcsSystems systems)
        {       
            foreach (var ent in _filter)
            {
                ref var orientation = ref _orientationPool.Get(ent);
                ref var input = ref _inputPool.Get(ent);

                var look = input.LookDirection;
                var threshold = _data.Config.Hero.RotateConfig.LookRotateThreshold;

                if (look.sqrMagnitude >= threshold * threshold)
                {
                    orientation.TargetYaw += look.x * _data.Config.Hero.RotateConfig.LookSens.y * Time.deltaTime;
                    orientation.TargetPitch += look.y * _data.Config.Hero.RotateConfig.LookSens.x * Time.deltaTime;
                }
                
                OrientationClampAngle(ref orientation, _data);
                
                orientation.OrientationLook.rotation = Quaternion.Euler(orientation.TargetPitch, orientation.TargetYaw, 0f);
            }
        }
        

        private void OrientationClampAngle(ref OrientationData orientation, SharedData data)
        {
            var rotate = data.Config.Hero.RotateConfig;
            orientation.TargetYaw = ClampAngle(orientation.TargetYaw, float.MinValue, float.MaxValue);
            orientation.TargetPitch = ClampAngle(orientation.TargetPitch, rotate.BottomClamp, rotate.TopClamp);
        }
        
        
        private float ClampAngle(float curAngle, float minValue, float maxValue)
        {
            if (curAngle < -360f) curAngle += 360f;
            if (curAngle > 360f) curAngle -= 360f;
            return Mathf.Clamp(curAngle, minValue, maxValue);
        }
    }
}