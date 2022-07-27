using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLoader : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
