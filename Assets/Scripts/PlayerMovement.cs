using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("Move");

    public float moveSpeed = 50f;

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    Vector3 dir = new();
    Vector3 hitPosition = new();

    private RaycastHit hit;
    public LayerMask targetLayer;
    public Camera cam;

    public Gun gun;

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 캐릭터 + 카메라 이동
        dir.Set(playerInput.MoveHorizon, 0, playerInput.MoveVert);
        dir.Normalize();

        bool hasVerticalInput = playerInput.MoveVert != 0;
        bool hasHorizontalInput = playerInput.MoveHorizon != 0;
        
        playerAnimator.SetBool(HashMove, hasHorizontalInput || hasVerticalInput);
        playerRigidbody.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        cam.GetComponent<Rigidbody>().MovePosition(cam.transform.position + dir * moveSpeed * Time.deltaTime);

        // 총알 발사
        if (playerInput.Fire)
        {
            gun.Shoot(hit);
        }
    }

    private void FixedUpdate()
    {
        // 캐릭터 회전
        hitPosition = Vector3.zero;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, targetLayer))
        {
            hitPosition = hit.point;
            transform.LookAt(hitPosition);
        }
    }
}
