using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private int _turnEndTimer = 5;

    private PlayerController[] _players = null;
    private PlayerController _activePlayer = null;
    private CameraManager _cameraManager = null;
    private TurnText _turnTextIndicator = null;
    private int _numOfCurrentPlayer = 0;

    private void TurnSwitcher()
    {
        _numOfCurrentPlayer++;
        if (_numOfCurrentPlayer >= _players.Length)
        {
            _numOfCurrentPlayer = 0;
        }
        _activePlayer = _players[_numOfCurrentPlayer];
        string text = _activePlayer.name + "'s turn";
        _turnTextIndicator.SetText(text);
        _activePlayer.SetTurn();

    }
    public void TurnBreak()
    {
        _cameraManager.SetCameraIdle();
        _activePlayer = null;
        Invoke("TurnSwitcher", _turnEndTimer);
    }

    private void Start()
    {
        if (FindObjectOfType<TurnManager>() != this)
        {
            Destroy(gameObject);
        }
        _players = FindObjectsOfType<PlayerController>();
        _cameraManager = FindObjectOfType<CameraManager>();
        _turnTextIndicator = FindObjectOfType<TurnText>();
        foreach (PlayerController player in _players)
        {
            player.SetEndTurnTimer(_turnEndTimer);
        }
        // _activePlayer = _players[_numOfCurrentPlayer];
        // _activePlayer.SetTurn();
        //Debug.Log(_players.Length);
        TurnBreak();
    }
}
