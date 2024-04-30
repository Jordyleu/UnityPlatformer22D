using UnityEngine;

public class HeroEntity : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody2D _rigidbody;
    public float DashCountdown = 2f;

    [Header("Horizontal Movements")]
    [SerializeField] private HeroHorizontalMovementsSettings _horizontalMovementsSettings;
    private float _horizontalSpeed = 0f;
    private float _moveDirX = 0f;

    [Header("Orientation")]
    [SerializeField] private Transform _orientVisualRoot;
    private float _orientX = 1f;

    [Header("Debug")]
    [SerializeField] private bool _guiDebug = false;

    [Header("Vertical Movements")]
    private float _verticalSpeed = 0f;

    [Header("Fall")]
    [SerializeField] private HeroFallSettings _fallSettings;

    [Header("Ground")]
    [SerializeField] private GroundDetector _groundDetector;

    public bool IsTouchingGround { get; private set; } = false;

    public void SetMoveDirX(float dirX)
    {
        _moveDirX = dirX;
    }

    private void FixedUpdate()
    {
        _ApplyGroundDetection();

        if (_AreOrientAndMovementOpposite())
        {
            _TurnBack();
        }
        else
        {
            _UpdateHorizontalSpeed();
            _ChangeOrientFromHorizontalMovement();
            //_Dash();
        }

        if (!IsTouchingGround)
        {
        _ApplyFallGravity();
        }
        else
        {
            _ResetVerticalSpeed();
        }


        _ApplyHorizontalSpeed();
        _ApplyVerticalSpeed();
    }

    private void _ApplyHorizontalSpeed()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.x = _horizontalSpeed * _orientX;
        _rigidbody.velocity = velocity;
    }

    private void Update()
    {
        _UpdateOrientVisual();
    }

    private void _UpdateOrientVisual()
    {
        Vector3 newScale = _orientVisualRoot.localScale;
        newScale.x = _orientX;
        _orientVisualRoot.localScale = newScale;
    }
    private void _ChangeOrientFromHorizontalMovement()
    {
        if (_moveDirX == 0f) return;
        _orientX = Mathf.Sign(_moveDirX);
    }

    #region Functions Accelerate
    private void _Accelerate()
    {
        _horizontalSpeed += _horizontalMovementsSettings.acceleration * Time.fixedDeltaTime;
        if (_horizontalSpeed > _horizontalMovementsSettings.speedmax)
        {
            _horizontalSpeed = _horizontalMovementsSettings.speedmax;
        }
    }
    #endregion

    #region Functions Decelerate
    private void _Decelerate()
    {
        _horizontalSpeed -= _horizontalMovementsSettings.deceleration * Time.fixedDeltaTime;
        if (_horizontalSpeed < 0f)
        {
            _horizontalSpeed = 0f;
        }
    }

    #endregion
    private void _UpdateHorizontalSpeed()
    {
        if (_moveDirX != 0f)
        {
            _Accelerate();
        }
        else
        {
            _Decelerate();
        }
    }
    private void _TurnBack()
    {
        _horizontalSpeed -= _horizontalMovementsSettings.turnBackFrictions * Time.fixedDeltaTime;
        if (_horizontalSpeed < 0f)
        {
            _horizontalSpeed = 0f;
            _ChangeOrientFromHorizontalMovement();
        }
    }

    private void _ApplyFallGravity()
    {
        _verticalSpeed -= _fallSettings.fallGravity * Time.fixedDeltaTime;
        if ( _verticalSpeed < -_fallSettings.fallSpeedMax)
        {
            _verticalSpeed = -_fallSettings.fallSpeedMax;
        }
    }
    private void _ApplyVerticalSpeed()
    {
        Vector2 velocity = _rigidbody.velocity;
        velocity.y = _verticalSpeed;
        _rigidbody.velocity = velocity;
    }
    //private void _Dash()
    //{
    //    if (_horizontalMovementsSettings.DashDuration > 0f)
    //    {
    //        _horizontalSpeed = 40f;
    //        _horizontalMovementsSettings.DashDuration -= Time.fixedDeltaTime;
    //    }
    //    else
    //    {

    //    }
    //    if ( DashCountdown > 0f) 
    //    {
    //        DashCountdown -= Time.deltaTime;
    //    }
    //    if (Input.GetKey(KeyCode.E) && DashCountdown <= 0f)
    //    {
    //        _horizontalSpeed += _horizontalMovementsSettings.DashSpeed * _horizontalMovementsSettings.DashDuration;
    //        //_horizontalSpeed -= _horizontalMovementsSettings.DashSpeed * Time.fixedDeltaTime;
    //        DashCountdown = 2f;
    //    }
    //}
    private bool _AreOrientAndMovementOpposite()
    {
        return _moveDirX * _orientX < 0f;
    }

    private void _ApplyGroundDetection()
    {
        IsTouchingGround = _groundDetector.DetectGroundNearBy();
    }

    private void _ResetVerticalSpeed()
    {
        _verticalSpeed = 0f;
    }

    private void OnGUI()
    {
        if (!_guiDebug) return;

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(gameObject.name);
        GUILayout.Label($"MoveDirX = {_moveDirX}");
        GUILayout.Label($"OrientX = {_orientX}");
        GUILayout.Label($"Horizontal Speed = {_horizontalSpeed}");
        GUILayout.EndVertical();
        GUILayout.Label($"Vertical Speed = {_verticalSpeed}");
        if (IsTouchingGround)
        {
            GUILayout.Label("OnGround");
        }
        else
        {
            GUILayout.Label("InAir");
        }
    }
}