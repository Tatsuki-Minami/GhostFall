using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpControl : MonoBehaviour
{

    private float jumpForce=10.0f;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        // 当たった相手のタグがPlayerだった場合
        if (other.gameObject.tag=="Player")
        {
            // 当たった相手のRigidbodyコンポーネントを取得して、上向きの力を加える
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
        }
    }
}
