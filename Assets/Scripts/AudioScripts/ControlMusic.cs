using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlaylist : MonoBehaviour
{
	private Data gameData;
	
    [SerializeField] private AudioClip[] musicClips = new AudioClip[8];
    private AudioSource audioSource;
    private List<AudioClip> playlist = new List<AudioClip>();
	
	private Scrollbar musicBar;

    void Start()
    {
		gameData = AssetManager.Instance.GameData;
		
        audioSource = GetComponent<AudioSource>();
		musicBar = AssetManager.Instance.MusicBar;
		
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
		
		SetInitialVolume();
		
        StartCoroutine(PlayMusicRoutine());
    }

    private void ShufflePlaylist()
    {
        playlist.Clear();
        playlist.AddRange(musicClips);

        for (int i = 0; i < playlist.Count; i++)
        {
            AudioClip temp = playlist[i];
            int randomIndex = Random.Range(i, playlist.Count);
            playlist[i] = playlist[randomIndex];
            playlist[randomIndex] = temp;
        }
    }

    private IEnumerator PlayMusicRoutine()
    {
        while (true)
        {
            ShufflePlaylist();

            for (int i = 0; i < playlist.Count; i++)
            {
                if (playlist[i] != null)
                {
                    audioSource.clip = playlist[i];
                    audioSource.Play();
                    yield return new WaitForSeconds(audioSource.clip.length);
                }
            }
        }
    }
	
	private void SetInitialVolume()
	{
		audioSource.volume = gameData.musicVolume;
		musicBar.value = gameData.musicVolume;
	}
	
	public void ControlVolume()
	{
		audioSource.volume = musicBar.value;
		gameData.musicVolume = musicBar.value;
	}
}
