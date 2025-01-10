using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public Button rewardedAdButton;

    public GameObject BetPanel;
    public Text betText;
    public Text currentMoneyText;

    private int playerMoney = 1000;
    private int currentBet = 0;

    public Text playerScoreText;
    public Text dealerScoreText;

    public GameObject GameOverPanel;
    public GameObject WinPanel;

    public Button hitButton;
    public Button standButton;

    public List<Card> cards = new List<Card>();
    public List<Card> playerHand = new List<Card>();
    public List<Card> dealerHand = new List<Card>();

    public Sprite[] cardSprites;
    public Image playerCardImagePrefab;
    public Image dealerCardImagePrefab;
    public Sprite hiddenCardSprite;

    public Transform playerCardParent;
    public Transform dealerCardParent;

    private int playerScore = 0;
    private int dealerScore = 0;
    private bool dealerTurn = false;

    void Start()
    {
        rewardedAdButton.gameObject.SetActive(false);

        GameOverPanel.SetActive(false);
        WinPanel.SetActive(false);
        BetPanel.SetActive(true);

        hitButton.interactable = false;
        standButton.interactable = false;

        currentMoneyText.text = "$" + playerMoney;

        InitializeDeck();
    }

    void Update()
    {
        if (playerMoney <= 0)
        {
            rewardedAdButton.gameObject.SetActive(true);
        }

        UpdateScoreText();
    }

    void InitializeDeck()
    {
        string[] suits = { "club", "diamond", "heart", "spade" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        foreach (string suit in suits)
        {
            for (int i = 0; i < ranks.Length; i++)
            {
                Card newCard = new Card();
                newCard.cardName = ranks[i] + " of " + suit;

                if (i < 9)
                {
                    newCard.cardValue = i + 2;
                }
                else if (i < 12)
                {
                    newCard.cardValue = 10;
                }
                else
                {
                    newCard.cardValue = 11;
                }

                string spriteName = ranks[i] + "_" + suit;
                Sprite cardSprite = FindCardSprite(spriteName);
                if (cardSprite != null)
                {
                    newCard.cardSprite = cardSprite;
                }

                cards.Add(newCard);
            }
        }
    }

    void StartGame()
    {
        UpdateScoreText();

        for (int i = 0; i < 2; i++)
        {
            Card drawnCard = DrawCard();
            playerHand.Add(drawnCard);
            playerScore += drawnCard.cardValue;
            ShowCard(drawnCard, playerCardParent);
        }

        for (int i = 0; i < 2; i++)
        {
            Card drawnCard = DrawCard();
            dealerHand.Add(drawnCard);

            if (i == 0)
            {
                ShowHiddenCard(dealerCardParent);
            }
            else
            {
                dealerScore += drawnCard.cardValue;
                ShowCard(drawnCard, dealerCardParent);
            }
        }

        if (playerScore == 21 && playerHand.Count == 2)
        {
            Invoke("ShowGameWin", 1f);

            playerMoney += Mathf.CeilToInt(currentBet * 2.5f);

            Debug.Log("Blackjack");
        }

        Debug.Log("Player Score: " + playerScore);
        Debug.Log("Dealer Visible Score: " + dealerScore);
    }

    void ShowGameWin()
    {
        WinPanel.SetActive(true);

        ResetGame();
    }

    public void Plus()
    {
        if (currentBet + 10 <= playerMoney)
        {
            currentBet += 10;
            betText.text = "$" + currentBet;
        }
    }

    public void Minus()
    {
        if (currentBet - 10 >= 0)
        {
            currentBet -= 10;
            betText.text = "$" + currentBet;
        }
    }

    public void SelectBet()
    {
        if (currentBet > 0)
        {
            playerMoney -= currentBet;
            currentMoneyText.text = "$" + playerMoney;

            BetPanel.SetActive(false);

            hitButton.interactable = true;
            standButton.interactable = true;

            StartGame();
        }
    }
    public void Hit()
    {
        UpdateScoreText();

        if (!dealerTurn)
        {
            Card drawnCard = DrawCard();

            if (drawnCard != null)
            {
                playerHand.Add(drawnCard);
                playerScore += drawnCard.cardValue;
                AdjustAceValueIfNecessary(ref playerScore, playerHand); // As'ı kontrol et
                ShowCard(drawnCard, playerCardParent);

                Debug.Log("Player drew: " + drawnCard.cardName + " (Score: " + playerScore + ")");

                if (playerScore > 21)
                {
                    Invoke("GameOver", 1.5f);

                    hitButton.interactable = false;
                    standButton.interactable = false;

                    Debug.Log("Player Bust! You lose.");
                }
            }
        }
    }

    void GameOver()
    {
        GameOverPanel.SetActive(true);

        Invoke("ResetGame", 1f);
    }

    public void ResetGame()
    {
        currentBet = 0;
        betText.text = "$0";
        BetPanel.SetActive(true);

        foreach (Transform child in playerCardParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in dealerCardParent)
        {
            Destroy(child.gameObject);
        }

        playerHand.Clear();
        dealerHand.Clear();

        playerScore = 0;
        dealerScore = 0;
        UpdateScoreText();

        hitButton.interactable = false;
        standButton.interactable = false;

        dealerTurn = false;
        cards.Clear();

        InitializeDeck();
    }

    public void Stand()
    {
        Debug.Log("Player stands with a score of: " + playerScore);
        dealerTurn = true;
        RevealHiddenCard();
        DealerTurn();
    }

    void DealerTurn()
    {
        UpdateScoreText();

        while (dealerScore < 17 || dealerScore < playerScore || dealerScore == playerScore)
        {
            Card drawnCard = DrawCard();

            if (drawnCard != null)
            {
                dealerHand.Add(drawnCard);
                dealerScore += drawnCard.cardValue;
                AdjustAceValueIfNecessary(ref dealerScore, dealerHand);
                ShowCard(drawnCard, dealerCardParent);

                Debug.Log("Dealer drew: " + drawnCard.cardName + " (Score: " + dealerScore + ")");

                if (dealerScore > 21)
                {
                    Invoke("GameWin", 1.5f);

                    hitButton.interactable = false;
                    standButton.interactable = false;

                    Debug.Log("Dealer Bust! You win.");
                    return;
                }
            }
        }

        if (dealerScore == playerScore && dealerScore <= 21)
        {
            Card extraCard = DrawCard();
            if (extraCard != null)
            {
                dealerHand.Add(extraCard);
                dealerScore += extraCard.cardValue;
                ShowCard(extraCard, dealerCardParent);

                Debug.Log("Dealer drew extra card: " + extraCard.cardName + " (Score: " + dealerScore + ")");

                if (dealerScore > 21)
                {
                    Invoke("GameWin", 1.5f);

                    hitButton.interactable = false;
                    standButton.interactable = false;

                    Debug.Log("Dealer Bust! You win.");
                    return;
                }
            }
        }

        if (dealerScore > playerScore && dealerScore <= 21)
        {
            Invoke("GameOver", 1.5f);

            hitButton.interactable = false;
            standButton.interactable = false;

            Debug.Log("Dealer wins.");
        }
        else
        {
            Invoke("GameWin", 1.5f);

            hitButton.interactable = false;
            standButton.interactable = false;

            Debug.Log("Player wins.");
        }
    }

    void GameWin()
    {
        playerMoney += currentBet * 2;

        currentMoneyText.text = "$" + playerMoney;
        WinPanel.SetActive(true);

        Invoke("ResetGame", 1f);
    }

    Card DrawCard()
    {
        if (cards.Count == 0)
        {
            Debug.LogWarning("No more cards to draw.");
            return null;
        }

        int randomIndex = Random.Range(0, cards.Count);
        Card drawnCard = cards[randomIndex];
        cards.RemoveAt(randomIndex);

        return drawnCard;
    }

    void UpdateScoreText()
    {
        playerScoreText.text = " " + playerScore;
        dealerScoreText.text = " " + dealerScore;
    }

    void ShowCard(Card card, Transform parent)
    {
        Image newCardImage = Instantiate(playerCardImagePrefab, parent);
        newCardImage.sprite = card.cardSprite;

        newCardImage.transform.localPosition = new Vector3(parent.childCount * 30f, 0f, 0f);
    }

    void ShowHiddenCard(Transform parent)
    {
        Image hiddenCard = Instantiate(dealerCardImagePrefab, parent);
        hiddenCard.sprite = hiddenCardSprite;

        hiddenCard.transform.localPosition = new Vector3(parent.childCount * 30f, 0f, 0f);
    }

    void RevealHiddenCard()
    {
        dealerScore += dealerHand[0].cardValue;
        dealerCardParent.GetChild(0).GetComponent<Image>().sprite = dealerHand[0].cardSprite;

        UpdateScoreText();
    }

    void AdjustAceValueIfNecessary(ref int score, List<Card> hand)
    {
        foreach (var card in hand)
        {
            if (card.cardName.Contains("Ace"))
            {
                // Eğer diğer kartların toplamı 10'dan büyükse Ace 1 olur
                int otherCardsScore = score - (card.cardValue == 11 ? 11 : 1);

                if (otherCardsScore > 10)
                {
                    score -= (card.cardValue == 11 ? 10 : 0); // Ace 1 olur
                    card.cardValue = 1;
                }
                else
                {
                    if (card.cardValue == 1)
                    {
                        score += 10; // Ace 11 olur
                        card.cardValue = 11;
                    }
                }
            }
        }
    }

    Sprite FindCardSprite(string spriteName)
    {
        foreach (Sprite sprite in cardSprites)
        {
            if (sprite.name == spriteName)
            {
                return sprite;
            }
        }
        Debug.LogWarning("Sprite not found: " + spriteName);
        return null;
    }
}