using UnityEngine;
using Leopotam.EcsLite;

namespace MarioECS
{
    public sealed class SpawnHeroSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var data = systems.GetShared<SharedData>();

            var heroOrientation = CreateHeroEntity(world, data);

            SendCreateEvent(world, data, heroOrientation);
        }

        
        private void SendCreateEvent(EcsWorld world, SharedData data, Transform heroOrientation)
        {
            var entity = world.NewEntity();
            var pool = world.GetPool<CreateHeroEvent>();
            ref var evt = ref pool.Add(entity);
            evt.OrientationTransform = heroOrientation;
        }


        private Transform CreateHeroEntity(EcsWorld world, SharedData data)
        {
            var entity = world.NewEntity();
            var heroView = GetHeroView(data);
            
            //hero
            world.GetPool<HeroTag>().Add(entity);

            //input
            world.GetPool<InputData>().Add(entity);

            //orientation
            ref var orientation = ref world.GetPool<OrientationData>().Add(entity);
            orientation.OrientationLook = heroView.Orientation;

            //orientation
            ref var translation = ref world.GetPool<TranslationData>().Add(entity);
            translation.Value = heroView.transform;

            //body
            ref var body = ref world.GetPool<PhysicsBodyData>().Add(entity);
            body.Rb = heroView.Rb;
            body.Collider = heroView.Collider;
            
            //Jump
            world.GetPool<JumpData>().Add(entity);
            
            //view
            ref var view = ref world.GetPool<HeroViewData>().Add(entity);
            view.ViewTransform = heroView.ViewBody;

            //animations
            ref var anim = ref world.GetPool<HeroAnimationData>().Add(entity);
            anim.AnimatorRef = heroView.Animator;

            //attack
            ref var attack = ref world.GetPool<AttackData>().Add(entity);
            attack.AttackPoint = heroView.AttackPoint;

            //hit
            ref var hit = ref world.GetPool<HitBoxData>().Add(entity);
            hit.HitBox = heroView.HitBox;
                
            return orientation.OrientationLook;
        }


        private HeroView GetHeroView(SharedData data)
        {
            var heroView = UnityEngine.Object.Instantiate
            (
                data.Config.Hero.PlayerPrefab,
                data.WorldData.PlayerSpawnPoint.position,
                Quaternion.identity
            );
            
            heroView.Init();
            
            return heroView;
        }
    }
}