using UnityEngine;

[ExecuteInEditMode]
public class SpawnPointMarker : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public float size = 0.2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(transform.position + Vector3.up * size, transform.position - Vector3.up * size);
        Gizmos.DrawLine(transform.position + Vector3.right * size, transform.position - Vector3.right * size);
    }
}