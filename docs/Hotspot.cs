using UnityEngine;
using UnityEngine.UI;

public class Hotspot : MonoBehaviour
{
    [Header("References")]
    public Transform player;            
    public CanvasGroup tooltipGroup;    
    public Text tooltipText;            

    [Header("Behavior")]
    public float triggerRadius = 5f;    
    [TextArea] public string detailMessage = "Stair handrail height must be >= 110 cm";
    public KeyCode interactKey = KeyCode.E;

    bool isInside;
    bool expanded;

    void Start()
    {
        SetTooltipVisible(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        bool insideNow = distance <= triggerRadius;

        if (insideNow && !isInside) SetTooltipVisible(true);
        if (!insideNow && isInside)
        {
            SetTooltipVisible(false);
            expanded = false;
        }
        isInside = insideNow;

        if (isInside && Input.GetKeyDown(interactKey))
        {
            expanded = !expanded;
            tooltipText.text = expanded
                ? detailMessage + "\n[E] Close"
                : detailMessage + "\n[E] Details";
        }
    }

    void SetTooltipVisible(bool visible)
    {
        if (tooltipGroup == null) return;
        tooltipGroup.alpha = visible ? 1f : 0f;
        tooltipGroup.interactable = visible;
        tooltipGroup.blocksRaycasts = visible;
        if (tooltipText != null)
            tooltipText.text = visible ? detailMessage + "\n[E] Details" : "";
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}