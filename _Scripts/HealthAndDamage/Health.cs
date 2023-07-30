using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public bool isPlayer;
    private int maxHealth = 100;
    private int health = 100;
    public event Action onDie;
    [field: SerializeField] public Collider ShieldCollider { get; private set; }
    public TextMeshProUGUI healthText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!isPlayer)
        {
            maxHealth = GameGlobalVals.Instance.enemyHealth;
            health = maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(int damage)
    {
        if (health ==0)
        {
            return;
        }

     
        health = Mathf.Max(0, health - damage); //Reduces health to damage, to a minumum of 0
        if (isPlayer)
        {
            UpdateHealthBar();
        }
        
        if (health == 0)
        {
            onDie?.Invoke();
        }
        
        
    }

    public void GiveHealth()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    public void GiveMaxHealth()
    {
        maxHealth += 20;
        UpdateHealthBar();
    }
    public void UpdateHealthBar() {
        healthText.text = "Health:" + health + "/" + maxHealth;
        //healthBarImage.fillAmount = Mathf.Clamp(health /maxHealth, 0, 1f);
    }
}
