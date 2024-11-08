using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public enum SFX
{
    PlayerWalk,
    PlayerJump,
    PlayerAttack,
    PlayerHurt,
    PlayerDeath,
    ButtonClick
}

public class AudioManager : MonoBehaviour
{
    [Serializable]
    private struct SFXConfig
    {
        public SFX Type;
        public AudioClip AudioClip;
        public float VolumeScale;
    }

    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource EnvironmentAudioSource;
    [SerializeField] private SFXConfig[] SFXConfigs;

    private Dictionary<SFX, SFXConfig> SFXs;

    private void Awake()
    {
        SFXs = SFXConfigs.ToDictionary(sfxConfig => sfxConfig.Type, sfxConfig => sfxConfig);
    }

    // Método para tocar SFX que aceita diretamente um valor do enum SFX
    public void PlaySFX(SFX type)
    {
        if (SFXs.TryGetValue(type, out SFXConfig config) && config.AudioClip != null)
        {
            Debug.Log($"Tentando tocar SFX: {type}, AudioClip: {config.AudioClip.name}, VolumeScale: {config.VolumeScale}");
            SFXAudioSource.PlayOneShot(config.AudioClip, config.VolumeScale);
        }
        else
        {
            Debug.LogWarning($"SFX '{type}' não encontrado ou sem áudio configurado!");
        }
    }

    public void SetVolume(float volume)
    {
        SFXAudioSource.volume = Mathf.Clamp01(volume);
    }

    public void PlayEnvironmentSound(AudioClip clip, float volumeScale = 1.0f)
    {
        if (clip != null)
        {
            EnvironmentAudioSource.PlayOneShot(clip, volumeScale);
        }
    }
}
