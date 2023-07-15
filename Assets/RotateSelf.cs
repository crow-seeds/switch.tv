using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateSelf : MonoBehaviour
{
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, GetComponent<RectTransform>().rotation.eulerAngles.z + Time.deltaTime * speed);
    }
}
