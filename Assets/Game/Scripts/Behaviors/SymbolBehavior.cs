using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SymbolBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _symbols;
    private int _symbolID;
    private Image _symbolImage;

    public void SetSymbol(int symbolID)
    {
        _symbolID = symbolID;
        for (int i = 0; i < _symbols.Length; i++)
        {
            if (_symbols[i].activeSelf)
            {
                _symbols[i].SetActive(false);
            }
        }
        _symbols[symbolID].SetActive(true);
    }


    public int GetSelectedSymbol()
    {
        return _symbolID;
    }

    public void Blur()
    {
        if (_symbolImage == null)
        {
            _symbolImage = _symbols[_symbolID].GetComponent<Image>();
        }
        _symbolImage.DOFade(1, 0.0004f);
        _symbolImage.DOFade(0, 0.45f).OnComplete(delegate {
            _symbolImage.DOFade(1, 0.45f);
        });
       
    }

    public void ClearAnimation()
    {
        _symbolImage.DOKill();
        _symbolImage.DOFade(1, 0.0004f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colliding!");
    }
}
