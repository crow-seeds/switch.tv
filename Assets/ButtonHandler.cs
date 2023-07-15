using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonHandler : MonoBehaviour
{
    int amountOfJs;
    int amountOfAny;
    float timer = 0;
    bool paused = false;

    [SerializeField] AudioClip anticipation;
    [SerializeField] AudioClip rage;
    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;
    [SerializeField] AudioSource streamerAudio;


    // Start is called before the first frame update
    void Start()
    {
        streamerAudio.clip = anticipation;
        streamerAudio.Play();
        ch.loadSubtitles(5, "normal");
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            timer += Time.deltaTime;
            progressBar.fillAmount = timer / 13;
            if (timer > 13)
            {
                paused = true;
                mh.endMicroGame(false);
                //streamer good ending
            }
        }

        if (canPressZ)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                streamerAudio.Stop();
            }
        }
    }

    public void checkStatus()
    {
        if(amountOfJs > 4 && amountOfAny > 7)
        {

            //streamer bad ending

            if (!paused)
            {
                paused = true;
                streamerAudio.clip = rage;
                streamerAudio.Play();
                ch.loadSubtitles(5, "rage");
                anim.SetBool("lose", true);
                
                StartCoroutine(endingCo(true));
                
            }
            

            
        }
    }

    bool canPressZ = false;
    IEnumerator endingCo(bool b)
    {
        StartCoroutine(skipWarning());
        canPressZ = true;
        yield return new WaitWhile(() => streamerAudio.isPlaying);
        mh.endMicroGame(b);
    }

    [SerializeField] Transform zToSkip;

    IEnumerator skipWarning()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(zToSkip, new Vector3(1, -4, -6), 1f);
        yield return new WaitForSeconds(3f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(zToSkip, new Vector3(1, -6, -6), 1f);
    }

    public void addToJ()
    {
        amountOfJs++;
        checkStatus();
    }

    public void addToAny()
    {
        amountOfAny++;
        checkStatus();
    }
}
