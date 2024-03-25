using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatBanner : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private float _disappearWaitTime;
    [SerializeField] private float _slideWaitTime;
    [SerializeField] private TextMeshProUGUI _infoText;
    private Animator _parentAnim;


    private void Start()
    {
        _parentAnim = GetComponent<Animator>();
        StartCoroutine(Appear("This is test string!"));
    }

    private IEnumerator Appear(string info)
    {
        _infoText.text = info;
        _anim.SetTrigger("appear");
        yield return new WaitForSeconds(_slideWaitTime);
        _parentAnim.SetTrigger("slide");
        yield return new WaitForSeconds(_disappearWaitTime);
        _anim.SetTrigger("disappear");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void CallPushDown()
    {
        StartCoroutine(PushDown());
    }

    public void CallDestroy()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator PushDown()
    {
        _parentAnim.ResetTrigger("slide");
        _parentAnim.SetTrigger("slide");
        _parentAnim.ResetTrigger("disappear");
        _anim.SetTrigger("disappear");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator Destroy()
    {
        _anim.ResetTrigger("disappear");
        _anim.SetTrigger("disappear");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
