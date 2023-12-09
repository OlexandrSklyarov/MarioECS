using UnityEngine;

namespace MarioECS
{
    public struct InputData
    {
        public Vector3 Direction;
        public Vector3 LookDirection;
        public bool IsRunPressed;
        public bool IsJumpPressed;
        public bool IsMouseControl;
        public bool IsAttackPressed;
    }
}