using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerUpgrade PlayerUpgrade;
    public PlayerMovement PlayerMovement;
    public PlayerCollision PlayerCollision;
    public PlayerHealth PlayerHealth;

    public static PlayerManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
