using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class winbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
        
    }
}
