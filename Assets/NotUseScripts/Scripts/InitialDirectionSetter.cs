using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitialDirectionSetter : MonoBehaviour
{
    public Vector3 InitialPosition { get; set; } // 初期位置
    private Camera _mainCamera; // タップ位置を取得するためのカメラ
    private Vector3 _initialDirection; // 初期向き

    [SerializeField] private Button initialDirectionSetButton; // 初期位置設定ボタン
    [SerializeField] private PlayerController playerController;
    [SerializeField] TMP_Text initialDirectionText; // 初期向き表示用のテキスト


    void Start()
    {
        _mainCamera = Camera.main; // メインカメラを取得
        initialDirectionSetButton.onClick.AddListener(OnSetButtonClicked); // ボタンのクリックイベントにメソッドを登録
        gameObject.SetActive(false); 
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.mousePosition.y < 1780f){
            GetInitialDirection();
        }
    }

    private void GetInitialDirection()
    {
        Vector3 screenPosition = Input.mousePosition;
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _mainCamera.nearClipPlane));
        worldPosition.z = 0;

        _initialDirection = worldPosition - InitialPosition; // 初期位置からのベクトルを計算
        _initialDirection.Normalize(); // 単位ベクトルに正規化

        UpdatePlayerDirection(); // プレイヤーの向きを更新
        UpdateInitialDirectionText(); // 初期向きのテキストを更新
    }

    private void UpdatePlayerDirection()
    {
        playerController.PlayerHeading = _initialDirection; // プレイヤーの向きを初期向きに設定
    }

    private void UpdateInitialDirectionText()
    {
        float playerYaw = Mathf.Atan2(_initialDirection.y, _initialDirection.x); // 初期向きを設定
        initialDirectionText.text = $"Heading: {playerYaw * Mathf.Rad2Deg:F2}°";
    }

    private void OnSetButtonClicked()
    {
        initialDirectionSetButton.interactable = false; // ボタンを無効化
        gameObject.SetActive(false);
    }

}
