
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool pause = false, GameEnd = false;
    public GameObject health, knives, player, paused;
    // Start is called before the first frame update
    void Start()
    {
        paused.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        health.GetComponent<Text>().text = "Health: " + player.GetComponent<playerController>().health +"/" + player.GetComponent<playerController>().maxHealth;
        knives.GetComponent<Text>().text = "Knives: " + player.GetComponent<playerController>().knives + "/" + player.GetComponent<playerController>().knivesMax;

        if (Input.GetKeyDown(KeyCode.Escape) && pause == false)
        {
            pause = true;
            Time.timeScale = 0f;
            paused.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pause == true)
        {
            pause = false;
            Time.timeScale = 1f;
            paused.SetActive(false);
        }

        if (GameEnd)
            loadMainMenu();
    }

    void loadMainMenu()
    {
        StartCoroutine(lvlLoader("MainMenu", 1));
    }

    IEnumerator lvlLoader(string levelName, int waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(levelName);
    }
}
