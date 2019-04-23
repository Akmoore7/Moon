using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ResetLevel() {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
        GameManager.instance.isStopped = false;
    }

    public void NextLevel() {
        SceneManager.LoadScene("Level 1 Boss");
        GameManager.instance.isStopped = false;
    }

    public void ExitMainMenu() {
        SceneManager.LoadScene("Main Menu");
        GameManager.instance.isStopped = false;
    }
}
