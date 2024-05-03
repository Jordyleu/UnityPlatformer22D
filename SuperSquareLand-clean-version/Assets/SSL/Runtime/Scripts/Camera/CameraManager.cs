using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Profile System")]
    [SerializeField] private CameraProfile _defaultCameraProfile;
    private CameraProfile _currentCameraProfile;

    private float _profileTransitionTimer = 0f;
    private float _profileTransitionDuration = 0f;
    private Vector3 _profileTransitionStartPosition;
    private float _profileTransitionStartSize;

    private Vector3 _profileLastFollowDestination;

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

        Vector3 nextPosition = _FindCameraNextPosition();
        if (IsPlayingProfileTransition())
        {
            _profileTransitionTimer += Time.deltaTime;
            Vector3 transitionPosition = _CalculateProfileTransitionPosition(nextPosition);
            _SetCameraPositon(transitionPosition);
            float transitionSize = _CalculateProfileTransitionCameraSize(_currentCameraProfile.CameraSize);
            _SetCameraSize(transitionSize);
        }
        else
        {
            _SetCameraSize(_currentCameraProfile.CameraSize);
            _SetCameraPositon(nextPosition);
        }
    }
    private void _InitToDefaultProfile()
    {
        _currentCameraProfile = _defaultCameraProfile;
        _SetCameraPositon(_currentCameraProfile.Position);
        _SetCameraSize(_currentCameraProfile.CameraSize);
    }

    public void EnterProfile(CameraProfile cameraProfile, CameraProfileTransition transition = null)
    {
        _currentCameraProfile = cameraProfile;
        if (transition != null)
        {
            _PlayProfileTrasition(transition);
        }
    }

    public void ExitProfile(CameraProfile cameraProfile, CameraProfileTransition transition = null)
    {
        if (_currentCameraProfile != cameraProfile) return;
        _currentCameraProfile = _defaultCameraProfile;
        if (transition != null)
        {
            _PlayProfileTrasition(transition);
        }
    }

    private void _PlayProfileTrasition(CameraProfileTransition transition)
    {
        _profileTransitionStartPosition = _camera.transform.position;
        _profileTransitionStartSize = _camera.orthographicSize;
        _profileTransitionTimer = 0f;
        _profileTransitionDuration = transition.duration;
    }

    private bool IsPlayingProfileTransition()
    {
        return _profileTransitionTimer < _profileTransitionDuration;
    }

    private float _CalculateProfileTransitionCameraSize(float endSize)
    {
        float percent = _profileTransitionTimer / _profileTransitionDuration;
        float startSize = _profileTransitionStartSize;
        return Mathf.Lerp(startSize, endSize, percent);
    }

    private Vector3 _CalculateProfileTransitionPosition(Vector3 destination)
    {
        float percent = _profileTransitionTimer / _profileTransitionDuration;
        Vector3 origin = _profileTransitionStartPosition;
        return Vector3.Lerp(origin, destination, percent);
    }

    private Vector3 _FindCameraNextPosition()
    {
        if (_currentCameraProfile.ProfileType == CameraProfile.CameraProfileType.FollowTarget)
        {
            if (_currentCameraProfile.TargetToFollow != null)
            {
                CameraFollowable targetToFollow = _currentCameraProfile.TargetToFollow;
                _profileLastFollowDestination.x = targetToFollow.FollowPositionX;
                _profileLastFollowDestination.y = targetToFollow.FollowPositionY;
                return _profileLastFollowDestination;
            }
        }
        return _currentCameraProfile.Position; 
    }
}