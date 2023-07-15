using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "platform")
        {
            Destroy(gameObject);
        }

        if(col.gameObject.GetComponent<SmashRoller>() != null)
        {
            col.gameObject.GetComponent<SmashRoller>().getHitByBomb();
            Destroy(gameObject);
        }
    }
}
