using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Item
{
    public int m_Count;
    public bool isCalled;

    public override void UseItem()
    {
        if (isCalled) return;

        Movement.Inst.count += m_Count;
        UIManager.Inst.SetPower(Movement.Inst.count);
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
