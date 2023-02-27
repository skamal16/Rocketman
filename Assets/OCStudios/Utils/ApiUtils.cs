using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetApiData<T>
{
    private readonly string mainURL = "https://www.footballstrategy.org/2022/new/app-api/";
    private readonly string key = "f00tb@llstr@t3gy";

    private T data;

    public GetApiData(MonoBehaviour context, string URL, string act, Action<T> callback, List<Tuple<string, string>> fields = null)
    {
        context.StartCoroutine(GetData(context, URL, act, callback, fields));
    }

    private IEnumerator GetData(MonoBehaviour context, string URL, string act, Action<T> callback, List<Tuple<string, string>> fields = null)
    {
        CoroutineWithData<object> coroutine = new CoroutineWithData<object>(context, Fetch(context, URL, act, fields));

        yield return coroutine.Coroutine;

        string result = coroutine.result.ToString();

        Debug.Log("API result: " + result);

        data = JsonUtility.FromJson<T>(result);

        callback(data);
    }

    public IEnumerator Fetch(MonoBehaviour context, string URL, string act, List<Tuple<string, string>> fields = null)
    {
        WWWForm form = new WWWForm();

        form.AddField("key", key);
        form.AddField("act", act);

        if(fields != null)
        {
            foreach (Tuple<string, string> field in fields)
                form.AddField(field.Item1, field.Item2 != null ? field.Item2 : "");
        }

        using (UnityWebRequest request = UnityWebRequest.Post(mainURL + URL, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                //throw new Exception(request.error);
                yield return request.error;
            else
                yield return request.downloadHandler.text;
        }
    }

    public void Get(MonoBehaviour context, string URL)
    {
        context.StartCoroutine(Get(URL));
    }

    public void Post(MonoBehaviour context, string URL, WWWForm form)
    {
        context.StartCoroutine(Post(URL, form));
    }

    private IEnumerator Get(string URL)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
                Debug.Log(request.error);
            else
                Debug.Log(request.downloadHandler.text);
        }
    }

    private IEnumerator Post(string URL, WWWForm form)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                throw new Exception(request.error);
            else
                yield return request.downloadHandler.text;
        }
    }
}
