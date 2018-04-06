using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public static bool allowMovement = false;
    public static int playerScore = 0;
    public static int enemyScore = 0;
    public static int round = 0;

    public GameObject cameraButton;
    public GameObject playerScoreOnScreen;
    public GameObject enemyScoreOnScreen;
    public GameObject backButton;
    public GameObject fwdButton;
    public GameObject punchButton;
    public GameObject kickButton;
    public GameObject qualityMeter;
    public GameObject[] points;

    public AudioClip[] audioClips;
    private AudioSource audioSource;

    private bool onGame = false;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!onGame)
        {
            if(DefaultTrackableEventHandler.trueFalse == true)
            {
                cameraButton.SetActive(false);
                qualityMeter.SetActive(false);
                playerScoreOnScreen.SetActive(true);
                enemyScoreOnScreen.SetActive(true);
                backButton.SetActive(true);
                fwdButton.SetActive(true);
                punchButton.SetActive(true);
                kickButton.SetActive(true);
                onGame = true;

                StartCoroutine(round1());
            }
        }
	}

    IEnumerator round1()
    {
        yield return new WaitForSeconds(0);
        PlayAudioTrack(0);
        StartCoroutine(PrepareYourself());
    }

    IEnumerator PrepareYourself()
    {
        yield return new WaitForSeconds(1.2f);
        PlayAudioTrack(1);
        StartCoroutine(Start321());
    }

    IEnumerator Start321()
    {
        yield return new WaitForSeconds(2f);
        PlayAudioTrack(2);
        StartCoroutine(AllowPlayerMovement());
    }

    IEnumerator AllowPlayerMovement()
    {
        yield return new WaitForSeconds(5f);
        allowMovement = true;
    }

    public void scorePlayer()
    {
        playerScore++;
    }

    public void scoreEnemy()
    {
        enemyScore++;
    }

    public void OnScreenPoints()
    {
        if(playerScore == 1)
        {
            points[0].SetActive(true);
        } else if(playerScore == 2)
        {
            points[1].SetActive(true);
        }

        if(enemyScore == 1)
        {
            points[2].SetActive(true);
        } else if (enemyScore == 2)
        {
            points[3].SetActive(true);
        }
    }

    public void rounds()
    {
        round = playerScore + enemyScore;
        if(round==1)
        {
            PlayAudioTrack(3);
        } else if(round == 2 && playerScore != 2 && enemyScore != 2)
        {
            PlayAudioTrack(4);
        }
    }

    public void DoReset()
    {
        if(playerScore == 2)
        {
            PlayAudioTrack(6);
        }
        else
        {
            PlayAudioTrack(5);
        }

        FighterController.instance.playerHealthBar.value = 100;
        FighterController.instance.health = 100;

        EnemyController.instance.enemyHealthBar.value = 100;
        EnemyController.instance.enemyHealth = 100;

        playerScore = 0;
        enemyScore = 0;
        StartCoroutine(RestartGame());
    }

    public void BackToMenu()
    {
        Invoke("LoadMenuScene", 0.5f);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(4.5f);
        points[0].SetActive(false);
        points[1].SetActive(false);
        points[2].SetActive(false);
        points[3].SetActive(false);

        allowMovement = true;
        StartCoroutine(RestartRoundAudio());
    }

    private IEnumerator RestartRoundAudio()
    {
        yield return new WaitForSeconds(2f);
        PlayAudioTrack(0);
    }

    private void PlayAudioTrack(int clip)
    {
        audioSource.clip = audioClips[clip];
        audioSource.Play();
    }
}
