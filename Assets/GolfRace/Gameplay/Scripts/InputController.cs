using System;
using UnityEngine;

namespace GolfRace.Gameplay
{
    public class InputController : MonoBehaviour
    {
        public event Action MousePressed;
        public event Action MouseReleased;

        [SerializeField]
        private float maxDistance;

        private Camera mainCamera;
        private Vector3 mousePositionOnPlane = Vector3.zero;

        public Vector3 MousePositionOnPlane { get => mousePositionOnPlane; }

        private void Awake()
        {
            mainCamera = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                MousePressed?.Invoke();
                mousePositionOnPlane = Vector3.zero;
            }
            else if (Input.GetMouseButton(0))
            {
                OnMousePressed();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                MouseReleased?.Invoke();
            }
        }

        public Vector3 GetDirectionTouchToPoint(Vector3 point)
        {
            return (mousePositionOnPlane - point).normalized;
        }

        public Vector3 GetDirectionPointToTouch(Vector3 point)
        {
            return -(mousePositionOnPlane - point).normalized;
        }

        public float GetDistanceBetweenTouchAndPoint(Vector3 point)
        {
            Vector3 offset = new Vector3(mousePositionOnPlane.x, point.y, mousePositionOnPlane.z) - point;

            return (point + Vector3.ClampMagnitude(offset, maxDistance)).magnitude;
        }

        private void OnMousePressed()
        {
            Plane plane = new Plane(Vector3.up, 0);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float distance))
            {
                mousePositionOnPlane = ray.GetPoint(distance);
            }
        }
    }
}