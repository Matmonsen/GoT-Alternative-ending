using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject _projectile;
        [SerializeField] private bool _isLeftPlayer;
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private float _increaseForceBy = .5f;
        [SerializeField] private float _timeBetweenEachForceIncrease = .01f;
        [SerializeField] private float _forceMin = 1;
        [SerializeField] private float _forceMax = 70;
        [SerializeField] private Sprite _killedSprite;

        public float CurrentForcePercentage { get; private set; }
        public float CurrentHealthPercentage { get; private set; } = 1;

        private Transform _projectileSpawn;
        private Transform _lever;
        private float _angle;
        private float _angleSpeed;
        private float _minAngle;
        private float _maxAngle;
        private float _force;
        private int _currentHealth;
        private bool _isMyTurn;
        private float _forceTimer;

        private GameController _gameController;
        private Vector3 _originalLeverRotation;

        #region Lifecycle
        private void Awake()
        {
            _forceTimer = 0;
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

            _originalLeverRotation = _lever.localEulerAngles;
            
        }

        private void Update()
        {
            Debug.Log(name + " " + _isMyTurn);
            if (!_isMyTurn)
                return;

            _forceTimer += Time.deltaTime;
            
            if (IsPressingShoot() && _forceTimer >= _timeBetweenEachForceIncrease)
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
            if (projectile.Shooter.name.Equals(name))
                return;
            TakeDamage(projectile.Damage);
        }

        #endregion
        
        private void TakeDamage(int damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);

            CurrentHealthPercentage = _maxHealth / 100f * _currentHealth / 100f;

            if (_currentHealth <= 0)
                Died();
        }
        private void Shoot()
        {
            var projectileObject = Instantiate(_projectile, _projectileSpawn.position, Quaternion.identity);
            var projectileRigidBody = projectileObject.GetComponent<Rigidbody2D>();
            var projectile = projectileObject.GetComponent<Projectile>();

            projectileObject.transform.localScale = Vector3.one * .1f;
            projectileRigidBody.AddForce(_lever.transform.up * _force, ForceMode2D.Impulse);
            projectile.Shooter = gameObject;

            FinishedTurn();
        }
        
        private void IncreaseShotForce()
        {
            _force = Mathf.Clamp(_force + _increaseForceBy, _forceMin, _forceMax);
            var percentage = _force / _forceMax;

            CurrentForcePercentage = percentage;
        }

        private void SetAngle(float speed)
        {
            var newAngle = _lever.localEulerAngles.z + speed;

            if (!(newAngle + _angleSpeed > _minAngle && newAngle + _angleSpeed < _maxAngle))
                newAngle = _lever.localEulerAngles.z;
            _lever.localEulerAngles = new Vector3(_lever.localEulerAngles.x, _lever.localEulerAngles.y, newAngle);
        }

        private void ResetAngle()
        {
            _angle = 0;
            _lever.localEulerAngles = _originalLeverRotation;
        }
    
        private void FinishedTurn()
        {
            _isMyTurn = false;
            _force = _forceMin;
            CurrentForcePercentage = 0;
            var co = _gameController.TurnOver(name);
            StartCoroutine(co);
            ResetAngle();
        }

        private void Died()
        {
            _gameController.PlayerDied(gameObject);
            _lever.gameObject.SetActive(false);
           GetComponent<SpriteRenderer>().sprite = _killedSprite;
        }

        public void SetTurn()
        {
            _isMyTurn = true;
        }

        #region Controls

        private bool IsPressingShoot()
        {
            var spaceIsPressed = Input.GetKey(KeyCode.Space);
            var mouseButtonIsPressed = Input.GetMouseButton(0);

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
