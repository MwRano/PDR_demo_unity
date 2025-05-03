using UnityEngine;

public class StepDetecter : MonoBehaviour
{
    public float _stepThreshold;
    private Vector3 _lastAcceleration;
    private bool _isStepping = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Input.gyro.enabled = true; // ジャイロセンサーを有効化
        _lastAcceleration = Input.acceleration;
    
    }

    // Update is called once per frame
    void Update()
    {
        float accelerationChange = Mathf.Abs(Input.acceleration.magnitude - _lastAcceleration.magnitude);

        if (accelerationChange > _stepThreshold && !_isStepping)
        {
            _isStepping = true;

        }
        else if (accelerationChange < _stepThreshold * 0.5f) // 閾値を下回ったらステップ終了とみなす
        {
            _isStepping = false;
        }

        _lastAcceleration = Input.acceleration;
    }

    private void OnStepDetected()
    {
        
    }
}
