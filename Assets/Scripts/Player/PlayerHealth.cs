using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxPlayerHealth = 10f;
    private float _currentPlayerHealth;
    private float _armor = 0f;
    [SerializeField] private Image healthBar;

    public float MaxPlayerHealth { get => _maxPlayerHealth; set => _maxPlayerHealth = value; }
    public float CurrentPlayerHealth { get => _currentPlayerHealth; set => _currentPlayerHealth = value; }
    public float Armor { get => _armor; set => _armor = value; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentPlayerHealth = MaxPlayerHealth;
    }

    //exepct amount to be negative
    public void ChangeHealth(float amount)
    {
        var armorDiff = amount + Armor;
        if(armorDiff <= 0) { 
            CurrentPlayerHealth += armorDiff;
        }
        else
        {
            CurrentPlayerHealth += amount;
        }
        
        UpdateHealthUI();

        if(CurrentPlayerHealth < 0 )
        {
            //die
            Debug.Log("no helath");
            return;
        }
    }

    private void UpdateHealthUI()
    {
        healthBar.fillAmount = CurrentPlayerHealth / MaxPlayerHealth;
    }
}
