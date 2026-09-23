using UnityEngine;
using UnityEngine.UI;

public class UISliderScript : MonoBehaviour
{

    [SerializeField] script _player;
    [SerializeField] Slider _slider;


    void Reset()
    {
        _slider = GetComponent<Slider>();
        _player = GameObject.FindFirstObjectByType<script>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player.OnDamaged += UpdateSlider;
    }

    void OnDestroy()
    {
        _player.OnDamaged -= UpdateSlider;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateSlider(int currentHP)
    {
        _slider.value = currentHP;
    }
}
