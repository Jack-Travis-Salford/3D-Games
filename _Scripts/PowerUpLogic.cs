using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUpLogic : MonoBehaviour
{
    //references to the 3 prefabs for power ups
    public GameObject health;

    public GameObject maxHealth;

    public GameObject attackDamage;

    public GameObject spawnedObject;
    //Audio source of Power up spawner
    [field: SerializeField]public AudioSource AudioSource { get; private set; }

    public int chosenPowerup;

    public bool objectSpawned;

    public float timeUntilSpawn;

    public float passedTime;
    // Start is called before the first frame update
    void Start()
    {
        timeUntilSpawn = Random.Range(30, 181);

    }
    void Update()
    {
        if (!GameGlobalVals.Instance.hasGameStarted)
        {
            return;
        }

        if (GameGlobalVals.Instance.isGamePaused)
        {
            return;
        }

        if (objectSpawned)
        {
            return;
        }

        passedTime += Time.deltaTime;
        if (passedTime >= timeUntilSpawn)
        {
            int chosenPowerup = Random.Range(0, 6);
            GameObject chosenObject;
            switch (chosenPowerup)
            {
                case 1:
                    chosenObject = maxHealth;
                    this.chosenPowerup = 0;
                    break;
                case 2:
                    chosenObject = attackDamage;
                    this.chosenPowerup = 1;
                    break;
                default:
                    chosenObject = health;
                    this.chosenPowerup = 2;
                    break;
            }

            Vector3 spawnPos = transform.position;
            spawnPos.y += 1;
            spawnedObject = Instantiate(chosenObject, spawnPos, transform.rotation);
            PowerUpTrigger trigger = spawnedObject.GetComponentInChildren<PowerUpTrigger>();
            trigger.powerUpLogic = this;
            objectSpawned = true;
        }
    }

    public void HasTriggered()
    {
        if (chosenPowerup == 2)
        {
            GameGlobalVals.Instance.GiveHealth();
        }else if (chosenPowerup == 0)
        {
            GameGlobalVals.Instance.GiveMaxHealth();
        }else if (chosenPowerup == 1)
        {
            GameGlobalVals.Instance.GiveMoreDamage();
        }
        AudioSource.Play();
        Destroy(spawnedObject);
        objectSpawned = false;
        spawnedObject = null;
        timeUntilSpawn = Random.Range(30, 300);
        passedTime = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
    }
}
