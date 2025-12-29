using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class NamedTrack
{
    public string key;       // e.g. "boss", "victory", "explore"
    public AudioClip clip;   // the track file
}

public class MusicManagerController : MonoBehaviour
{
    private const float m_crossFadeTime = 1.0f;
    private AudioSource m_currentTrack;
    private AudioSource m_previousTrack;
    private Coroutine crossFadeRoutine;

    [Header("Music Setup")]
    [SerializeField] AudioMixerGroup musicMixerGroup;
    [SerializeField] List<NamedTrack> namedTracks;

    private Dictionary<string, AudioClip> m_trackLibrary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // Build dictionary
        foreach (var nt in namedTracks)
        {
            if (nt != null && nt.clip != null && !string.IsNullOrEmpty(nt.key))
            {
                var lowerKey = nt.key.ToLower();
                if (!m_trackLibrary.ContainsKey(lowerKey))
                {
                    m_trackLibrary.Add(lowerKey, nt.clip);
                }
                else
                {
                    Debug.LogWarning($"Duplicate track key '{nt.key}' in MusicManager namedTracks!");
                }
            }
        }

        // Create two AudioSources to swap between
        m_currentTrack = gameObject.AddComponent<AudioSource>();
        m_previousTrack = gameObject.AddComponent<AudioSource>();

        foreach (var src in new[] { m_currentTrack, m_previousTrack })
        {
            src.playOnAwake = false;
            src.loop = true;
            src.volume = 0f;
            src.outputAudioMixerGroup = musicMixerGroup;
            src.bypassListenerEffects = true;
        }
    }

    public void Play(AudioClip clip)
    {
        if (!clip) return;

        //Debug.Log($"Play started for track: {clip.name}");

        // If current is already playing this clip, do nothing
        if (m_currentTrack.isPlaying && m_currentTrack.clip == clip)
            return;

        // Swap references (old current becomes previous)
        var temp = m_previousTrack;
        m_previousTrack = m_currentTrack;
        m_currentTrack = temp;

        // Configure new current track
        m_currentTrack.clip = clip;
        m_currentTrack.volume = 0f;
        m_currentTrack.Play();

        // Start crossfade
        if (crossFadeRoutine != null)
            StopCoroutine(crossFadeRoutine);
        crossFadeRoutine = StartCoroutine(CrossFadeTracks());
    }

    public void OnGameEvent(string eventKey)
    {
        eventKey = eventKey.ToLower();
        if (m_trackLibrary.TryGetValue(eventKey, out var track))
        {
            //Debug.Log($"Event triggered: {eventKey}, playing {track.name}");
            Play(track);
        }
    }

    private IEnumerator CrossFadeTracks()
    {
        //Debug.Log($"Crossfade started. previous: {m_previousTrack.clip?.name}, current: {m_currentTrack.clip?.name}");
        float elapsedTime = 0f;

        while (elapsedTime < m_crossFadeTime)
        {
            elapsedTime += Time.deltaTime;
            float fraction = Mathf.Clamp01(elapsedTime / m_crossFadeTime);

            if (m_previousTrack)
                m_previousTrack.volume = 1f - fraction;
            if (m_currentTrack)
                m_currentTrack.volume = fraction;

            yield return null;
        }

        // Ensure final volumes
        if (m_previousTrack) m_previousTrack.Stop();
        if (m_currentTrack) m_currentTrack.volume = 1f;
    }
}
