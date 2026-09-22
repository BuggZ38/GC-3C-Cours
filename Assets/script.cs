using UnityEngine;
using UnityEngine.InputSystem;

public class script : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] float _speed;
    [SerializeField] Vector3 _direction;
    [SerializeField] InputActionReference _moveInput;


    void Reset()
    {
        _speed = 5f;
        _direction = Vector3.forward;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 direction = _moveInput.action.ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);

        transform.Translate(moveDirection * (_speed * Time.deltaTime));
    }
}
