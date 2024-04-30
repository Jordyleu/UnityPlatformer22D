using UnityEngine;

public class HeroController : MonoBehaviour
{
    private void Update()
    {
        _entity.SetMoveDirX(GetInputMoveX());
        if (_GetInputDownJump())
        {
            if (_entity.IsTouchingGround && !_entity.IsJumping)
            {
                _entity.JumpStart();
            }
        }
    }
    private float GetInputMoveX()
    {
        float InputMoveX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            //Negative means : To the left <=
            InputMoveX = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Positive means : To the right =>
            InputMoveX = 1f;
        }
        return InputMoveX;
    }
    [Header("Entity")]
    [SerializeField] private HeroEntity _entity;

    [Header("Debug")]
    [SerializeField] private bool _guiDebug = false;

    private void OnGUI()
    {
        if (!_guiDebug) return;

        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label(gameObject.name);
        GUILayout.EndVertical();
    }
    private bool _GetInputDownJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}