using UnityEngine;

public class CreditsMainMenuMusic : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.InstanceAudio.PlayMusic("Credits", true);
    }
    private void OnDisable()
    {
        AudioManager.InstanceAudio.PlayMusic("Menu", true);
    }
   
}
