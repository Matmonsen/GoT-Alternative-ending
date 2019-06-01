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
        
    }

    void Start()
    {
        _leftPlayer.SetTurn();
    }
    #endregion

    public void FinishedTurn(string name)
    {
        if (name.Equals(_leftPlayerObject.name))
            _rightPlayer.SetTurn();
        else
            _leftPlayer.SetTurn();
    }

}
