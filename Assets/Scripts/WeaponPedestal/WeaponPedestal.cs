using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPedestal : MonoBehaviour
{
    [SerializeField]
    private GameObject weaponPrefab;

    [SerializeField]
    private float minCollectDistance = 5.0f;

    private GameObject player;

    private GameObject weaponHolder;

    private bool gotWeapon = false;

    void Start()
    {
        player = GameObject.Find("Player");

        weaponHolder = GameObject.Find("WeaponHolder");

    }

    void Update()
    {
        if (!gotWeapon && Input.GetKeyDown("e") && Vector3.Distance(player.transform.position, transform.position) <= minCollectDistance)
        {
            GameObject newWeapon = Instantiate(weaponPrefab, weaponHolder.transform);
            Destroy(transform.GetChild(4).gameObject);
            Destroy(transform.GetChild(3).gameObject);
            gotWeapon = true;
            weaponHolder.GetComponent<WeaponSwitching>().makeCurrentWeaponInactive();
        }
    }
}
