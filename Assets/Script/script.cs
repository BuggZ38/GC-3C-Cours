using UnityEngine;
using UnityEngine.InputSystem;


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

    bool _isGrounded;


    void Reset()
    {
        
        _speed = 5f;
        _direction = Vector3.forward;
        _jumpForce = 5f;
        _jumpRaycastDistance = 1f;
    }

    void Awake()
    {
        _moveInput.action.Enable();
        _jumpInput.action.Enable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.Raycast(_footPosition.transform.position, Vector3.down, _jumpRaycastDistance);
        Debug.DrawRay(_footPosition.transform.position, Vector3.down * _jumpRaycastDistance, Color.red);

        Vector2 direction = _moveInput.action.ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);

        transform.Translate(moveDirection * (_speed * Time.deltaTime));


        if (_jumpInput.action.WasPressedThisFrame() && _isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Collided with: " + collision.gameObject.name);

    //     var gt = collision.gameObject.GetComponent<GroundTag>();

    //     if (gt != null)
    //     {
    //         Debug.Log("GameOver");

    //         #if UNITY_EDITOR
    //         UnityEditor.EditorApplication.isPlaying = false;
    //         #endif
    //     }

        // if (collision.gameObject.TryGetComponent<GroundTag>(out var groundTag))
        // {
        //     Debug.Log("GameOver");

        //     #if UNITY_EDITOR
        //     UnityEditor.EditorApplication.isPlaying = false;
        //     #endif
        // }

    // }
}
