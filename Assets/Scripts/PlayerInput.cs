using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string Vertical = "Vertical";
    public static readonly string Horizontal = "Horizontal";
    public static readonly string FireButton = "Fire1";

    public float MoveVert { get; private set; }
    public float MoveHorizon { get; private set; }
    public bool Fire { get; private set; }

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    void Update()
    {
        MoveVert = Input.GetAxis(Vertical);
        MoveHorizon = Input.GetAxis(Horizontal);
        Fire = Input.GetButton(FireButton);
    }
}
