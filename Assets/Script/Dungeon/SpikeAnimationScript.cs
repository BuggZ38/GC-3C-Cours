using NaughtyAttributes;
using UnityEngine;

public class SpikeAnimationScript : MonoBehaviour
{
    [SerializeField] GameObject _spike;
    [SerializeField] PlayerControllerScript _player;

    [ShowNonSerializedField] Vector3 _spikePosition;

    void Reset()
    {
        
    }

    void Awake()
    {
        _spikePosition = new Vector3(0, -2, 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player.OnDamaged += OnDamagePlayer;
    }

    void Update()
    {
        _spike.transform.localPosition = _spikePosition;
    }

    void OnDestroy()
    {
        _player.OnDamaged -= OnDamagePlayer;
    }

    
    void OnDamagePlayer(int health)
    {
        StartCoroutine(SpikeAnimation());
    }


    System.Collections.IEnumerator SpikeAnimation()
    {
        _spikePosition = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(2f);
        _spikePosition = new Vector3(0, -2, 0);
    }

}
