using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlappyBird : MonoBehaviour
{
    [SerializeField] float timeForStreamerToWin = 15f;
    [SerializeField] Bird bird;
    [SerializeField] Camera cam;
    [SerializeField] Color backgroundColor;
    [SerializeField] AudioClip normal;
    [SerializeField] AudioClip rage;
    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;
    List<Pipe> pipeList = new List<Pipe>();
    float timer = 0;
    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        cam.backgroundColor = backgroundColor;
        streamerAudio.clip = normal;
        streamerAudio.Play();
        ch.loadSubtitles(0, "normal");
        StartCoroutine(generatePipe());
    }

    bool canPressZ = false;
    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            timer += Time.deltaTime;
            progressBar.fillAmount = timer / 12f;
            if (timer > 12)
            {
                paused = true;
                mh.endMicroGame(false);
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

    IEnumerator generatePipe()
    {
        if (!paused)
        {
            pipeList.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Pipe Pair"), new Vector3(30f, Random.Range(-3f, 5f), 0), Quaternion.Euler(0, 0, 0)).GetComponent<Pipe>());
            yield return new WaitForSeconds(2f);
            StartCoroutine(generatePipe());
        }
        
    }

    [SerializeField] AudioSource streamerAudio;

    public void streamerDead()
    {
        paused = true;
        foreach(Pipe p in pipeList)
        {
            if(p != null)
            {
                p.paused = true;
            }
        }
        bird.killBird();
        streamerAudio.clip = rage;
        streamerAudio.Play();
        anim.SetBool("rage", true);
        ch.loadSubtitles(0, "rage");
        StartCoroutine(endStream());
    }

    IEnumerator endStream()
    {
        canPressZ = true;
        StartCoroutine(skipWarning());
        yield return new WaitWhile(() => streamerAudio.isPlaying);
        foreach (Pipe p in pipeList)
        {
            if (p != null)
            {
                Destroy(p.gameObject);
            }
        }
        mh.endMicroGame(true);
        
    }

    [SerializeField] Transform zToSkip;

    IEnumerator skipWarning()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(zToSkip, new Vector3(1, -4, -6), 1f);
        yield return new WaitForSeconds(3f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(zToSkip, new Vector3(1, -6, -6), 1f);
    }
}
