using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject _playerLeft;
        [SerializeField] private GameObject _playerRight;
        public GameObject CurrentPlayer { get; private set; }

        private Canvas _canvas;
        private Text _turnText;
        private Text _winnerText;
        private GameObject _gameEnded;

        void Awake()
        {
            _canvas = GameObject.Find("MainUICanvas").GetComponent<Canvas>();
            _turnText = _canvas.transform.Find("Turn").Find("TurnText").GetComponent<Text>();
            _gameEnded = _canvas.transform.Find("GameEnded").gameObject;
            _winnerText = _gameEnded.transform.Find("Winner").Find("WinnerText").GetComponent<Text>();

            _gameEnded.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            SetPlayer(_playerLeft);
        }
        

        public IEnumerator TurnOver(string playerName)
        {
            yield return new WaitForSeconds(.5f);
            SetPlayer(playerName == _playerLeft.name ? _playerRight : _playerLeft);
        }

        private void SetPlayer(GameObject player)
        {
            CurrentPlayer = player;
            CurrentPlayer.GetComponent<Player>().SetTurn();
            _turnText.text = CurrentPlayer.name;
        }

        public void PlayerDied(GameObject player)
        {
            Time.timeScale = 0;
            _gameEnded.SetActive(true);
            _winnerText.text = player.name == _playerLeft.name ? _playerRight.name : _playerLeft.name;
            _canvas.transform.Find("Turn").gameObject.SetActive(false);
        }
    }
}
