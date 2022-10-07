using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private int _turnEndTimer = 5;

    private PlayerController[] _players = null;
    private PlayerController _activePlayer = null;
    private CameraManager _cameraManager = null;
    private TurnText _turnTextIndicator = null;
    private AmmoText _ammoText = null;

    private int _numOfCurrentPlayer = 0;
    private bool _gameFinished = false;

    public event Action<bool> TurnPassed = delegate { };

    private void TurnSwitcher()
    {
        if (!_gameFinished)
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
    }
    public void TurnBreak()
    {
        _cameraManager.SetCameraIdle();
        _activePlayer = null;
        Invoke("TurnSwitcher", _turnEndTimer);
        TurnPassed(true);
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
        _ammoText = FindObjectOfType<AmmoText>();
        foreach (PlayerController player in _players)
        {
            player.SetEndTurnTimer(_turnEndTimer);
            player.IJustLost += GameManager;
        }
        // _activePlayer = _players[_numOfCurrentPlayer];
        // _activePlayer.SetTurn();
        //Debug.Log(_players.Length);
        TurnBreak();
    }

    private void GameManager(string loser)
    {
        _turnTextIndicator.SetText(loser + " Lost the game");
        _gameFinished = true;
        Invoke("RestartScene", 5.0f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
