using UnityEngine;

public class SymbolBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _symbols;

    public void SetSymbol(int symbolID)
    {
        for (int i = 0; i < _symbols.Length; i++)
        {
            if (_symbols[i].activeSelf)
            {
                _symbols[i].SetActive(false);
            }
        }
        _symbols[symbolID].SetActive(true);
    }
}
