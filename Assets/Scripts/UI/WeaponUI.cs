using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WeaponUI : MonoBehaviour
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
    [SerializeField]
    private GameObject LMBCoolDown;
    [SerializeField]
    private GameObject LMBCombo;

    [SerializeField]
    private GameObject RMBCoolDown;

      [SerializeField]
    private GameObject LMBCharging;

    private WeaponSwitching.Weapon lastWeapon;

    // Start is called before the first frame update
    void Start()
    {
        LMBCoolDown.SetActive(false);
        RMBCoolDown.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(lastWeapon != weaponChanger.currentWeapon){
            LMBCoolDown.GetComponent<Slider>().value = 0;
            LMBCoolDown.SetActive(false);
            RMBCoolDown.GetComponent<Slider>().value = 0;
            RMBCoolDown.SetActive(false);
            LMBCombo.SetActive(false);
            LMBCharging.SetActive(false);
            LMBCharging.GetComponent<Slider>().value = 0;

            lastWeapon = weaponChanger.currentWeapon;
        }

        updateLMBCooldown();
        updateRMBCooldown();
        updateIcons();
    }

    void updateLMBCooldown(){

         switch(weaponChanger.currentWeapon){
            case WeaponSwitching.Weapon.Fists:
                FistAttack script = weaponChanger.currentWeaponObject.GetComponent<FistAttack>();
                if (script == null) return;
                if (script.isLMBCooldown()){
                    if (LMBCoolDown.GetComponent<Slider>().value == 0 || !LMBCoolDown.activeSelf){
                        LMBCoolDown.GetComponent<Slider>().value = 1;
                        LMBCoolDown.SetActive(true);
                    }
                    else{
                        
                        LMBCoolDown.GetComponent<Slider>().value = script.getLMBCooldown();
                    }

                }else{
                    LMBCoolDown.GetComponent<Slider>().value = 0;
                    LMBCoolDown.SetActive(false);
                }
                break;
            case WeaponSwitching.Weapon.Rapier:
                RapierAttack scriptRapier= weaponChanger.currentWeaponObject.GetComponent<RapierAttack>();
                if (scriptRapier == null) return;
                if (scriptRapier.isLMBCooldown()){
                    LMBCombo.SetActive(true);
                    LMBCombo.GetComponent<TMPro.TextMeshProUGUI>().text = scriptRapier.getAttackPhase() + "X";
                    if (LMBCoolDown.GetComponent<Slider>().value == 0 || !LMBCoolDown.activeSelf){
                        LMBCoolDown.GetComponent<Slider>().value = 1;
                        LMBCoolDown.SetActive(true);
                    }
                    else{
                        
                        LMBCoolDown.GetComponent<Slider>().value -= Time.deltaTime / scriptRapier.getLMBCooldown();
                    }
                }else{
                    LMBCoolDown.GetComponent<Slider>().value = 0;
                    LMBCoolDown.SetActive(false);
                    LMBCombo.SetActive(false);
                }
                break;
            case WeaponSwitching.Weapon.BroadSword:
                break;
            case WeaponSwitching.Weapon.Daggers:
                DaggerAttack scriptDagger = weaponChanger.currentWeaponObject.GetComponent<DaggerAttack>();
                if (scriptDagger == null) return;
                if (scriptDagger.isCharging()){
                    if (LMBCharging.GetComponent<Slider>().value == 0 || !LMBCharging.activeSelf){
                        LMBCharging.GetComponent<Slider>().value = 1;
                        LMBCharging.SetActive(true);
                    }
                    else{
                        if (scriptDagger.getChargeUp() < 1){
                            LMBCharging.GetComponent<Slider>().value = scriptDagger.getChargeUp();
                        }
                    }

                }else{
                    LMBCharging.GetComponent<Slider>().value = 0;
                    LMBCharging.SetActive(false);
                }
                break;
            default:
                break;
        }
       
    }


    void updateRMBCooldown(){
         switch(weaponChanger.currentWeapon){
            case WeaponSwitching.Weapon.Fists:
                break;
            case WeaponSwitching.Weapon.Rapier:
                RapierAttack scriptRapier = weaponChanger.currentWeaponObject.GetComponent<RapierAttack>();
                if (scriptRapier == null) return;
                if (scriptRapier.isRMBCooldown()){
                    if (RMBCoolDown.GetComponent<Slider>().value == 0 || !RMBCoolDown.activeSelf){
                        RMBCoolDown.GetComponent<Slider>().value = 1;
                        RMBCoolDown.SetActive(true);
                    }
                    else{
                        
                        RMBCoolDown.GetComponent<Slider>().value = scriptRapier.getRMBCooldown();
                    }

                }else{
                    RMBCoolDown.GetComponent<Slider>().value = 0;
                    RMBCoolDown.SetActive(false);
                }
                break;
            case WeaponSwitching.Weapon.BroadSword:

                break;
            case WeaponSwitching.Weapon.Daggers:
                DaggerAttack scriptDagger = weaponChanger.currentWeaponObject.GetComponent<DaggerAttack>();
                if (scriptDagger == null) return;
                if (scriptDagger.isRMBCooldown()){
                    if (RMBCoolDown.GetComponent<Slider>().value == 0 || !RMBCoolDown.activeSelf){
                        RMBCoolDown.GetComponent<Slider>().value = 1;
                        RMBCoolDown.SetActive(true);
                    }
                    else{
                        
                        RMBCoolDown.GetComponent<Slider>().value = scriptDagger.getRMBCooldown();
                    }

                }else{
                    RMBCoolDown.GetComponent<Slider>().value = 0;
                    RMBCoolDown.SetActive(false);
                }
                break;
            default:
        
                break;
        }
       
    }


    void updateChips(){
        toggleChip(WeaponSwitching.Weapon.Fists,"FistChip");
        toggleChip(WeaponSwitching.Weapon.Rapier,"RapierChip");
        toggleChip(WeaponSwitching.Weapon.BroadSword,"BroadSwordChip");
        toggleChip(WeaponSwitching.Weapon.Daggers,"DaggersChip");
    }

    void updateIcons(){
        updateChips();

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
                toggleWeaponWheelOptions("DaggersSelected");
                toggleAttacks("Daggers");
                toggleWeapons("Daggers");
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
                if (child.name != "LMB"&& child.name != "RMB"){
                child.gameObject.SetActive(false);}
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
