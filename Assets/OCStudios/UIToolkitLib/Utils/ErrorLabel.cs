using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

class ErrorLabel
{
    private static Label errorLabel;
    private static bool crFadeRunning = false;

    private MonoBehaviour context;

    public ErrorLabel(MonoBehaviour context, VisualElement root)
    {
        this.context = context;
        crFadeRunning = false;
        errorLabel = root.Q<Label>("label-error");
    }

    public void Display()
    {
        if (!crFadeRunning)
            context.StartCoroutine(Fade());
    }

    public void SetMessage(string message)
    {
        errorLabel.text = message;
    }

    static IEnumerator Fade()
    {
        crFadeRunning = true;

        errorLabel.visible = true;
        errorLabel.style.opacity = 1f;

        yield return new WaitForSeconds(2f);

        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            errorLabel.style.opacity = alpha;
            yield return new WaitForSeconds(.05f);
        }

        errorLabel.visible = false;

        crFadeRunning = false;
    }
}
