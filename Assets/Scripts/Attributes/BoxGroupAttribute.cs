using System.Collections.Generic;
using UnityEngine;

public class BoxGroupAttribute : PropertyAttribute
{
    public string groupName;
    //public int index;
    public static Dictionary<string, int> GroupIndices = new Dictionary<string, int>();

    public int index { get; private set; }
    
    public BoxGroupAttribute(string groupName)
    {
        this.groupName = groupName;
        // this.index = index;
        
        // Automatically assign and increment the index for each group name
        if (!GroupIndices.ContainsKey(groupName))
        {
            GroupIndices[groupName] = 0;
        }
        this.index = GroupIndices[groupName];
        GroupIndices[groupName]++;
    }
}