using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxPlayerHealth = 10f;
    private float currentPlayerHealth;
    [SerializeField] private Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayerHealth = maxPlayerHealth;
    }

    public void ChangeHealth(float amount)
    {
        currentPlayerHealth += amount;
        UpdateHealthUI();

        if(currentPlayerHealth < 0 )
        {
            //die
            Debug.Log("no helath");
            return;
        }
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = currentPlayerHealth / maxPlayerHealth;
    }
}
