using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmSource;  // Background music source
    public AudioSource sfxSource;  // Separate source for sound effects

    public AudioClip gameOverClip;
    public AudioClip eggCollectedClip;
    public AudioClip eggBrokenClip;
    public AudioClip eggBonusClip;
    public AudioClip ClickSound;

    private float originalVolume;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
         if (bgmSource == null)
        {
            Debug.LogError("BGM Source is missing!");
            return;
        }

        Debug.Log("Playing audio: " + bgmSource.clip.name);
        bgmSource.Play();  

        originalVolume = bgmSource.volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    void Update()
    {
        if (!bgmSource.isPlaying)
        {
         Debug.Log("BGM is NOT playing!");
        }
    }

    // 🎵 Function 1: Play Game Over Music 🎵
    public void PlayGameOverMusic()
    {
        StartCoroutine(FadeOutAndPlay(gameOverClip));
    }

    // 🎵 Function 2: Play Egg Collected Sound 🎵
    public void PlayEggCollectedSound()
    {
        sfxSource.PlayOneShot(eggCollectedClip);
    }

    public void PlayClickSound()
    {
        sfxSource.PlayOneShot(ClickSound);
    }

    // 🎵 Function 3: Play Egg Broken Sound 🎵
    public void PlayEggBrokenSound()
    {
        sfxSource.PlayOneShot(eggBrokenClip);
    }

    public void PlayEggBonusSound()
    {
        sfxSource.PlayOneShot(eggBonusClip);
    }

    private IEnumerator FadeOutAndPlay(AudioClip newClip)
    {
        float fadeDuration = 0.5f; // 1.5 seconds fade time
        float startVolume = bgmSource.volume;

        // **Step 1: Fade Out BGM**
        while (bgmSource.volume > 0.05f)
        {
            bgmSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        bgmSource.volume = 0;
        bgmSource.Pause();

        // **Step 2: Play the new clip (Game Over music)**
        sfxSource.clip = newClip;
        sfxSource.Play();

        // **Step 3: Wait for the new clip to finish**
        yield return new WaitForSeconds(newClip.length);

        // **Step 4: Resume and Fade In BGM**
        bgmSource.UnPause();
        while (bgmSource.volume < originalVolume)
        {
            bgmSource.volume += originalVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        bgmSource.volume = originalVolume;
    }
}