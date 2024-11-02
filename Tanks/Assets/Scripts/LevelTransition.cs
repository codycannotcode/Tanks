using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = String.Format("Level {0}", PlayerStats.level);

        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level");
    }
}
