using DG.Tweening;
using UnityEngine;

namespace GolfRace.Gameplay.Car
{
    public class CarController : MonoBehaviour
    {
        [SerializeField]
        private float explosionRadius;
        [SerializeField]
        private float explosionForce;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float rotateDuration;
        [SerializeField]
        private ParticleSystem collisionEffectPrefab;
        [SerializeField]
        private ParticleSystem carCollisionEffectPrefab;

        private InputController inputController;
        private TrajectoryMovement trajectoryMovement;
        private CameraController cameraController;
        private Tween trajectoryMovementTween;
        private Rigidbody carBody;
        private bool mousePressed;
        private bool moving;
        private bool collisionDetected;
        private int frameCounter;

        private void Awake()
        {
            carBody = GetComponent<Rigidbody>();
            trajectoryMovement = GetComponentInChildren<TrajectoryMovement>();
            inputController = FindObjectOfType<InputController>();
            cameraController = FindObjectOfType<CameraController>();

            foreach (var wheel in FindObjectsOfType<WheelCollider>())
            {
                wheel.brakeTorque = Mathf.Infinity;
            }
        }

        private void Update()
        {
            if (mousePressed)
            {
                trajectoryMovement.ShowTrajectory();
                trajectoryMovement.FollowTrajectory(inputController.GetDirectionPointToTouch(transform.position));
            }

            frameCounter++;

            if (frameCounter == 60)
            {
                frameCounter = 0;

                RaycastHit raycastHit;
                var ray = new Ray(transform.position, -transform.up);

                if (Physics.Raycast(ray, out raycastHit, 10))
                {
                    float distance = Vector3.Distance(transform.position, raycastHit.point);

                    if (distance >= 1f)
                    {
                        Stop();
                    }
                }
                else
                {
                    Stop();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (moving == false || mousePressed)
            {
                return;
            }

            collisionDetected = true;

            cameraController.Shake();
            trajectoryMovementTween?.Kill();

            if (collision.transform.CompareTag("DynamicObstacle"))
            {
                var collisionEffect = Instantiate(carCollisionEffectPrefab, collision.contacts[0].point, Quaternion.identity);
                collisionEffect.Play();

                Collider[] colliders = Physics.OverlapSphere(collision.contacts[0].point, explosionRadius);

                foreach (var collider in colliders)
                {
                    Rigidbody rb;

                    if (collider.TryGetComponent(out rb) && collider.gameObject != gameObject && collider.transform.CompareTag("DynamicObstacle"))
                    {
                        rb.AddExplosionForce(explosionForce, collision.contacts[0].point + Vector3.down, explosionRadius);
                    }
                }
            }
            else
            {
                var collisionEffect = Instantiate(collisionEffectPrefab, collision.contacts[0].point, Quaternion.identity);
                collisionEffect.Play();
            }
        }

        private void OnEnable()
        {
            inputController.MousePressed += OnMousePressed;
            inputController.MouseReleased += OnMouseReleased;
        }

        private void OnDisable()
        {
            inputController.MousePressed -= OnMousePressed;
            inputController.MouseReleased -= OnMouseReleased;
        }

        public void Stop()
        {
            carBody.freezeRotation = false;
            trajectoryMovement.StopMovement();
            trajectoryMovementTween?.Kill();
        }

        private void OnMousePressed()
        {
            mousePressed = true;
        }

        private void OnMouseReleased()
        {
            mousePressed = false;

            trajectoryMovement.FixTrajectoryDirections();
            StartMovement();
        }

        private async void StartMovement()
        {
            int i = 0;
            collisionDetected = true;
            moving = true;

            while (trajectoryMovement.GetNextDirection() && collisionDetected)
            {
                collisionDetected = false;
                trajectoryMovementTween = transform.DOMove(trajectoryMovement.CurrentPath, trajectoryMovement.CurrentDistance / speed)
                                                   .SetEase(i + 1 == trajectoryMovement.DirectionCount ? Ease.OutSine : Ease.Linear);
                transform.DOLookAt(trajectoryMovement.CurrentPath, rotateDuration);
                await trajectoryMovementTween.AsyncWaitForCompletion();

                i++;
            }

            moving = false;
        }
    }
}