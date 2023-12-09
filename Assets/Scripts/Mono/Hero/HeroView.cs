using UnityEngine;

namespace MarioECS
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class HeroView : MonoBehaviour
    {
        public Rigidbody Rb => _rb;
        public CapsuleCollider Collider => _collider;
        public Transform Orientation => _orientation;
        public Transform ViewBody => _viewBody;
        public Animator Animator => _animator;
        public Transform AttackPoint => _attackPoint;
        public Collider HitBox => _hitBox;

        [SerializeField] private Transform _orientation;
        [SerializeField] private Transform _viewBody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private Collider _hitBox;
        
        private Rigidbody _rb;
        private CapsuleCollider _collider;
        

        public void Init()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            
            _collider = GetComponent<CapsuleCollider>();
        }
    }
}
