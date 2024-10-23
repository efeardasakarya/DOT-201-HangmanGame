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

    // TextMeshPro bile�enleri
    public TextMeshProUGUI wordDisplay; // Kelimeyi g�sterecek TextMeshPro bile�eni
    public TextMeshProUGUI wrongGuessesDisplay; // Yanl�� tahminleri g�sterecek TextMeshPro bile�eni
    public int maxWrongGuesses = 6;

    private int wrongGuessCount = 0; // Yanl�� tahmin say�s�n� tutacak
    private List<char> wrongGuesses = new List<char>(); // Yanl�� tahmin edilen harfleri tutacak

    public GameObject[] hangmanStages; // Adam asmaca par�alar�

    // Input Field ve Button
    public TMP_InputField letterInputField; // Harf giri� alan� (Input Field)
    public TextMeshProUGUI messageDisplay; // Kullan�c�ya mesaj g�stermek i�in (do�ru ya da yanl��)
    public GameObject submitButton; // Tahmini onaylamak i�in buton

    // Start metodunda rastgele bir kelime se�ilir ve adam asmaca par�alar� gizlenir
    void Start()
    {
        chosenWord = wordList[Random.Range(0, wordList.Length)].ToUpper();
        guessedWord = new char[chosenWord.Length];
        for (int i = 0; i < guessedWord.Length; i++)
        {
            guessedWord[i] = '_'; // Ba�lang��ta t�m harfler "_" olarak g�sterilir
        }
        UpdateWordDisplay();

        // T�m adam asmaca par�alar�n� ba�lang��ta gizle
        foreach (GameObject part in hangmanStages)
        {
            part.SetActive(false); // T�m par�alar ba�lang��ta gizlenir
        }

        // Submit butonuna t�klanma olay�n� dinler
        submitButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SubmitLetter);
    }

    // Kullan�c�n�n tahminini al�r ve kontrol eder
    public void SubmitLetter()
    {
        string input = letterInputField.text.ToUpper(); // Input field'dan harfi al�r ve b�y�k harfe �evirir
        letterInputField.text = ""; // Harf girildikten sonra input'u temizler

        if (input.Length == 1)
        {
            char letter = input[0];

            // E�er kelimede varsa do�ru tahmin
            if (chosenWord.Contains(letter.ToString()))
            {
                for (int i = 0; i < chosenWord.Length; i++)
                {
                    if (chosenWord[i] == letter)
                    {
                        guessedWord[i] = letter; // Do�ru tahmin edilen harf yerle�tirilir
                    }
                }
                messageDisplay.text = "Do�ru!";
            }
            else
            {
                // E�er yanl�� tahmin yap�ld�ysa
                if (!wrongGuesses.Contains(letter))
                {
                    wrongGuesses.Add(letter); // Yanl�� tahmin edilen harfi ekler
                    wrongGuessesDisplay.text = "Wrong Guesses: " + string.Join(", ", wrongGuesses);
                    wrongGuessCount++; // Yanl�� tahmin say�s�n� art�r
                    UpdateHangmanStage(); // Yanl�� tahmin yap�ld�k�a adam�n par�alar�n� g�ster
                    messageDisplay.text = "Yanl��!";
                }
                else
                {
                    messageDisplay.text = "Bunu zaten girdin!";
                }
            }

            // Her durumda kelimeyi g�ncelle
            UpdateWordDisplay();
            CheckGameStatus();
        }
        else
        {
            messageDisplay.text = "Ge�erli bir harf gir";
        }
    }

    // Kelimeyi ekranda g�nceller
    void UpdateWordDisplay()
    {
        wordDisplay.text = string.Join(" ", guessedWord); // TextMeshPro kullanarak metni g�ncelle
    }

    // Adam asmaca resmini g�nceller
    void UpdateHangmanStage()
    {
        if (wrongGuessCount <= hangmanStages.Length)
        {
            hangmanStages[wrongGuessCount - 1].SetActive(true); // Sadece bir par�ay� g�ster
        }
    }

    // Oyun durumunu kontrol eder
    void CheckGameStatus()
    {
        if (wrongGuessCount >= maxWrongGuesses)
        {
            messageDisplay.text = "Kaybettin! Do�ru Kelime: " + chosenWord;
            submitButton.SetActive(false); // Oyun bitti�inde butonu devre d��� b�rak
        }
        else if (new string(guessedWord) == chosenWord)
        {
            messageDisplay.text = "Kazand�n!";
            submitButton.SetActive(false); // Kazan�ld���nda butonu devre d��� b�rak
        }
    }
}
