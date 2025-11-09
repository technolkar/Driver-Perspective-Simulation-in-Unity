
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class NavMeshVisualizer : MonoBehaviour
{
    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.AI.NavMeshVisualizationSettings.showNavigation = true;
#endif
    }
}
