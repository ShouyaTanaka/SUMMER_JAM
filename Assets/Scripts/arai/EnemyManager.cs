using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyManager : MonoBehaviour
{
    //定数:生成位置
    private const int RIGHT = 0;
    private const int LEFT = 1;

    [SerializeField] private GameObject enemyPrefab; //元のオブジェクトを参照
    private Transform parent;                        //指定する親オブジェクト
    private GameObject rightTarget;                  //生成右
    private GameObject leftTarget;                   //生成左
    public static float count;                       //仮カウント時間
    private float m_timer;                           //生成間隔タイマー
    private float setTime;                           //生成間隔タイマーセット用

    void SetTime(float time) {  setTime = time; }    //生成間隔セット関数
    float GetTime() { return setTime; }              //生成間隔セットしたのをゲットするゲッター関数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (count > 0)
        {
            count -= Time.deltaTime;
        }
        GenerateTiming(GetTime());
    }

    void Init()
    {
        parent = GameObject.Find("Parent").transform;
        rightTarget = GameObject.Find("RightTarget");
        leftTarget = GameObject.Find("LeftTarget");
        count = 60;
        m_timer = 0;
        //敵の生成間隔（仮）試し用
        if (count >= 50)
        {
            SetTime(7);
        }
        else if (count < 50 && count > 20)
        {
            SetTime(5);
        }
        else if (count < 20)
        {
            SetTime(3);
        }
    }

    //敵の生成タイミング（仮）試し用
    void GenerateTiming(float setTime)
    {
        if (m_timer <= 0)
        {
            if (count >= 50)
            {
                EnemyGenerate(RIGHT);
                EnemyGenerate(LEFT);
            }
            else if (count < 50 && count > 30)
            {
                EnemyGenerate(RIGHT);
            }
            else if (count < 30 && count > 20)
            {
                EnemyGenerate(LEFT);
            }
            else if (count < 20)
            {
                EnemyGenerate(RIGHT);
                EnemyGenerate(LEFT);
            }
            m_timer = setTime;
        }
        if(m_timer > 0)
        {
            m_timer -= Time.deltaTime;
        }
        Debug.Log(m_timer);
        //Debug.Log(count);
    }

    //敵生成処理
    void EnemyGenerate(int position)
    {
        switch (position) {
            case RIGHT:
                //Prefabをシーン上に生成
                GameObject enemyRight = Instantiate(enemyPrefab, rightTarget.transform.position, Quaternion.Euler(0, 180, 0));

                //生成したオブジェクトのTransformを親に設定
                enemyRight.transform.SetParent(parent, true);
                break;
            case LEFT:
                //Prefabをシーン上に生成
                GameObject enemyLeft = Instantiate(enemyPrefab, leftTarget.transform.position, Quaternion.identity);

                //生成したオブジェクトのTransformを親に設定
                enemyLeft.transform.SetParent(parent, true);
                break;
        }
    }
}
