using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject yellow;

    [SerializeField]
    private GameObject red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yellow.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetYellowPoints().ToString();
        red.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetRedPoints().ToString();
    }
}
