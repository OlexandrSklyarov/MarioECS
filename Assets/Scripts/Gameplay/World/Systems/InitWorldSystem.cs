using Leopotam.EcsLite;
using UnityEngine;

namespace MarioECS
{
    public sealed class InitWorldSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Util.Debug.PrintColor("Init world!!!", Color.green);
        }
    }
}