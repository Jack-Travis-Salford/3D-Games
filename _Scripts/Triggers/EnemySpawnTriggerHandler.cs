using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class EnemySpawnTriggerHandler : MonoBehaviour
{
    [field: SerializeField] public GameObject _spawnPoint { set; private get; }
    [field: SerializeField] public GameObject _enemy { set; private get; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            
            GameObject newObject = Instantiate(_enemy, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
                
            
            
           
        }
    }
}
