using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoPressController : MonoBehaviour
{
    public Animator animator;
    private bool isPlaying = false;

    private Camera piano_camera;

    void Start() 
    {
        animator = gameObject.GetComponent<Animator>();
        piano_camera = GameObject.FindWithTag("PianoCamera").GetComponent<Camera>();
    }

    void OnMouseDown()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        isPlaying = true;
        Ray ray = piano_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                animator.SetTrigger("PlayAnim");
            }
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.ResetTrigger("PlayAnim");
        isPlaying = false;
    }
}
