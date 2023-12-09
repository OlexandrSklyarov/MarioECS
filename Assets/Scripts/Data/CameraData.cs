using UnityEngine;

namespace MarioECS
{
    [CreateAssetMenu(menuName = "SO/CameraData", fileName = "CameraConfig")]
    public sealed class CameraData : ScriptableObject
    {
        [field: SerializeField] public Vector3 ShoulderOffset { get; private set; } = new Vector3(0f, 3.25f, -10f);
        [field: SerializeField, Min (1f)] public float FOV{ get; private set; } = 25f;
        [field: SerializeField, Min (1f)] public float Distance { get; private set; } = 15f;
    }
}