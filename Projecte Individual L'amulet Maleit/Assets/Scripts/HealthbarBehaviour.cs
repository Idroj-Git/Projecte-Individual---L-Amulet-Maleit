using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 offset;
    public Canvas canvas;

    private void Start()
    {
        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
        }

        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
    }

    public void SetHealth(float health, float maxHealth)
    {
        slider.gameObject.SetActive(true); // test amb health < maxHealth per veure com queda
        slider.maxValue = maxHealth;
        slider.value = health;

        Image fillImage = slider.fillRect.GetComponentInChildren<Image>();
        if (fillImage != null)
        {
            Debug.Log("Fill image found: " + fillImage.name);
            fillImage.color = Color.Lerp(low, high, slider.normalizedValue);
        }
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    void Update()
    {
        slider.transform.position = transform.parent.position + offset;
    }
}
