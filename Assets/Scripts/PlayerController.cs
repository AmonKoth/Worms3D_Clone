using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Input
    [SerializeField]
    private float _xSensitivity = 0.2f;
    [SerializeField]
    private float _ySensitivity = 0.2f;
    [SerializeField]
    private float _xClamp = 45.0f;


    private Vector2 _moveDirection = new Vector2(0, 0);
    private Vector2 _lookDirection = new Vector2(0, 0);
    private Vector2 _lastLookDir;
    //Worms
    private Worm[] worms = null;
    private Worm _activeWorm = null;
    private int _activeWormNumber = 0;
    private int _AliveWorms = 0;
    //Turn
    private TurnManager _turnManager = null;
    private bool _isTurn = false;
    private int _endTurnTimer = 1;

    public int GetEndTurnTimer() => _endTurnTimer;
    public void SetEndTurnTimer(int endTurn) { _endTurnTimer = endTurn; }
    private bool GetTurn() => _isTurn;

    //Handle Inputs
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _lookDirection = context.ReadValue<Vector2>();
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isTurn && _activeWorm.GetIsActive() && !_activeWorm.GetIsFired())
            {
                _activeWorm.Fire();
                Invoke("EndTurn", _endTurnTimer);
            }
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isTurn)
            {
                _activeWorm.Jump();
            }
        }
    }
    public void OnWormSwitch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_isTurn && _activeWorm.GetIsActive() && !_activeWorm.GetIsFired())
            {
                ActiveWormChanger();
            }
        }
    }

    private void ActiveWormChanger()
    {
        _activeWorm.SetIsActive(false);
        _activeWormNumber++;
        if (_activeWormNumber == worms.Length)
        {
            _activeWormNumber = 0;
        }
        _activeWorm = worms[_activeWormNumber];
        _activeWorm.SetIsActive(true);
    }

    public void SetTurn()
    {
        _isTurn = true;
        _activeWorm.TurnStart();
    }
    private void EndTurn()
    {
        _activeWorm.SetIsActive(false);
        _isTurn = false;
        _turnManager.TurnBreak();
    }

    private void HandleAim()
    {
        _lastLookDir += new Vector2(-_lookDirection.y * _ySensitivity, _lookDirection.x * _xSensitivity);
        _lastLookDir.x = Mathf.Clamp(_lastLookDir.x, -_xClamp, _xClamp);
        _activeWorm.Aim(_lastLookDir);
    }

    private void Start()
    {
        worms = GetComponentsInChildren<Worm>();
        _activeWorm = worms[_activeWormNumber];
        _activeWorm.SetIsActive(true);
        _AliveWorms = worms.Length;
        _turnManager = FindObjectOfType<TurnManager>();

    }

    private void Update()
    {
        if (_isTurn)
        {
            _activeWorm.HandleMovement(_moveDirection);
            HandleAim();
        }
    }
}
