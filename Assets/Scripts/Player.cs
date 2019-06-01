using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _projectile;
        [SerializeField] private bool _isLeftPlayer;
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private float _forceRate = 50;
        [SerializeField] private float _forceMin = 300;
        [SerializeField] private float _forceMax = 1000;
    

        private Transform _projectileSpawn;
        private Transform _lever;
        private float _angle;
        private float _angleSpeed;
        private float _minAngle;
        private float _maxAngle;
        private float _force;
        private int _currentHealth;
        private bool _isMyTurn;

        private GameController _gameController;

        #region Lifecycle
        private void Awake()
        {
            _isMyTurn = false;
            gameObject.AddComponent<PolygonCollider2D>();
            _currentHealth = _maxHealth;
            _force = _forceMin;
            _angleSpeed = 1;
            _lever = transform.Find("Lever");
            _projectileSpawn = _lever.Find("ProjectileSpawn");

            if (_isLeftPlayer)
            {
                _minAngle = 270;
                _maxAngle = 360;
            }
            else
            {
                _minAngle = 0;
                _maxAngle = 90;
                _angleSpeed *= -1;
                GetComponent<SpriteRenderer>().flipX = true;
                _lever.Rotate(new Vector3(0,0,180));
            }

            _gameController = GameObject.FindObjectOfType<GameController>().GetComponent<GameController>();
        }

        private void Update()
        {
            if (!_isMyTurn)
                return;

            if (IsPressingShoot())
                IncreaseShotForce();

            if (DidReleaseShoot())
                Shoot();

            if(IsIncreasingAngle())
                SetAngle(_angleSpeed);
            else if (IsDecreasingAngle())
                SetAngle(_angleSpeed * -1);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var projectile = other.GetComponent<Projectile>();
            if (projectile == null)
                return;

            TakeDamage(projectile.Damage);
        }

        #endregion

        private void TakeDamage(int damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        }
        private void Shoot()
        {
            FinishedTurn();
            var projectileObject = Instantiate(_projectile, _projectileSpawn.position, Quaternion.identity);
            var projectileRigidBody = projectileObject.GetComponent<Rigidbody2D>();
            projectileRigidBody.AddForce(_lever.transform.up * _force);
        }

        private void IncreaseShotForce()
        {
            _force = Mathf.Clamp(_force + _forceRate, _forceMin, _forceMax);
        }

        private void SetAngle(float speed)
        {
            var newAngle = _lever.localEulerAngles.z + speed;

            if (!(newAngle + _angleSpeed > _minAngle && newAngle + _angleSpeed < _maxAngle))
                newAngle = _lever.localEulerAngles.z;
            _lever.localEulerAngles = new Vector3(_lever.localEulerAngles.x, _lever.localEulerAngles.y, newAngle);
        }
    
        private void FinishedTurn()
        {
            _isMyTurn = false;
            var co = _gameController.TurnOver(name);
            StartCoroutine(co);
        }

        public void SetTurn()
        {
            _isMyTurn = true;
        }

        #region Controls

        private bool IsPressingShoot()
        {
            var spaceIsPressed = Input.GetKeyDown(KeyCode.Space);
            var mouseButtonIsPressed = Input.GetMouseButtonDown(0);

            return spaceIsPressed || mouseButtonIsPressed;
        }

        private bool DidReleaseShoot()
        {
            var spaceIsReleased = Input.GetKeyUp(KeyCode.Space);
            var mouseButtonIsReleased = Input.GetMouseButtonUp(0);

            return spaceIsReleased || mouseButtonIsReleased;
        }

        bool IsIncreasingAngle()
        {
            return Input.GetKey(KeyCode.W) ||
                   Input.GetKey(KeyCode.D) ||
                   Input.GetKey(KeyCode.UpArrow) ||
                   Input.GetKey(KeyCode.RightArrow);
        }

        bool IsDecreasingAngle()
        {
            return Input.GetKey(KeyCode.S) ||
                   Input.GetKey(KeyCode.A) ||
                   Input.GetKey(KeyCode.DownArrow) ||
                   Input.GetKey(KeyCode.LeftArrow);
        }
        #endregion
    }
}
