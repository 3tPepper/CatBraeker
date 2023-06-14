using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (PlayerMove.player_status.Equals("attack") && collision.gameObject.tag == "Enemy")    //���� ����
        {
            GetComponentInParent<PlayerMove>().StopCoroutine(PlayerMove.coroutine);
            PlayerMove.animator.SetBool("Is Attack", false);
            PlayerMove.player_status = "none";
            //�ش� enemy ��ü ü�� ����
            GameManager.instance.EnemyAttack();
            
        }
    }
}
