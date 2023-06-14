using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    const int atk = 1000;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (PlayerMove.player_status.Equals("attack"))    //���� ����
        {
            GetComponentInParent<PlayerMove>().StopCoroutine(PlayerMove.coroutine);
            //�ش� enemy ��ü ü�� ����
            GameManager.instance.EnemyAttack(atk);
            PlayerMove.player_status = "none";
        }
    }
}
