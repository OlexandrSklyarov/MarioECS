
using UnityEngine;

namespace MarioECS
{
    [CreateAssetMenu(menuName = "SO/CollisionConfig", fileName = "CollisionConfig")]
    public class CollisionConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask HitBoxLayer { get; private set; }
        
    }
}