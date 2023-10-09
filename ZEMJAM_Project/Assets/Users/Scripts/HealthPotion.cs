using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    public bool isCalled;
    public override void UseItem()
    {
        HealthManager.Inst.OnHealth(1);
        if (TryGetComponent(out Animator animator))
        {
            animator.SetTrigger("use");
            isCalled = true;
        }
        else
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (isCalled)
            transform.position = Movement.Inst.transform.position;
    }

    public void RemoveItem()
    {
        Destroy(gameObject);
    }
}
