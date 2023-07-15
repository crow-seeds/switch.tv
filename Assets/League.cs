using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class League : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        streamerAudio.clip = anticipation;
        streamerAudio.Play();
        ch.loadSubtitles(7, "normal");
    }

    float timer = 0;

    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;
    [SerializeField] AudioSource streamerAudio;
    [SerializeField] AudioClip anticipation;

    bool finger = false;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        progressBar.fillAmount = timer / 18f;

        if(timer > 17f && !finger)
        {
            anim.SetBool("finger", true);
            finger = true;
        }

        if(timer >= 18)
        {
            mh.endMicroGame(true);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {

            mh.endMicroGame(true);
        }
    }
}
