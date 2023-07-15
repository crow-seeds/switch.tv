using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombDropper : MonoBehaviour
{
    Vector2 mov = Vector2.zero;
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] Camera cam;
    [SerializeField] Color backgroundColor;

    public bool paused = false;

    [SerializeField] AudioClip anticipation;
    [SerializeField] AudioClip rage;
    [SerializeField] AudioClip win;
    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;
    [SerializeField] AudioSource streamerAudio;

    bool canPressZ = false;
    bool isRaging = false;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam.backgroundColor = backgroundColor;

        streamerAudio.clip = anticipation;
        streamerAudio.Play();
        ch.loadSubtitles(4, "normal");
    }

    // Update is called once per frame
    void Update()
    {
        mov = Vector2.zero;
        if (!paused)
        {
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > -7f)
            {
                mov += Vector2.left;
            }

            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < 4.5f)
            {
                mov += Vector2.right;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Bomb"), transform.position, Quaternion.Euler(0, 0, 0));
            }

            timer += Time.deltaTime;
            progressBar.fillAmount = timer / 15f;
            if (timer >= 15)
            {
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

    void FixedUpdate()
    {
        mov = Vector2.ClampMagnitude(mov, 1) * speed;

        float targetSpeedX = mov.x;
        targetSpeedX = Mathf.Lerp(rig.velocity.x, targetSpeedX, 1);
        float accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? acceleration : deceleration;
        float speedDifX = targetSpeedX - rig.velocity.x;
        float movementX = speedDifX * accelRateX;
        rig.AddForce(movementX * Vector2.right, ForceMode2D.Force);
    }

    public void loseSmashMatch()
    {
        canPressZ = true;
        anim.SetBool("lose", true);
        streamerAudio.clip = rage;
        streamerAudio.Play();
        StartCoroutine(endingCo(true));
        ch.loadSubtitles(4, "rage");
    }

    public void winSmashMatch()
    {
        canPressZ = true;
        streamerAudio.clip = win;
        streamerAudio.Play();
        StartCoroutine(endingCo(false));
        ch.loadSubtitles(4, "win");
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
