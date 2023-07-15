using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamblingThirdSlot : MonoBehaviour
{
    public bool paused = false;
    [SerializeField] float speed;
    [SerializeField] float timer;
    [SerializeField] float timeForStreamerToWin;
    [SerializeField] Gambling gamblingHandler;

    [SerializeField] AudioClip anticipation;
    [SerializeField] AudioClip rage;
    [SerializeField] AudioClip success;
    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;

    [SerializeField] AudioSource streamerAudio;

    // [SerializeField] bool isBomb;

    // Start is called before the first frame update
    void Start()
    {
        streamerAudio.clip = anticipation;
        streamerAudio.Play();
        ch.loadSubtitles(1, "normal");

        if(speed * Time.timeScale >= 20)
        {
            speed = 20 / Time.timeScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            //Debug.Log("something");
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - Time.deltaTime * speed, -1);

            if (transform.localPosition.y <= -16f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, 0f, -1);
            }
            timer += Time.deltaTime;
            progressBar.fillAmount = timer / timeForStreamerToWin;

        }
        

        if ((Input.GetKeyDown(KeyCode.Space) || timer > timeForStreamerToWin) && !paused)
        {
            paused = true;
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(transform, new Vector3(transform.localPosition.x, Mathf.RoundToInt(transform.localPosition.y / 4f) * 4, 0f), 1);
            chooseResult();
        }

        if (canPressZ)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                streamerAudio.Stop();
            }
        }
    }

    void chooseResult()
    {
        Debug.Log(transform.localPosition.y);
        bool bomb = Mathf.Abs(transform.localPosition.y + 8f) < 1.9f;
        if (!bomb)
        {
            streamerAudio.clip = success;
            streamerAudio.Play();
            anim.SetBool("winning", true);
            StartCoroutine(endingCo(false));
        }
        else
        {
            streamerAudio.clip = rage;
            streamerAudio.Play();
            anim.SetBool("losing", true);
            StartCoroutine(endingCo(true));
            ch.loadSubtitles(1, "rage");
        }


        gamblingHandler.getResult(bomb);


    }

    bool canPressZ = false;
    IEnumerator endingCo(bool b)
    {
        canPressZ = true;
        StartCoroutine(skipWarning());
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
}
