using UnityEngine;

public class CircleMove : MonoBehaviour
{
    [Header("円運動の設定")]
    public float radius = 0.1f;        // 半径
    public float speed = 100f;         // 回転速度
    [SerializeField] Transform _parentTransform;
    Vector3 center;  // 中心点

    private float angle = 0f;        // 現在の角度

    void Start()
    {
        center = _parentTransform.position;
    }

    void Update()
    {
        // 角度を時間に応じて増加
        angle += speed * Time.deltaTime;

        // 三角関数で円の座標を計算
        float x = center.x + radius * Mathf.Cos(angle);
        float z = center.z + radius * Mathf.Sin(angle);

        // オブジェクトの位置を更新
        transform.position = new Vector3(x, center.y, z);
    }
}