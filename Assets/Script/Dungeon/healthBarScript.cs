using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    [SerializeField] PlayerControllerScript _player;
    [SerializeField] Slider _slider;

    [SerializeField] TextMeshProUGUI _deadMessage;


    void Reset()
    {
        _slider = GetComponent<Slider>();
        _player = GameObject.FindFirstObjectByType<PlayerControllerScript>();
    }

    void Awake()
    {
        _slider.maxValue = _player.MaxHP;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player.OnDamaged += UpdateSlider;
        _player.EIsDead += ShowDeadMessage;
        _slider.value = _player.HP;

        _deadMessage.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        _player.OnDamaged -= UpdateSlider;
        _player.EIsDead -= ShowDeadMessage;
    }

    void UpdateSlider(int currentHP)
    {
        _slider.value = currentHP;
    }

    void ShowDeadMessage()
    {
        _deadMessage.gameObject.SetActive(true);
    }
}
