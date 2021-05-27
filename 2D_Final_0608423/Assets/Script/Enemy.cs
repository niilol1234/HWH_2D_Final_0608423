﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("追蹤範圍"), Range(0, 500)]
    public float rangeTrack = 2;
    [Header("攻擊範圍"), Range(0, 50)]
    public float rangeAttack = 0.7f;
    [Header("移動速度"), Range(0, 50)]
    public float speed = 2;
    [Header("攻擊特效")]
    public ParticleSystem psAttack;
    [Header("攻擊冷卻時間"), Range(0, 10)]
    public float cdAttack = 3;
    [Header("攻擊力"), Range(0, 1000)]
    public float attack = 20;


    private Transform player;
    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;

    private void Start()
    {
        // 玩家變形 = 尋找遊戲物件("物件名稱").變形
        player = GameObject.Find("玩家").transform;
    }

    // 繪製圖示事件：在 Unity 內顯示輔助開發
    private void OnDrawGizmos()
    {
        // 先指定顏色再畫圖
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        // 繪製球體(中心點，半徑)
        Gizmos.DrawSphere(transform.position, rangeTrack);

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void Update()
    {
        Track();
    }
    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track()
    {
        // 距離 等於 三維向量 的 距離(A點，B點)
        float dis = Vector3.Distance(transform.position, player.position);

        // 如果 距離 小於等於 攻擊範圍 進入攻擊狀態
        // 如果 距離 小於等於 追蹤範圍 才開始追蹤
        if (dis <= rangeAttack)
        {
            Attack();
        }
        else if (dis <= rangeTrack)
        {
            // 物件 的 座標 更新為 三維向量 的 往前移動(物件 的座標，目標 的座標，速度 * 一幀的時間)
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    private void Attack()
    {
        timer += Time.deltaTime;    // 累加時間

        if ( timer >= cdAttack)
        {
            timer = 0;                  // 計時器 歸零
            psAttack.Play();            // 播放 攻擊特效

            // 2D 碰撞 = 2D 物理.覆蓋圓形範圍(中心點，半徑)
            Collider2D hit = Physics2D.OverlapCircle(transform.position, rangeAttack);
            // 碰到的物件 取得元件<玩家>().受傷(攻擊力);
            hit.GetComponent<Player>().Hit(attack);
        }  
    }
}
