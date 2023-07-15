using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    bool isHeld = false;
    [SerializeField] Transform otherButton;

    // Start is called before the first frame update
    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { onClick(); });

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((eventData) => { onRelease(); });

        trigger.triggers.Add(entry);
        trigger.triggers.Add(entry2);
    }

    [SerializeField] Canvas myCanvas;

    // Update is called once per frame
    void Update()
    {
        if (isHeld)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            
            transform.position = new Vector3(Mathf.Min(Mathf.Max(-4.6f, myCanvas.transform.TransformPoint(pos).x), 0.87f), transform.position.y, 0);
            Debug.Log(transform.position.x);
            otherButton.position = new Vector3(-1 * (transform.position.x + 1.88f) - 1.88f, otherButton.position.y, 0);
        }


        


    }

    void onClick()
    {
        isHeld = true;
    }

    void onRelease()
    {
        isHeld = false;
    }
}
