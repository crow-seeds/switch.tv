using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool moveLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        cam.backgroundColor = backgroundColor;
        StartCoroutine(moveBackground());
        StartCoroutine(shoot());
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
        }

        if(timer >= timeForStreamerToLose)
        {
            fellaObject.paused = true;
            paused = true;
            //win condition for actual player;
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
                fellaObject.paused = true;
                paused = true;
                fella.gameObject.GetComponent<Animator>().SetBool("isDead", true);
            }
            yield return new WaitForSeconds(0.1f);
            crosshairSprite.color = Color.red;
            StartCoroutine(shoot());
        }
    }
}
