using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmingRPG : MonoBehaviour
{
    [SerializeField] AudioClip anticipation;
    [SerializeField] AudioClip rage;
    [SerializeField] AudioClip win;
    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;
    [SerializeField] AudioSource streamerAudio;

    [SerializeField] GameObject saveScreen;

    bool canPressZ = false;

    float timer = 0;
    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        streamerAudio.clip = anticipation;
        streamerAudio.Play();
        ch.loadSubtitles(8, "normal");
    }
    int state = 0;
    [SerializeField] Transform deleteButton;

    // Update is called once per frame
    void Update()
    {
        if (canPressZ)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                streamerAudio.Stop();
            }
        }



        if (!paused)
        {
            timer += Time.deltaTime;
            progressBar.fillAmount = timer / 6.2f;

            if (timer > 2 && state == 0)
            {
                saveScreen.SetActive(true);
                state++;
            }else if(timer > 6.2 && state == 1)
            {
                paused = true;
                state++;
                optionsMenu.SetActive(false);
                canPressZ = true;
                StartCoroutine(flashCursor());
                if (deleteButton.localPosition.x < -16)
                {
                    anim.SetBool("swapped", true);
                    streamerAudio.clip = rage;
                    streamerAudio.Play();
                    StartCoroutine(endingCo(true));
                    ch.loadSubtitles(8, "rage");
                    saveImage.sprite = badEnding;
                }
                else
                {
                    StartCoroutine(endingCo(false));
                    saveImage.sprite = goodEnding;
                }
            }
        }
    }

    [SerializeField] RawImage cursor;
    [SerializeField] SpriteRenderer saveImage;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] Sprite goodEnding;
    [SerializeField] Sprite badEnding;
    IEnumerator flashCursor()
    {
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(cursor, Color.gray, 0.1f);
        yield return new WaitForSeconds(0.2f);
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Color Fader")).GetComponent<ColorFader>().set(cursor, Color.white, 0.1f);
    }

    IEnumerator endingCo(bool b)
    {
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
