using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{

    [SerializeField]
    private WeaponSwitching weaponChanger;

    [SerializeField]
    private GameObject weaponWheel;
    [SerializeField]
    private GameObject attacks;
    [SerializeField]
    private GameObject weapons;
    [SerializeField]
    private GameObject chips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        toggleChip(WeaponSwitching.Weapon.Fists,"FistChip");
        toggleChip(WeaponSwitching.Weapon.Rapier,"RapierChip");
        toggleChip(WeaponSwitching.Weapon.BroadSword,"BroadSwordChip");
        toggleChip(WeaponSwitching.Weapon.Daggers,"DaggersChip");

        switch(weaponChanger.currentWeapon){
            case WeaponSwitching.Weapon.Fists:
                toggleWeaponWheelOptions("FistSelected");
                toggleAttacks("Fists");
                toggleWeapons("Fists");
                break;
            case WeaponSwitching.Weapon.Rapier:
                toggleWeaponWheelOptions("RapierSelected");
                toggleAttacks("Rapier");
                toggleWeapons("Rapier");
                break;
            case WeaponSwitching.Weapon.BroadSword:
                toggleWeaponWheelOptions("BroadSwordSelected");
                toggleAttacks("BroadSword");
                toggleWeapons("BroadSword");
                break;
            case WeaponSwitching.Weapon.Daggers:
                /*toggleWeaponWheelOptions("DaggersSelected");
                toggleAttacks("Daggers");
                toggleWeapons("Daggers");*/
                break;
            default:
                toggleWeaponWheelOptions("");
                toggleAttacks("");
                toggleWeapons("NoWeapon");
                break;
        }

        
    }



    void toggleChip(WeaponSwitching.Weapon weapon, string name){
        if (weaponChanger.availableWeapons.Contains(weapon)){
            foreach (Transform child in chips.transform){
                if (child.name == name){
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }else{
            foreach (Transform child in chips.transform){
            if (child.name == name){
                child.gameObject.SetActive(false);
                break;
            }
        }
    }
    }

    void toggleWeaponWheelOptions(string name){
       
        foreach (Transform child in weaponWheel.transform){
            if (child.name == name){
                child.gameObject.SetActive(true);
            }else{
                child.gameObject.SetActive(false);
            }
        }
    }

    void toggleAttacks(string name){
       
        foreach (Transform child in attacks.transform){
            if (child.name == name){
                child.gameObject.SetActive(true);
            }else{
                child.gameObject.SetActive(false);
            }
        }
    }

       void toggleWeapons(string name){
       
        foreach (Transform child in weapons.transform){
            if (child.name == name){
                child.gameObject.SetActive(true);
            }else{
                child.gameObject.SetActive(false);
            }
        }
    }
}
