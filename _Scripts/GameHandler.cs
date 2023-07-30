using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    //Controls the waves of enemies in a level
    
    private List<EnemySpawnLogic> EnemySpawns = new List<EnemySpawnLogic>();
    private List<EnemyStateMachine> DeadEnemies = new List<EnemyStateMachine>();
    public GameObject EnemySpawnsHolder;
    private bool roundActive = false;
    private bool playerIsDead;
    private bool allEnemiesSpawned = false;
    private int roundNo;
    private int enemiesToSpawn;
    private int currentEnemies;

    public TextMeshProUGUI txtWaveNo;
    public TextMeshProUGUI txtEnemiesLeft;
    public TextMeshProUGUI txtHelp;
    public Button btnReturn;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        foreach (EnemySpawnLogic spawnLogic in EnemySpawnsHolder.GetComponentsInChildren<EnemySpawnLogic>())
        {
            EnemySpawns.Add(spawnLogic);
        }

        roundNo = 0;
    }
    
    void Update()
    {
        if (DeadEnemies.Count > 15)
        {
            EnemyStateMachine state = DeadEnemies[0];
            DeadEnemies.RemoveAt(0);
            Destroy(state.EnemyRig);
        }
        if (playerIsDead)
        {
            return;
        }
        if (!GameGlobalVals.Instance.hasGameStarted || GameGlobalVals.Instance.isGamePaused)
        {
            return;
        }
        if (!roundActive)
        {
            BeginRound();
            roundActive = true;
        }
        if (allEnemiesSpawned)
        {
            return;
        }

        int SpawnPoint = Random.Range(0, EnemySpawns.Count);
        EnemySpawns[SpawnPoint].Spawn();
        enemiesToSpawn--;
        if (enemiesToSpawn == 0)
        {
            allEnemiesSpawned = true;
        }
            
        
        
    }

    private void BeginRound()
    {
        roundNo++;
        PrepareRound();
        txtWaveNo.text = "Current Wave:" + roundNo;
        txtEnemiesLeft.text = "Enemies Left:" + currentEnemies;
        
    }
    //Sets how many enemies to spawn, applies stat boosts
    private void PrepareRound()
    {
        allEnemiesSpawned = false;
        switch (roundNo)
        {
            case 1:
                enemiesToSpawn = 1;
                break;
            case 2:case 3:
                enemiesToSpawn = 2;
                break;
            case 4:
                enemiesToSpawn = 3;
                break;
            case 5:
                enemiesToSpawn = 4;
                break;
            case 6: case 7:
                enemiesToSpawn = 5;
                break;
            case 8:
                enemiesToSpawn = 7;
                break;
            case 9:
                enemiesToSpawn = 9;
                break;
            default:
                enemiesToSpawn = 10;
                break;
        }
        currentEnemies = enemiesToSpawn;
        switch (roundNo)
        {
            case 1:
                break;
            case 2: case 3:
                GameGlobalVals.Instance.enemyWeaponDamage += 1;
                break;
            case 4:case 5: case 6:
                GameGlobalVals.Instance.enemyWeaponDamage += 2;
                break;
            case 7: case 8: case 9:case 10:
                GameGlobalVals.Instance.enemyWeaponDamage += 3;
                break;
            case >10 and <=15:
                Debug.Log("ran");
                GameGlobalVals.Instance.enemyWeaponDamage += 5;
                GameGlobalVals.Instance.enemyHealth += 20;
                break;
            case >15 and <=20:
                GameGlobalVals.Instance.enemyWeaponDamage += 5;
                GameGlobalVals.Instance.enemyHealth += 50;
                break;
            default:
                GameGlobalVals.Instance.enemyWeaponDamage += 10;
                GameGlobalVals.Instance.enemyHealth += 50;
                break;
        }
    }

    public void ReportEnemyDead(EnemyStateMachine enemyStateMachine)
    {
        DeadEnemies.Add(enemyStateMachine);
        currentEnemies--;
        txtEnemiesLeft.text = "Enemies Left:" + currentEnemies;
        if (currentEnemies == 0)
        {
            roundActive = false;
        }
    }

    public void PlayerIsDead()
    {
        playerIsDead = true;
        txtHelp.text = "You survived " + (roundNo - 1) + " rounds!";
        txtHelp.enabled = true;
        btnReturn.enabled = true;
        btnReturn.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

}
