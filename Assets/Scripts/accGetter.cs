using UnityEngine;

public class accGetter : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // 加速度センサの値を取得
        Vector3 val = Input.acceleration;
        //x,y,zそれぞれの値を出力する
        Debug.Log("x:" + val.x + "y:" + val.y + "z:" + val.z);
    }
}
