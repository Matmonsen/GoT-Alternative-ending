using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    private Rigidbody2D _rigidBody;

    public int Damage => _damage;

    void Awake()
    {
        _rigidBody = gameObject.AddComponent<Rigidbody2D>();
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
    }
}
