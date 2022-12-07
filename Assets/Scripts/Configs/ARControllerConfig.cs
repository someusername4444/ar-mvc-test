using UnityEngine;
using UnityEngine.XR.ARFoundation;

[CreateAssetMenu(fileName = "ARControllerConfig", menuName = "Matterless/ARControllerConfig")]
public class ARControllerConfig : ScriptableObject
{
    public ARSessionOrigin arSessionOrigin;
    public ARSession arSession;
    public GameObject objectToPlace;
}
