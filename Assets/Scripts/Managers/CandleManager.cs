using UnityEngine;
using TMPro;

public class CandleManager : MonoBehaviour
{
    #region Variables
    public TMP_Text candleText;
    private AudioManager audioManager;
    private int candleCounter = 0;
    #endregion

    #region Methods
    private void Start()
    {
        UpdateCandleText(); // updates when game starts
        audioManager = AudioManager.GetInstance;
    }

    ///<summary> ads value to candle counter</summary>
    public void AddCandle(int value)
    {
        if (value > 0) audioManager.PlaySFX(AudioManager.SFX.PickUp);
        candleCounter += value;
        UpdateCandleText();
    }

    /// <summary>
    /// checks for candle
    /// then deactivates seal and removes a candle
    /// </summary>
    public void BurnSeal(GameObject other)
    {
        bool canBurn = candleCounter > 0;

        if (canBurn)
        {
            other.SetActive(false);
            AddCandle(-1);
        }
    }

    /// <summary> updates candle text </summary>
    private void UpdateCandleText()
    {
        candleText.text = "Candles: " + candleCounter.ToString();
    }
    #endregion
}
