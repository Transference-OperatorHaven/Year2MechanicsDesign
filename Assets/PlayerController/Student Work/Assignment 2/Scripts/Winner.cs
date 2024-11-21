using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Winner : MonoBehaviour
{
    [SerializeField] Image image;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        image.enabled = true;
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }

}
