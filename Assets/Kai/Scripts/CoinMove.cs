using UnityEngine;

public class CoinMove : MonoBehaviour
{
    #region 変数宣言

    [SerializeField] float fallSpeed;       // 落ちるスピード.

    private int playerLayer;                // プレイヤーのレイヤー保存用.

    Vector3 screenLeftBottom;               // スクリーンの左下の座標.

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // スクリーン左下のワールド座標を取得、代入.
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // レイヤーを取得.
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -fallSpeed, 0);

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

        // スコア加算.
    }
}
