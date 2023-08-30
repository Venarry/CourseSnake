using UnityEngine;

public class CameraFactory
{
    private readonly CameraMovement _mainCameraPrefab = Resources.Load<CameraMovement>(ResourcesPath.MainCamera);

    public CameraMovement Create()
    {
        CameraMovement cameraMovement = Object.Instantiate(_mainCameraPrefab);

        return cameraMovement;
    }
}
