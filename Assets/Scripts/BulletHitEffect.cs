using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    public Transform player;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().GetComponent<Transform>();

        anim = GetComponent<Animator>();
        anim.PlayInFixedTime("bullet_hit_0");

    }

    private void Update()
    {
        transform.forward = player.forward.normalized;

        //destroy the object when the animation ends (4 seconds)
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
        }
    }
}