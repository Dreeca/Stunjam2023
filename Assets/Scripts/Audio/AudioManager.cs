using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] m_Sounds;
    private static AudioManager s_Instance;
    public static AudioManager Instance
    {
        get
        {
            return s_Instance;
        }
    }

    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        if (m_Sounds == null)
        {
            throw new Exception("Sounds array is null. Please add sound on AudioManager");
        }
        s_Instance = this;
    }

    public void Play(Vector3 _position, SoundType _type)
    {
        GameObject _audio = new GameObject(Guid.NewGuid().ToString());
        AudioSource _src = _audio.AddComponent<AudioSource>();
        AudioClip _clip = null;
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (m_Sounds[i].type == _type)
            {
                _clip = m_Sounds[i].clip;
                break;
            }
        }

        if (_clip == null)
        {
            throw new Exception($"The clip you are trying to play in NULL: {_type}");
        }

        _audio.transform.position = _position;

        if (_type != SoundType.MAIN_THEME)
        {
            Destroy(_audio, 5);
            _src.spatialBlend = 1.0f;
            _src.loop = false;
        }
        else
        {
            _src.loop = true;
        }

        _src.PlayOneShot(_clip);
    }
}


[Serializable]
public struct Sound
{
    public SoundType type;
    public AudioClip clip;
}

public enum SoundType
{
    MAIN_THEME,
    ITEM_PICKUP,
    ITEM_DROP,
    DISTANT_COMBAT_FIRE,
    CLOSE_COMBAT_FIRE,
    GUN_FIRE
}