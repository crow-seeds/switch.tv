using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndlessScoreTracker : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Endless Mode (" + PlayerPrefs.GetInt("endless_record", 0) + ")";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
