using UnityEngine;
using UnityEngine.InputSystem;

using Unity.Cinemachine;
using NaughtyAttributes;
using System;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float _speed;
    [SerializeField] float _modelRotationSpeed;
    [SerializeField] float _jumpForce;
    [SerializeField] float _jumpRaycastDistance;
    [ShowNonSerializedField] Vector3 _direction;
    [ShowNonSerializedField] Vector3 _modelDirection;

    [Header ("References")]
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _jumpInput;
    [SerializeField] InputActionReference _sprintInput;
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject _footPosition;
    [SerializeField] GameObject _playerModel;

    [SerializeField] CinemachineCamera _cinemachineCamera;

    // Personnal variables
    [ShowNonSerializedField] bool _isGrounded;
    [ShowNonSerializedField] int _health;
    [SerializeField] int _maxHealth;

    // Health getter
    public int HP => _health;
    public int MaxHP => _maxHealth;

    
    public event Action EStartWalk;
    public event Action EStopWalk;
    public event Action EStartSprint;
    public event Action EStopSprint;

    public event Action EIsDead;


    public event Action<int> OnDamaged;


    void Reset()
    {
        _speed = 5f;
        _modelRotationSpeed = 720f;
        _direction = Vector3.forward;
        _jumpForce = 5f;
        _jumpRaycastDistance = 1f;
        _maxHealth = 10;
    }


    void Awake()
    {
        _moveInput.action.Enable();
        _jumpInput.action.Enable();
        _sprintInput.action.Enable();

        _health = _maxHealth;
    }

    void Start()
    {
        _moveInput.action.started += UpdateMove;
        _moveInput.action.performed += UpdateMove;
        _moveInput.action.canceled += StopMove;

        _sprintInput.action.started += StartSprint;
        _sprintInput.action.canceled += StopSprint;
    }

    void OnDestroy()
    {
        _moveInput.action.started -= UpdateMove;
        _moveInput.action.performed -= UpdateMove;
        _moveInput.action.canceled -= StopMove;

        _sprintInput.action.started -= StartSprint;
        _sprintInput.action.canceled -= StopSprint;
    }

    void UpdateMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        _direction = new Vector3(direction.x, 0, direction.y);
        EStartWalk?.Invoke();
    }

    void StopMove(InputAction.CallbackContext context)
    {
        _direction = Vector3.zero;
        EStopWalk?.Invoke();
    }

    void StartSprint(InputAction.CallbackContext context)
    {
        _speed += 3f;
        EStartSprint?.Invoke();
    }

    void StopSprint(InputAction.CallbackContext context)
    {
        _speed -= 3f;
        EStopSprint?.Invoke();
    }

    void Update()
    {
        // Check _isGrounded
        _isGrounded = Physics.Raycast(_footPosition.transform.position, Vector3.down, _jumpRaycastDistance);
        Debug.DrawRay(_footPosition.transform.position, Vector3.down * _jumpRaycastDistance, Color.red);

        // Move
        transform.Translate(_direction * (_speed * Time.deltaTime));

        if (_playerModel != null && _direction.sqrMagnitude > 0f)
        {
            _modelDirection = _direction.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(_modelDirection);
            _playerModel.transform.localRotation = Quaternion.RotateTowards(
                _playerModel.transform.localRotation,
                targetRotation,
                _modelRotationSpeed * Time.deltaTime);
        }

        // Jump
        if (_jumpInput.action.WasPressedThisFrame() && _isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collied with: " + col.gameObject.name);

        var gt = col.gameObject.GetComponent<SpikedFloorsTag>();
        
        if (gt != null)
        {
            gettingDamaged(1);
        }
    }


    void gettingDamaged(int health)
    {
        _health -= health;

        OnDamaged?.Invoke(_health);

        if (_health <= 0)
        {
            Death();
        }
    }


    void Death()
    {
        _moveInput.action.Disable();
        _jumpInput.action.Disable();
        _sprintInput.action.Disable();

        EIsDead?.Invoke();
        Debug.Log("GameOver");
    }

}
