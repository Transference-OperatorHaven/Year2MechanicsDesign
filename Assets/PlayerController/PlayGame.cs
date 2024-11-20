using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Assignment 1", LoadSceneMode.Single);
    }
}
