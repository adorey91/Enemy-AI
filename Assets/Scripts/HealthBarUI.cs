using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Character character;

    public void Start()
    {
        UpdateHealthBar();
    }

    public void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void OnEnable()
    {
        character.onTakeDamage += UpdateHealthBar;
    }

    public void OnDisable()
    {
        character.onTakeDamage -= UpdateHealthBar;
    }


    void UpdateHealthBar()
    {
        healthFill.fillAmount = (float)character.curHp / (float)character.maxHp;
    }
}
