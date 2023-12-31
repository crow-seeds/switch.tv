using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonHandler : MonoBehaviour
{
    [SerializeField] GameObject enemyPokemon;
    [SerializeField] GameObject yourPokemon;
    [SerializeField] GameObject caption;
    [SerializeField] GameObject caption1;
    [SerializeField] GameObject caption2;
    [SerializeField] GameObject caption3;
    [SerializeField] GameObject caption4;
    [SerializeField] GameObject caption3part2;
    [SerializeField] GameObject trainerCircle;
    [SerializeField] Camera cam;
    [SerializeField] Color backgroundColor;

    [SerializeField] GameObject optionsContainer;
    [SerializeField] List<SpriteRenderer> optionChoices;


    [SerializeField] AudioClip anticipation;
    [SerializeField] AudioClip rage;
    [SerializeField] AudioClip success;
    [SerializeField] Animator anim;
    [SerializeField] MicrogameHandler mh;
    [SerializeField] Image progressBar;
    [SerializeField] Chat ch;
    [SerializeField] AudioSource streamerAudio;

    bool canPlay = false;
    int currentChoice;
    int correctChoice;
    float timer = 0;

    bool canPressZ;

    achivementHandler aHandler;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(intro());
        cam.backgroundColor = backgroundColor;

        streamerAudio.clip = anticipation;
        streamerAudio.Play();
        ch.loadSubtitles(3, "normal");
        aHandler = FindObjectOfType<achivementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        progressBar.fillAmount = timer / 18f;

        if (canPressZ)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                streamerAudio.Stop();
            }
        }


        if (canPlay)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                optionChoices[currentChoice].sprite = Resources.Load<Sprite>("Sprites/pokemon/option" + optionNums[currentChoice].ToString());

                if (currentChoice % 2 == 0)
                {
                    currentChoice++;
                }
                else
                {
                    currentChoice--;
                }

                optionChoices[currentChoice].sprite = Resources.Load<Sprite>("Sprites/pokemon/option" + optionNums[currentChoice].ToString() + "_select");
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                optionChoices[currentChoice].sprite = Resources.Load<Sprite>("Sprites/pokemon/option" + optionNums[currentChoice].ToString());

                currentChoice = (currentChoice + 2) % 4;

                optionChoices[currentChoice].sprite = Resources.Load<Sprite>("Sprites/pokemon/option" + optionNums[currentChoice].ToString() + "_select");
            }
        }
    }

    IEnumerator intro()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(enemyPokemon.transform, new Vector3(-6.22f, 2.19f, 0f), 1.5f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(yourPokemon.transform, new Vector3(1.71f, -2.43f, 0f), 1.5f);
        yield return new WaitForSeconds(2f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(caption.transform, new Vector3(-1.55f, 1.87f, -0.1f), 1);
        yield return new WaitForSeconds(2.5f);
        caption1.SetActive(false);
        caption2.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(caption.transform, new Vector3(15.07f, 1.87f, -0.1f), 1);
        trainerCircle.SetActive(true);
        yourPokemon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/stroobback");
        yield return new WaitForSeconds(2f);
        yourPokemon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/plingusback");
        enemyPokemon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/stroobfront");
        yield return new WaitForSeconds(2f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(caption.transform, new Vector3(-1.55f, 1.87f, -0.1f), 1);
        caption2.SetActive(false);
        optionsContainer.SetActive(true);
        canPlay = true;
        CreateOptions();
        yield return new WaitForSeconds(4f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(caption.transform, new Vector3(15.07f, 1.87f, -0.1f), 1);
        yourPokemon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/stroobback");
        enemyPokemon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/plingusfront");
        yield return new WaitForSeconds(2f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(caption.transform, new Vector3(-1.55f, 1.87f, -0.1f), 1);
        optionsContainer.SetActive(false);
        caption3.SetActive(true);
        caption3part2.SetActive(true);
        caption3part2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/option" + optionNums[currentChoice].ToString());
        yield return new WaitForSeconds(2f);
        caption3.SetActive(false);
        caption3part2.SetActive(false);
        caption4.SetActive(true);
        canPressZ = true;
        if (currentChoice == correctChoice)
        {
            caption4.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/captionFaint");
            enemyPokemon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/plingusdead");

            anim.SetBool("lose", true);
            streamerAudio.clip = rage;
            streamerAudio.Play();
            ch.loadSubtitles(3, "rage");
            StartCoroutine(endingCo(true));
        }
        else
        {
            caption4.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/captionCaught");
            enemyPokemon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/pokemon/plingusball");

            if (aHandler != null) { aHandler.unlockAchievement(2); }
            anim.SetBool("win", true);
            streamerAudio.clip = success;
            streamerAudio.Play();
            ch.loadSubtitles(3, "win");
            StartCoroutine(endingCo(false));
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

    List<int> optionNums = new List<int>() { 0, 1, 2, 3 };

    void CreateOptions()
    {
        
        Shuffle(optionNums);
        correctChoice = optionNums.IndexOf(0);
        currentChoice = 0;


        for (int i = 0; i < 4; i++)
        {
            if(i != currentChoice)
            {
                optionChoices[i].sprite = Resources.Load<Sprite>("Sprites/pokemon/option" + optionNums[i].ToString());
            }
            else
            {
                optionChoices[i].sprite = Resources.Load<Sprite>("Sprites/pokemon/option" + optionNums[i].ToString() + "_select");
            }
        }

        
    }

    IEnumerator endingCo(bool b)
    {
        StartCoroutine(skipWarning());
        yield return new WaitWhile(() => streamerAudio.isPlaying);
        Debug.Log("yessir");
        mh.endMicroGame(b);
    }

    [SerializeField] Transform zToSkip;

    IEnumerator skipWarning()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(zToSkip, new Vector3(1, -4, -6), 1f);
        yield return new WaitForSeconds(3f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Mover")).GetComponent<Mover>().set2(zToSkip, new Vector3(1, -6, -6), 1f);
    }
}
