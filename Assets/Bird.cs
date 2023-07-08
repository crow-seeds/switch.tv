using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    float jumpTimer = 0;
    [SerializeField] GameObject bottomDetector;
    [SerializeField] GameObject topDetector;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] float minHeight;
    [SerializeField] FlappyBird flapHandler;

    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(jumpTimer < 0.3f)
        {
            jumpTimer += Time.deltaTime;
        }

        /*if (transform.localPosition.y < minHeight)
        {
            rig.gravityScale = 0;
            rig.velocity = Vector2.zero;
        }*/
    }



    private void FixedUpdate()
    {
        if (detectPipe() && jumpTimer >= 0.3f && transform.localPosition.y < 4f && !paused)
        {
            jumpTimer = 0;
            rig.velocity = Vector2.up * 8f;
            rig.gravityScale = 2;
        }
    }
    [SerializeField] LayerMask bottompipeLayer;
    [SerializeField] LayerMask toppipeLayer;
    bool detectPipe()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(bottomDetector.transform.position, Vector2.right, 12f, bottompipeLayer);
        RaycastHit2D hit2;
        hit2 = Physics2D.Raycast(topDetector.transform.position, Vector2.right, 8f, toppipeLayer);

        Debug.DrawRay(bottomDetector.transform.position, Vector2.right * 12, Color.green);
        Debug.DrawRay(topDetector.transform.position, Vector2.right * 8, Color.green);
        return (hit.collider == true && hit2.collider == false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "pipe")
        {
            flapHandler.streamerDead();
        }
    }

    public void killBird()
    {
        paused = true;
        rig.velocity = Vector2.zero;
        rig.gravityScale = 0;
    }
}
