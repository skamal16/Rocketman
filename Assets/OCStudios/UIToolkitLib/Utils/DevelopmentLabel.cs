using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

class DevelopmentLabel
{
    private static Label developmentNotice;
    private static bool crFadeRunning = false;

    private MonoBehaviour context;

    public DevelopmentLabel(MonoBehaviour context, VisualElement root)
    {
        this.context = context;
        crFadeRunning = false;
        developmentNotice = root.Q<Label>("label-development");
    }

    public void DisplayDevelopmentNotice()
    {
        if (!crFadeRunning)
            context.StartCoroutine(Fade());
    }

    static IEnumerator Fade()
    {
        crFadeRunning = true;

        developmentNotice.visible = true;
        developmentNotice.style.opacity = 1f;

        yield return new WaitForSeconds(2f);

        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            developmentNotice.style.opacity = alpha;
            yield return new WaitForSeconds(.05f);
        }

        developmentNotice.visible = false;

        crFadeRunning = false;
    }
}
