#nullable enable

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InitialScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TMP_Dropdown floorDropdown;
    [SerializeField] Button setInitialPositionButton;
    [SerializeField] Button setInitialHeadingButton;
    [SerializeField] Button startPDRButton;

    [Header("User Prefab")]
    [SerializeField] GameObject userPrefab;

    Image _floorMapImage;
    int _selectedFloorLevel;
    float _selectedFloorPressure;
    Vector3 _initialPosition;
    Vector3 _initialHeading;
    bool _isSetInitPosition = false;
    bool _isSetHeading = false;

    FloorMapManager _floorMapManager;
    FloorEstimator _floorEstimator;
    User _user;

    // コメント
    // 最初にuserオブジェクト(User), MapManagerをインスタンス化
    // userオブジェクトからUserを取得しておく
    // PDR開始したら、PDRManager(User), FloorEstimator(MapManger)をインスタンス化
    // 現在の状況（フロア、向き、座標、マップ）をUIに表示するクラスが必要（一旦なしでも、やっぱいるかも）
    void OnAwake()
    { 
        GameObject userGameObject = Instantiate(userPrefab);
        _user = userGameObject.GetComponent<User>();
        _floorMapManager = new FloorMapManager();
        
        setInitialPositionButton.onClick.AddListener(OnSetInitialPositionButtonClicked);
        setInitialHeadingButton.onClick.AddListener(OnSetInitialHeadingButtonClicked);
        startPDRButton.onClick.AddListener(OnStartPDRButtonClicked);
        floorDropdown.onValueChanged.AddListener(OnFloorSelected);
    }

    void Update()
    {
        if(Input.touchCount > 0 && Input.mousePosition.y < 1780f){
            OnMapClicked();
        }
    }

    void OnFloorSelected(int selectedFloorIndex){
        _selectedFloorLevel = selectedFloorIndex + 6; // 0から始まるインデックスを6から始まるフロアレベルに変換
        _selectedFloorPressure = PressureSensor.current.atmosphericPressure.ReadValue();
    }

    void OnMapClicked(){
        if(_isSetInitPosition == false)
        {
            UpdateInitialPosition();
        }
        else if(_isSetHeading == false)
        {
            UpdateInitialHeading();
        }
    }

    void UpdateInitialPosition(){
        Vector3 screenPosition = Input.mousePosition;
        _initialPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        _initialPosition.z = 0;
    }

    void UpdateInitialHeading()
    {
        Vector3 screenPosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0;

        _initialHeading = worldPosition - _initialPosition; // 初期位置からのベクトルを計算
        _initialHeading.Normalize(); // 単位ベクトルに正規化
    }

    void OnSetInitialPositionButtonClicked()
    {
        _isSetInitPosition = true; // 初期位置が設定されたのでフラグを立てる
        setInitialPositionButton.interactable = false; // ボタンを無効化
        setInitialHeadingButton.interactable = true; // 初期向き設定ボタンを有効化
    }

    void OnSetInitialHeadingButtonClicked()
    {
        _isSetHeading = true; // 初期向きが設定されたのでフラグを立てる
        setInitialHeadingButton.interactable = false; // ボタンを無効化
        startPDRButton.interactable = true; // PDR開始ボタンを有効化
    }
    void OnStartPDRButtonClicked()
    {
        _floorEstimator = new FloorEstimator(_selectedFloorLevel, _selectedFloorPressure);
    }
}
