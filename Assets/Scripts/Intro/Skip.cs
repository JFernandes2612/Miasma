using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Skip : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S)){
            PlayableDirector director = GetComponent<PlayableDirector>();
            director.time = 78.08333;
        }
        
    }
}
