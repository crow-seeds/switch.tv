using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PixelButton : MonoBehaviour
{
    [SerializeField] ButtonHandler bh;
    // Start is called before the first frame update
    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { onEnter(); });

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerDown;
        entry3.callback.AddListener((eventData) => { onEnter2(); });

        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onEnter()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if(GetComponent<RawImage>().color != Color.red)
            {
                if(GetComponent<RawImage>().color.b == 1)
                {
                    bh.addToJ();
                }
                bh.addToAny();
                GetComponent<RawImage>().color = Color.red;
            }
            
        }
    }

    public void onEnter2()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (GetComponent<RawImage>().color != Color.red)
            {
                if (GetComponent<RawImage>().color.b == 1)
                {
                    bh.addToJ();
                }
                bh.addToAny();
                GetComponent<RawImage>().color = Color.red;
            }

        }
    }
}
