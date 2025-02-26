using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Bgm { None = -1, Lobby, stage1, stage2, stage3, stage4, stage5, stage6, stage7, stage8, stage9, stage10 }
public enum Sfx
{
    Button, Lose, Success1, Success2, Success3, Success4, TypeFailed, Typing, Win
}

public class AudioManager : Singleton<AudioManager>
{
    protected override void Awake()
    {
        base.Awake();
        if (Instance == this)
        {
            Init();
        }
    }
    [Header("MIXER")]
    [SerializeField] private AudioMixer _audioMixer;

    [Header("BGM")]
    [SerializeField] private AudioClip[] _bgmClips;
    private AudioSource _bgmPlayer;

    [Header("SFX")]
    [SerializeField] private AudioClip[] _sfxClips;
    public int sfxChannels;
    private AudioSource[] _sfxPlayers;
    private int _channelIndex;

    private Dictionary<string, Bgm> _stageBgmMap = new Dictionary<string, Bgm>();

    private void Init()
    {
        AudioMixerGroup[] group = _audioMixer.FindMatchingGroups("Master");

        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.outputAudioMixerGroup = group[1];
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.volume = 0.7f;
        _bgmPlayer.loop = true;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[sfxChannels];
        for (int i = 0; i < sfxChannels; i++)
        {
            _sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[i].outputAudioMixerGroup = group[2];
            _sfxPlayers[i].playOnAwake = false;
            _sfxPlayers[i].loop = false;
        }

        _stageBgmMap.Add(Bgm.stage1.ToString(), Bgm.stage1);
        _stageBgmMap.Add(Bgm.stage2.ToString(), Bgm.stage2);
        _stageBgmMap.Add(Bgm.stage3.ToString(), Bgm.stage3);
        _stageBgmMap.Add(Bgm.stage4.ToString(), Bgm.stage4);
        _stageBgmMap.Add(Bgm.stage5.ToString(), Bgm.stage5);
        _stageBgmMap.Add(Bgm.stage6.ToString(), Bgm.stage6);
        _stageBgmMap.Add(Bgm.stage7.ToString(), Bgm.stage7);
        _stageBgmMap.Add(Bgm.stage8.ToString(), Bgm.stage8);
        _stageBgmMap.Add(Bgm.stage9.ToString(), Bgm.stage9);
        _stageBgmMap.Add(Bgm.stage10.ToString(), Bgm.stage10);
    }

    public void PlayStageBgm(string stageName)
    {
        if (_stageBgmMap.ContainsKey(stageName))
        {
            PlayBgm(_stageBgmMap[stageName]);
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxChannels; i++)
        {
            int loopIndex = (i + _channelIndex) % sfxChannels;

            if (_sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            _channelIndex = loopIndex;
            _sfxPlayers[_channelIndex].clip = _sfxClips[(int)sfx];
            _sfxPlayers[_channelIndex].Play();
            break;
        }
    }

    public void PlayBgm(Bgm bgm)
    {
        if (bgm == Bgm.None)
        {
            _bgmPlayer.clip = null;
            return;
        }

        int random = 0;


        _bgmPlayer.clip = _bgmClips[(int)bgm + random];
        _bgmPlayer.Play();
    }
}