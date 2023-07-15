using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashArrow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform kirb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = kirb.position + Vector3.up * 2;
    }
}
