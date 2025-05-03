using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitialPositionSetter : MonoBehaviour
{
    private Camera _mainCamera; // タップ位置を取得するためのカメラ
    private Vector3 _initialPosition;

    [SerializeField] private InitialDirectionSetter initialDirectionSetter; // 初期位置設定パネル
    [SerializeField] private Button initialPositionSetButton; // 初期位置設定ボタン
    [SerializeField] private PlayerController playerController; 
    [SerializeField] private TMP_Text initialPositionText; // 初期位置表示用のテキスト

    void Start()
    {
        _mainCamera = Camera.main; // メインカメラを取得
        initialPositionSetButton.onClick.AddListener(OnSetButtonClicked); // ボタンのクリックイベントにメソッドを登録
    }

    void Update()
    {
        if(Input.touchCount > 0 && Input.mousePosition.y < 1780f){
            GetInitialPosition();
        }
    }

    private void GetInitialPosition()
    {
        Vector3 screenPosition = Input.mousePosition;
        _initialPosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _mainCamera.nearClipPlane));
        _initialPosition.z = 0;
        UpdatePlayerPosition(); // プレイヤーの位置を更新
        UpdateInitialPositionText(); // 初期位置のテキストを更新
    }

    private void UpdatePlayerPosition()
    {
        playerController.PlayerPosition = _initialPosition; // プレイヤーの位置を初期位置に設定
    }

    private void UpdateInitialPositionText()
    {
        initialPositionText.text = $"Initial Position: {_initialPosition.x:F2}, {_initialPosition.y:F2}"; // 初期位置を表示
    }

    private void OnSetButtonClicked()
    {
        initialPositionSetButton.interactable = false; // ボタンを無効化
        initialDirectionSetter.InitialPosition = _initialPosition; // 初期位置を設定
        initialDirectionSetter.gameObject.SetActive(true);
        gameObject.SetActive(false); 
    }
}
