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

        void Awake()
        {
            _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            _turnText = _canvas.transform.Find("Turn").Find("TurnText").GetComponent<Text>();
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
    }
}
