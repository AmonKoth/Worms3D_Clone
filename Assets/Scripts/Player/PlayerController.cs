using System.Collections;
using System.Collections.Generic;
using System;
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
    [SerializeField]
    private Color _colorOfWorms = Color.yellow;

    private Vector2 _moveDirection = new Vector2(0, 0);
    private Vector2 _lookDirection = new Vector2(0, 0);
    private Vector2 _lastLookDir;
    //Worms
    //private Worm[] worms = null;
    private Worm _activeWorm = null;
    private int _activeWormNumber = 0;
    private List<Worm> _worms = new List<Worm>();

    //Turn
    private TurnManager _turnManager = null;
    private bool _isTurn = false;
    private int _endTurnTimer = 1;

    public int GetEndTurnTimer() => _endTurnTimer;
    public void SetEndTurnTimer(int endTurn) { _endTurnTimer = endTurn; }
    public bool GetTurn() => _isTurn;

    private WeaponWheel _weaponWheel = null;
    //Events
    public event Action<Color> HandleColor = delegate { };
    public event Action<String> SwitchingWorm = delegate { };

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
    public void OnWeaponMenu(InputAction.CallbackContext context)
    {
        if (context.performed && _isTurn && _activeWorm.GetIsActive() && !_activeWorm.GetIsFired())
        {
            _weaponWheel.gameObject.SetActive(!_weaponWheel.gameObject.activeSelf);
        }
    }

    public void WeaponSwitch(AmmoType switchedAmmoType)
    {
        _activeWorm.SetActiveWeapon(switchedAmmoType);
    }

    //Worms
    private void ActiveWormChanger()
    {
        _activeWorm.SetIsActive(false);
        _activeWormNumber++;
        if (_activeWormNumber == _worms.Count)
        {
            _activeWormNumber = 0;
        }
        //Debug.Log(_worms.Count);
        //Debug.Log($"Active worm number" + _activeWormNumber);
        _activeWorm = _worms[_activeWormNumber];
        _activeWorm.SetIsActive(true);
        _activeWorm.OnHazardCollision += HazardDeath;
        SwitchingWorm(_activeWorm.transform.name);

    }

    public void SetTurn()
    {
        _isTurn = true;
        if (_activeWorm.GetIsDead())
        {
            ActiveWormChanger();
        }
        //Debug.Log(_activeWorm.transform.name);
        _activeWorm.TurnStart();
        _activeWorm.OnHazardCollision += HazardDeath;
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
    private void Initializer()
    {
        //worms = GetComponentsInChildren<Worm>();
        foreach (Worm worm in GetComponentsInChildren<Worm>())
        {
            _worms.Add(worm);
        }
        _activeWorm = _worms[_activeWormNumber];
        _activeWorm.SetIsActive(true);
        _turnManager = FindObjectOfType<TurnManager>();
        HandleColor(_colorOfWorms);
        _weaponWheel = FindObjectOfType<WeaponWheel>();
    }

    private void HazardDeath(bool death)
    {
        if (death)
        {
            UpdateList();
            //Debug.Log(_worms.Count);
            if (_isTurn)
            {
                EndTurn();
            }
        }
    }

    private void UpdateList()
    {
        _worms.Clear();
        _activeWormNumber = 0;
        foreach (Worm worm in GetComponentsInChildren<Worm>())
        {
            if (!worm.GetIsDead())
            {
                _worms.Add(worm);
            }
        }
    }

    private void Start()
    {
        Initializer();
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
