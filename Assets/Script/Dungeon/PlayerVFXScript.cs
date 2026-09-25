using UnityEngine;

public class PlayerVFXScript : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] PlayerControllerScript _playerscript;
    [SerializeField] GameObject _jumpVFX;
    [SerializeField] GameObject _sprintVFX;

    GameObject _instantiatedSprintVFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerscript.EOnJump += OnJump;
        _playerscript.EStartSprint += OnStartSprint;
        _playerscript.EStopSprint += OnStopSprint;
    }

    void OnDestroy()
    {
        _playerscript.EOnJump -= OnJump;
        _playerscript.EStartSprint -= OnStartSprint;
        _playerscript.EStopSprint -= OnStopSprint;
    }

    void OnJump()
    {
        Vector3 vfxPosition = _playerscript.transform.position + Vector3.up * 2f;
        GameObject vfx = Instantiate(_jumpVFX, vfxPosition, Quaternion.identity);

        vfx.SetActive(true);
        vfx.GetComponent<ParticleSystem>().Play();
    }

    void OnStartSprint()
    {
        _instantiatedSprintVFX = Instantiate(_sprintVFX, _player.transform.position, Quaternion.identity);
        
        

        _instantiatedSprintVFX.SetActive(true);
        _instantiatedSprintVFX.GetComponent<ParticleSystem>().Play();
    }

    void OnStopSprint()
    {
        _instantiatedSprintVFX.SetActive(false);
        _instantiatedSprintVFX.GetComponent<ParticleSystem>().Stop();
    }

    void Update()
    {
        if (_instantiatedSprintVFX != null)
        {
            _instantiatedSprintVFX.transform.position = _player.transform.position;
            _instantiatedSprintVFX.transform.forward = -_player.transform.forward;
        }
    }
}
