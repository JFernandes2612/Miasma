using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject warningObject;

    public WeaponEnum currentWeapon = WeaponEnum.None;
    private WeaponEnum previousSelectedWeapon;
    public List<WeaponEnum> availableWeapons;
    public GameObject currentWeaponObject;

    private Movement playerMovement;

    public enum WeaponEnum { Fists, Rapier, BroadSword, Daggers, None };


    public int numberOfWeapons = 0;

    void Start()
    {
        menu.SetActive(false);
        updateCurrentWeapon();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
    }


    void Update()
    {
        previousSelectedWeapon = currentWeapon;
        updateCurrentWeapon();
        updateAvailableWeapons();
        HandleSelectWheel();
    }

    public void disableCurrentWeapon()
    {
        if (currentWeaponObject != null)
        {
            Weapon weaponScript = currentWeaponObject.GetComponent<Weapon>();
            if (weaponScript != null) weaponScript.disableScript();
        }
    }

    public void enableCurrentWeapon()
    {
        if (currentWeaponObject != null)
        {
            Weapon weaponScript = currentWeaponObject.GetComponent<Weapon>();
            if (weaponScript != null) weaponScript.enableScript();
        }
    }



    public void HandleSelectWheel()
    {


        Weapon weaponScript = currentWeaponObject.GetComponent<Weapon>();
        if (weaponScript != null && !weaponScript.getIsAttacking() && warningObject.activeSelf){
            warningObject.SetActive(false);
            return;
        }


        if (Input.GetKey(KeyCode.F))
        {
            if (weaponScript.getIsAttacking() ){
                warningObject.SetActive(true);
                return;
            }

            warningObject.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            menu.SetActive(true);
            Vector3 pos = Input.mousePosition;

            float middleX = (Screen.width / 2);
            float middleY = (Screen.height / 2);

            if (pos.x < middleX - 30 && pos.y > middleY + 25)
            {
                if (availableWeapons.Contains(WeaponEnum.Fists))
                {
                    currentWeapon = WeaponEnum.Fists;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("fists(Clone)");
                    }
                }
            }
            else if (pos.x > middleX - 30 && pos.x < middleX + 30 && pos.y > middleY + 25)
            {
                if (availableWeapons.Contains(WeaponEnum.Rapier))
                {
                    currentWeapon = WeaponEnum.Rapier;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("rapier(Clone)");
                    }
                }
            }
            else if (pos.x > middleX + 30 && pos.y > middleY + 25)
            {
                if (availableWeapons.Contains(WeaponEnum.BroadSword))
                {
                    currentWeapon = WeaponEnum.BroadSword;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("Broadsword(Clone)");
                    }
                }
            }
            else if (pos.x > middleX + 30 && pos.y < middleY + 25 && pos.y > middleY - 25)
            {
                if (availableWeapons.Contains(WeaponEnum.Daggers))
                {
                    currentWeapon = WeaponEnum.Daggers;
                    if (previousSelectedWeapon != currentWeapon)
                    {
                        SelectWeapon("daggers(Clone)");
                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            Cursor.visible = false;
            menu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ResetWeapons()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i > NumberOfWeapons())
            {
                Destroy(weapon.gameObject);
            }
            i++;
        }
        currentWeapon = WeaponEnum.None;
        currentWeaponObject = null;
        availableWeapons = new List<WeaponEnum>();
        updateAvailableWeapons();
    }

    public int NumberOfWeapons()
    {
        int i = -1;
        foreach (Transform weapon in transform) i++;
        return i;
    }

    void updateCurrentWeapon()
    {

        if (transform.childCount == 1 || transform.childCount == 0)
        {
            currentWeapon = WeaponEnum.None;
            currentWeaponObject = null;
        }

        foreach (Transform weapon in transform)
        {
            if (weapon.gameObject.activeSelf)
            {
                currentWeaponObject = weapon.gameObject;
                switch (weapon.name)
                {
                    case "fists(Clone)":
                        currentWeapon = WeaponEnum.Fists;
                        if (previousSelectedWeapon != currentWeapon)
                        {
                            SelectWeapon(weapon.name);
                        }
                        break;
                    case "rapier(Clone)":
                        currentWeapon = WeaponEnum.Rapier;
                        if (previousSelectedWeapon != currentWeapon)
                        {
                            SelectWeapon(weapon.name);
                        }
                        break;
                    case "Broadsword(Clone)":
                        currentWeapon = WeaponEnum.BroadSword;
                        if (previousSelectedWeapon != currentWeapon)
                        {
                            SelectWeapon(weapon.name);
                        }
                        break;
                    case "daggers(Clone)":
                        currentWeapon = WeaponEnum.Daggers;
                        if (previousSelectedWeapon != currentWeapon)
                        {
                            SelectWeapon(weapon.name);
                        }
                        break;
                }
            }
        }


    }

    void updateAvailableWeapons()
    {
        availableWeapons = new List<WeaponEnum>();
        foreach (Transform weapon in transform)
        {
            switch (weapon.name)
            {
                case "fists(Clone)":
                    availableWeapons.Add(WeaponEnum.Fists);
                    break;
                case "rapier(Clone)":
                    availableWeapons.Add(WeaponEnum.Rapier);
                    break;
                case "Broadsword(Clone)":
                    availableWeapons.Add(WeaponEnum.BroadSword);
                    break;
                case "daggers(Clone)":
                    availableWeapons.Add(WeaponEnum.Daggers);
                    break;
            }
        }



    }

    public void makeCurrentWeaponInactive()
    {
        currentWeaponObject.SetActive(false);
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
