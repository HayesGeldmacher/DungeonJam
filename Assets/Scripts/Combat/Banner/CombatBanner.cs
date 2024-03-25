using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatBanner : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private float _waitTime;
    [SerializeField] private TextMeshProUGUI _infoText;



    private void Start()
    {
        StartCoroutine(Appear("This is test string!"));
    }

    private IEnumerator Appear(string info)
    {
        _infoText.text = info;
        _anim.SetTrigger("appear");
        yield return new WaitForSeconds(_waitTime);
        _anim.SetTrigger("disappear");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
