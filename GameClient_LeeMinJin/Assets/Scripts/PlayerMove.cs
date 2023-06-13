using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;

    public static int hp = 3;
    public static string player_status = "none";
    const float jump_power = 400.0f;  //player jump speed
    Boolean is_ground = true;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
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
            }
            else if (player_status.Equals("attack"))    //���� ����
            {
                //�ش� enemy ��ü ��Ȱ��ȭ
            }
            else
            {
                GameManager.instance.EnemyUp();
                //�÷��̾� hp����
                hp--;
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
            Debug.Log("press jump btn");

            rb.AddForce(Vector2.up * jump_power);

            is_ground = false;
        }

    }


    //defence ��ư ������ �ִ� ���� -> defence ����
    public void PointerDownDefBtn()
    {
        if(!player_status.Equals("game over"))
        {
            Debug.Log("press defence btn");
            player_status = "defence";
        }

    }


    public void PointerUpDefBtn()
    {
        if (!player_status.Equals("game over"))
        {
            Debug.Log("no press defence btn");
            player_status = "none";
        }
    }


    public void PressAttackBtn()
    {
        //if player_status == "none" �� ��� ����
        if (player_status.Equals("none"))
        {
            Debug.Log("press attack btn");
            player_status = "attack";
            //animation play
        }
    }


}