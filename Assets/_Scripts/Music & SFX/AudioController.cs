using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private SoundEffectsSO _soundEffects;
    [SerializeField] private Board _board;

    private bool _isMuted = false;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _board.OnLetterPlaced += LetterPlacedHandler;
        _board.OnInvalidWord += InvalidWordEnteredHandler;
        _board.OnGameOver += GameOverHandler;
    }

    private void OnDestroy()
    {
        _board.OnLetterPlaced -= LetterPlacedHandler;
        _board.OnInvalidWord -= InvalidWordEnteredHandler;
        _board.OnGameOver -= GameOverHandler;
    }

    public bool IsMuted => _isMuted;

    public void ToggleSound() => _isMuted = !_isMuted;

    private void LetterPlacedHandler() => PlayClip(_soundEffects.ClickSound, true);

    private void InvalidWordEnteredHandler() => PlayClip(_soundEffects.InvalidWordSound);

    private void GameOverHandler(bool hasWon, string word)
    {
        if (hasWon)
        {
            PlayClip(_soundEffects.VictorySound);
        }
        else
        {
            PlayClip(_soundEffects.LoseSound);
        }
    }

    private void PlayClip(AudioClip clip, bool pitched = false)
    {
        if (_isMuted) { return; }

        if (pitched)
            _audioSource.pitch = Random.Range(.8f, 1.2f);

        _audioSource.clip = clip;
        _audioSource.Play();

        if (pitched)
            _audioSource.pitch = 1f;
    }
}
