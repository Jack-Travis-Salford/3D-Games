using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [field: SerializeField] private GameObject weaponLogic;
    [field: SerializeField] private AudioSource WeaponAudioSource;
    [field: SerializeField] private AudioClip WeaponSwing;

    public void EnableWeapon()
    {
        weaponLogic.SetActive(true);
        WeaponAudioSource.PlayOneShot(WeaponSwing);
    }
    public void DisableWeapon()
    {
        weaponLogic.SetActive(false);
    }
}
