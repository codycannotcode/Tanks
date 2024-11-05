using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI livesText;
    void Start()
    {
        levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        levelText.text = String.Format("Level {0}", SessionStats.Level);

        livesText = transform.Find("LivesText").GetComponent<TextMeshProUGUI>();
        livesText.text = String.Format("Lives: {0}", SessionStats.Lives);

        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel() {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Level");
    }
}
