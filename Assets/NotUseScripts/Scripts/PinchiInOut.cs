using UnityEngine;

public class PinchiInOut : MonoBehaviour
{
    public Camera targetCamera; // ズーム操作を行うカメラ
    public float zoomSpeed = 0.1f; // ズーム速度
    public float minZoom = 5f; // 最小ズーム値
    public float maxZoom = 20f; // 最大ズーム値

    void Update()
    {
        // タッチが2本以上ある場合にピンチ操作を検出
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // 各タッチの前回位置を取得
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            // 前回と現在のタッチ間の距離を計算
            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float currentTouchDeltaMag = (touch1.position - touch2.position).magnitude;

            // 距離の差分を計算
            float deltaMagnitudeDiff = prevTouchDeltaMag - currentTouchDeltaMag;

            // 2本指の中間点を計算
            Vector2 midPoint = (touch1.position + touch2.position) / 2;

            // 中間点をワールド座標に変換
            Vector3 worldMidPoint = targetCamera.ScreenToWorldPoint(new Vector3(midPoint.x, midPoint.y, targetCamera.nearClipPlane));

            // カメラのフィールドオブビューを調整してズームイン・アウト
            if (targetCamera.orthographic)
            {
                // 正射影カメラの場合
                targetCamera.orthographicSize += deltaMagnitudeDiff * zoomSpeed;
                targetCamera.orthographicSize = Mathf.Clamp(targetCamera.orthographicSize, minZoom, maxZoom);
            }
            else
            {
                // パースペクティブカメラの場合
                targetCamera.fieldOfView += deltaMagnitudeDiff * zoomSpeed;
                targetCamera.fieldOfView = Mathf.Clamp(targetCamera.fieldOfView, minZoom, maxZoom);

                // カメラの位置を調整して中間点を中心にズーム
                Vector3 direction = targetCamera.transform.position - worldMidPoint;
                targetCamera.transform.position += direction * (deltaMagnitudeDiff * zoomSpeed);
            }
        }
    }
}
