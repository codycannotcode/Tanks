using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton = GetComponentInChildren<Button>();
        startButton.onClick.AddListener(StartButtonClicked);
    }

    // Update is called once per frame
    void StartButtonClicked()
    {
        SceneManager.LoadScene("Transition");
    }
}
