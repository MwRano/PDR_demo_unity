using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        Button restartButton = gameObject.GetComponent<Button>();
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked); // ボタンがクリックされたときにOnRestartButtonClickedメソッドを呼び出す
        }
        else
        {
            Debug.LogError("Button component not found on the GameObject.");
        }
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene("AppScene");
    }
}
