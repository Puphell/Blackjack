using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> shuffledDeck;  // Kar��t�r�lm�� kart destesi
    private CardManager cardManager;  // CardManager referans�

    void Start()
    {
        // CardManager bile�enine eri�
        cardManager = FindObjectOfType<CardManager>();

        // E�er CardManager'deki kartlar y�klenmi�se desteyi kar��t�r
        if (cardManager != null && cardManager.cards.Count > 0)
        {
            ShuffleDeck();
        }
        else
        {
            Debug.LogError("CardManager veya kartlar bulunamad�.");
        }
    }

    // Kart destesini kar��t�rma i�lemi
    public void ShuffleDeck()
    {
        shuffledDeck = new List<Card>(cardManager.cards);  // CardManager'den kartlar� al
        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            Card temp = shuffledDeck[i];
            int randomIndex = Random.Range(i, shuffledDeck.Count);
            shuffledDeck[i] = shuffledDeck[randomIndex];
            shuffledDeck[randomIndex] = temp;
        }

        Debug.Log("Kartlar kar��t�r�ld�!");
    }

    // Bir kart �ekme i�lemi
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