using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] public float ShakeIntensity = 0.5f;
    [SerializeField] public float ShakeTime = 0.1f;

    public float timer = 0;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private bool inputPress;

    public const string CAMERA_SHAKE = "CAMERA_SHAKE";

    private void Start() {
        StopShake();
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.CAMERA_SHAKE, this.EnableShake);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.CAMERA_SHAKE);
    }

    private void Update() {
        if(timer > 0) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                StopShake();
            } 
        }
    }

    private void EnableShake(Parameters parameters) {
        inputPress = parameters.GetBoolExtra(CAMERA_SHAKE, false);

        if(inputPress) {
            Debug.Log("Shaking"); ShakeCamera();  
        }
    }

    public void ShakeCamera() {
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = ShakeIntensity;
        timer = ShakeTime;
    }

    public void StopShake() {
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = 0;
    }
}
