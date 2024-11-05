using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPlayer : MonoBehaviour
{   
    private LevelGenerator levelGenerator;
    public Level currentLevel;
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip musicLoop;
    [SerializeField]
    private AudioClip roundLost;
    [SerializeField]
    private AudioClip roundWon;

    private Animator startText;
    private TextMeshProUGUI clearText;

    
    
    void Start() {
        startText = GameObject.Find("Start").GetComponent<Animator>();
        clearText = GameObject.Find("Clear").GetComponent<TextMeshProUGUI>();

        audioSource = GetComponent<AudioSource>();
        levelGenerator = GetComponent<LevelGenerator>();
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        currentLevel = levelGenerator.Generate(SessionStats.Level);
        yield return new WaitForSeconds(3.9f);
        Debug.Log("start");
        currentLevel.SetActive(true);
        audioSource.clip = musicLoop;
        audioSource.loop = true;
        audioSource.Play();

        StartCoroutine(StartText());
        StartCoroutine(WaitForLevelEnd());
    }

    IEnumerator StartText() {
        TextMeshProUGUI text = startText.GetComponent<TextMeshProUGUI>();
        text.enabled = true;
        startText.enabled = true;
        yield return new WaitUntil(() => startText.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95);
        startText.enabled = false;
        yield return new WaitForSeconds(0.5f);
        text.enabled = false;
    }

    void ClearText() {
        clearText.enabled = true;
    }

    IEnumerator WaitForLevelEnd() {
        yield return new WaitUntil(() => currentLevel.Complete);
        audioSource.Stop();
        
        if (currentLevel.PlayerIsAlive) {
            audioSource.PlayOneShot(roundWon);
            currentLevel.SetActive(false);
            ClearText();
            yield return new WaitForSeconds(3);
            NextLevel();
        }
        else {
            audioSource.PlayOneShot(roundLost);
            currentLevel.SetActive(false);
            SessionStats.Lives--;
            yield return new WaitForSeconds(3);
            if (SessionStats.Lives <= 0) {
                GameOver();
            }
            else {
                ReplayLevel();
            }
        }
    }

    void NextLevel() {
        SessionStats.Level++;
        SceneManager.LoadScene("Transition");
    }

    void ReplayLevel() {
        SceneManager.LoadScene("Transition");
    }

    void GameOver() {
        Debug.Log("Game over");
        SessionStats.Reset();
        SceneManager.LoadScene("StartMenu");
    }
}
