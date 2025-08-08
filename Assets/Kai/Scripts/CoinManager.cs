using UnityEditor;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    #region 変数宣言

    [SerializeField] float cerateSpan;      // 生成スパン.
    [SerializeField] GameObject coinPrefab; // 生成するオブジェクト.
    [SerializeField] GameObject rockPrefab; // 生成するオブジェクト.
    [SerializeField] GameObject coins;      // 生成する場所.

    float delta;                            // 生成からの経過時間.

    Vector3 screenLeftBottom;               // 画面左下の座標.
    Vector3 screenRightTop;                 // 画面右上の座標.

    public Vector3 ScreenLeftBottom => screenLeftBottom;    // 読み取り用.

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 左下の座標.
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
        // 右上の座標.
        screenRightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
    }

    // Update is called once per frame
    void Update()
    {
        GameObject prefabToSpawn = (Random.value < 0.5f) ? coinPrefab : rockPrefab;
        CreateCoins(prefabToSpawn);
    }

    public void CreateCoins(GameObject prefab)
    {
        // 生成からの経過時間.
        delta += Time.deltaTime;

        if (delta >= cerateSpan)
        {
            delta = 0;      // 経過時間を初期化.
            // 生成.
            GameObject coin = Instantiate(prefab,coins.transform);

            // 生成場所をランダムに設定.
            var start = screenLeftBottom.x;
            var end = screenRightTop.x;
            var rangeX = Random.Range(start, end);

            // 生成場所を設定.
            coin.transform.position = new Vector3(rangeX, screenRightTop.y, 0);
        }
    }
}
