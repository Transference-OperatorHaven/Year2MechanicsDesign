
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Winner : MonoBehaviour
{
    [SerializeField] Image image;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            image.gameObject.SetActive(true);
            Time.timeScale = 0.1f;
        }
        
    }

    public void Restart()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }

}
