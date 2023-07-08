using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashRoller : MonoBehaviour
{

    [SerializeField] bool isKirby;
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    Vector2 mov = Vector2.left;


    // Start is called before the first frame update
    void Start()
    {
        if (isKirby)
        {
            mov = Vector2.right;
        }
    }

    // Update is called once per frame
    void Update()
    {
       // rig.AddForce(Vector2.right, ForceMode2D.Force);
    }

    private void FixedUpdate()
    {
        mov = Vector2.ClampMagnitude(mov, 1) * speed;

        float targetSpeedX = mov.x;
        targetSpeedX = Mathf.Lerp(rig.velocity.x, targetSpeedX, 1);
        float accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? acceleration : deceleration;
        float speedDifX = targetSpeedX - rig.velocity.x;
        float movementX = speedDifX * accelRateX;
        Debug.Log(movementX * Vector2.right);
        rig.AddForce(movementX * Vector2.right, ForceMode2D.Force);
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "leftwall")
        {
            mov = Vector2.right;
        }

        if(col.transform.tag == "rightwall")
        {
            mov = Vector2.left;
        }

        if((col.transform.tag == "kirby" && !isKirby))
        {
            if(transform.position.x < col.transform.position.x)
            {
                col.rigidbody.AddForce(new Vector2(100, 3), ForceMode2D.Impulse);
                rig.AddForce(new Vector2(-100, 3), ForceMode2D.Impulse);
            }
            else
            {
                col.rigidbody.AddForce(new Vector2(-100, 3), ForceMode2D.Impulse);
                rig.AddForce(new Vector2(20, 3), ForceMode2D.Impulse);
            }
            
        }
    }
}
