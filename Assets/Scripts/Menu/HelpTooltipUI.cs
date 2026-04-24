using UnityEngine;
using UnityEngine.EventSystems;

public class HelpTooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Tooltip")]
    public GameObject tooltipBox;

    private static GameObject globalHelpInstance;
    private bool tooltipVisible = false;

    private void Awake()
    {
        // Keep the whole Canvas alive between scenes.
        GameObject rootObject = transform.root.gameObject;

        // If one already exists, delete the duplicate.
        if (globalHelpInstance != null && globalHelpInstance != rootObject)
        {
            Destroy(rootObject);
            return;
        }

        globalHelpInstance = rootObject;
        DontDestroyOnLoad(rootObject);

        // Hide tooltip at the start.
        if (tooltipBox != null)
        {
            tooltipBox.SetActive(false);
        }
    }

    // Mouse enters the icon.
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    // Mouse leaves the icon.
    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    // Optional: clicking also toggles it.
    // This is useful if you ever test with touch controls.
    public void OnPointerClick(PointerEventData eventData)
    {
        tooltipVisible = !tooltipVisible;

        if (tooltipBox != null)
        {
            tooltipBox.SetActive(tooltipVisible);
        }
    }

    private void ShowTooltip()
    {
        tooltipVisible = true;

        if (tooltipBox != null)
        {
            tooltipBox.SetActive(true);
        }
    }

    private void HideTooltip()
    {
        tooltipVisible = false;

        if (tooltipBox != null)
        {
            tooltipBox.SetActive(false);
        }
    }
}