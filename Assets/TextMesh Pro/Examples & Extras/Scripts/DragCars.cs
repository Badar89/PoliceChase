using UnityEngine;

public class DragCars : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedCar;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, mainCamera.nearClipPlane));

            if (touch.phase == TouchPhase.Began)
            {
                // Raycast to check if the touch hits a car
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Car"))
                    {
                        selectedCar = hit.collider.gameObject;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && selectedCar != null)
            {
                // Move the selected car with the touch
                Vector3 newPosition = new Vector3(touchPosition.x, selectedCar.transform.position.y, touchPosition.z);
                selectedCar.transform.position = newPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Release the car
                selectedCar = null;
            }
        }
    }
}
