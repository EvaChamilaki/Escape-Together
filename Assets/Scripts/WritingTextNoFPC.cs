using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WritingTextNoFPC : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;
    public string[] stringArray;
    [SerializeField] float timeBetweenCharacters;
    [SerializeField] float timeBetweenWords;
    
    void Start()
    {
        StartCoroutine(TextVisible());
        Cursor.lockState = CursorLockMode.None;
    }
    
    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if(visibleCount >= totalVisibleCharacters)
            {
                GetComponent<AudioSource>().Stop();
                yield break;
            }
            counter += 1;

            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }
}
