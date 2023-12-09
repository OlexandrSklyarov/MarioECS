using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroCheckGroundSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<GroundTag> _groundPool;
        private EcsPool<TranslationData> _translationPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _data = systems.GetShared<SharedData>();

            _filter = world
                .Filter<HeroTag>()
                .Inc<TranslationData>()
                .End();

            _groundPool = world.GetPool<GroundTag>();
            _translationPool = world.GetPool<TranslationData>();
        }
        
        public void Run(IEcsSystems systems)
        {           
            foreach (var ent in _filter)
            {
                ref var translation = ref _translationPool.Get(ent);
                
                if (IsGroundChecked(ref translation, _data))
                {
                    if (!_groundPool.Has(ent)) _groundPool.Add(ent);
                }
                else
                {
                    if (_groundPool.Has(ent)) _groundPool.Del(ent);
                }
            }
        }        


        private bool IsGroundChecked(ref TranslationData translation, SharedData data)
        {
            var origin = translation.Value.position + Vector3.up;
            var dist = 1f + data.Config.Hero.MovementConfig.GroundCheckDistance;
            
            return Physics.Raycast(origin, Vector3.down, dist, data.Config.Hero.MovementConfig.GroundLayerMask);
        }
    }
}