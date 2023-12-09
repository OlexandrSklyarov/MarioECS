using Cinemachine;
using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class SetupCameraSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsPool<FollowCameraData> _cameraPool;
        private EcsPool<CreateHeroEvent> _eventPool;
        private EcsPool<OrientationData> _orientationPool;
        private SharedData _data;
        private EcsFilter _eventFilter;
        private EcsFilter _cameraFilter;
        private EcsFilter _heroFilter;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
           
            _data = systems.GetShared<SharedData>();

            _eventFilter = world.Filter<CreateHeroEvent>().End();
            _cameraFilter = world.Filter<FollowCameraData>().End();
            _heroFilter = world.Filter<HeroTag>().Inc<OrientationData>().End();

            _cameraPool = world.GetPool<FollowCameraData>();
            _eventPool = world.GetPool<CreateHeroEvent>();
            _orientationPool = world.GetPool<OrientationData>();

            //Setup camera
            var entity = world.NewEntity();
            ref var followCamera = ref _cameraPool.Add(entity);
            followCamera.VC = _data.WorldData.PlayerCamera;
        }
        

        public void Run(IEcsSystems systems)
        {
            foreach (var camEnt in _cameraFilter)
            {
                ref var cam = ref _cameraPool.Get(camEnt);

                foreach (var eventEnt in _eventFilter)
                {
                    ref var evt = ref _eventPool.Get(eventEnt);
                    SetCameraTarget(ref cam, evt.OrientationTransform);

                    _eventPool.Del(eventEnt);
                }

            #if UNITY_EDITOR //Debug
                foreach (var heroEnt in _heroFilter)
                {
                    ref var orientation = ref _orientationPool.Get(heroEnt);                    
                    SetCameraTarget(ref cam, orientation.OrientationLook);
                }               
            #endif                
            }  
        }

        private void SetCameraTarget(ref FollowCameraData cam, Transform lookTransform)
        {
            cam.VC.Follow = lookTransform;
            cam.VC.m_Lens.FieldOfView = _data.Config.Camera.FOV;

            var comp = cam.VC.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                
            comp.ShoulderOffset = _data.Config.Camera.ShoulderOffset;
            comp.CameraDistance = _data.Config.Camera.Distance;
        }
    }
}