using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimatorGirl : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private int animIndex;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        AnimatorClipInfo[] clips = this.anim.GetCurrentAnimatorClipInfo(0);
        float time = clips[0].clip.length;
        anim.Play("Idle", 0, Random.Range(0, time));

        gameManager = GameManager.Instance;
        gameManager.GameLastWin += DanceTime;
    }

    public void DanceTime()
    {
        anim.SetInteger("Dance", animIndex);
    }
   
}
