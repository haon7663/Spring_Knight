using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinMark : MonoBehaviour
{
    public Vector2 randomMark;
    void Start()
    {
        var charType = Character.Inst.characters[(int)Character.Inst.playerType].skillObjPrf_2;

        var ran = Random.Range(0, 4);
        switch (ran)
        {
            case 0:
                randomMark = Vector2.right;
                break;
            case 1:
                randomMark = Vector2.down;
                break;
            case 2:
                randomMark = Vector2.left;
                break;
            case 3:
                randomMark = Vector2.up;
                break;
        }
    }

    public bool AssassinKill(Vector2 value)
    {
        return randomMark == value;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, randomMark * 3);
    }
}
