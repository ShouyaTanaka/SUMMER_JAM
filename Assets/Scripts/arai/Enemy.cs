using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //�萔:�G���
    private const int NORMAL = 0;
    private const int STOP = 1;

    [SerializeField] private LayerMask playerChecklayer; //Player�����蔻��p�̃��C���[
    private bool isHit;                                  //���������̃t���O
    private bool hitOne;                                 //RayCast����1�񂾂��ɂ���p�t���O
    private float count;                                 //�m�F�p�i���j
    private float m_speed;                               //�G�X�s�[�h
    private int m_state;                                 //�G���

    void SetState(int state) { m_state = state; }        //�G�̎�ރZ�b�g�֐�
    int GetState() { return m_state; }                   //�G�̎�ރZ�b�g�����̂��Q�b�g����Q�b�^�[�֐�

    void SetSpeed(float speed) { m_speed = speed; }      //�G�̃X�s�[�h�Z�b�g�֐�
    float GetSpeed() {  return m_speed; }                //�G�̃X�s�[�h�Z�b�g�����̂��Q�b�g����Q�b�^�[�֐�


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
            //��Ԑ؂�ւ��i���j�����p
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
                    //Debug.Log("�v���C���[�Ƀq�b�g�A2�b��~");
                    yield return new WaitForSeconds(2.0f);
                    isHit = false;
                    hitOne = true;
                }
            }

            //���t���[���ړ�
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

    //�i�ޕ�����Ray���΂���player�Ƃ̓����蔻����s��
    private bool IsHitPlayer()
    {
        float rayLength = 1.5f;                //Ray�̋���
        var velocity = Vector3.zero;
        Vector2 origin = transform.position; //Ray�̎n�_
        transform.position += transform.rotation * velocity;

        //�i�ޕ�����Raycast�iplayerChecklayer�ɓ���������hit�j
        RaycastHit2D hit = Physics2D.Raycast(origin, transform.right, rayLength, playerChecklayer);

        //�f�o�b�O�p��Ray��\��
        Debug.DrawRay(origin, transform.right * rayLength, Color.green);

        return hit.collider != null; //player�ɓ���������true
    }

    //�G�폜�i�܂�����ĂȂ��j
    void EnemyDestroy()
    {
        //Destroy()
    }
}