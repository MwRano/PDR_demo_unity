using UnityEngine;

public class accGetter : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // �����x�Z���T�̒l���擾
        Vector3 val = Input.acceleration;
        //x,y,z���ꂼ��̒l���o�͂���
        Debug.Log("x:" + val.x + "y:" + val.y + "z:" + val.z);
    }
}
