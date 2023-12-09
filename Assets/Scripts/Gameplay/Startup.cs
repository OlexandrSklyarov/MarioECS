using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class Startup : MonoBehaviour
    {
        [SerializeField] private WorldData _worldData;
        [Space(10f), SerializeField] private UIData _uiData;
        [SerializeField] private GameConfig _config;
        
        private EcsWorld _world;
        private IEcsSystems _updateSystems;
        private IEcsSystems _fixedUpdateSystems;
        private IEcsSystems _lateUpdateSystems;


        private void Start()
        {
            _world = new EcsWorld();

            var inputAction = new PlayerInputAction();

            var data = new SharedData()
            {
                InputProvider = new InputHandleProvider(inputAction),
                Config = _config,
                WorldData = _worldData,
                UIData = _uiData
            };
            
            _updateSystems = new EcsSystems(_world, data);
            _fixedUpdateSystems = new EcsSystems(_world, data);
            _lateUpdateSystems = new EcsSystems(_world, data);

            AddSystems();
        }
        
        
        private void AddSystems()
        {
            _updateSystems

                //editor debug
            #if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
            #endif

                //world
                .Add(new InitWorldSystem())

                //hero
                .Add(new SpawnHeroSystem())
                .Add(new HeroInputSystem())
                .Add(new HeroCheckGroundSystem())
                .Add(new HeroSpeedLimitSystem())
                .Add(new HeroJumpSystem())
                .Add(new HeroRotateViewBodySystem())
                .Add(new ResetHeroAttackEventSystem())
                .Add(new HeroAttackSystem())
                .Add(new HeroAnimationSystem())
                
                //camera
                .Add(new SetupCameraSystem())
                .Init();
            
            _fixedUpdateSystems
                //hero
                .Add(new HeroMovementSystem())
                .Init();
            
            _lateUpdateSystems
                 //hero
                .Add(new HeroOrientationLookRotateSystem())
                .Init();
        }


        private void Update() => _updateSystems?.Run();
        
        
        private void FixedUpdate() => _fixedUpdateSystems?.Run();
        
        
        private void LateUpdate() => _lateUpdateSystems?.Run();


        private void OnDestroy()
        {
            _updateSystems?.Destroy();
            _updateSystems = null;
            
            _fixedUpdateSystems?.Destroy();
            _fixedUpdateSystems = null;
            
            _lateUpdateSystems?.Destroy();
            _lateUpdateSystems = null;
            
            _world?.Destroy();
            _world = null;
        }
    }
}
