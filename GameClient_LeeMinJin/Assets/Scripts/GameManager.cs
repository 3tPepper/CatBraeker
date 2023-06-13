using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    
    [SerializeField]
    GameObject enemy_folder;

    [SerializeField]
    GameObject[] player_hp = new GameObject[3];

    //Wave total
    const int wave_tot = 8;
    const float enemy_height = 1.7f;
    const float enemy_up_power = 400.0f;

    //now wave
    int now_wave = 1;
    //field enemies cnt
    int now_enemies = 0;


    //enemies object pooling
    Enemy[] enemies = new Enemy[wave_tot*5 + 5]; //wave당 적 객체 수: 10, 15, 20, 25...최대 8*5+5


    void Start()
    {
        //ui 초기화
        PlayerHPUI(PlayerMove.hp);

        GameObject[] enemies_prefab = Resources.LoadAll<GameObject>("Prefabs/Enemies/");
        int enemyP_len = enemies_prefab.Length;
        
        // 적 객체 오브젝트 풀링
        for(int i=0; i<enemies.Length; i++)
        {
            int num = Random.Range(0, enemyP_len);
            string enemyP_name = enemies_prefab[num].name;
            enemies[i] = new Enemy(Instantiate(enemies_prefab[num]), enemyP_name);
            //enemy_folder의 하위로 이동
            enemies[i].enemy.transform.parent = enemy_folder.transform;
            //비활성화
            enemies[i].enemy.SetActive(false);
        }
        StartCoroutine(Wave(now_wave));
    }

    IEnumerator Wave(int wave_num)
    {
        enemy_folder.transform.position = new Vector2(0, 5);

        //해당 wave에서 등장할 적 객체 수
        int enemy_len = wave_num * 5 + 5;
        Debug.Log(enemy_len);

        for (int i=0; i<enemy_len; i++)
        {
            yield return new WaitForSeconds(1);
            //적 객체 활성화 & 위치 지정
            enemies[i].enemy.SetActive(true);
            Vector2 now_pos = new Vector2(0, 0);

            now_pos.y = now_pos.y + (now_enemies * enemy_height);
            Debug.Log(now_pos.y);
            enemies[i].enemy.transform.localPosition = now_pos;

            //적 객체 수 카운트 변수 증가
            now_enemies++;
        }
    }

    //player와 부딪히면 enemy_folder 위로 상승한다.
    public void EnemyUp()
    {
        Rigidbody2D rb = enemy_folder.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * enemy_up_power);
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        //stop enemy
        enemy_folder.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //stop player
        PlayerMove.player_status = "game over";
    }

    //---------------UI------------------//
    public void GameStartBtn()
    {
        StartCoroutine(Wave(now_wave));
    }

    public void PlayerHPUI(int hp)
    {
        for (int i = 0; i < player_hp.Length; i++)
        {
            player_hp[i].SetActive(false);
            if (i < hp)
            {
                player_hp[i].SetActive(true);
            }
        }
    }
}
