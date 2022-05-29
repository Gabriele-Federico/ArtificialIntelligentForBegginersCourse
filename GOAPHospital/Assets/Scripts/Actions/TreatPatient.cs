using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreatPatient : GAction
{

    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle"); 
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("TreatingPatient", 1);
        GWorld.Instance.AddCubicle(target);
        inventory.RemoveItem(target);
        GWorld.Instance.GetWorld().ModifyState("FreeCubicle", 1);
        return true;
    }
}
