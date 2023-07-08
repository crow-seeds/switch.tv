using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingThirdSlot : MonoBehaviour
{
    public bool paused = false;
    [SerializeField] float speed;
    [SerializeField] float timer;
    [SerializeField] float timeForStreamerToWin;
    [SerializeField] Gambling gamblingHandler;
   // [SerializeField] bool isBomb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            //Debug.Log("something");
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - Time.deltaTime * speed, -1);

            if (transform.localPosition.y <= -16f)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, 0f, -1);
            }
            timer += Time.deltaTime;
        }
        

        if ((Input.GetKeyDown(KeyCode.Space) || timer > timeForStreamerToWin) && !paused)
        {
            paused = true;
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(transform, new Vector3(transform.localPosition.x, Mathf.RoundToInt(transform.localPosition.y) - Mathf.RoundToInt(transform.localPosition.y) % 4, 0f), 1);
            chooseResult();
        }
    }

    void chooseResult()
    {
        Debug.Log(transform.localPosition.y);
        bool bomb = Mathf.Abs(transform.localPosition.y + 8f) < 1.9f;
        gamblingHandler.getResult(bomb);
    }
}
