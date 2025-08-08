using UnityEditor;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    #region �ϐ��錾

    [SerializeField] float cerateSpan;      // �����X�p��.
    [SerializeField] GameObject coinPrefab; // ��������I�u�W�F�N�g.
    [SerializeField] GameObject rockPrefab; // ��������I�u�W�F�N�g.
    [SerializeField] GameObject coins;      // ��������ꏊ.

    float delta;                            // ��������̌o�ߎ���.

    Vector3 screenLeftBottom;               // ��ʍ����̍��W.
    Vector3 screenRightTop;                 // ��ʉE��̍��W.

    public Vector3 ScreenLeftBottom => screenLeftBottom;    // �ǂݎ��p.

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �����̍��W.
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);
        // �E��̍��W.
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
        // ��������̌o�ߎ���.
        delta += Time.deltaTime;

        if (delta >= cerateSpan)
        {
            delta = 0;      // �o�ߎ��Ԃ�������.
            // ����.
            GameObject coin = Instantiate(prefab,coins.transform);

            // �����ꏊ�������_���ɐݒ�.
            var start = screenLeftBottom.x;
            var end = screenRightTop.x;
            var rangeX = Random.Range(start, end);

            // �����ꏊ��ݒ�.
            coin.transform.position = new Vector3(rangeX, screenRightTop.y, 0);
        }
    }
}
