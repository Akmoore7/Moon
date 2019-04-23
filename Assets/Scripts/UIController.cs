using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    public GameObject gameOverMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Pause")) {
            if(gameOverMenu.activeSelf) {
                // ignore
            }
            else if(optionsMenu.activeSelf){
                optionsMenu.GetComponent<OptionsMenuActions>().Back();
            }
            else if(pauseMenu.activeSelf) {
                pauseMenu.GetComponent<PauseMenuActions>().Resume();
            }
            else {
                pauseMenu.GetComponent<PauseMenuActions>().Pause();
            }
        }
    }

    public void GameOver() {
        GameManager.instance.isStopped = true;
        Time.timeScale = 0.0f;
        gameOverMenu.SetActive(true);
    }
}
