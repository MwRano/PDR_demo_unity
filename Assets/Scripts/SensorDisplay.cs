using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class SensorDisplay : MonoBehaviour
{
    //public TMP_Text accelText;
    public TMP_Text pressureText;


    void Start()
    {
        InputSystem.EnableDevice(PressureSensor.current);
    }

    void Update()
    {
        //Vector3 acceleration = Input.acceleration;

        float pressure = PressureSensor.current.atmosphericPressure.ReadValue();
        
        // 表示更新
        //accelText.text = $"Acceleration:\nX: {acceleration.x:F2}\nY: {acceleration.y:F2}\nZ: {acceleration.z:F2}";
        pressureText.text = $"Pressure:\n{pressure:F2} hPa";

    }
}
