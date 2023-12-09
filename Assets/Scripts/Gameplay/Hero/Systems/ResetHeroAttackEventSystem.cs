using Leopotam.EcsLite;

namespace MarioECS
{
    public sealed class ResetHeroAttackEventSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<HeroAttackEvent> _evtPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<HeroAttackEvent>().End();
            _evtPool = world.GetPool<HeroAttackEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                _evtPool.Del(ent); 
            }
        }
    }
}