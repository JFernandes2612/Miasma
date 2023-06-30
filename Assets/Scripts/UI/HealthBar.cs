using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{


    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Slider>().maxValue = player.getMaxHealth();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Slider>().value = player.currentHealth;
    }
}
