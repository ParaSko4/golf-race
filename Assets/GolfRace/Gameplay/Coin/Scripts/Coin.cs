using DG.Tweening;
using GolfRace.Gameplay.Car;
using UnityEngine;

namespace GolfRace.Gameplay.Coins
{
    [RequireComponent(typeof(BoxCollider))]
    public class Coin : MonoBehaviour
    {
        [SerializeField]
        private float rotataionDuration;
        [SerializeField]
        private float scaleDuration;
        [SerializeField]
        private ParticleSystem fadePrefab;
        [SerializeField]
        private Transform fadeEffectSpawnPosition;

        private Tween coinRotation;
        private bool started;

        private void Awake()
        {
            coinRotation = transform.DORotate(Vector3.up * 360f, rotataionDuration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        private void OnTriggerEnter(Collider other)
        {
            CarController carController;

            if (other.transform.parent.TryGetComponent(out carController) && started == false)
            {
                Take();
            }
        }

        public async void Take()
        {
            started = true;

            coinRotation?.Kill();

            var fade = Instantiate(fadePrefab, fadeEffectSpawnPosition.position, Quaternion.identity);
            fade.transform.SetParent(transform);
            fade.Play();

            await transform.DOScale(Vector3.zero, scaleDuration).SetEase(Ease.InOutSine).AsyncWaitForCompletion();

            Destroy(gameObject);
        }
    }
}
