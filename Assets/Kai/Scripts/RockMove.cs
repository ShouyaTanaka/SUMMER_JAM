using UnityEngine;

public class RockMove : MonoBehaviour
{
    #region 変数宣言

    [SerializeField] float rockSpeed;       // 落ちるスピード.

    private int playerLayer;                // プレイヤーのレイヤー保存用.
    [SerializeField] private Transform playerTransform; // プレイヤーのTransformをセット
    [SerializeField] private float horizontalSpeed = 3f; // 横方向追尾速度

    Vector3 screenLeftBottom;               // スクリーンの左下の座標.

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // スクリーン左下のワールド座標を取得、代入.
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // レイヤーを取得.
        playerLayer = LayerMask.NameToLayer("Player");

        // プレイヤーの座標を取得.
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Playerタグのオブジェクトが見つかりません");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null)
        {
            Debug.Log("プレイヤーの座標を取得できません");

            return;
        }

        // 横方向(X)だけプレイヤーに近づく
        float newX = Mathf.MoveTowards(
            transform.position.x,
            playerTransform.position.x,
            horizontalSpeed * Time.deltaTime
        );

        // 縦方向(Y)は一定速度で下に落ちる
        float newY = transform.position.y - rockSpeed;

        // 新しい座標を適用
        transform.position = new Vector2(newX, newY);

        if (transform.position.y < screenLeftBottom.y)  // スクリーンのy座標を下回ったら.
        {
            Destroy(gameObject);                        // このオブジェクトを削除.
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)  // プレイヤーレイヤーを持つオブジェクトに触れたら.
        {
            TouchPlayer();                              // 関数呼び出し.
        }
    }
    public void TouchPlayer()       // プレイヤーに触れたら.
    {
        Debug.Log("プレイヤーに当たった");

        Destroy(gameObject);        // このオブジェクトを消す.

    }
}
