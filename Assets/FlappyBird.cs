using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBird : MonoBehaviour
{
    [SerializeField] float timeForStreamerToWin = 15f;
    [SerializeField] Bird bird;
    [SerializeField] Camera cam;
    [SerializeField] Color backgroundColor;
    List<Pipe> pipeList = new List<Pipe>();
    float timer = 0;
    bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        cam.backgroundColor = backgroundColor;
        StartCoroutine(generatePipe());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator generatePipe()
    {
        if (!paused)
        {
            pipeList.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Pipe Pair"), new Vector3(30f, Random.Range(-3f, 5f), 0), Quaternion.Euler(0, 0, 0)).GetComponent<Pipe>());
            yield return new WaitForSeconds(2f);
            StartCoroutine(generatePipe());
        }
        
    }

    public void streamerDead()
    {
        paused = true;
        foreach(Pipe p in pipeList)
        {
            if(p != null)
            {
                p.paused = true;
            }
        }
        bird.killBird();
    }
}
