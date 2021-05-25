
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadLevel1()
    {
        StartCoroutine("levelLoader", "Level1");

    }
    public void quitGame()
    {
        Application.Quit();
    }

    IEnumerator levelLoader(string levelName)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelName);
    }


}
