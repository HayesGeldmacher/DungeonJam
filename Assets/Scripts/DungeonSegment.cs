using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSegment : MonoBehaviour
{
    public List<Transform> Connections = new List<Transform>();
    public List<BoxCollider> Colliders = new List<BoxCollider>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var connection in Connections)
        {
            if (connection == null) continue;
            Gizmos.DrawSphere(connection.position, .1f);
            Gizmos.DrawRay(connection.position, connection.forward/2);
        }
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(1, 0, 0, .25f);
        foreach (var collider in Colliders)
        {
            if (collider == null) continue;
            Gizmos.DrawCube(collider.center, collider.size);
        }
    }
}
