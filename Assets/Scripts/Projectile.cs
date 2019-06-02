using UnityEngine;

namespace Assets.Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private int _damage = 10;
        [SerializeField] private int _ttl = 5;
        private Rigidbody2D _rigidBody;
        public GameObject Shooter { get; set; }

        private float _timeLived;

        public int Damage => _damage;

        void Awake()
        {
            _rigidBody = gameObject.AddComponent<Rigidbody2D>();
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }

        void Update()
        {
            _timeLived += Time.deltaTime;

            if (_timeLived > _ttl)
                Destroy(gameObject);
        }

        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
