using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pushForce;
    [SerializeField] private float cubeMaxPosX;
    [Space]
    [SerializeField] private TouchSlider touchSlider;

    [Header("Bomb Mechanics")]
    [SerializeField] private int maxBombs = 3; // Maksimum bomba sayýsý
    [SerializeField] private int maxTotalBombsPerGame = 6; // Bir oyun boyunca kullanýlabilecek maksimum bomba sayýsý
    [SerializeField] private Text bombCountText; // Bomba sayýsýný gösterecek Text
    [SerializeField] private GameObject bombPanel; // Bomba arttýrma paneli
    [SerializeField] private Button bombButton; // Bombayý tetikleyen buton

    private int currentBombs;
    private int totalBombsUsedInGame;

    private Cube mainCube;
    private bool isPointerDown;
    private bool canMove;
    private bool controlEnabled = true; // Kontrolün aktif olup olmadýðýný belirten bayrak
    private Vector3 cubePos;
    private bool isBombActive;

    private void Start()
    {
        // PlayerPrefs'ten mevcut bomba sayýsýný yükle, eðer yoksa maxBombs deðerini kullan
        currentBombs = PlayerPrefs.GetInt("CurrentBombs", maxBombs);
        totalBombsUsedInGame = 0; // Bir oyun baþýnda kullanýlan bomba sayýsý sýfýrlanýr
        UpdateBombCountText();

        SpawnCube();
        canMove = true;

        // Slider eventlerini dinle:
        touchSlider.OnPointerDownEvent += OnPointerDown;
        touchSlider.OnPointerDragEvent += OnPointerDrag;
        touchSlider.OnPointerUpEvent += OnPointerUp;
    }

    private void Update()
    {
        if (isPointerDown && controlEnabled)
        {
            mainCube.transform.position = Vector3.Lerp(
               mainCube.transform.position,
               cubePos,
               moveSpeed * Time.deltaTime
            );
        }
    }

    private void OnPointerDown()
    {
        if (controlEnabled)
        {
            isPointerDown = true;
        }
    }

    private void OnPointerDrag(float xMovement)
    {
        if (isPointerDown && controlEnabled)
        {
            cubePos = mainCube.transform.position;
            cubePos.x = xMovement * cubeMaxPosX;
        }
    }

    private void OnPointerUp()
    {
        if (isPointerDown && canMove && controlEnabled)
        {
            isPointerDown = false;
            canMove = false;

            // Küpü veya bombayý ileri gönder:
            mainCube.CubeRigidbody.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);

            if (isBombActive)
            {
                BombSkill bombSkill = mainCube.GetComponent<BombSkill>();
                if (bombSkill != null)
                {
                    bombSkill.StartMoving();
                }
                isBombActive = false;
            }
            else
            {
                // Meyve veya bombanýn kontrolü devre dýþý býrakýldýktan sonra 0.3 saniye bekleyip yeni bir küp spawnlayýn.
                DisableControl(); // Kontrolü devre dýþý býrak
                Invoke("SpawnNewCube", 0.3f);
            }
        }
    }

    public void DisableControl()
    {
        controlEnabled = false; // Kontrolü devre dýþý býrak
    }

    public void EnableControl()
    {
        controlEnabled = true; // Kontrolü yeniden etkinleþtir
    }

    private void SpawnNewCube()
    {
        mainCube.IsMainCube = false;
        canMove = true;
        SpawnCube();
        EnableControl(); // Yeni küp spawnlandýktan sonra kontrolü yeniden etkinleþtir
    }

    private void SpawnCube()
    {
        mainCube = CubeSpawner.Instance.SpawnRandom();
        mainCube.IsMainCube = true;

        // cubePos deðiþkenini resetle
        cubePos = mainCube.transform.position;
    }

    public void OnBombButtonClicked()
    {
        if (currentBombs > 0 && totalBombsUsedInGame < maxTotalBombsPerGame)
        {
            Cube lastCube = CubeSpawner.Instance.GetLastSpawnedCube(); // Son spawnlanan küpü al

            if (lastCube != null)
            {
                // Mevcut mainCube'u yok et
                Destroy(lastCube.gameObject);

                // Bombayý oluþtur ve mainCube olarak ata
                mainCube = CubeSpawner.Instance.SpawnBomb();
                if (mainCube != null)
                {
                    mainCube.IsMainCube = true;

                    // Bombanýn aktif olduðunu belirle
                    isBombActive = true;

                    // cubePos deðiþkenini resetle
                    cubePos = mainCube.transform.position;

                    currentBombs--; // Bomba sayýsýný azalt
                    totalBombsUsedInGame++; // Toplam kullanýlan bomba sayýsýný artýr
                    UpdateBombCountText(); // Bomba sayýsýný güncelle
                    SaveBombCount(); // Bomba sayýsýný kaydet

                    if (totalBombsUsedInGame >= maxTotalBombsPerGame)
                    {
                        DisableBombButton();
                    }
                }
            }
        }
        else if (currentBombs <= 0)
        {
            ShowBombPanel(); // Bomba sayýsý 0 ise paneli göster
        }
    }

    private void UpdateBombCountText()
    {
        if (currentBombs > 0)
        {
            bombCountText.text = currentBombs.ToString(); // Mevcut bomba sayýsýný göster
        }
        else
        {
            bombCountText.text = "+"; // Bomba yoksa "+" iþaretini göster
        }
    }

    private void DisableBombButton()
    {
        bombButton.interactable = false; // Butonu devre dýþý býrak
    }

    private void ShowBombPanel()
    {
        bombPanel.SetActive(true); // Paneli aktif hale getir
    }

    public void OnAddBombsButtonClicked()
    {
        currentBombs += 3; // 3 bomba ekle
        UpdateBombCountText(); // Bomba sayýsýný güncelle
        SaveBombCount(); // Bomba sayýsýný kaydet
        CloseBombPanel(); // Paneli kapat
    }

    public void OnCloseBombPanelButtonClicked()
    {
        CloseBombPanel(); // Paneli kapat
    }

    private void CloseBombPanel()
    {
        bombPanel.SetActive(false); // Paneli kapat
    }

    private void SaveBombCount()
    {
        PlayerPrefs.SetInt("CurrentBombs", currentBombs); // Bomba sayýsýný kaydet
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // Slider eventlerini kaldýr:
        touchSlider.OnPointerDownEvent -= OnPointerDown;
        touchSlider.OnPointerDragEvent -= OnPointerDrag;
        touchSlider.OnPointerUpEvent -= OnPointerUp;
    }
}
