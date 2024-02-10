using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField] private Color pathColor = Color.red;

    public List<Transform> Points { get; } = new();

    private void Start()
    {
        foreach (Transform t in GetComponentInChildren<Transform>())
            Points.Add(t);
    }
    
    private void OnDrawGizmos()
    {
        EnemyPatroller enemy = GetComponentInParent<EnemyPatroller>();
        
        if (!enemy.ShowPath)
            return;

        var transforms = GetComponentsInChildren<Transform>();

        if (transforms.Length != Points.Count)
        {
            Points.Clear();

            for (var i = 1; i < transforms.Length; i++)
                Points.Add(transforms[i]);
        }

        Gizmos.color = pathColor;

        for (var i = 0; i < Points.Count; i++)
        {
            Gizmos.DrawSphere(Points[i].position, 0.15f);
            
            if (i + 1 >= Points.Count)
                return;
            
            if (Points[i + 1] != null)
            {
                Gizmos.DrawLine(Points[i].position, Points[i + 1].position);
            }
        }
    }
}