using UnityEngine;

public class knightAnimationScript : MonoBehaviour
{
    [SerializeField] PlayerControllerScript _player;

    [SerializeField] Animator _animator;


    void Reset()
    {
        _player = GameObject.FindFirstObjectByType<PlayerControllerScript>();
        _animator = GetComponent<Animator>();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player.EStartWalk += OnWalk;
        _player.EStopWalk += OnStopWalk;
        _player.EStartSprint += OnSprint;
        _player.EStopSprint += OnStopSprint;
        _player.EIsDead += OnDead;
    }

    void OnDestroy()
    {
        _player.EStartWalk -= OnWalk;
        _player.EStopWalk -= OnStopWalk;
        _player.EStartSprint -= OnSprint;
        _player.EStopSprint -= OnStopSprint;
    }

    void OnWalk()
    {
        _animator.SetBool("isWalking", true);
    }

    void OnSprint()
    {
        _animator.SetBool("isSprinting", true);
    }

    void OnStopSprint()
    {
        _animator.SetBool("isSprinting", false);
    }

    void OnStopWalk()
    {
        _animator.SetBool("isWalking", false);
    }

    void OnDead()
    {
        _animator.SetTrigger("isDead");
    }
}
