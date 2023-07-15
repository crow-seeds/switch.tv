using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startNormalMode()
    {
        PlayerPrefs.SetInt("mode", 0);
        SceneManager.LoadScene("Game");
    }

    public void startEndlessMode()
    {
        PlayerPrefs.SetInt("mode", 1);
        SceneManager.LoadScene("Game");
    }
}
