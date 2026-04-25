using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Roulette : MonoBehaviour
{
    // Main roulette minigame script. Handles bet selection, wheel spinning, item effects, and win/loss results.
    // Wheel animation script. Roulette calls this, then waits for it to finish before resolving the spin.
    public WheelAnimation anim;

    // Used to hide/close the roulette game after the round ends.
    public UIManager ui;

    public Button but_0;
    public Button but_00;
    public Button but_1;
    public Button but_2;
    public Button but_3;
    public Button but_4;
    public Button but_5;
    public Button but_6;
    public Button but_7;
    public Button but_8;
    public Button but_9;
    public Button but_10;
    public Button but_11;
    public Button but_12;
    public Button but_13;
    public Button but_14;
    public Button but_15;
    public Button but_16;
    public Button but_17;
    public Button but_18;
    public Button but_19;
    public Button but_20;
    public Button but_21;
    public Button but_22;
    public Button but_23;
    public Button but_24;
    public Button but_25;
    public Button but_26;
    public Button but_27;
    public Button but_28;
    public Button but_29;
    public Button but_30;
    public Button but_31;
    public Button but_32;
    public Button but_33;
    public Button but_34;
    public Button but_35;
    public Button but_36;
    public Button but_2_1_bot;
    public Button but_2_1_mid;
    public Button but_2_1_top;
    public Button but_1st_12;
    public Button but_2nd_12;
    public Button but_3rd_12;
    public Button but_1to18;
    public Button but_Even;
    public Button but_Red;
    public Button but_Black;
    public Button but_Odd;
    public Button but_19to36;

    // Spin button the player presses after choosing a valid bet.
    public Button spin;

    // Text that shows selected bet, invalid selections, result, and win/lose messages.
    public TextMeshProUGUI selected;

    // Restriction item: only allows number bets 1-36 or Red/Black, and triples valid wins.
    // NOTE: Your earlier item ID was 10, but this uploaded file currently says 21. Change to 10 if needed.
    private const int RestrictionItemId = 21;

    // Item 33: blocks betting on 0/00. If the wheel lands on 0/00, player gets a half-win instead of losing.
    private const int ZeroProtectionItemId = 33;

    // Saves the current selected bet, for example "Red", "5", or "1st 12".
    private string currentBet = "";

    // Stops the player from clicking bets or spinning again while the wheel is moving.
    private bool spinning = false;

    // Prevents button listeners from being added more than once.
    private bool listenersAdded = false;

    // Numbers that count as red. Non-red numbers from 1-36 count as black.
    private readonly HashSet<int> redNumbers = new HashSet<int>
    {
        1, 3, 5, 7, 9, 12, 14, 16, 18,
        19, 21, 23, 25, 27, 30, 32, 34, 36
    };

    private void Awake()
    {
        RegisterButtons();
    }

    public void beginRoulette()
    {
        // Starts/resets a roulette round.
        currentBet = "";
        spinning = false;

        RegisterButtons();
        SetBoardInteractable(true);

        if (spin != null)
            spin.interactable = false;

        if (selected != null)
        {
            selected.SetText("Select a bet");
        }

        if (anim != null)
            anim.ResetToIdle();
    }

    private void RegisterButtons()
    {
        // Connects every roulette UI button to the correct bet name.
        if (listenersAdded)
            return;

        listenersAdded = true;

        AddBetListener(but_0, "0");
        AddBetListener(but_00, "00");
        AddBetListener(but_1, "1");
        AddBetListener(but_2, "2");
        AddBetListener(but_3, "3");
        AddBetListener(but_4, "4");
        AddBetListener(but_5, "5");
        AddBetListener(but_6, "6");
        AddBetListener(but_7, "7");
        AddBetListener(but_8, "8");
        AddBetListener(but_9, "9");
        AddBetListener(but_10, "10");
        AddBetListener(but_11, "11");
        AddBetListener(but_12, "12");
        AddBetListener(but_13, "13");
        AddBetListener(but_14, "14");
        AddBetListener(but_15, "15");
        AddBetListener(but_16, "16");
        AddBetListener(but_17, "17");
        AddBetListener(but_18, "18");
        AddBetListener(but_19, "19");
        AddBetListener(but_20, "20");
        AddBetListener(but_21, "21");
        AddBetListener(but_22, "22");
        AddBetListener(but_23, "23");
        AddBetListener(but_24, "24");
        AddBetListener(but_25, "25");
        AddBetListener(but_26, "26");
        AddBetListener(but_27, "27");
        AddBetListener(but_28, "28");
        AddBetListener(but_29, "29");
        AddBetListener(but_30, "30");
        AddBetListener(but_31, "31");
        AddBetListener(but_32, "32");
        AddBetListener(but_33, "33");
        AddBetListener(but_34, "34");
        AddBetListener(but_35, "35");
        AddBetListener(but_36, "36");

        AddBetListener(but_2_1_bot, "2-1 Bottom");
        AddBetListener(but_2_1_mid, "2-1 Middle");
        AddBetListener(but_2_1_top, "2-1 Top");
        AddBetListener(but_1st_12, "1st 12");
        AddBetListener(but_2nd_12, "2nd 12");
        AddBetListener(but_3rd_12, "3rd 12");
        AddBetListener(but_1to18, "1 to 18");
        AddBetListener(but_Even, "Even");
        AddBetListener(but_Red, "Red");
        AddBetListener(but_Black, "Black");
        AddBetListener(but_Odd, "Odd");
        AddBetListener(but_19to36, "19 to 36");

        if (spin != null)
            spin.onClick.AddListener(SpinRoulette);
    }

    private void AddBetListener(Button button, string betName)
    {
        // Adds a click event to one bet button.
        if (button == null)
            return;

        button.onClick.AddListener(() => SelectBet(betName));
    }

    private bool HasRestrictionItem()
    {
        // Checks PlayerData to see if the restriction/triple-damage item is active.
        if (PlayerData.Instance == null || PlayerData.Instance.items == null)
            return false;

        for (int i = 0; i < PlayerData.Instance.items.Length; i++)
        {
            if (PlayerData.Instance.items[i] == RestrictionItemId)
                return true;
        }

        return false;
    }


    private bool HasZeroProtectionItem()
    {
        // Checks PlayerData to see if item 33 is active.
        if (PlayerData.Instance == null || PlayerData.Instance.items == null)
            return false;

        for (int i = 0; i < PlayerData.Instance.items.Length; i++)
        {
            if (PlayerData.Instance.items[i] == ZeroProtectionItemId)
                return true;
        }

        return false;
    }

    private bool IsZeroBet(string betName)
    {
        // Used by item 33 to block direct bets on 0 and 00.
        return betName == "0" || betName == "00";
    }

    private bool IsZeroPocket(string landedPocket)
    {
        // Used by item 33 to detect if the wheel landed on 0 or 00.
        return landedPocket == "0" || landedPocket == "00";
    }

    private bool IsRestrictedAllowedBet(string betName)
    {
        // Restriction item only allows Red, Black, or exact number bets from 1-36.
        if (betName == "Red" || betName == "Black")
            return true;

        if (int.TryParse(betName, out int number))
            return number >= 1 && number <= 36;

        return false;
    }

    public void SelectBet(string betName)
    {
        // Runs whenever the player clicks any roulette bet button.
        if (spinning)
            return;

        EventSystem.current?.SetSelectedGameObject(null);

        // Restriction item: invalid bets are rejected and spin stays disabled.
        if (HasRestrictionItem() && !IsRestrictedAllowedBet(betName))
        {
            currentBet = "";

            if (selected != null)
                selected.SetText("Selection not valid");

            if (spin != null)
                spin.interactable = false;

            return;
        }

        // Item 33: player cannot choose 0 or 00 as the bet.
        if (HasZeroProtectionItem() && IsZeroBet(betName))
        {
            currentBet = "";

            if (selected != null)
                selected.SetText("Selection not valid");

            if (spin != null)
                spin.interactable = false;

            return;
        }

        currentBet = betName;

        if (selected != null)
            selected.SetText("Selected: " + currentBet);

        if (spin != null)
            spin.interactable = true;
    }

    public void SpinRoulette()
    {
        // Runs when the player presses the spin button.

        if (spinning)
        {
            return;
        }

        if (string.IsNullOrEmpty(currentBet))
        {

            if (selected != null)
                selected.SetText("Selection not valid");

            return;
        }

        // Extra safety check for restriction item before the wheel spins.
        if (HasRestrictionItem() && !IsRestrictedAllowedBet(currentBet))
        {

            if (selected != null)
                selected.SetText("Selection not valid");

            if (spin != null)
                spin.interactable = false;

            return;
        }

        // Extra safety check for item 33 before the wheel spins.
        if (HasZeroProtectionItem() && IsZeroBet(currentBet))
        {

            if (selected != null)
                selected.SetText("Selection not valid");

            if (spin != null)
                spin.interactable = false;

            return;
        }

        spinning = true;
        SetBoardInteractable(false);

        SoundEffectManager.Play("WheelSpin");

        if (selected != null)
            selected.SetText("Spinning...");

        if (anim != null)
        {
            // Start the wheel animation. ResolveSpin runs after the animation is finished.
            anim.SpinWheel(ResolveSpin);
        }
        else
        {
            ResolveSpin();
        }
    }

    private void ResolveSpin()
    {
        // Chooses the final roulette result after the wheel animation ends.

        int roll = Random.Range(0, 38);
        string landedPocket = roll == 37 ? "00" : roll.ToString();
        bool won = BetWins(currentBet, landedPocket);
        // Item 33 activates only when the player loses because the result was 0 or 00.
        bool zeroProtectionActivated = HasZeroProtectionItem() && !won && IsZeroPocket(landedPocket) && !IsZeroBet(currentBet);
        // Item 33 uses a half-win payout instead of a normal loss.
        float payout = zeroProtectionActivated ? 0.5f : GetPayoutMultiplier(currentBet);
        string colorText = GetPocketColorText(landedPocket);

        StartCoroutine(ShowSpinOutcome(landedPocket, colorText, won, payout, zeroProtectionActivated));
    }

    private void SetBoardInteractable(bool value)
    {
        // Turns all roulette bet buttons on or off.
        SetButtonInteractable(but_0, value);
        SetButtonInteractable(but_00, value);
        SetButtonInteractable(but_1, value);
        SetButtonInteractable(but_2, value);
        SetButtonInteractable(but_3, value);
        SetButtonInteractable(but_4, value);
        SetButtonInteractable(but_5, value);
        SetButtonInteractable(but_6, value);
        SetButtonInteractable(but_7, value);
        SetButtonInteractable(but_8, value);
        SetButtonInteractable(but_9, value);
        SetButtonInteractable(but_10, value);
        SetButtonInteractable(but_11, value);
        SetButtonInteractable(but_12, value);
        SetButtonInteractable(but_13, value);
        SetButtonInteractable(but_14, value);
        SetButtonInteractable(but_15, value);
        SetButtonInteractable(but_16, value);
        SetButtonInteractable(but_17, value);
        SetButtonInteractable(but_18, value);
        SetButtonInteractable(but_19, value);
        SetButtonInteractable(but_20, value);
        SetButtonInteractable(but_21, value);
        SetButtonInteractable(but_22, value);
        SetButtonInteractable(but_23, value);
        SetButtonInteractable(but_24, value);
        SetButtonInteractable(but_25, value);
        SetButtonInteractable(but_26, value);
        SetButtonInteractable(but_27, value);
        SetButtonInteractable(but_28, value);
        SetButtonInteractable(but_29, value);
        SetButtonInteractable(but_30, value);
        SetButtonInteractable(but_31, value);
        SetButtonInteractable(but_32, value);
        SetButtonInteractable(but_33, value);
        SetButtonInteractable(but_34, value);
        SetButtonInteractable(but_35, value);
        SetButtonInteractable(but_36, value);
        SetButtonInteractable(but_2_1_bot, value);
        SetButtonInteractable(but_2_1_mid, value);
        SetButtonInteractable(but_2_1_top, value);
        SetButtonInteractable(but_1st_12, value);
        SetButtonInteractable(but_2nd_12, value);
        SetButtonInteractable(but_3rd_12, value);
        SetButtonInteractable(but_1to18, value);
        SetButtonInteractable(but_Even, value);
        SetButtonInteractable(but_Red, value);
        SetButtonInteractable(but_Black, value);
        SetButtonInteractable(but_Odd, value);
        SetButtonInteractable(but_19to36, value);

        if (spin != null)
        {
            // Spin is only allowed if the board is active, a bet exists, and the wheel is not spinning.
            bool canSpin = value && !string.IsNullOrEmpty(currentBet) && !spinning;

            // Restriction item can also disable the spin button.
            if (HasRestrictionItem())
                canSpin = canSpin && IsRestrictedAllowedBet(currentBet);

            // Item 33 keeps spin disabled if the current bet is 0 or 00.
            if (HasZeroProtectionItem())
                canSpin = canSpin && !IsZeroBet(currentBet);

            spin.interactable = canSpin;
        }
    }

    private void SetButtonInteractable(Button button, bool value)
    {
        if (button != null)
            button.interactable = value;
    }

    private bool BetWins(string betName, string landedPocket)
    {
        // Checks if the selected bet won based on the landed pocket.
        if (IsStraightUpBet(betName))
            return betName == landedPocket;

        if (!int.TryParse(landedPocket, out int landedNumber))
        {
            if (betName == "0" || betName == "00")
                return betName == landedPocket;

            return false;
        }

        switch (betName)
        {
            case "Red":
                return IsRed(landedNumber);

            case "Black":
                return IsBlack(landedNumber);

            case "Even":
                return landedNumber != 0 && landedNumber % 2 == 0;

            case "Odd":
                return landedNumber % 2 == 1;

            case "1 to 18":
                return landedNumber >= 1 && landedNumber <= 18;

            case "19 to 36":
                return landedNumber >= 19 && landedNumber <= 36;

            case "1st 12":
                return landedNumber >= 1 && landedNumber <= 12;

            case "2nd 12":
                return landedNumber >= 13 && landedNumber <= 24;

            case "3rd 12":
                return landedNumber >= 25 && landedNumber <= 36;

            case "2-1 Bottom":
                return landedNumber >= 1 && landedNumber <= 34 && landedNumber % 3 == 1;

            case "2-1 Middle":
                return landedNumber >= 2 && landedNumber <= 35 && landedNumber % 3 == 2;

            case "2-1 Top":
                return landedNumber >= 3 && landedNumber <= 36 && landedNumber % 3 == 0;
        }

        return false;
    }

    private bool IsStraightUpBet(string betName)
    {
        // A straight-up bet is one exact pocket, like 5, 12, 0, or 00.
        if (betName == "0" || betName == "00")
            return true;

        if (int.TryParse(betName, out int number))
            return number >= 1 && number <= 36;

        return false;
    }

    private float GetPayoutMultiplier(string betName)
    {
        // Calculates the payout multiplier for the selected bet.
        float payout;

        if (IsStraightUpBet(betName))
            payout = 35f;
        else if (betName == "1st 12" || betName == "2nd 12" || betName == "3rd 12" ||
                 betName == "2-1 Bottom" || betName == "2-1 Middle" || betName == "2-1 Top")
            payout = 2f;
        else
            payout = 1f;

        // Restriction item: valid allowed wins are multiplied by 3.
        if (HasRestrictionItem() && IsRestrictedAllowedBet(betName))
            payout *= 3f;

        return payout;
    }

    private bool IsRed(int number)
    {
        return redNumbers.Contains(number);
    }

    private bool IsBlack(int number)
    {
        return number >= 1 && number <= 36 && !redNumbers.Contains(number);
    }

    private string GetPocketColorText(string landedPocket)
    {
        if (landedPocket == "0" || landedPocket == "00")
            return "Green";

        if (int.TryParse(landedPocket, out int number))
            return IsRed(number) ? "Red" : "Black";

        return "";
    }

    private void ApplyRestrictedItemLoss()
    {
        // Restriction item loss behavior. Your GameManager loseHand already removes the wager.
        // Right now this keeps the same project flow and still compiles.
        // If GameManager.Instance.loseHand() already removes the wager from the player,
        // then this is already the correct behavior.
        //
        // If your GameManager does NOT already subtract the current wager amount,
        // replace the line below with your project's real wager/health deduction code.
        GameManager.Instance.loseHand();
    }

    private IEnumerator ShowSpinOutcome(string landedPocket, string colorText, bool won, float payout, bool zeroProtectionActivated)
    {
        // Shows result text, waits, shows win/lose/protection text, then applies the final game effect.
        if (selected != null)
            selected.SetText("Result: " + landedPocket + " " + colorText);

        yield return new WaitForSeconds(3.5f);

        if (selected != null)
        {
            if (won)
                selected.SetText("YOU WIN");
            else if (zeroProtectionActivated)
                selected.SetText("ZERO PROTECTION");
            else
                selected.SetText("YOU LOSE");
        }

        yield return new WaitForSeconds(3.5f);

        spinning = false;

        if (ui != null)
            ui.hideGame();

        // Normal wins and item 33 half-wins both use winHand.
        if (won || zeroProtectionActivated)
            GameManager.Instance.winHand(payout);
        // Restriction item loss keeps the current loseHand behavior.
        else if (HasRestrictionItem())
            ApplyRestrictedItemLoss();
        else
            GameManager.Instance.loseHand();
    }
}
