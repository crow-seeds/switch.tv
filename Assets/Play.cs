using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{

    achivementHandler aHandler;

    // Start is called before the first frame update
    void Start()
    {
        aHandler = FindObjectOfType<achivementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }

    public void loadGame()
    {
        if (aHandler != null) { aHandler.achInit(); }
        SceneManager.LoadScene("MainMenu");
    }
}
