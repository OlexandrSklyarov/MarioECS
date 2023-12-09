using System;
using Cinemachine;
using UnityEngine;

namespace MarioECS
{
    [Serializable]
    public sealed class WorldData
    {
        public Transform PlayerSpawnPoint => _playerSpawnPoint;
        public CinemachineVirtualCamera PlayerCamera => _palyerCamera;
        
        
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private CinemachineVirtualCamera _palyerCamera;
    }
}