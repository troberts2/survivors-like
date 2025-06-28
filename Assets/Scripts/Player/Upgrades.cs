using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    private PlayerUpgrade pu;

    public void Init(PlayerUpgrade playerUpgrade)
    {
        pu = playerUpgrade;
    }

    public void DamageBoost()
    {
        pu.DamageBoost();
    }

    public void SpeedBoost()
    {
        pu.SpeedBoost();
    }    

    public void HealthBoost()
    {
        pu.HealthBoost();
    }

    public void ArmorBoost()
    {
        pu.ArmorBoost();
    }
}
