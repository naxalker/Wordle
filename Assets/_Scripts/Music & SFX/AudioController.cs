using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private SoundEffectsSO _soundEffects;
    [SerializeField] private Board _board;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _board.OnLetterPlaced += LetterPlacedHandler;
    }

    private void OnDestroy()
    {
        _board.OnLetterPlaced -= LetterPlacedHandler;
    }

    private void LetterPlacedHandler() => PlayClip(_soundEffects.ClickSound);

    private void PlayClip(AudioClip clip, bool pitched = false)
    {
        if (pitched)
            _audioSource.pitch = Random.Range(.8f, 1.2f);

        _audioSource.clip = clip;
        _audioSource.Play();

        if (pitched)
            _audioSource.pitch = 1f;
    }
}
