using UnityEngine;

namespace MarioECS
{
    [CreateAssetMenu(menuName = "SO/GameConfig", fileName = "GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [field: SerializeField] public HeroData Hero { get; private set; }
        [field: Space(10f), SerializeField] public CameraData Camera { get; private set; }
        [field: Space(10f), SerializeField] public CollisionConfig Collision { get; private set; }
    }
}
