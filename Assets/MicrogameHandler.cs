using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MicrogameHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> microgameObjects;
    [SerializeField] List<string> microgameNames;
    [SerializeField] TextMeshProUGUI currentGameText;
    [SerializeField] TextMeshProUGUI upNextText;
    
    List<int> orderToBePlayed = new List<int>();
    [SerializeField] Image progressBar;

    [SerializeField] int debugGame = -1;
    [SerializeField] TextMeshProUGUI finalScoreText;

    [SerializeField] AudioSource additionalLayer;
    [SerializeField] List<AudioClip> musicTracks;

    bool endlessMode = false;
    float gameSpeed = 1;
    achivementHandler aHandler;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("mode",0) == 1)
        {
            endlessMode = true;
            score = PlayerPrefs.GetInt("score", 0);
            scoreText.text = "Endless Score: " + score.ToString();
            gameSpeed = 1 + score * .06f;
            Time.timeScale = gameSpeed;

            foreach(AudioSource a in FindObjectsOfType<AudioSource>())
            {
                a.pitch = gameSpeed;
            }
        }
        aHandler = FindObjectOfType<achivementHandler>();
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameComplete && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void StartGame()
    {
        for(int i = 0; i < microgameNames.Count; i++)
        {
            orderToBePlayed.Add(i);
        }

        Shuffle(orderToBePlayed);

        if (debugGame != -1)
        {
            initiateMicroGame(debugGame);
        }
        else
        {
            initiateMicroGame();
        }
        
    }

    [SerializeField] AudioSource backingTrack;
    public void initiateMicroGame()
    {
        progressBar.fillAmount = 0;
        foreach (GameObject g in microgameObjects)
        {
            g.SetActive(false);
        }

        microgameObjects[orderToBePlayed[0]].SetActive(true);
        currentGameText.text = microgameNames[orderToBePlayed[0]];

        additionalLayer.mute = false;
        additionalLayer.clip = musicTracks[orderToBePlayed[0]];
        additionalLayer.time = backingTrack.time;
        additionalLayer.Play();

         
        microgameObjects[orderToBePlayed[0]].SetActive(true);
        StartCoroutine(showPrompt(orderToBePlayed[0]));
        orderToBePlayed.RemoveAt(0);
        if(orderToBePlayed.Count == 0)
        {
            upNextText.text = "Up Next: ???";
        }
        else
        {
            upNextText.text = "Up Next: " + microgameNames[orderToBePlayed[0]];
        }


        
    }

    public void initiateMicroGame(int d)
    {
        progressBar.fillAmount = 0;
        foreach (GameObject g in microgameObjects)
        {
            g.SetActive(false);
        }

        additionalLayer.mute = false;
        additionalLayer.clip = musicTracks[d];
        additionalLayer.time = backingTrack.time;
        additionalLayer.Play();


        microgameObjects[d].SetActive(true);
        currentGameText.text = microgameNames[d];
        orderToBePlayed.Remove(d);
        StartCoroutine(showPrompt(d));

        upNextText.text = "Up Next: " + microgameNames[orderToBePlayed[0]];
    }

    [SerializeField] AudioSource streamerAudio;

    public void endMicroGame(bool ragequit)
    {
        streamerAudio.Stop();
        foreach (GameObject g in microgameObjects)
        {
            g.SetActive(false);
        }

        additionalLayer.mute = true;

        if (endlessMode && ragequit)
        {
            gameSpeed += .06f;
            Time.timeScale = gameSpeed;
            PlayerPrefs.SetFloat("speed", gameSpeed);
            PlayerPrefs.SetInt("score", score);

            foreach (AudioSource a in FindObjectsOfType<AudioSource>())
            {
                a.pitch = gameSpeed;
            }
        }


        StartCoroutine(endMicroGameCo(ragequit));
    }
    int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject endBroadcast;
    [SerializeField] GameObject loadBroadcast;
    [SerializeField] Chat ch;

    [SerializeField] RectTransform thanksForPlaying;
    bool gameComplete = false;

    [SerializeField] Transform zToSkip;
    IEnumerator endMicroGameCo(bool ragequit)
    {
        if(Mathf.Abs(zToSkip.position.y + 6) > .1f)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(zToSkip, new Vector3(1, -6, -6), 0.5f);
        }

        ch.clearMessages();
        if (ragequit)
        {
            score++;
            scoreText.text = "Score: " + score.ToString();
            if (endlessMode)
            {
                scoreText.text = "Endless Score: " + score.ToString();
            }
            endBroadcast.SetActive(true);
            yield return new WaitForSeconds(3f);
            endBroadcast.SetActive(false);

            
            
        }
        else
        {
            loadBroadcast.SetActive(true);
            yield return new WaitForSeconds(3f);
            

            if (endlessMode)
            {
                if(score > PlayerPrefs.GetInt("endless_record", 0))
                {
                    PlayerPrefs.SetInt("endless_record", score);
                }

                if(score >= 20)
                {
                    if (aHandler != null) { aHandler.unlockAchievement(3); }
                }

                if (score >= 40)
                {
                    if (aHandler != null) { aHandler.unlockAchievement(4); }
                }

                aHandler.submitScore(score);

                Time.timeScale = 1;
                PlayerPrefs.SetFloat("speed", 1);
                PlayerPrefs.SetInt("score", 0);
                SceneManager.LoadScene("MainMenu");
            }
            loadBroadcast.SetActive(false);
        }

        if(orderToBePlayed.Count == 0)
        {
            if(PlayerPrefs.GetInt("mode",0) == 0)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(thanksForPlaying, new Vector3(transform.localPosition.x, 0, 1), 2, false);
                gameComplete = true;
                finalScoreText.text = "Story Score: " + score.ToString();
                if (aHandler != null) { aHandler.unlockAchievement(0); }

                if(score == microgameNames.Count)
                {
                    if (aHandler != null) { aHandler.unlockAchievement(1); }
                }
            }
            else
            {
                PlayerPrefs.SetFloat("speed", gameSpeed);
                PlayerPrefs.SetInt("mode", 1);
                PlayerPrefs.SetInt("score", score);
                SceneManager.LoadScene("Game");
            }
        }
        else
        {
            initiateMicroGame();
        }

        
    }

    [SerializeField] RawImage prompt;
    IEnumerator showPrompt(int num)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(prompt.rectTransform, new Vector3(-100, 240, 0f), 1.5f, true);
        prompt.texture = Resources.Load<Texture>("Sprites/prompts/" + num.ToString());
        yield return new WaitForSeconds(3);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set(prompt.rectTransform, new Vector3(-100, 500, 0f), 1.5f, true);
    }
}
