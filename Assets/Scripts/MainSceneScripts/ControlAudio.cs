using UnityEngine;

public class ControlAudio : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	
	void Start()
	{
		PlaySound();
	}
	
    private void PlaySound()
	{
		audioSource.loop = true;
		audioSource.Play();
	}
}
