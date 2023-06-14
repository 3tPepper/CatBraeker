using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;


    //---player status---//
    public static int hp = 3;
    const float jump_power = 400.0f;  //player jump speed
    Boolean is_ground = true;
    public static string player_status = "none";

    public static Coroutine coroutine;
    public static Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player_status = "none";
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            is_ground = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //�÷��̾� ���� üũ
            if (player_status.Equals("defence"))        //��� ����
            {
                //enemy folder ���� ���
                GameManager.instance.EnemyUp();
                GameManager.combo_num = 0;
            }
            else
            {
                GameManager.instance.EnemyUp();
                //�÷��̾� hp����
                hp--;
                GameManager.combo_num = 0;
                GameManager.instance.PlayerHPUI(hp);
                if (hp <= 0)
                {
                    GameManager.instance.GameOver();
                }
            }
        }
    }



    //------------Button-----------//
    public void PressJumpBtn()
    {
        //if ���� ���� ������ ��� ����
        if (is_ground)
        {
            Sound.instance.JumpSound();

            rb.AddForce(Vector2.up * jump_power);
            is_ground = false;
        }

    }


    //defence ��ư ������ �ִ� ���� -> defence ����
    public void PointerDownDefBtn()
    {
        if (!player_status.Equals("game over"))
        {
            Sound.instance.DefSound();
            animator.SetBool("Is Defence", true);
            player_status = "defence";
        }

    }


    public void PointerUpDefBtn()
    {
        animator.SetBool("Is Defence", false);
        if (!player_status.Equals("game over"))
        {
            player_status = "none";
        }
    }


    public void PressAttackBtn()
    {
        //if player_status == "none" �� ��� ����
        if (player_status.Equals("none"))
        {
            Sound.instance.AtkSound();
            Effects.instance.AtkFX(gameObject);
            animator.SetBool("Is Attack", true);
            coroutine = StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        player_status = "attack";
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Is Attack", false);
        if (player_status.Equals("attack"))
        {
            player_status = "none";
        }
    }


}
