using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Profile System")]
    [SerializeField] private CameraProfile _defaultCameraProfile;
    private CameraProfile _currentCameraProfile;

    public static CameraManager Instance { get; private set; }

    private void _SetCameraPositon(Vector3 positon)
    {
        Vector3 newCameraPositon = _camera.transform.position;
        newCameraPositon.x = positon.x;
        newCameraPositon.y = positon.y;
        _camera.transform.position = newCameraPositon;
    }

    private void _SetCameraSize(float size)
    {
        _camera.orthographicSize = size;
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _InitToDefaultProfile();
    }

    private void Update()
    {
        _SetCameraPositon(_currentCameraProfile.Position);
        _SetCameraSize(_currentCameraProfile.CameraSize);
    }
    private void _InitToDefaultProfile()
    {
        _currentCameraProfile = _defaultCameraProfile;
        _SetCameraPositon(_currentCameraProfile.Position);
        _SetCameraSize(_currentCameraProfile.CameraSize);
    }

    public void EnterProfile(CameraProfile cameraProfile)
    {
        _currentCameraProfile = cameraProfile;
    }

    public void ExitProfile(CameraProfile cameraProfile)
    {
        if (_currentCameraProfile != cameraProfile) return;
        _currentCameraProfile = _defaultCameraProfile;
    }
}