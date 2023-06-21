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
    public GameObject currentWeaponObject;

    public enum Weapon {Fists, Rapier, BroadSword, Daggers, None};


    public int numberOfWeapons = 0;

    void Start()
    {
        menu.SetActive(false);
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

            float middleX = (Screen.width / 2);
            float middleY = (Screen.height / 2);

            if (pos.x < middleX - 30 && pos.y > middleY + 25 ){
                if (availableWeapons.Contains(Weapon.Fists)){
                    currentWeapon = Weapon.Fists;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("fists(Clone)");
                    }
                }
            }else if(pos.x > middleX - 30 && pos.x < middleX + 30 && pos.y > middleY + 25 ){
                 if (availableWeapons.Contains(Weapon.Rapier)){
                    currentWeapon = Weapon.Rapier;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("rapier(Clone)");
                    }
                 }
            }else if(pos.x > middleX + 30 && pos.y > middleY + 25 ){
                if (availableWeapons.Contains(Weapon.BroadSword)){
                    currentWeapon = Weapon.BroadSword;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("broadsword(Clone)");
                    }
                }
            }else if(pos.x > middleX + 30 && pos.y < middleY + 25  && pos.y > middleY - 25){
                if (availableWeapons.Contains(Weapon.Daggers)){
                    currentWeapon = Weapon.Daggers;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("daggers(Clone)");
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
                currentWeaponObject = weapon.gameObject;
                switch(weapon.name){
                    case "fists(Clone)":
                        currentWeapon = Weapon.Fists;
            
                        break;
                    case "rapier(Clone)":
                        currentWeapon = Weapon.Rapier;
               
                        break;
                    case "broadsword(Clone)":
                        currentWeapon = Weapon.BroadSword;
          
                        break;
                    case "daggers(Clone)":
                        currentWeapon = Weapon.Daggers;
      
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
                case "rapier(Clone)":
                    availableWeapons.Add(Weapon.Rapier);
                    break;
                case "broadsword(Clone)":
                    availableWeapons.Add(Weapon.BroadSword);
                    break;
                case "daggers(Clone)":
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
