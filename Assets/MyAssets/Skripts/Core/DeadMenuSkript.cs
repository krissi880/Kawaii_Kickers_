using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenuSkript : MonoBehaviour
{
   
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
