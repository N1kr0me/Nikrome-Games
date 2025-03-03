using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerButton : MonoBehaviour
{
    public void OpenScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
