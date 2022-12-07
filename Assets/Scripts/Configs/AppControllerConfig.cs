using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "AppControllerConfig", menuName = "Matterless/AppControllerConfig")]
public class AppControllerConfig : ScriptableObject
{
    public GameObject mainMenuViewPrefab;
    public EventSystem eventSystemPrefab;
    public Canvas canvasPrefab;
    public GameObject inGameViewPrefab;
}