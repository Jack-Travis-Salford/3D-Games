using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameGlobalVals : MonoBehaviour
{
    public static GameGlobalVals Instance;
    public Health playerHealth;
    public WeaponDamage playerWeaponDamage;
    public bool isGamePaused;
    public bool hasGameStarted;
    public int enemyHealth;
    public int enemyWeaponDamage;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        isGamePaused = false;
        enemyHealth = 100;
        enemyWeaponDamage = 5;
    }

    public void SetGamePaused(bool isPaused)
    {
        isGamePaused = isPaused;
    }

    public void SetGameStarted(bool hasStarted)
    {
        hasGameStarted = hasStarted;
    }

    public void GiveHealth()
    {
        playerHealth.GiveHealth();
    }
    public void GiveMaxHealth()
    {
        playerHealth.GiveMaxHealth();
    }
    public void GiveMoreDamage()
    {
        playerWeaponDamage.IncreaseDamage();
    }
   
}
