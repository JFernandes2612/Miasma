using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Death()
    {
        throw new System.NotImplementedException();
    }
}
