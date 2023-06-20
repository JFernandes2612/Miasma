using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{


        [SerializeField]
    private GameObject startBtt;

       [SerializeField]
    private GameObject quitBtt;
    // Start is called before the first frame update
    void Start()
    {

        Button button = quitBtt.GetComponent<Button>();
        button.onClick.AddListener(Quit);

        Button button1 = startBtt.GetComponent<Button>();
        button1.onClick.AddListener(StartGame);
        
    }

    void Quit(){
        Application.Quit();
    }

    void StartGame(){
        //Start logic
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
