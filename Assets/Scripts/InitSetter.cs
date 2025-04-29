using UnityEngine;

public class InitSetter : MonoBehaviour
{
    public Camera mainCamera; // タップ位置を取得するためのカメラ
    bool isSetInitPos = false; // 初期位置が設定されたかどうかのフラグ
    bool isSetHeading = false; // 初期向きが設定されたかどうかのフラグ
    private bool isWaitingForSecondTouch = false; // 2本目のタッチを待機中かどうか
    private float touchTimer = 0f; // タッチの猶予時間を計測するタイマー
    public float touchWaitTime = 0.2f; // 2本目のタッチを待つ猶予時間

    Vector3 initPos; // 初期位置
    Vector3 initHeading; // 初期向き
    PDR pdrScripts; // PDRスクリプトの参照
    public Transform player;

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

    void Update()
    {
        // 2本目のタッチを待機中の場合
        if (isWaitingForSecondTouch)
        {
            touchTimer += Time.deltaTime;

            // 猶予時間内に2本目のタッチが検出された場合
            if (Input.touchCount == 2)
            {
                Debug.Log("2本目のタッチが検出されました。1本指タップの処理をキャンセルします。");
                isWaitingForSecondTouch = false;
                touchTimer = 0f;
                return;
            }

            // 猶予時間を超えた場合
            if (touchTimer >= touchWaitTime)
            {
                Debug.Log("猶予時間を超えたため、1本指タップの処理を実行します。");
                isWaitingForSecondTouch = false;
                touchTimer = 0f;

                // 初期位置または初期向きの設定処理を実行
                if (isSetInitPos == false)
                {
                    initPos = GetInitPos();
                    player.position = initPos; // プレイヤーの位置を初期位置に設定
                    isSetInitPos = true; // 初期位置が設定されたのでフラグを立てる
                }
                else if (isSetHeading == false)
                {
                    initHeading = GetInitHeading(initPos);
                    isSetHeading = true; // 初期向きが設定されたのでフラグを立てる
                }

            }
        }
        else
        {
            // 1本指タップを検出
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Debug.Log("1本指タップが検出されました。猶予時間を開始します。");
                isWaitingForSecondTouch = true;
                touchTimer = 0f;
            }
        }

        if(isSetInitPos && isSetHeading)
        {
            EnablePDR();
        }
    }

    void EnablePDR()
    {
        // 初期位置と向きをPDRスクリプトに設定
        pdrScripts.initPos = initPos;
        pdrScripts.initHeading = initHeading;
        pdrScripts.enabled = true; // PDRスクリプトを有効にする
    }

    Vector3 GetInitPos()
    {
        // タップしたスクリーン座標を取得
        Vector3 screenPosition = Input.mousePosition;

        // スクリーン座標をワールド座標に変換
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane));

        // Z座標を0に固定（XY平面上の座標を取得する場合）
        worldPosition.z = 0;

        // デバッグ用に座標を表示
        Debug.Log($"Tapped Position: {worldPosition}");

        return worldPosition;
    }

    Vector3 GetInitHeading(Vector3 initPos)
    {
        // タップしたスクリーン座標を取得
        Vector3 screenPosition = Input.mousePosition;

        // スクリーン座標をワールド座標に変換
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane));

        // Z座標を0に固定（XY平面上の座標を取得する場合）
        worldPosition.z = 0;

        Vector3 heading = worldPosition - initPos;
        heading.Normalize(); // 単位ベクトルに正規化

        Debug.Log($"Heading: {heading}");

        return heading;
    }
}
