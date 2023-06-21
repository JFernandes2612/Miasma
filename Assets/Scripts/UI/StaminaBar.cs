using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{

    private float currentStamina = 1;

    [SerializeField]
    private Movement movement;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (!movement.canQuickStep && GetComponent<Slider>().value == 1)
        {
            currentStamina = 0;
        }
        else if (movement.canQuickStep)
        {
            currentStamina = 1;
        }
        else
        {
            currentStamina += Time.deltaTime / (movement.dashCooldown + movement.dashTime);
        }

        GetComponent<Slider>().value = currentStamina;
    }

}
