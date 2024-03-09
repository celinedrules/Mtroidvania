using System.Collections;
using TMPro;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private GameObject pickupMessage;

    protected string UnlockMessage;

    protected void DisplayMessage()
    {
        if(pickupMessage != null)
        {
            Time.timeScale = 0;
            pickupMessage.GetComponentInChildren<TextMeshProUGUI>().text = UnlockMessage;
            pickupMessage.transform.localScale = Vector3.right;
            pickupMessage.SetActive(true);

            StartCoroutine(PlayAudioAndAnimateMessage());
        }
        else
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        pickupMessage.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator PlayAudioAndAnimateMessage()
    {
        AudioManager.Instance.PauseAudio(AudioType.LevelMusic);
        AudioManager.Instance.PlayAudio(AudioType.PickupAttribute);

        yield return new WaitForSecondsRealtime(1.0f);
        
        yield return StartCoroutine(AnimateScale(pickupMessage.transform, Vector3.right, Vector3.one,
            0.05f));

        while (AudioManager.Instance.IsAudioPlaying(AudioType.PickupAttribute))
            yield return null;

        yield return StartCoroutine(AnimateScale(pickupMessage.transform, Vector3.one, Vector3.right, 
            0.05f));

        AudioManager.Instance.ResumeAudio(AudioType.LevelMusic);
        
        Time.timeScale = 1.0f;
        Destroy();
    }

    private IEnumerator AnimateScale(Transform target, Vector3 startScale, Vector3 endScale, float duration)
    {
        float time = 0.0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            yield return null;
        }

        target.localScale = endScale;
    }
}