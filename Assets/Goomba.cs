using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Goomba : MonoBehaviour
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
    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;
    [SerializeField] AudioSource streamerAudio;

    [SerializeField] Transform mario;

    bool canPressZ = false;

    float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        cam.backgroundColor = backgroundColor;

        streamerAudio.clip = anticipation;
        streamerAudio.Play();

        if (Random.Range(0f, 1f) < .5f)
        {
            Vector3 temp = transform.position;
            transform.position = mario.position;
            mario.position = temp;
        }

        ch.loadSubtitles(6, "normal");


        
    }

    bool sand = false;
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

            if (Input.GetKeyDown(KeyCode.Space) && isTouchingGround())
            {
                rig.velocity += Vector2.up * 8;
            }

            timer += Time.deltaTime;
            progressBar.fillAmount = timer / 15f;

            if(timer > 9.5f && !sand)
            {
                sand = true;
                anim.SetBool("sandwich", true);
            }

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


    [SerializeField] BoxCollider2D boxCol;
    [SerializeField] LayerMask ground;

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

    public bool isTouchingGround()
    {
        RaycastHit2D rayHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0f, Vector2.down, 0.2f, ground);
        return rayHit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "mario" && !paused)
        {
            col.gameObject.SetActive(false);
            paused = true;

            anim.SetBool("lose", true);
            streamerAudio.clip = rage;
            streamerAudio.Play();
            StartCoroutine(endingCo(true));
            ch.loadSubtitles(6, "rage");
        }
    }

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
