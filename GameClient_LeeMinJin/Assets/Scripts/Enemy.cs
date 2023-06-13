using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    //enemy list
    Dictionary<string, int> enemy_dic = new Dictionary<string, int>(){
        { "Enemy1", 1000 },
        { "Enemy2", 1500 }
    };

    public GameObject enemy;
    public int hp;

    public Enemy(GameObject enemy, string name)
    {
        this.enemy = enemy;
        hp = enemy_dic[name];
    }
}
