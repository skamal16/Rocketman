using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Presenter : MonoBehaviour
{
    public static Dictionary<string, Presenter> presenters;

    private static string currentPresenterName = "gamePresenter";

    private static Stack<string> presenterStack;

    internal new string name;

    [SerializeField]
    private List<View> views;

    internal Dictionary<string, View> screenMap;

    internal string currentViewName = "";

    internal Stack<string> screenStack;

    public void Awake()
    {
        Init();

        if (presenters == null) presenters = new Dictionary<string, Presenter>();
        if (presenterStack == null) presenterStack = new Stack<string>();

        presenters.Add(name, this);

        screenMap = new Dictionary<string, View>();

        foreach (View view in views)
        {
            view.presenter = this;
            view.Initialize();
            screenMap.Add(view.name, view);
        }

        screenStack = new Stack<string>();

        foreach (KeyValuePair<string, View> screenMapping in screenMap)
            screenMapping.Value.SetActive(false);

        PostInit();
    }

    internal abstract void Init();

    internal abstract void PostInit();

    public static Presenter GetPresenter(string presenterName)
    {
        return presenters[presenterName];
    }

    public static void GoTo(string presenterName, string screen)
    {
        if (presenters.ContainsKey(presenterName))
        {
            if (presenters.ContainsKey(currentPresenterName))
            {
                presenters[currentPresenterName].screenMap[presenters[currentPresenterName].currentViewName].SetActive(false);
                presenters[currentPresenterName].screenStack.Clear();
            }

            presenters[presenterName].GoToView(screen);

            presenterStack.Clear();

            currentPresenterName = presenterName;
        }
        else
        {
            Debug.Log("Presenter not registered: " + presenterName);
        }
    }

    public void GoToView(string viewName)
    {
        if (screenMap.ContainsKey(viewName))
        {
            if(screenMap.ContainsKey(currentViewName)) screenMap[currentViewName].SetActive(false);
            screenMap[viewName].SetActive(true);

            screenStack.Clear();

            currentViewName = viewName;
        }
        else
        {
            Debug.Log("View not registered: " + viewName);
        }
    }

    public static void GoToRetaining(string presenterName, string screen)
    {
        if (presenters.ContainsKey(presenterName))
        {
            presenters[currentPresenterName].screenMap[presenters[currentPresenterName].currentViewName].SetActive(false);

            presenterStack.Push(currentPresenterName);

            presenters[presenterName].GoToView(screen);

            currentPresenterName = presenterName;
        }
        else
        {
            Debug.Log("Presenter not registered: " + presenterName);
        }
    }

    public void GoToViewRetaining(string viewName)
    {
        if (screenMap.ContainsKey(viewName))
        {
            if (screenMap.ContainsKey(currentViewName)) screenMap[currentViewName].SetActive(false);
            screenMap[viewName].SetActive(true);

            screenStack.Push(currentViewName);

            currentViewName = viewName;
        }
        else
        {
            Debug.Log("View not registered: " + viewName);
        }
    }

    public static void ReturnToLastPresenter()
    {
        presenters[currentPresenterName].screenMap[presenters[currentPresenterName].currentViewName].SetActive(false);
        currentPresenterName = presenterStack.Pop();

        presenters[currentPresenterName].screenMap[presenters[currentPresenterName].currentViewName].SetActive(true);
    }

    public void ReturnToLastScreen()
    {
        screenMap[currentViewName].SetActive(false);
        currentViewName = screenStack.Pop();

        screenMap[currentViewName].SetActive(true);
    }

    public static void OpenPopupInPresenter(string presenterName, string viewName)
    {
        if (presenters.ContainsKey(presenterName))
        {
            Presenter currentPresenter = presenters[currentPresenterName];
            Presenter presenter = presenters[presenterName];

            currentPresenter.screenMap[currentPresenter.currentViewName].GetComponent<UIDocument>().rootVisualElement.SetEnabled(false);
            presenter.screenMap[viewName].SetActive(true);
            presenter.currentViewName = viewName;

            presenterStack.Push(currentPresenterName);

            currentPresenterName = presenterName;
        }
        else
        {
            Debug.Log("Presenter not registered: " + presenterName);
        }
    }

    public void OpenPopup(string viewName)
    {
        if (screenMap.ContainsKey(viewName))
        {
            screenMap[currentViewName].GetComponent<UIDocument>().rootVisualElement.SetEnabled(false);
            screenMap[viewName].SetActive(true);

            screenStack.Push(currentViewName);

            currentViewName = viewName;
        }
        else
        {
            Debug.Log("View not registered: " + viewName);
        }
    }

    public static void ClosePresenterPopup()
    {
        Presenter currentPresenter = presenters[currentPresenterName];
        currentPresenterName = presenterStack.Pop();
        Presenter presenter = presenters[currentPresenterName];

        currentPresenter.screenMap[currentPresenter.currentViewName].SetActive(false);

        presenter.screenMap[presenter.currentViewName].GetComponent<UIDocument>().rootVisualElement.SetEnabled(true);
    }

    public void ClosePopup()
    {
        screenMap[currentViewName].SetActive(false);
        currentViewName = screenStack.Pop();

        screenMap[currentViewName].GetComponent<UIDocument>().rootVisualElement.SetEnabled(true);
        screenMap[currentViewName].OnReEnable();
    }

    public void CloseView()
    {
        screenMap[currentViewName].SetActive(false);
    }
}