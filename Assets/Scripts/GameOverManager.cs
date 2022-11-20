using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private bool victory;
    private bool defeat;

    public float restartDelayAfterDefeat;
    public float restartDelayAfterVictory;
    // Start is called before the first frame update
    void Start()
    {
        victory = false;
        defeat = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(defeat==true)
        {
            restartDelayAfterDefeat -= Time.deltaTime;
            if(restartDelayAfterDefeat <=0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); //Shorter way to do it ?
            }
        }
        if(victory==true)
        {
            restartDelayAfterVictory -= Time.deltaTime;
            if(restartDelayAfterVictory <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); //Shorter way to do it ?
            }
        }
    }

    private void Defeat()
    {
        defeat = true;
    }

    private void Victory()
    {
        victory = true;
    }
}
