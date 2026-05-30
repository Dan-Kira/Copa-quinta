using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void SinglePlayerButton()
    {
        SceneManager.LoadScene(1);
    }

    public void MultiPlayerButton()
    {
        SceneManager.LoadScene(2);
    }
}
