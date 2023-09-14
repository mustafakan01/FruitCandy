using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public void LoadNextScene()
    {
        // Aktif sahnenin endeksini al
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Bir sonraki sahnenin endeksini hesapla
        int nextSceneIndex = currentSceneIndex + 1;

        // Eğer bir sonraki sahne varsa yükle, yoksa oyunu tamamla gibi bir işlem yapabilirsiniz.
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Tüm sahneleri geçtiyseniz oyunu tamamlayabilirsiniz.
            Debug.Log("Oyunu tamamladınız!");
        }
    }
}
