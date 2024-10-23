using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class GameManager : MonoBehaviour
{
    // Kelime havuzu
    public string[] wordList;
    private string chosenWord;
    private char[] guessedWord;

    // TextMeshPro bileþenleri
    public TextMeshProUGUI wordDisplay; // Kelimeyi gösterecek TextMeshPro bileþeni
    public TextMeshProUGUI wrongGuessesDisplay; // Yanlýþ tahminleri gösterecek TextMeshPro bileþeni
    public int maxWrongGuesses = 6;

    private int wrongGuessCount = 0; // Yanlýþ tahmin sayýsýný tutacak
    private List<char> wrongGuesses = new List<char>(); // Yanlýþ tahmin edilen harfleri tutacak

    public GameObject[] hangmanStages; // Adam asmaca parçalarý

    // Input Field ve Button
    public TMP_InputField letterInputField; // Harf giriþ alaný (Input Field)
    public TextMeshProUGUI messageDisplay; // Kullanýcýya mesaj göstermek için (doðru ya da yanlýþ)
    public GameObject submitButton; // Tahmini onaylamak için buton

    // Start metodunda rastgele bir kelime seçilir ve adam asmaca parçalarý gizlenir
    void Start()
    {
        chosenWord = wordList[Random.Range(0, wordList.Length)].ToUpper();
        guessedWord = new char[chosenWord.Length];
        for (int i = 0; i < guessedWord.Length; i++)
        {
            guessedWord[i] = '_'; // Baþlangýçta tüm harfler "_" olarak gösterilir
        }
        UpdateWordDisplay();

        // Tüm adam asmaca parçalarýný baþlangýçta gizle
        foreach (GameObject part in hangmanStages)
        {
            part.SetActive(false); // Tüm parçalar baþlangýçta gizlenir
        }

        // Submit butonuna týklanma olayýný dinler
        submitButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SubmitLetter);
    }

    // Kullanýcýnýn tahminini alýr ve kontrol eder
    public void SubmitLetter()
    {
        string input = letterInputField.text.ToUpper(); // Input field'dan harfi alýr ve büyük harfe çevirir
        letterInputField.text = ""; // Harf girildikten sonra input'u temizler

        if (input.Length == 1)
        {
            char letter = input[0];

            // Eðer kelimede varsa doðru tahmin
            if (chosenWord.Contains(letter.ToString()))
            {
                for (int i = 0; i < chosenWord.Length; i++)
                {
                    if (chosenWord[i] == letter)
                    {
                        guessedWord[i] = letter; // Doðru tahmin edilen harf yerleþtirilir
                    }
                }
                messageDisplay.text = "Doðru!";
            }
            else
            {
                // Eðer yanlýþ tahmin yapýldýysa
                if (!wrongGuesses.Contains(letter))
                {
                    wrongGuesses.Add(letter); // Yanlýþ tahmin edilen harfi ekler
                    wrongGuessesDisplay.text = "Wrong Guesses: " + string.Join(", ", wrongGuesses);
                    wrongGuessCount++; // Yanlýþ tahmin sayýsýný artýr
                    UpdateHangmanStage(); // Yanlýþ tahmin yapýldýkça adamýn parçalarýný göster
                    messageDisplay.text = "Yanlýþ!";
                }
                else
                {
                    messageDisplay.text = "Bunu zaten girdin!";
                }
            }

            // Her durumda kelimeyi güncelle
            UpdateWordDisplay();
            CheckGameStatus();
        }
        else
        {
            messageDisplay.text = "Geçerli bir harf gir";
        }
    }

    // Kelimeyi ekranda günceller
    void UpdateWordDisplay()
    {
        wordDisplay.text = string.Join(" ", guessedWord); // TextMeshPro kullanarak metni güncelle
    }

    // Adam asmaca resmini günceller
    void UpdateHangmanStage()
    {
        if (wrongGuessCount <= hangmanStages.Length)
        {
            hangmanStages[wrongGuessCount - 1].SetActive(true); // Sadece bir parçayý göster
        }
    }

    // Oyun durumunu kontrol eder
    void CheckGameStatus()
    {
        if (wrongGuessCount >= maxWrongGuesses)
        {
            messageDisplay.text = "Kaybettin! Doðru Kelime: " + chosenWord;
            submitButton.SetActive(false); // Oyun bittiðinde butonu devre dýþý býrak
        }
        else if (new string(guessedWord) == chosenWord)
        {
            messageDisplay.text = "Kazandýn!";
            submitButton.SetActive(false); // Kazanýldýðýnda butonu devre dýþý býrak
        }
    }
}
