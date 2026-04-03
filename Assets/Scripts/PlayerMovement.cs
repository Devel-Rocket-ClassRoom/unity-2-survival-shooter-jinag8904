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

    Player player;

    private float lastShotTime = 0;
    private float shotInterval = 0.1f;

    public GameManager gameManager;
    bool isPaused;

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Player>();

        isPaused = false;
        cam.transform.position = new Vector3(-5, 35, -75);
    }

    private void Update()
    {
        if (gameManager.isPaused && !isPaused) 
        {
            OnPause();
            return;
        }

        else if (!gameManager.isPaused && isPaused)
        {
            OffPause();
            return;
        }

        if (isPaused) return;
        if (player.isDead) return;

        // 캐릭터 + 카메라 이동
        dir.Set(playerInput.MoveHorizon, 0, playerInput.MoveVert);
        dir.Normalize();

        bool hasVerticalInput = playerInput.MoveVert != 0;
        bool hasHorizontalInput = playerInput.MoveHorizon != 0;

        bool isMoving = hasHorizontalInput || hasVerticalInput;

        if (isMoving)
        {
            playerRigidbody.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        }

        playerAnimator.SetBool(HashMove, isMoving);

        var camTarget = new Vector3(transform.position.x -5f, cam.transform.position.y, transform.position.z - 75);
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camTarget, 0.5f);

        // 총알 발사
        if (playerInput.Fire && Time.time > lastShotTime + shotInterval)
        {
            gun.Shoot();
            lastShotTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.isPaused) return;
        if (player.isDead) return;

        // 캐릭터 회전
        hitPosition = Vector3.zero;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, targetLayer))
        {
            hitPosition = hit.point;
        }

        else
        {
            hitPosition = transform.position + ray.direction;
        }

        transform.LookAt(hitPosition);
    }

    public void OnPause()
    {
        isPaused = true;
        playerAnimator.speed = 0;
    }

    public void OffPause()
    {
        isPaused = false;
        playerAnimator.speed = 1;
    }
}
