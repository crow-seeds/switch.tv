using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    float minHeight;
    float maxHeight = 8.2f;
    public bool paused = false;
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rig;
    
    
    // Start is called before the first frame update
    void Start()
    {
        minHeight = transform.localPosition.y;

    }

    bool pressedSpace = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressedSpace = true;
        }

        
        if(transform.localPosition.y < minHeight)
        {
            rig.gravityScale = 0;
            rig.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            if (transform.localPosition.x > 11)
            {
                transform.Translate(Vector3.left * speed);
            }
            else
            {
                Destroy(gameObject);
            }

            if (pressedSpace && transform.localPosition.y < 9f)
            {
                rig.velocity = Vector2.up * 7f;
                rig.gravityScale = 2;
                pressedSpace = false;
            }
        }
        
    }
}
