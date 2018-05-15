using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData.asset", menuName = "Assets/Interaction/DialogData", order = 2)]
public class DialogData : ScriptableObject
{
    public List<DialogAction> actions;
}
