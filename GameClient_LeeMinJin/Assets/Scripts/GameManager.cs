using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    Slider enemy_hp_slider;
    [SerializeField]
    Text enemy_hp_txt;
    [SerializeField]
    Text score_txt;
    [SerializeField]
    Text wave_txt;
    [SerializeField]
    Slider wave_slider;

    //Wave total
    const int wave_tot = 4;
    int all_enemy_cnt;
    const float enemy_height = 1.7f;
    const float enemy_up_power = 600.0f;

    //now wave
    int now_wave = 1;
    //field enemies cnt
    int now_enemies = 0;    
    int front_enemy = 0;    //���� ���ο� �ִ� �� ��ü �ε���
    int solve_enemy = 0;    //��ġ�� ���� ��
    int now_score = 0;


    //enemies object pooling
    Enemy[] enemies = new Enemy[wave_tot * 5 + 5]; //wave�� �� ��ü ��: 10, 15, 20, 25...�ִ� 8*5+5


    void Start()
    {
        for(int i=1; i<=wave_tot; i++)
        {
            all_enemy_cnt += (i * 5) + 5;
        }
        GameObject[] enemies_prefab = Resources.LoadAll<GameObject>("Prefabs/Enemies/");
        int enemyP_len = enemies_prefab.Length;
        
        // �� ��ü ������Ʈ Ǯ��
        for(int i=0; i<enemies.Length; i++)
        {
            int num = Random.Range(0, enemyP_len);
            string enemyP_name = enemies_prefab[num].name;
            enemies[i] = new Enemy(Instantiate(enemies_prefab[num]), enemyP_name);
            //enemy_folder�� ������ �̵�
            enemies[i].enemy.transform.parent = enemy_folder.transform;
            //��Ȱ��ȭ
            enemies[i].enemy.SetActive(false);
        }

        //ui �ʱ�ȭ
        wave_slider.maxValue = all_enemy_cnt;
        Wave(now_wave);
        PlayerHPUI(PlayerMove.hp);
        EnemyHPUI();
    }

    void Wave(int wave_num)
    {
        WaveUI();

        enemy_folder.transform.position = new Vector2(0, 10);

        //�ش� wave���� ������ �� ��ü ��
        int enemy_len = wave_num * 5 + 5;

        for (int i=0; i<enemy_len; i++)
        {
            //�� ��ü Ȱ��ȭ & ��ġ ����
            enemies[i].enemy.SetActive(true);
            Vector2 now_pos = new Vector2(0, 0);

            now_pos.y = now_pos.y + (now_enemies * enemy_height);
            enemies[i].enemy.transform.localPosition = now_pos;

            //�� ��ü �� ī��Ʈ ���� ����
            now_enemies++;
        }
        front_enemy = 0;
    }

    //player�� �ε����� enemy_folder ���� ����Ѵ�.
    public void EnemyUp()
    {
        Rigidbody2D rb = enemy_folder.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * enemy_up_power);
    }


    //�����ϸ�, enemy�� ü���� �����Ѵ�.
    public void EnemyAttack(int atk)
    {
        enemies[front_enemy].hp -= atk;
        if(enemies[front_enemy].hp <= 0) {
            //score ����: ����Ʈ�� ���� �ִ� ü�¸�ŭ ���� ȹ��
            ScoreUI(enemies[front_enemy].max_hp);
            //init pool
            enemies[front_enemy].enemy.SetActive(false);
            enemies[front_enemy].hp = enemies[front_enemy].max_hp;

            now_enemies--;
            solve_enemy++;
            if (now_enemies > 0)
            {
                front_enemy++;
            }
        }
        WaveUI();
        ChkEnemyCnt();
        EnemyHPUI();
    }

    void ChkEnemyCnt()
    {
        if(now_enemies == 0)
        {
            if (now_wave == wave_tot)
            {
                GameClear();
            }
            else
            {
                now_wave++;
                Wave(now_wave);
            }
        }
    }


    void GameClear()
    {
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
        Wave(now_wave);
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

    public void EnemyHPUI()
    {
        enemy_hp_slider.maxValue = enemies[front_enemy].max_hp;
        enemy_hp_slider.value = enemies[front_enemy].hp;

        string hp_txt = enemy_hp_slider.value + " / " + enemy_hp_slider.maxValue;
        enemy_hp_txt.text = hp_txt;
    }

    void ScoreUI(int get_score)
    {
        now_score += get_score;
        score_txt.text = now_score.ToString();
    }

    void WaveUI()
    {
        wave_txt.text = now_wave.ToString();
        wave_slider.value = solve_enemy;
    }
}
