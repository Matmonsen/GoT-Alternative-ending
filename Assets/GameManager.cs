using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _leftPlayerObject;
    [SerializeField] private GameObject _rightPlayerObject;


    private Player _leftPlayer;
    private Player _rightPlayer;


    #region Lifecycle
    void Awake()
    {
        _leftPlayer = _leftPlayerObject.GetComponent<Player>();
        _rightPlayer = _rightPlayerObject.GetComponent<Player>();

        _leftPlayer.FinishedPlayerTurnEvent += name => _rightPlayer.SetTurn();
        _rightPlayer.FinishedPlayerTurnEvent += name => _leftPlayer.SetTurn();
    }

    void Start()
    {
        _leftPlayer.SetTurn();
    }
    #endregion

}
