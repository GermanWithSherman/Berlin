using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SexSelectDialogPrefab : Dialog
{
    public void genderSelect(string gender)
    {
        data["gender"] = gender;
        submit();
    }
}
