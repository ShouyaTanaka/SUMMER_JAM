using UnityEngine;
using UniRx;

public class InputPlayer : MonoBehaviour
{
    public Subject<int> OnMove = new();   // -1=左, 1=右
    public Subject<Unit> OffMove = new();
    public Subject<Unit> OnJump = new();

    Vector2 startPos;
    bool isMoving;

    int activeFingerId = -1; // 操作中の指ID

    [Header("判定しきい値(px)")]
    [SerializeField] float tapThreshold = 20f;         // タップ判定距離
    [SerializeField] float swipeJumpThreshold = 80f;   // 上フリック判定距離
    [SerializeField] float swipeMoveThreshold = 30f;   // 横移動判定距離

    void Update()
    {
#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isMoving = false;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - startPos;

            // 横移動
            if (Mathf.Abs(delta.x) > swipeMoveThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                int dir = delta.x > 0 ? 1 : -1;
                OnMove.OnNext(dir);
                isMoving = true;
            }
            // 上フリック
            else if (!isMoving && delta.y > swipeJumpThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
            {
                OnJump.OnNext(Unit.Default);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endDelta = (Vector2)Input.mousePosition - startPos;

            if (isMoving)
            {
                OffMove.OnNext(Unit.Default);
            }
            else if (endDelta.magnitude < tapThreshold)
            {
                OnJump.OnNext(Unit.Default); // タップジャンプ
            }

            isMoving = false;
        }
    }

    void HandleTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // タッチ開始
            if (touch.phase == TouchPhase.Began && activeFingerId == -1)
            {
                activeFingerId = touch.fingerId;
                startPos = touch.position;
                isMoving = false;
            }

            // 対象の指以外は無視
            if (touch.fingerId != activeFingerId) continue;

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Vector2 delta = touch.position - startPos;

                    // 横移動
                    if (Mathf.Abs(delta.x) > swipeMoveThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    {
                        int dir = delta.x > 0 ? 1 : -1;
                        OnMove.OnNext(dir);
                        isMoving = true;
                    }
                    // 上フリック
                    else if (!isMoving && delta.y > swipeJumpThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
                    {
                        OnJump.OnNext(Unit.Default);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    Vector2 endDelta = touch.position - startPos;

                    if (isMoving)
                    {
                        OffMove.OnNext(Unit.Default);
                    }
                    else if (endDelta.magnitude < tapThreshold)
                    {
                        OnJump.OnNext(Unit.Default); // タップジャンプ
                    }

                    isMoving = false;
                    activeFingerId = -1;
                    break;
            }
        }
    }
}
