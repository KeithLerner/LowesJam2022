using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public string levelID;

    // public list<AIControllers> aiControllers;

    private void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
        }
        levelID = SceneManager.GetActiveScene().name;

        // Get all aiControllers in scene

        // Do Level Cut Scene?
    }

    // Update is called once per frame
    void Update()
    {
        // If player dead activate death screen
            // UI
            // Disable controller
        
        // Update UI timer

        // If player achieved clear condition
            // UI
            // Disable all controllers
            // Victory Cut Scene?
            // Victory screen
    }

    public void DoScream()
    {

    }

    public void RestartLevel()
    {

    }

    public void DisablePlayerController()
    {

    }

    public void DisableAIControllers()
    {

    }

    public void DisableAllControllers()
    {

    }
}
