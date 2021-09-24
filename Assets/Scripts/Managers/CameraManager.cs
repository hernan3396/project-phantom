using UnityEngine;
using System.Collections;


public class CameraManager : MonoBehaviour
{
    #region Variables
    public int transitionSpeed = 20;
    public Transform cameraPos;
    #endregion

    #region Methods
    public void SetCamera(Transform destination)
    {
        // instantly moves camera
        cameraPos.position = SetCameraPosition(destination.position);
    }

    public void MoveCamera(Transform nextPosition)
    {
        // smoothly moves camera
        StartCoroutine(MovingCamera(nextPosition));
    }

    IEnumerator MovingCamera(Transform nextPosition)
    {
        // creates objective for camera
        Vector3 destination = SetCameraPosition(nextPosition.position);

        while (IsCameraOnPlace(destination))
        {
            // moves camera if its position is different from destination
            cameraPos.position = Vector3.MoveTowards(cameraPos.position, destination, transitionSpeed * Time.deltaTime);
            yield return null;
        }
    }

    Vector3 SetCameraPosition(Vector3 position)
    {
        return new Vector3(position.x, position.y, cameraPos.position.z);
    }

    private bool IsCameraOnPlace(Vector3 destination)
    {
        // false while camera is moving
        return cameraPos.position.x != destination.x || cameraPos.position.y != destination.y;
    }
    #endregion
}
