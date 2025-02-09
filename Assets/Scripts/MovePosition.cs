using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f; // �~�̔��a

    [SerializeField]
    private float speed = 1.0f; // �ړ����x

    private float angle = 0.0f; // ���݂̊p�x

    // Update is called once per frame
    void Update()
    {
        // �p�x���X�V
        angle += speed * Time.deltaTime;

        // �V�����ʒu���v�Z
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // �I�u�W�F�N�g�̈ʒu���X�V
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
