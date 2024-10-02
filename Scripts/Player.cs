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
    [SerializeField] private int maxBombs = 3; // Maksimum bomba say�s�
    [SerializeField] private int maxTotalBombsPerGame = 6; // Bir oyun boyunca kullan�labilecek maksimum bomba say�s�
    [SerializeField] private Text bombCountText; // Bomba say�s�n� g�sterecek Text
    [SerializeField] private GameObject bombPanel; // Bomba artt�rma paneli
    [SerializeField] private Button bombButton; // Bombay� tetikleyen buton

    private int currentBombs;
    private int totalBombsUsedInGame;

    private Cube mainCube;
    private bool isPointerDown;
    private bool canMove;
    private bool controlEnabled = true; // Kontrol�n aktif olup olmad���n� belirten bayrak
    private Vector3 cubePos;
    private bool isBombActive;

    private void Start()
    {
        // PlayerPrefs'ten mevcut bomba say�s�n� y�kle, e�er yoksa maxBombs de�erini kullan
        currentBombs = PlayerPrefs.GetInt("CurrentBombs", maxBombs);
        totalBombsUsedInGame = 0; // Bir oyun ba��nda kullan�lan bomba say�s� s�f�rlan�r
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

            // K�p� veya bombay� ileri g�nder:
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
                // Meyve veya bomban�n kontrol� devre d��� b�rak�ld�ktan sonra 0.3 saniye bekleyip yeni bir k�p spawnlay�n.
                DisableControl(); // Kontrol� devre d��� b�rak
                Invoke("SpawnNewCube", 0.3f);
            }
        }
    }

    public void DisableControl()
    {
        controlEnabled = false; // Kontrol� devre d��� b�rak
    }

    public void EnableControl()
    {
        controlEnabled = true; // Kontrol� yeniden etkinle�tir
    }

    private void SpawnNewCube()
    {
        mainCube.IsMainCube = false;
        canMove = true;
        SpawnCube();
        EnableControl(); // Yeni k�p spawnland�ktan sonra kontrol� yeniden etkinle�tir
    }

    private void SpawnCube()
    {
        mainCube = CubeSpawner.Instance.SpawnRandom();
        mainCube.IsMainCube = true;

        // cubePos de�i�kenini resetle
        cubePos = mainCube.transform.position;
    }

    public void OnBombButtonClicked()
    {
        if (currentBombs > 0 && totalBombsUsedInGame < maxTotalBombsPerGame)
        {
            Cube lastCube = CubeSpawner.Instance.GetLastSpawnedCube(); // Son spawnlanan k�p� al

            if (lastCube != null)
            {
                // Mevcut mainCube'u yok et
                Destroy(lastCube.gameObject);

                // Bombay� olu�tur ve mainCube olarak ata
                mainCube = CubeSpawner.Instance.SpawnBomb();
                if (mainCube != null)
                {
                    mainCube.IsMainCube = true;

                    // Bomban�n aktif oldu�unu belirle
                    isBombActive = true;

                    // cubePos de�i�kenini resetle
                    cubePos = mainCube.transform.position;

                    currentBombs--; // Bomba say�s�n� azalt
                    totalBombsUsedInGame++; // Toplam kullan�lan bomba say�s�n� art�r
                    UpdateBombCountText(); // Bomba say�s�n� g�ncelle
                    SaveBombCount(); // Bomba say�s�n� kaydet

                    if (totalBombsUsedInGame >= maxTotalBombsPerGame)
                    {
                        DisableBombButton();
                    }
                }
            }
        }
        else if (currentBombs <= 0)
        {
            ShowBombPanel(); // Bomba say�s� 0 ise paneli g�ster
        }
    }

    private void UpdateBombCountText()
    {
        if (currentBombs > 0)
        {
            bombCountText.text = currentBombs.ToString(); // Mevcut bomba say�s�n� g�ster
        }
        else
        {
            bombCountText.text = "+"; // Bomba yoksa "+" i�aretini g�ster
        }
    }

    private void DisableBombButton()
    {
        bombButton.interactable = false; // Butonu devre d��� b�rak
    }

    private void ShowBombPanel()
    {
        bombPanel.SetActive(true); // Paneli aktif hale getir
    }

    public void OnAddBombsButtonClicked()
    {
        currentBombs += 3; // 3 bomba ekle
        UpdateBombCountText(); // Bomba say�s�n� g�ncelle
        SaveBombCount(); // Bomba say�s�n� kaydet
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
        PlayerPrefs.SetInt("CurrentBombs", currentBombs); // Bomba say�s�n� kaydet
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // Slider eventlerini kald�r:
        touchSlider.OnPointerDownEvent -= OnPointerDown;
        touchSlider.OnPointerDragEvent -= OnPointerDrag;
        touchSlider.OnPointerUpEvent -= OnPointerUp;
    }
}
