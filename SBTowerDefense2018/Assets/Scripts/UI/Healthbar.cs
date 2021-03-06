﻿using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public GameObject healthbarPrefab;

    private Image healthbarFill;

    private RectTransform rectTransform;

    public void UpdateHealthbar(int currentHealth, int maxHealth)
    {
        healthbarFill.fillAmount = (float) currentHealth / maxHealth;
    }

    private bool vissible = true;
    public bool Vissible
    {
        get { return vissible; }
        set
        {
            vissible = value;
            rectTransform.gameObject.SetActive(vissible);
        }
    }

    public void RemoveHealthbar()
    {
        Destroy(rectTransform.gameObject);
    }

    private void Awake()
    {
        //Debug.Log("Awake called!");
        GameObject canvas = GameObject.Find("HealthBars");
        if (canvas == null)
            Debug.Log("Canvas not found!");
        rectTransform = Instantiate(healthbarPrefab, GetScreenPosition(), Quaternion.identity).GetComponent<RectTransform>();
        rectTransform.SetParent(canvas.transform);
        healthbarFill = rectTransform.GetChild(0).GetComponent<Image>();
        if (healthbarFill == null)
            Debug.Log("HEALTHBARFILL");
        //Debug.Log(healthbarFill.transform.parent.name);
    }

    private void Update()
    {
        if (!Vissible)
            return;
        Vector3 vec = GetScreenPosition();
        rectTransform.position = vec;
    }

    private Vector3 GetScreenPosition()
    {
        //Debug.Log("UPDATE");
        return Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
    }
}
