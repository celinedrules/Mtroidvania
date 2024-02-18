using UnityEngine;

public class IndentAttribute : PropertyAttribute
{
    public int indentLevel;

    public IndentAttribute(int indentLevel = 1)
    {
        this.indentLevel = indentLevel;
    }
}