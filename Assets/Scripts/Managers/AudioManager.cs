using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private bool debug;
    [SerializeField] private AudioTrack[] tracks;

    private Hashtable _audioTable;
    private Hashtable _jobTable;

    public AudioTrack[] Tracks
    {
        get => tracks;
        set => tracks = value;
    }
    
    private enum AudioAction
    {
        Start,
        Stop,
        Restart
    }

    private class AudioJob
    {
        public readonly AudioAction Action;
        public readonly AudioType Type;
        public readonly bool Fade;
        public readonly float Delay;
        public readonly float Speed;

        public AudioJob(AudioAction action, AudioType type, bool fade, float delay, float speed = 1.0f)
        {
            Action = action;
            Type = type;
            Fade = fade;
            Delay = delay;
            Speed = speed;
        }
    }

    [Serializable]
    public class AudioObject
    {
        public AudioType type;
        public AudioClip clip;
    }

    [Serializable]
    public class AudioTrack
    {
        public AudioSource source;
        public AudioObject[] audio;
    }

    protected override void Awake()
    {
        base.Awake();
        Configure();
    }

    private void OnDisable() => Dispose();

    private void Configure()
    {
        _audioTable = new Hashtable();
        _jobTable = new Hashtable();
        GenerateAudioTable();
    }

    private void Dispose()
    {
        foreach (IEnumerator job in from DictionaryEntry entry in _jobTable select (IEnumerator) entry.Value)
            StopCoroutine(job);
    }

    private void GenerateAudioTable()
    {
        foreach (AudioTrack track in tracks)
        {
            foreach (AudioObject audioObject in track.audio)
            {
                if (_audioTable.ContainsKey(audioObject.type))
                {
                    LogWarning("You are trying to register audio [" + audioObject.type +
                               "] that has already been registered.");
                }
                else
                {
                    _audioTable.Add(audioObject.type, track);
                    Log("Registering audio [" + audioObject.type + "].");
                }
            }
        }
    }

    private void Log(string msg)
    {
        if (!debug)
            return;

        Debug.Log("[AudioController]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug)
            return;

        Debug.LogWarning("[AudioController]: " + msg);
    }

    private IEnumerator RunAudioJob(AudioJob job)
    {
        yield return new WaitForSeconds(job.Delay);

        var track = (AudioTrack) _audioTable[job.Type];
        track.source.clip = GetAudioClipFromAudioTrack(job.Type, track);

        track.source.pitch = job.Speed;
        
        switch (job.Action)
        {
            case AudioAction.Start:
                track.source.Play();
                break;
            case AudioAction.Stop:
                if (!job.Fade)
                    track.source.Stop();
                break;
            case AudioAction.Restart:
                track.source.Stop();
                track.source.Play();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (job.Fade)
        {
            float initial = job.Action is AudioAction.Start or AudioAction.Restart ? 0 : 1;
            float target = initial == 0 ? 1 : 0;
            const float duration = 1.0f;
            var timer = 0.0f;

            while (timer <= duration)
            {
                track.source.volume = Mathf.Lerp(initial, target, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            if (job.Action == AudioAction.Stop)
                track.source.Stop();
        }

        _jobTable.Remove(job.Type);
        Log("Job count: " + _jobTable.Count);

        yield return null;
    }

    private void AddJob(AudioJob job)
    {
        RemoveConflictingJobs(job.Type);

        IEnumerator runningJob = RunAudioJob(job);
        _jobTable.Add(job.Type, runningJob);
        StartCoroutine(runningJob);
        Log("Starting job on [" + job.Type + "] with the operation " + job.Action);
    }

    private void RemoveJob(AudioType type)
    {
        if (!_jobTable.ContainsKey(type))
        {
            LogWarning("Trying to stop a job [" + type + "] that is not running.");
            return;
        }

        var runningJob = (IEnumerator) _jobTable[type];
        StopCoroutine(runningJob);
        _jobTable.Remove(type);
    }

    private void RemoveConflictingJobs(AudioType type)
    {
        if (_jobTable.ContainsKey(type))
            RemoveJob(type);

        var conflictingAudio = AudioType.None;

        foreach (AudioType audioType in from DictionaryEntry entry in _jobTable
            select (AudioType) entry.Key
            into audioType
            let audioTrackInUse = (AudioTrack) _audioTable[audioType]
            let audioTrackNeeded = (AudioTrack) _audioTable[type]
            where audioTrackNeeded.source == audioTrackInUse.source
            select audioType)
        {
            conflictingAudio = audioType;
        }

        if (conflictingAudio != AudioType.None)
            RemoveJob(conflictingAudio);
    }

    private AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track) =>
        (from audioObject in track.audio where audioObject.type == type select audioObject.clip).FirstOrDefault();

    public void PlayAudio(AudioType type, bool fade = false, float delay = 0.0f, float speed = 1.0f) =>
        AddJob(new AudioJob(AudioAction.Start, type, fade, delay, speed));

    public void StopAudio(AudioType type, bool fade = false, float delay = 0.0f) =>
        AddJob(new AudioJob(AudioAction.Stop, type, fade, delay));

    public void RestartAudio(AudioType type, bool fade = false, float delay = 0.0f, float speed = 1.0f) =>
        AddJob(new AudioJob(AudioAction.Restart, type, fade, delay, speed));

    public bool IsAudioPlaying(AudioType type)
    {
        if (!_audioTable.Contains(type))
        {
            LogWarning("Trying to check a clip [" + type + "] that is not registered.");
            return false;
        }

        AudioTrack track = (AudioTrack)_audioTable[type];
        return track.source.isPlaying;
    }
}
