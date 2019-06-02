using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject _playerLeft;
        [SerializeField] private GameObject _playerRight;
        [SerializeField] private float _deadFadeRate = .2f;
        public GameObject CurrentPlayer { get; private set; }

        private Player _leftPlayer;
        private Player _rightPlayer;
        private Image _playerLeftForce;
        private Image _playerLeftHealth;
        private Image _winnerImage;

        private Image _playerRightForce;
        private Image _playerRightHealth;


        private Text _playerLeftText;
        private Text _playerRightText;


        private Button _playAgain;
        private Button _exitGame;

        private Canvas _canvas;
        private GameObject _gameEnded;

        private Player _deadPlayer;
        private SceneController _sceneManager;
        private bool _gameHasEnded;
        void Awake()
        {
            _leftPlayer = _playerLeft.GetComponent<Player>();
            _rightPlayer = _playerRight.GetComponent<Player>();

            _canvas = GameObject.Find("MainUICanvas").GetComponent<Canvas>();
            _gameEnded = _canvas.transform.Find("GameEnded").gameObject;

            _playerLeftHealth = _canvas.transform.Find("Left").Find("Healthbar").Find("Green").GetComponent<Image>();
            _playerRightHealth = _canvas.transform.Find("Right").Find("Healthbar").Find("Green").GetComponent<Image>();

            _playerLeftForce = _canvas.transform.Find("Left").Find("Forcebar").Find("Green").GetComponent<Image>();
            _playerRightForce = _canvas.transform.Find("Right").Find("Forcebar").Find("Green").GetComponent<Image>();

            _playerLeftText = _canvas.transform.Find("Left").Find("PlayerName").GetComponent<Text>();
            _playerRightText = _canvas.transform.Find("Right").Find("PlayerName").GetComponent<Text>();

            _exitGame = _gameEnded.transform.Find("Exit").GetComponent<Button>();
            _playAgain = _gameEnded.transform.Find("PlayAgain").GetComponent<Button>();

            _winnerImage = _gameEnded.transform.Find("WinnerImage").GetComponent<Image>();

            _sceneManager = GameObject.Find("SceneController").GetComponent<SceneController>();

        }

        void Start()
        {
            SetPlayer(_playerLeft);
            _playAgain.onClick.AddListener(() => _sceneManager.ReloadScene());
            _exitGame.onClick.AddListener(() => _sceneManager.LoadScene(SceneController.Scene_Menu));


            _gameEnded.SetActive(false);
        }

        void Update()
        {
            SetForce(_playerLeftForce, _leftPlayer.CurrentForcePercentage);
            SetForce(_playerRightForce, _rightPlayer.CurrentForcePercentage);

            SetHealthbar(_playerLeftHealth, _leftPlayer.CurrentHealthPercentage);
            SetHealthbar(_playerRightHealth, _rightPlayer.CurrentHealthPercentage);


            if (!_gameHasEnded)
                return;
        }

        public IEnumerator TurnOver(string playerName)
        {
            yield return new WaitForSeconds(.5f);
            SetPlayer(playerName == _playerLeft.name ? _playerRight : _playerLeft);
        }

        private void SetPlayer(GameObject player)
        {
            if (player == null)
            {
                CurrentPlayer = new GameObject("NO NAME");
                _playerRightText.text = _playerRight.name;
                _playerLeftText.text = _playerLeft.name;
                return;
            }

            StartCoroutine(nameof(StartNewTurn), player);
        }

        public void PlayerDied(GameObject player)
        {
            Time.timeScale = 0;
            _gameEnded.SetActive(true);
            _canvas.transform.Find("Left").gameObject.SetActive(false);
            _canvas.transform.Find("Right").gameObject.SetActive(false);

            _deadPlayer = player.GetComponent<Player>();
            _winnerImage.sprite = _deadPlayer.ShameImage;
            _gameHasEnded = true;

            _rightPlayer.gameObject.SetActive(false);
            _leftPlayer.gameObject.SetActive(false);
        }

        void SetHealthbar(Image healthbar, float percentage)
        {
            healthbar.fillAmount = percentage;
        }

        void SetForce(Image forceBar, float percentage)
        {
            var color = "#d91e18";

            if (percentage < .1f)
                color = "#2ecc71";
            else if (percentage < .2f)
                color = "#3fc380";
            else if (percentage < .3f)
                color = "#00b16a";
            else if (percentage < .4f)
                color = "#019875";
            else if (percentage < .5f)
                color = "#fef160";
            else if (percentage < .6f)
                color = "#f5e653";
            else if (percentage < .7f)
                color = "#f5e51b";
            else if (percentage < .8f)
                color = "#f62459";
            else if (percentage < .9f)
                color = "#f22613";
            ColorUtility.TryParseHtmlString(color, out var newColor);
            forceBar.fillAmount = percentage;
            forceBar.color = newColor;
        }

        IEnumerator StartNewTurn(GameObject player)
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag(Constants.Tags.Projectile).Length == 0);
            CurrentPlayer = player;
            CurrentPlayer.GetComponent<Player>().SetTurn();


            if (player.name.Equals(_playerLeft.name))
            {
                _playerRightText.text = _playerRight.name;
                _playerLeftText.text = _playerLeft.name + " (turn)";
            }
            else
            {
                _playerRightText.text = _playerRight.name + " (turn)";
                _playerLeftText.text = _playerLeft.name;
            }
        }
        
    }
}
