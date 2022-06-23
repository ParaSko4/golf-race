using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace GolfRace.Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float shakeTime;
        [SerializeField]
        private float amplitudeGain;

        private CinemachineVirtualCamera cinemachineVirtualCamera;
        private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

        private float currentAmplitudeGain;

        private void Awake()
        {
            cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake()
        {
            DOVirtual.Float(currentAmplitudeGain, amplitudeGain, shakeTime, (newAmplitudeGain) =>
            {
                currentAmplitudeGain = newAmplitudeGain;

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = currentAmplitudeGain;
            }).OnComplete(() =>
            {
                currentAmplitudeGain = 0f;
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            });
        }
    }
}
