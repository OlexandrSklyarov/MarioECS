using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class HeroAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _data;
        private EcsFilter _filter;
        private EcsPool<AttackData> _attackPool;
        private EcsPool<InputData> _inputPool;
        private EcsPool<HitBoxData> _hitBoxPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _data = systems.GetShared<SharedData>();

            _filter = world
                .Filter<AttackData>()
                .Inc<InputData>()
                .Inc<HitBoxData>()
                .End();

            _attackPool = world.GetPool<AttackData>();
            _inputPool = world.GetPool<InputData>();
            _hitBoxPool = world.GetPool<HitBoxData>();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();            

            foreach (var ent in _filter)
            {
                ref var input = ref _inputPool.Get(ent); 
                ref var attack = ref _attackPool.Get(ent); 
                ref var hitBox = ref _hitBoxPool.Get(ent); 

                if (!input.IsAttackPressed) continue;
                if (attack.LastAttackTime + _data.Config.Hero.Attack.Delay < Time.time) continue;

                CheckCollision(world, _data, ref attack, ref hitBox);
            }
        }

        private void CheckCollision(EcsWorld world, SharedData data, 
            ref AttackData attack, ref HitBoxData attackerHitBox)
        {
            var colliders = Physics.OverlapSphere
            (
                attack.AttackPoint.position, 
                data.Config.Hero.Attack.Range,
                data.Config.Collision.HitBoxLayer
            );

            if (colliders.Length <= 0) return;

            var entity = world.NewEntity();
            ref var evt = ref world.GetPool<HeroAttackEvent>().Add(entity);

            evt.Position = attack.AttackPoint.position;
            evt.Collisions = colliders;
            evt.Attacker = attackerHitBox.HitBox;
        }
    }
}