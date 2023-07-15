using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterMovingBackground : MonoBehaviour
{
    [SerializeField] Transform fella;
    [SerializeField] Fella fellaObject;
    [SerializeField] Transform crosshair;
    [SerializeField] float speed;
    [SerializeField] SpriteRenderer crosshairSprite;
    [SerializeField] float timeForStreamerToLose;
    [SerializeField] float timer;
    public bool paused = false;

    [SerializeField] Camera cam;
    [SerializeField] Color backgroundColor;

    [SerializeField] AudioClip anticipation;
    [SerializeField] AudioClip rage;
    [SerializeField] AudioClip success;
    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;
    [SerializeField] AudioSource streamerAudio;

    bool moveLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        cam.backgroundColor = backgroundColor;
        StartCoroutine(moveBackground());
        StartCoroutine(shoot());

        streamerAudio.clip = anticipation;
        streamerAudio.Play();
        ch.loadSubtitles(2, "normal");
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (!moveLeft)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            timer += Time.deltaTime;
            progressBar.fillAmount = timer / timeForStreamerToLose;

            if (timer >= timeForStreamerToLose)
            {
                fellaObject.paused = true;
                paused = true;
                //win condition for actual player;

                streamerAudio.clip = rage;
                streamerAudio.Play();
                anim.SetBool("lose", true);
                StartCoroutine(endingCo(true));
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

    IEnumerator moveBackground()
    {
        if (!paused)
        {
            yield return new WaitForSeconds(0.5f);
            moveLeft = true;
            if(crosshair.position.x > fella.position.x)
            {
                moveLeft = false;
            }
            StartCoroutine(moveBackground());
        }
    }

    IEnumerator shoot()
    {
        if (!paused)
        {
            yield return new WaitForSeconds(2f);
            if (paused)
            {
                yield break;
            }
            crosshairSprite.color = Color.yellow;
            if (fellaObject.isTouchingCrosshair)
            {
                streamerAudio.clip = success;
                streamerAudio.Play();
                anim.SetBool("win", true);
                StartCoroutine(endingCo(false));
                ch.loadSubtitles(2, "win");

                fellaObject.paused = true;
                paused = true;
                fella.gameObject.GetComponent<Animator>().SetBool("isDead", true);
            }
            yield return new WaitForSeconds(0.1f);
            crosshairSprite.color = Color.red;
            StartCoroutine(shoot());
        }
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
