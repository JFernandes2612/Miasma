using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;

    public int numberOfWeapons = 0;

    void Start()
    {
        SelectWeapon();
        numberOfWeapons = NumberOfWeapons();
    }


    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedWeapon = (selectedWeapon + 1) % transform.childCount;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedWeapon = (selectedWeapon - 1) % transform.childCount;
        }
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
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
        numberOfWeapons = NumberOfWeapons();
    }

    public int NumberOfWeapons() {
        int i = 0;
        foreach (Transform weapon in transform) i++;
        return i;
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
