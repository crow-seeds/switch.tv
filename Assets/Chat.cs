using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Globalization;
using System.IO;
using System;
using TMPro;

public class Chat : MonoBehaviour
{
    List<string> messages = new List<string>();
    [SerializeField] List<TextMeshProUGUI> textObjects;
    IFormatProvider format = new CultureInfo("en-US");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    TextAsset subtitleData;
    int currentIndex = 0;

    public void loadSubtitles(int level, string status)
    {
        StopAllCoroutines();
        messages = new List<string>();

        //currentIndex = 0;
        subtitleData = Resources.Load<TextAsset>("Data/chat_" + level.ToString() + "_" + status);

        string data = subtitleData.text;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(data));

        XmlNodeList dialogueNodeList = xmlDoc.SelectNodes("//data/chat");

        foreach (XmlNode infonode in dialogueNodeList)
        {
            messages.Add(infonode.Attributes["message"].Value);
        }
        StartCoroutine(messageLoop());
    }

    IEnumerator messageLoop()
    {
        if(messages.Count > 0)
        {
            string[] spl = messages[0].Split(':');

            messages[0] = "<color=\"red\">" + spl[0] + "</color>" + ":" + spl[1];

            if (currentIndex < textObjects.Count)
            {
                textObjects[currentIndex].text = messages[0];
                messages.RemoveAt(0);
                currentIndex++;
            }
            else
            {
                for(int i = 0; i < textObjects.Count-1; i++)
                {
                    textObjects[i].text = textObjects[i + 1].text;
                }
                textObjects[textObjects.Count - 1].text = messages[0];
                messages.RemoveAt(0);
            }
            


            yield return new WaitForSeconds(2f);
            StartCoroutine(messageLoop());
        }
    }

    public void clearMessages()
    {
        currentIndex = 0;
        StopAllCoroutines();
        foreach (TextMeshProUGUI t in textObjects)
        {
            t.text = "";
        }
    }
}
