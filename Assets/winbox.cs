using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
            SceneManager.LoadScene("Testing Scene", LoadSceneMode.Single);
        
    }
}
