using UnityEngine;

public class CircleMove : MonoBehaviour
{
    [Header("�~�^���̐ݒ�")]
    public float radius = 0.1f;        // ���a
    public float speed = 100f;         // ��]���x
    [SerializeField] Transform _parentTransform;
    Vector3 center;  // ���S�_

    private float angle = 0f;        // ���݂̊p�x

    void Start()
    {
        center = _parentTransform.position;
    }

    void Update()
    {
        // �p�x�����Ԃɉ����đ���
        angle += speed * Time.deltaTime;

        // �O�p�֐��ŉ~�̍��W���v�Z
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);

        // �I�u�W�F�N�g�̈ʒu���X�V
        transform.position = new Vector3(x, center.y, z);
    }
}