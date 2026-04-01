using System;

[Serializable]
public class DialogChoice
{
    public string choiceText;
    public DialogData nextDialog; // null = end dialog after this choice
}
