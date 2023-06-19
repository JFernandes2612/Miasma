using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    [SerializeField]
    private GameObject menu;

    public Weapon currentWeapon = Weapon.None;
    private  Weapon previousSelectedWeapon;
    public List<Weapon> availableWeapons;

    public enum Weapon {Fists, Rapier, BroadSword, Daggers, None};


    public int numberOfWeapons = 0;

    void Awake() {
        numberOfWeapons = NumberOfWeapons();
    }

    void Start()
    {
        updateCurrentWeapon();
    }


    void Update()
    {
        previousSelectedWeapon = currentWeapon;
        updateCurrentWeapon();
        updateAvailableWeapons();
        HandleSelectWheel();
    }


    public void HandleSelectWheel(){
        if (Input.GetKey(KeyCode.F))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            menu.SetActive(true);
            Vector3 pos = Input.mousePosition;

            if (pos.x < 405 && pos.y > 250 ){
                if (availableWeapons.Contains(Weapon.Fists)){
                    currentWeapon = Weapon.Fists;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("fists(Clone)");
                    }
                }
            }else if(pos.x > 405 && pos.x < 450 && pos.y > 250 ){
                 if (availableWeapons.Contains(Weapon.Rapier)){
                    currentWeapon = Weapon.Rapier;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("Rapier");
                    }
                 }
            }else if(pos.x > 450 && pos.y > 250 ){
                if (availableWeapons.Contains(Weapon.BroadSword)){
                    currentWeapon = Weapon.BroadSword;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("BroadSword");
                    }
                }
            }else if(pos.x > 470 && pos.y < 250  && pos.y > 200){
                if (availableWeapons.Contains(Weapon.Daggers)){
                    currentWeapon = Weapon.Daggers;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("Daggers");
                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.F)){
            Cursor.visible = false;
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ResetWeapons() {
        int i = 0;
        foreach (Transform weapon in transform) {
            if (i >= numberOfWeapons)
            {
                Destroy(weapon.gameObject);
            }
            i++;
        }
        availableWeapons = new List<Weapon>();
    }

    public int NumberOfWeapons() {
        int i = 0;
        foreach (Transform weapon in transform) i++;
        return i;
    }

    void updateCurrentWeapon(){
        foreach (Transform weapon in transform)
        {
            if (weapon.gameObject.activeSelf){
                switch(weapon.name){
                    case "fists(Clone)":
                        currentWeapon = Weapon.Fists;
                        if (previousSelectedWeapon != currentWeapon)
                        {
                            SelectWeapon("fists(Clone)");
                        }
                        break;
                    case "Rapier":
                        currentWeapon = Weapon.Rapier;
                        if (previousSelectedWeapon != currentWeapon)
                        {
                            SelectWeapon("Rapier");
                        }
                        break;
                    case "BroadSword":
                        currentWeapon = Weapon.BroadSword;
                        if (previousSelectedWeapon != currentWeapon)
                        {
                            SelectWeapon("BroadSword");
                        }
                        break;
                    case "Dagger":
                        currentWeapon = Weapon.Daggers;
                        if (previousSelectedWeapon != currentWeapon)
                        {
                            SelectWeapon("Daggers");
                        }
                        break;    
                }
            }
        }
    }

    void updateAvailableWeapons(){
        availableWeapons = new List<Weapon>();
        foreach (Transform weapon in transform)
        {
            switch(weapon.name){
                case "fists(Clone)":
                    availableWeapons.Add(Weapon.Fists);
                    break;
                case "Rapier":
                    availableWeapons.Add(Weapon.Rapier);
                    break;
                case "BroadSword":
                    availableWeapons.Add(Weapon.BroadSword);
                    break;
                case "Dagger":
                    availableWeapons.Add(Weapon.Daggers);
                    break;    
            }
        }
    }

    void SelectWeapon(string name)
    {
        foreach (Transform weapon in transform)
        {
            if (weapon.name == name)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }
}
