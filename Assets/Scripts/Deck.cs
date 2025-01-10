using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> shuffledDeck;  // Karýþtýrýlmýþ kart destesi
    private CardManager cardManager;  // CardManager referansý

    void Start()
    {
        // CardManager bileþenine eriþ
        cardManager = FindObjectOfType<CardManager>();

        // Eðer CardManager'deki kartlar yüklenmiþse desteyi karýþtýr
        if (cardManager != null && cardManager.cards.Count > 0)
        {
            ShuffleDeck();
        }
        else
        {
            Debug.LogError("CardManager veya kartlar bulunamadý.");
        }
    }

    // Kart destesini karýþtýrma iþlemi
    public void ShuffleDeck()
    {
        shuffledDeck = new List<Card>(cardManager.cards);  // CardManager'den kartlarý al
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            Card temp = shuffledDeck[i];
            int randomIndex = Random.Range(i, shuffledDeck.Count);
            shuffledDeck[i] = shuffledDeck[randomIndex];
            shuffledDeck[randomIndex] = temp;
        }

        Debug.Log("Kartlar karýþtýrýldý!");
    }

    // Bir kart çekme iþlemi
    public Card DrawCard()
    {
        if (shuffledDeck.Count == 0)
        {
            Debug.Log("Deck is empty");
            return null;
        }

        Card cardToDraw = shuffledDeck[0];
        shuffledDeck.RemoveAt(0);
        return cardToDraw;
    }
}