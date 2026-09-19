using UnityEngine;
using UnityEngine.UI;
using System;

public class ControlSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundClips = new AudioClip[2];
	
	private AudioSource audioSource;
	private Scrollbar soundEffectBar;
	
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		soundEffectBar = AssetManager.Instance.SoundEffectBar;
		
		if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
		
		SetInitialVolume();
	}
	
	private void OnEnable()
	{
		MoveArrow.OnCorrectMoveChanged += PlayCorrectAttempt;
		MoveArrow.OnWrongMoveChanged += PlayWrongAttempt;
		DeclareResult.OnFireworkStart += PlayFirework;
		NewScene.OnFireworkStop += StopPlayingFirework;
	}
	
	private void OnDisable()
	{
		MoveArrow.OnCorrectMoveChanged -= PlayCorrectAttempt;
		MoveArrow.OnWrongMoveChanged -= PlayWrongAttempt;
		DeclareResult.OnFireworkStart -= PlayFirework;
		NewScene.OnFireworkStop -= StopPlayingFirework;
	}
	
	private void PlayCorrectAttempt()
	{	
		audioSource.clip = soundClips[0];
        audioSource.Play();
	}
	
	private void PlayWrongAttempt()
	{		
		audioSource.clip = soundClips[1];
        audioSource.Play();
	}
	
	private void SetInitialVolume()
	{
		audioSource.volume = 1f;
		soundEffectBar.value = 1f;
	}
	
	private void PlayFirework()
	{
		audioSource.clip = soundClips[2];
        audioSource.Play();
	}
	
	private void StopPlayingFirework()
	{
		audioSource.Stop();
	}
	
	public void ControlVolume()
	{
		audioSource.volume = soundEffectBar.value;
	}
}
