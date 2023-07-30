using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTrigger : MonoBehaviour
{
    //Handles trigger events on Power ups. Informs spawner of said event
    public PowerUpLogic powerUpLogic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            powerUpLogic.HasTriggered();
        }
    }
}
