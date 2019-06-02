using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //[SerializeField]
    //private Image _door;

    private Button _button;
    private Image _backgroundImage;
    //private Image _flag1;
    // private Image _flag2;

    private float _animationTime;

    private float _timeSinceLastChange;
    private float _changeRate = .01f;
    private float _camZoomSpeed = 1.1f;
    private bool _pressedStartGame;
    private bool _runSaveAnimation = false;

    private Vector3 _maxSize;
    private Vector3 _endPosition;

    void Awake()
    {
        _timeSinceLastChange = 0;

        _button = GameObject.Find("StartGameButton").GetComponent<Button>();
        _button.onClick.AddListener(StartGame);

        _backgroundImage = GameObject.Find("Image").GetComponent<Image>();
        _maxSize = _backgroundImage.transform.localScale * 7f;

        _endPosition = _backgroundImage.transform.position + (Vector3.down * -1450f); //TODO: Bør ikke være hardkodet, men relativ til _backgroundImage-størrelsen

        //_animationTime = 3f;
    }

    void StartGame()
    {
        _pressedStartGame = true;
        _button.gameObject.SetActive(false);
        // TODO: Create generic hide method
        //  _flag1 = GameObject.Find("Flag1").GetComponent<Image>();
        //  _flag1.gameObject.SetActive(false);
        // _flag2 = GameObject.Find("Flag2").GetComponent<Image>();
        //_flag2.gameObject.SetActive(false);

        _runSaveAnimation = true;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_pressedStartGame)
            _timeSinceLastChange += Time.deltaTime;
        
        if (_runSaveAnimation)
        {
            _backgroundImage.transform.localScale =
                Vector3.Lerp(_backgroundImage.transform.localScale, _maxSize, Time.deltaTime/4);

            _backgroundImage.transform.position =
                Vector3.Lerp(_backgroundImage.transform.position, _endPosition, Time.deltaTime/4);
        }

        if (_timeSinceLastChange > 2)
            SceneManager.LoadScene(SceneController.Scene_Game);



    }
}
