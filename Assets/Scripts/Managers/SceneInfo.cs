using UnityEngine;


[CreateAssetMenu(fileName ="SceneInfo", menuName = "Persistence")]
public class SceneInfo : ScriptableObject
{
    public bool GlobalDimmingIsOn;
    public bool GlobalEyeTrackingIsOn;

    private void OnEnable()
    {
        GlobalDimmingIsOn = false;
        GlobalEyeTrackingIsOn = true;
    }
}