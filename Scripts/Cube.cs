using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    static int staticID = 0;
    [SerializeField] private TMP_Text[] numbersText;

    [HideInInspector] public int CubeID;
    [HideInInspector] public int CubeIndex; // Hangi s�rada oldu�unu belirleyen index (meyve seviyesi)
    [HideInInspector] public Rigidbody CubeRigidbody;
    [HideInInspector] public bool IsMainCube;

    private void Awake()
    {
        CubeID = staticID++;
        CubeRigidbody = GetComponent<Rigidbody>();
    }

    // Meyveye index atayan fonksiyon (Hangi seviyedeki meyve oldu�unu belirler)
    public void SetIndex(int index)
    {
        CubeIndex = index;
    }

    // K�p numaras�n� ayarlayan fonksiyon (gerekirse)
    public void SetNumber(int number)
    {
        CubeIndex = number;
        for (int i = 0; i < numbersText.Length; i++)
        {
            numbersText[i].text = number.ToString();
        }
    }

    // Meyvenin �zerindeki say�y� almak i�in
    public string GetNumberText()
    {
        return numbersText[0].text;
    }
}
