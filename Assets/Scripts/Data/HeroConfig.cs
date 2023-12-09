using System;
using UnityEngine;

namespace MarioECS
{
    [CreateAssetMenu(menuName = "SO/HeroData", fileName = "HeroConfig")]
    public sealed class HeroData : ScriptableObject
    {
        [field: SerializeField] public HeroView PlayerPrefab { get; private set; }
        [field: Space(10f), SerializeField] public HeroRotateConfig RotateConfig { get; private set; }
        [field: Space(10f), SerializeField] public HeroMovementConfig MovementConfig { get; private set; }
        [field: Space(10f), SerializeField] public HeroJumpConfig Jump { get; private set; }
        [field: Space(10f), SerializeField] public HeroViewConfig View { get; private set; }
        [field: Space(10f), SerializeField] public HeroAttackConfig Attack { get; private set; }
    }


    [Serializable]
    public sealed class HeroRotateConfig
    {
        [field: SerializeField, Min(0f)] public float LookRotateThreshold { get; private set; } = 0.01f;
        [field: SerializeField, Range(-90f, 90f)] public float BottomClamp { get; private set; } = -10f;
        [field: SerializeField, Range(-90f, 90f)] public float TopClamp { get; private set; } = 45f;
        [field: SerializeField] public Vector2 LookSens { get; private set; } = new Vector2(80f, 80f);
    }


    [Serializable]
    public sealed class HeroMovementConfig
    {
        [field: SerializeField, Min(1f)] public float Speed { get; private set; } = 8f;
        [field: SerializeField, Min(1f)] public float SpeedMultiplier { get; private set; } = 10f;
        [field: SerializeField, Min(0.01f)] public float GroundCheckDistance { get; private set; } = 0.3f;        
        [field: SerializeField, Min(0.01f)] public float GroundDrag { get; private set; } = 2f;    
        [field: SerializeField] public LayerMask GroundLayerMask { get; private set; }        
    }
    
    
    [Serializable]
    public sealed class HeroJumpConfig
    {
        [field: SerializeField, Min(0.01f)] public float Force { get; private set; } = 500f;       
        [field: SerializeField, Min(0.01f)] public float Cooldown { get; private set; } = 0.25f;       
        [field: SerializeField, Min(0.01f)] public float AirMultiplier { get; private set; } = 0.4f;       
    }
    
    
    [Serializable]
    public sealed class HeroViewConfig
    {
        [field: SerializeField, Min(0.01f)] public float RotateSpeed { get; private set; } = 360f;
        [field: SerializeField, Min(0.01f)] public float VelocityThreshold { get; private set; } = 0.5f;
    }


    [Serializable]
    public sealed class HeroAttackConfig
    {
        [field: SerializeField, Min(0.01f)] public float Range { get; private set; } = 0.5f;
        [field: SerializeField, Min(0.01f)] public float Delay { get; private set; } = 1.5f;
    }
}