using UnityEngine;

public class CoinMove : MonoBehaviour
{
    #region �ϐ��錾

    [SerializeField] float fallSpeed;       // ������X�s�[�h.

    private int playerLayer;                // �v���C���[�̃��C���[�ۑ��p.

    Vector3 screenLeftBottom;               // �X�N���[���̍����̍��W.

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �X�N���[�������̃��[���h���W���擾�A���.
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // ���C���[���擾.
        playerLayer = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -fallSpeed, 0);

        if (transform.position.y < screenLeftBottom.y)  // �X�N���[����y���W�����������.
        {
            Destroy(gameObject);                        // ���̃I�u�W�F�N�g���폜.
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)  // �v���C���[���C���[�����I�u�W�F�N�g�ɐG�ꂽ��.
        {
            TouchPlayer();                              // �֐��Ăяo��.
        }
    }
    public void TouchPlayer()       // �v���C���[�ɐG�ꂽ��.
    {
        Debug.Log("�v���C���[�ɓ�������");

        Destroy(gameObject);        // ���̃I�u�W�F�N�g������.

        // �X�R�A���Z.
    }
}
