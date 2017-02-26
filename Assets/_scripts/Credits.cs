// Author(s): Paul Calande
// Credits script. To be attached to a text UI object.
// Make sure the text has the following properties...
// - Vertical Overflow is set to Overflow.
// - The Height value is very small (like 10).
// - The vertical Paragraph Alignment is set to top.

// Comment out the following line to stop debug console messages about credits.
//#define CREDITS_DEBUG
// Comment out the following line to allow the credits to scroll.
//#define CREDITS_PREVENT_SCROLLING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    // How fast the text scrolls.
    public float scrollSpeed;
    // How large the text's starting offset is.
    public float startingOffset = 1000f;
    // How many seconds the credits wait when they reach the top.
    public float endingHangTime;

    // Text component of the credits.
    private Text creditsText;
    // RectTransform component of the credits.
    private RectTransform creditsTransform;
    // How high the text has to be for the game to end.
    private float endingHeight;
    // How many seconds the credits have waited at the top thus far.
    private float endingHanged = 0f;

    private void Awake()
    {
        // Get the components.
        creditsText = GetComponent<Text>();
        creditsTransform = GetComponent<RectTransform>();
        // Calculate the ending height and starting height based on the text height.
        endingHeight = creditsText.preferredHeight;
        Vector3 newPos = creditsTransform.position;
        newPos.y = -startingOffset;
        creditsTransform.localPosition = newPos;

#if CREDITS_DEBUG
        Debug.Log("Credits endingHeight: " + endingHeight);
#endif
    }

    private void Update()
    {
#if CREDITS_DEBUG
        Debug.Log("Credits RectTransform y position: " + creditsTransform.localPosition.y);
#endif

        // If the credits haven't reached the top yet...
        if (creditsTransform.localPosition.y < endingHeight)
        {
#if !CREDITS_PREVENT_SCROLLING
            // Move the credits upwards a bit.
            Vector3 newPos = creditsTransform.localPosition;
            newPos.y += scrollSpeed * Time.deltaTime;
            creditsTransform.localPosition = newPos;
#endif
        }
        else
        {
            // The credits are at the top. Wait for a bit before doing anything else.
            endingHanged += Time.deltaTime;
            if (endingHanged > endingHangTime)
            {
                CreditsFinish();
            }
        }
    }

    private void CreditsFinish()
    {
        PaulUtility.EndGame();
    }
}