using UnityEngine;

public class RockMove : MonoBehaviour
{
    #region �ϐ��錾

    [SerializeField] float rockSpeed;       // ������X�s�[�h.

    private int playerLayer;                // �v���C���[�̃��C���[�ۑ��p.
    [SerializeField] private Transform playerTransform; // �v���C���[��Transform���Z�b�g
    [SerializeField] private float horizontalSpeed = 3f; // �������ǔ����x

    Vector3 screenLeftBottom;               // �X�N���[���̍����̍��W.

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �X�N���[�������̃��[���h���W���擾�A���.
        screenLeftBottom = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // ���C���[���擾.
        playerLayer = LayerMask.NameToLayer("Player");

        // �v���C���[�̍��W���擾.
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player�^�O�̃I�u�W�F�N�g��������܂���");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null)
        {
            Debug.Log("�v���C���[�̍��W���擾�ł��܂���");

            return;
        }

        // ������(X)�����v���C���[�ɋ߂Â�
        float newX = Mathf.MoveTowards(
            transform.position.x,
            playerTransform.position.x,
            horizontalSpeed * Time.deltaTime
        );

        // �c����(Y)�͈�葬�x�ŉ��ɗ�����
        float newY = transform.position.y - rockSpeed;

        // �V�������W��K�p
        transform.position = new Vector2(newX, newY);

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

    }
}
