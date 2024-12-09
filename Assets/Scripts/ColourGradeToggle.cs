using UnityEngine;
using UnityEngine.UI;

public class ColorGradingToggle : MonoBehaviour
{
    // Reference to the Material using the color grading shader
    [SerializeField] private Material colorGradingMaterial;

    // Toggles the color grading effect
    private bool isColorGradingEnabled = false; // Initial state for color grading

    private void Update()
    {
        // Check if the '1' key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Toggle the state
            isColorGradingEnabled = !isColorGradingEnabled;
            ToggleColorGrading(isColorGradingEnabled);
        }
    }

    public void ToggleColorGrading(bool isEnabled)
    {
        if (colorGradingMaterial != null)
        {
            // Set the _UseColorGrading property based on the toggle state
            colorGradingMaterial.SetFloat("_UseColorGrading", isEnabled ? 1.0f : 0.0f);
        }
        else
        {
            Debug.LogWarning("Color grading material is not assigned!");
        }
    }
}
