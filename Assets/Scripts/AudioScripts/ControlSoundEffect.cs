using UnityEngine;
using UnityEngine.UI;

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
	}
	
	private void OnDisable()
	{
		MoveArrow.OnCorrectMoveChanged -= PlayCorrectAttempt;
		MoveArrow.OnWrongMoveChanged -= PlayWrongAttempt;
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
	
	public void ControlVolume()
	{
		audioSource.volume = soundEffectBar.value;
	}
}
