using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{


    [SerializeField]
    private Movement movement;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!movement.canQuickStep){
            GetComponent<Slider>().value = 0;
        }
        else
        {
            GetComponent<Slider>().value = 1;
        }
        
    }
}
