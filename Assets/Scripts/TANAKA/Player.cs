using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("移動パラメータ")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;

    Rigidbody2D rb;
    InputPlayer input;
    Camera mainCam;
    int moveDir;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        input = GetComponent<InputPlayer>();

        // 横移動開始
        input.OnMove.Subscribe(dir => moveDir = dir).AddTo(this);
        // 横移動終了
        input.OffMove.Subscribe(_ => moveDir = 0).AddTo(this);
        // ジャンプ
        input.OnJump.Subscribe(_ => Jump()).AddTo(this);
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(moveDir * moveSpeed, rb.linearVelocity.y);
        ClampToCamera();
    }

    void Jump()
    {
        // 接地判定なしの試験用（常時ジャンプ可能）
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void ClampToCamera()
    {
        Vector3 pos = transform.position;
        float halfHeight = mainCam.orthographicSize;
        float halfWidth = halfHeight * mainCam.aspect;

        float minX = mainCam.transform.position.x - halfWidth;
        float maxX = mainCam.transform.position.x + halfWidth;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
