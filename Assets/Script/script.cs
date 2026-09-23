using UnityEngine;
using UnityEngine.InputSystem;

using Unity.Cinemachine;
using NaughtyAttributes;
using System;


public class script : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] float _speed;
    [SerializeField] Vector3 _direction;
    [SerializeField] float _jumpForce;
    [SerializeField] float _jumpRaycastDistance;

    [Header ("References")]
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _jumpInput;
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject _footPosition;

    [SerializeField] CinemachineCamera _cinemachineCamera;

    // Personnal variables
    bool _isGrounded;
    [ShowNonSerializedField] int _health;
    [SerializeField] int _maxHealth;

    [SerializeField] Vector3 _joystickDirection;


    public int HP => _health;

    public event Action<int> OnDamaged;


    // Camera variables
    Vector3 _cameraDirection;



    void Reset()
    {
        
        _speed = 5f;
        _direction = Vector3.forward;
        _jumpForce = 5f;
        _jumpRaycastDistance = 1f;
        _maxHealth = 10;
    }

    void Awake()
    {
        _moveInput.action.Enable();
        _jumpInput.action.Enable();

        _health = _maxHealth;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Action<int, float, string> a;
        // Func<UnityEngine.Object, Vector3, Quaternion, UnityEngine.Object> f = Instantiate;

        // event started : au debut de l'action
        // event performed : au changement de l'action
        // event canceled : a la fin de l'action
        _moveInput.action.performed += UpdateMove;
        _moveInput.action.canceled += StopMove;
    }

    void UpdateMove(InputAction.CallbackContext context)
    {
        // Move
        Vector2 direction = context.ReadValue<Vector2>();

        _joystickDirection = new Vector3(direction.x, 0, direction.y);

        
    }

    void StopMove(InputAction.CallbackContext context)
    {
        _joystickDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Check _isGrounded
        _isGrounded = Physics.Raycast(_footPosition.transform.position, Vector3.down, _jumpRaycastDistance);
        Debug.DrawRay(_footPosition.transform.position, Vector3.down * _jumpRaycastDistance, Color.red);


        transform.Translate(_joystickDirection * (_speed * Time.deltaTime));

        // Get camera direction and set player forward direction
        _cameraDirection = _cinemachineCamera.transform.forward;
        _cameraDirection.y = 0;

        transform.forward = _cameraDirection;

        // Jump
        if (_jumpInput.action.WasPressedThisFrame() && _isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        var gt = collision.gameObject.GetComponent<GroundTag>();

        if (gt != null)
        {
            Debug.Log("GameOver");

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        if (collision.gameObject.TryGetComponent<piegeTag>(out var piegeTag))
        {
            _health--;
            OnDamaged?.Invoke(_health);

            if (_health <= 0)
            {
                Debug.Log("GameOver");

                Destroy(gameObject);
            }
        }

    }
}
