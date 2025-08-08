using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyManager : MonoBehaviour
{
    //�萔:�����ʒu
    private const int RIGHT = 0;
    private const int LEFT = 1;

    [SerializeField] private GameObject enemyPrefab; //���̃I�u�W�F�N�g���Q��
    private Transform parent;                        //�w�肷��e�I�u�W�F�N�g
    private GameObject rightTarget;                  //�����E
    private GameObject leftTarget;                   //������
    public static float count;                       //���J�E���g����
    private float m_timer;                           //�����Ԋu�^�C�}�[
    private float setTime;                           //�����Ԋu�^�C�}�[�Z�b�g�p

    void SetTime(float time) {  setTime = time; }    //�����Ԋu�Z�b�g�֐�
    float GetTime() { return setTime; }              //�����Ԋu�Z�b�g�����̂��Q�b�g����Q�b�^�[�֐�

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
        //�G�̐����Ԋu�i���j�����p
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

    //�G�̐����^�C�~���O�i���j�����p
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

    //�G��������
    void EnemyGenerate(int position)
    {
        switch (position) {
            case RIGHT:
                //Prefab���V�[����ɐ���
                GameObject enemyRight = Instantiate(enemyPrefab, rightTarget.transform.position, Quaternion.Euler(0, 180, 0));

                //���������I�u�W�F�N�g��Transform��e�ɐݒ�
                enemyRight.transform.SetParent(parent, true);
                break;
            case LEFT:
                //Prefab���V�[����ɐ���
                GameObject enemyLeft = Instantiate(enemyPrefab, leftTarget.transform.position, Quaternion.identity);

                //���������I�u�W�F�N�g��Transform��e�ɐݒ�
                enemyLeft.transform.SetParent(parent, true);
                break;
        }
    }
}
