using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class InitializeManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown floorLevelDropdown; // プレイヤーオブジェクト
    [SerializeField] GameObject floorMaps; 

    public bool isSetInitPosition = false; // 初期位置が設定されたかどうかのフラグ
    public bool isSetHeading = false; // 初期向きが設定されたかどうかのフラグ

    public Camera mainCamera; // タップ位置を取得するためのカメラ

    PDR pdrScripts; // PDRスクリプトの参照

    Vector3 initPosition;
    Vector3 initHeading;
    float pressure;

    struct PlayerInfo
    {
        public int floorLevel;
        public Vector3 position;
        public Vector3 heading;
    }


    void Start()
    {
        // カメラが設定されていない場合は、メインカメラを取得
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // PDRスクリプトの参照を取得
        pdrScripts = GetComponent<PDR>();
        pdrScripts.enabled = false; // 初期位置と向きが設定されるまでPDRスクリプトを無効にする

    }


    // Update is called once per frame
    void Update()
    {
        // print(Input.mousePosition);
        if(Input.touchCount > 0 && Input.mousePosition.y < 1780f){
            

            if(isSetInitPosition == false)
            {
                initPosition = GetStartingPosition();
                gameObject.transform.position = initPosition; // プレイヤーの位置を初期位置に設定
                //isSetInitPosition = true; // 初期位置が設定されたのでフラグを立てる
            }else if(isSetHeading == false)
            {
                initHeading = GetStartingHeading(transform.position);
                float cumulativeYaw = Mathf.Atan2(initHeading.y, initHeading.x); // 初期向きを設定
                gameObject.transform.rotation = Quaternion.Euler(0, 0, cumulativeYaw * Mathf.Rad2Deg - 90); // 初期向きを反映
                //isSetHeading = true; // 初期向きが設定されたのでフラグを立てる
            }
        }

        if(isSetInitPosition && isSetHeading)
        {
            EnablePDR();
        }

        UpdateFloorMaps();

        pressure = PressureSensor.current.atmosphericPressure.ReadValue();
    }

    /// <summary>
    /// Gets the starting floor level of the player based on the tap position.
    /// </summary>
    int GetStartingFloorLevel()
    {
        int floorLevelOffset = 6;
        return floorLevelDropdown.value + floorLevelOffset;
    }

    /// <summary>
    /// 
    /// <summary>
    void UpdateFloorMaps()
    {        // フロアマップの表示を更新
        for (int i = 0; i < floorMaps.transform.childCount; i++)
        {
            Transform child = floorMaps.transform.GetChild(i);
            child.gameObject.SetActive(false); // すべてのフロアマップを非表示にする 

            int currentFloorLevel = GetStartingFloorLevel();
            string floorTag = $"FLOOR{currentFloorLevel}";
            if(child.gameObject.CompareTag(floorTag)){
                child.gameObject.SetActive(true);
            }


        }
    }



    /// <summary>
    /// Gets the starting position of the player based on the tap position.
    /// </summary>
    Vector3 GetStartingPosition()
    {
        // タップしたスクリーン座標を取得
        Vector3 screenPosition = Input.mousePosition;

        // スクリーン座標をワールド座標に変換
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane));

        // Z座標を0に固定（XY平面上の座標を取得する場合）
        worldPosition.z = 0;

        // デバッグ用に座標を表示
        // Debug.Log($"screen Position: {screenPosition}");
        Debug.Log($"Tapped Position: {worldPosition}");

        return worldPosition;
    }

    /// <summary>
    /// Gets the starting heading of the player based on the tap position.
    /// </summary>
    Vector3 GetStartingHeading(Vector3 initPosition)
    {
        // タップしたスクリーン座標を取得
        Vector3 screenPosition = Input.mousePosition;

        // スクリーン座標をワールド座標に変換
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane));

        // Z座標を0に固定（XY平面上の座標を取得する場合）
        worldPosition.z = 0;

        Vector3 heading = worldPosition - initPosition;
        heading.Normalize(); // 単位ベクトルに正規化

        Debug.Log($"Heading: {heading}");

        return heading;
    }

    /// <summary>
    /// isSetInitPositionをtrueにするメソッド
    /// </summary>
    public void SetInitPosition()
    {
        isSetInitPosition = true;
    }

    /// <summary>
    /// isSetHeadingをtrueにするメソッド
    /// </summary>
    public void SetInitHeading()
    {
        isSetHeading = true;
    }

    void EnablePDR()
    {
        // 初期位置と向きをPDRスクリプトに設定
        pdrScripts.initPos = initPosition;
        pdrScripts.initHeading = initHeading;
        pdrScripts.enabled = true; // PDRスクリプトを有効にする
    }

    
}

