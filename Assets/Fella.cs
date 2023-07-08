using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fella : MonoBehaviour
{
    public bool paused = false;
    Vector2 mov = Vector2.zero;
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;

    public bool isTouchingCrosshair = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mov = Vector2.zero;
        if (!paused)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                mov += Vector2.left;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                mov += Vector2.right;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "cross")
        {
            Debug.Log("touch cross");
            isTouchingCrosshair = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "cross")
        {
            isTouchingCrosshair = false;
        }
    }
}
