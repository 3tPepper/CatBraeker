using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private static Effects _instance;
    public static Effects instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Effects>();
            }
            return _instance;
        }
    }

    public GameObject attack_fx;

    public void AtkFX(GameObject target)
    {
        GameObject fx = Instantiate(attack_fx, target.transform);
    }
}
