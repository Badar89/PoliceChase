using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.EnhancedTouch;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace TMPro.Examples
{
    public class CameraController : MonoBehaviour
    {
        public enum CameraModes { Follow, Isometric, Free }

        private Transform cameraTransform;
        private Transform dummyTarget;

        public Transform CameraTarget;

        public float FollowDistance = 30.0f;
        public float MaxFollowDistance = 100.0f;
        public float MinFollowDistance = 2.0f;

        public float ElevationAngle = 30.0f;
        public float MaxElevationAngle = 85.0f;
        public float MinElevationAngle = 0f;

        public float OrbitalAngle = 0f;

        public CameraModes CameraMode = CameraModes.Follow;

        public bool MovementSmoothing = true;
        public bool RotationSmoothing = false;
        private bool previousSmoothing;

        public float MovementSmoothingValue = 25f;
        public float RotationSmoothingValue = 5.0f;

        public float MoveSensitivity = 2.0f;
        public float ZoomSensitivity = 0.1f;
        public float RotationSmoothTime = 0.1f;
        public float ElevationSmoothTime = 0.1f;

        private float currentOrbitalAngle;
        private float currentElevationAngle;
        private float orbitalVelocity;
        private float elevationVelocity;

        private Vector3 currentVelocity = Vector3.zero;
        private Vector3 desiredPosition;
        private float mouseX;
        private float mouseY;
        private Vector3 moveVector;
        private float mouseWheel;

        bool m_HasInput;
        Vector3 m_InputPosition;
        

        void Awake()
        {
            if (QualitySettings.vSyncCount > 0)
                Application.targetFrameRate = 60;
            else
                Application.targetFrameRate = -1;

            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
                Input.simulateMouseWithTouches = false;

            cameraTransform = transform;
            previousSmoothing = MovementSmoothing;
        }

        void Start()
        {
            if (CameraTarget == null)
            {
                dummyTarget = new GameObject("Camera Target").transform;
                CameraTarget = dummyTarget;
            }
        }
        private void Update()
        {
            GetPlayerTouchInput();
        }
      
        void GetPlayerTouchInput()
        {
            if (Input.touchCount > 0)
            {
                 Touch touch = Input.GetTouch(0);

                        if (touch.phase == TouchPhase.Moved)
                        {
                            float touchX = touch.deltaPosition.x;

                            // Rotate the car around the Y-axis based on touch input
                            CameraTarget.Rotate(Vector3.up, touchX * MoveSensitivity * Time.deltaTime);
                        }       
            }
        }


        void LateUpdate()
        {
               GetPlayerInput();

            if (CameraTarget != null)
            {
                if (CameraMode == CameraModes.Isometric)
                {
                    desiredPosition = CameraTarget.position + Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * new Vector3(0, 0, -FollowDistance);
                }
                else if (CameraMode == CameraModes.Follow)
                {
                    desiredPosition = CameraTarget.position + CameraTarget.TransformDirection(Quaternion.Euler(ElevationAngle, OrbitalAngle, 0f) * (new Vector3(0, 0, -FollowDistance)));
                }

                if (MovementSmoothing)
                {
                    cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosition, ref currentVelocity, MovementSmoothingValue * Time.fixedDeltaTime);
                }
                else
                {
                    cameraTransform.position = desiredPosition;
                }

                if (RotationSmoothing)
                {
                    cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, Quaternion.LookRotation(CameraTarget.position - cameraTransform.position), RotationSmoothingValue * Time.deltaTime);
                }
                else
                {
                    cameraTransform.LookAt(CameraTarget);
                }
            }
        }

        void GetPlayerInput()
        {
            moveVector = Vector3.zero;

          
            float touchCount = Input.touchCount;

            if (Input.GetMouseButton(0)) // Right Mouse Button for camera rotation
            {
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");

                ElevationAngle -= mouseY * MoveSensitivity;
                ElevationAngle = Mathf.Clamp(ElevationAngle, MinElevationAngle, MaxElevationAngle);

                OrbitalAngle += mouseX * MoveSensitivity;
                OrbitalAngle = OrbitalAngle % 360; // Keep within 0-360 range
            }

            //if (mouseWheel < -0.01f || mouseWheel > 0.01f)
            //{
            //    FollowDistance -= mouseWheel * 5.0f;
            //    FollowDistance = Mathf.Clamp(FollowDistance, MinFollowDistance, MaxFollowDistance);
            //}
        }
    }
}
