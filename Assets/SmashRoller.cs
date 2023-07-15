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

    [SerializeField] Transform enemy;
    [SerializeField] GameObject invisWall;
    [SerializeField] GameObject invisWall2;

    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isKirby)
        {
            mov = Vector2.right;
        }
    }

    bool blownOff = false;
    [SerializeField] BombDropper bombDropper;
    // Update is called once per frame
    void Update()
    {
       // rig.AddForce(Vector2.right, ForceMode2D.Force);

        if((transform.position.x > 9 || transform.position.x < -9) && !blownOff)
        {
            blownOff = true;
            invisWall.SetActive(true);
            invisWall2.SetActive(true);
            paused = true;
            
            enemy.GetComponent<SmashRoller>().paused = true;

            bombDropper.paused = true;

            if (isKirby)
            {
                bombDropper.loseSmashMatch();
            }
            else
            {
                bombDropper.winSmashMatch();
                
            }
        }
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            mov = Vector2.ClampMagnitude(mov, 1) * speed;

            float targetSpeedX = mov.x;
            targetSpeedX = Mathf.Lerp(rig.velocity.x, targetSpeedX, 1);
            float accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? acceleration : deceleration;
            float speedDifX = targetSpeedX - rig.velocity.x;
            float movementX = speedDifX * accelRateX;
            rig.AddForce(movementX * Vector2.right, ForceMode2D.Force);
        }
        
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
            float randForce = Random.Range(20, 100);
            float randForce2 = Random.Range(20, 100);

            if(transform.position.x < col.transform.position.x)
            {
                col.rigidbody.AddForce(new Vector2(randForce, 3), ForceMode2D.Impulse);
                rig.AddForce(new Vector2(-randForce2, 3), ForceMode2D.Impulse);
            }
            else
            {
                col.rigidbody.AddForce(new Vector2(-randForce, 3), ForceMode2D.Impulse);
                rig.AddForce(new Vector2(randForce2, 3), ForceMode2D.Impulse);
            }
            
        }
    }

    public void getHitByBomb()
    {
        if(transform.position.x > enemy.position.x)
        {
            rig.AddForce(new Vector2(300, 3), ForceMode2D.Impulse);
        }
        else
        {
            rig.AddForce(new Vector2(-300, 3), ForceMode2D.Impulse);
        }
        invisWall.SetActive(false);
        invisWall2.SetActive(false);
    }
}
