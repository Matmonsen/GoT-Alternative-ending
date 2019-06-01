using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject _playerLeft;
        [SerializeField] private GameObject _playerRight;
        public GameObject CurrentPlayer { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            CurrentPlayer = _playerLeft;
            CurrentPlayer.GetComponent<Player>().SetTurn();
        }

        public IEnumerator TurnOver(string playerName)
        {
            yield return new WaitForSeconds(.5f);
            CurrentPlayer = playerName == _playerLeft.name ? _playerRight : _playerLeft;
            CurrentPlayer.GetComponent<Player>().SetTurn();
        }
    }
}
