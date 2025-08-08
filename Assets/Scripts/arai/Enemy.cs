using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //定数:敵種類
    private const int NORMAL = 0;
    private const int STOP = 1;

    [SerializeField] private LayerMask playerChecklayer; //Player当たり判定用のレイヤー
    private bool isHit;                                  //当ったかのフラグ
    private bool hitOne;                                 //RayCast判定1回だけにする用フラグ
    private float count;                                 //確認用（仮）
    private float m_speed;                               //敵スピード
    private int m_state;                                 //敵種類

    void SetState(int state) { m_state = state; }        //敵の種類セット関数
    int GetState() { return m_state; }                   //敵の種類セットしたのをゲットするゲッター関数

    void SetSpeed(float speed) { m_speed = speed; }      //敵のスピードセット関数
    float GetSpeed() {  return m_speed; }                //敵のスピードセットしたのをゲットするゲッター関数


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
        StartCoroutine(EnemyBehaviorLoop());
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyMove(10, NORMAL);
        count = EnemyManager.count;
    }

    IEnumerator EnemyBehaviorLoop()
    {
        while (true)
        {
            //状態切り替え（仮）試し用
            if (count > 50)
            {
                SetState(NORMAL);
                SetSpeed(5);
            }
            else if (count > 30)
            {
                SetState(STOP);
                SetSpeed(5);
            }
            else if (count > 15)
            {
                SetState(NORMAL);
                SetSpeed(10);
            }
            else
            {
                SetState(STOP);
                SetSpeed(10);
            }

            if (GetState() == STOP)
            {
                if (!hitOne) { isHit = IsHitPlayer(); }
                if (isHit)
                {
                    //Debug.Log("プレイヤーにヒット、2秒停止");
                    yield return new WaitForSeconds(2.0f);
                    isHit = false;
                    hitOne = true;
                }
            }

            //毎フレーム移動
            if(!isHit)
            {
                EnemyMove(GetSpeed());
            }

            yield return null;
        }
    }

    void Init()
    {
        isHit = false;
        hitOne = false;
        count = 0;
    }

    void EnemyMove(float speed)
    {
        Vector3 velocity = Vector3.zero;
        velocity.x = speed;
        transform.position += transform.rotation * velocity * Time.deltaTime;
    }

    //進む方向にRayを飛ばしてplayerとの当たり判定を行う
    private bool IsHitPlayer()
    {
        float rayLength = 1.5f;                //Rayの距離
        var velocity = Vector3.zero;
        Vector2 origin = transform.position; //Rayの始点
        transform.position += transform.rotation * velocity;

        //進む方向にRaycast（playerChecklayerに当たったらhit）
        RaycastHit2D hit = Physics2D.Raycast(origin, transform.right, rayLength, playerChecklayer);

        //デバッグ用にRayを表示
        Debug.DrawRay(origin, transform.right * rayLength, Color.green);

        return hit.collider != null; //playerに当たったらtrue
    }

    //敵削除（まだ入れてない）
    void EnemyDestroy()
    {
        //Destroy()
    }
}