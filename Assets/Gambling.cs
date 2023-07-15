using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gambling : MonoBehaviour
{

    [SerializeField] Camera cam;
    [SerializeField] Color backgroundColor;
    

    // Start is called before the first frame update
    void Start()
    {
        cam.backgroundColor = backgroundColor;
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void getResult(bool r)
    {

    }
}
