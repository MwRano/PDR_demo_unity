using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScone(){
        SceneManager.LoadScene("SampleScene");
    }
}
