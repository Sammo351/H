using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkViewerApp : MonoBehaviour
{
    public GameObject NetworkNodeView, NetworkNodeLine;
    public Dictionary<NetworkDevice, GameObject> Devices = new Dictionary<NetworkDevice, GameObject>();



    public Dictionary<NetworkDevice, GameObject> Lines = new Dictionary<NetworkDevice, GameObject>();
    public RectTransform Holder;

    NetworkDevice lastDevice;
    Vector2 centerPos;

    void Start()
    {
        lastDevice = TerminalNetwork.GetCurrentDevice();
        PopulateNodes();
        //centerPos = Holder.GetComponent<RectTransform>().
    }

    void PopulateNodes()
    {
        NetworkDevice currentDevice = TerminalNetwork.GetCurrentDevice();
        GameObject current = Instantiate(NetworkNodeView);
        current.transform.parent = Holder;

        current.GetComponent<RectTransform>().localScale = Vector2.one;
        current.GetComponent<RectTransform>().localPosition = Vector2.one;
        //current.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //current.GetComponent<RectTransform>().position = Vector2.zero;

        Devices.Add(currentDevice, current);
        current.GetComponentInChildren<UnityEngine.UI.Text>().text = currentDevice.GetDeviceName();

        for (int i = 0; i < currentDevice.networkNode._nodes.Count; i++)
        {
            var node = currentDevice.networkNode._nodes[i];

            float spacing = 360f / (float)currentDevice.networkNode._nodes.Count;

            float x = UnityEngine.Random.Range(-300f, 300f);
            float y = UnityEngine.Random.Range(-300f, 300f);



            Vector2 pos = new Vector2(x, y);
            if(pos.magnitude < 100)
                pos = pos.normalized * 100f;

            GameObject go = Instantiate(NetworkNodeView);
            go.transform.parent = Holder;
            go.transform.localPosition = Vector2.zero;
            go.GetComponent<RectTransform>().anchoredPosition = (pos);
            go.GetComponent<RectTransform>().localScale = Vector2.one;
            UnityEngine.UI.Text text = go.GetComponentInChildren<UnityEngine.UI.Text>();

            go.GetComponent<Animator>().Play("Intro");

            text.text = currentDevice.networkNode._nodes[i].networkDevice.GetDeviceName();
            go.GetComponent<NodeViewButton>().Device = currentDevice.networkNode._nodes[i].networkDevice;

            GameObject line = Instantiate(NetworkNodeLine);
            Lines.Add(currentDevice.networkNode._nodes[i].networkDevice, line);
            line.transform.parent = Holder;
            line.GetComponent<RectTransform>().anchoredPosition = (pos).normalized * 150;
            var lPos = line.GetComponent<RectTransform>().localPosition;
            lPos.z = 0f;
            line.GetComponent<RectTransform>().localPosition = lPos;
            line.GetComponent<RectTransform>().localScale = new Vector3(3f, 1f, 1f);
            Vector2 dir = pos;
            line.GetComponent<RectTransform>().right = (pos).normalized * 300f;
            line.GetComponent<RectTransform>().SetAsFirstSibling();
            Devices.Add(currentDevice.networkNode._nodes[i].networkDevice, go);
        }

        current.GetComponent<RectTransform>().localPosition = Vector2.zero;
        current.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    void Update()
    {
        if (lastDevice != TerminalNetwork.GetCurrentDevice())
        {
            UpdateNodes();
        }

        lastDevice = TerminalNetwork.GetCurrentDevice();
    }

    void UpdateNodes()
    {
        GameObject current;
        List<NodeViewButton> RemovedDevices = new List<NodeViewButton>();

        foreach (KeyValuePair<NetworkDevice, GameObject> pair in Devices)
        {
            if (pair.Key != TerminalNetwork.GetCurrentDevice())
            {
                RemovedDevices.Add(pair.Value.GetComponent<NodeViewButton>());
                //GameObject.Destroy(pair.Value);
            }
        }

        for (int i = 0; i < RemovedDevices.Count; i++)
        {
            RemovedDevices[i].Destroy();
        }

        StartCoroutine(DoTransition(TerminalNetwork.GetCurrentDevice()));
    }

    IEnumerator DoTransition(NetworkDevice current)
    {
        foreach (KeyValuePair<NetworkDevice, GameObject> g in Lines)
        {
            Destroy(g.Value);
        }

        if (current != null)
        {
            if (Devices.ContainsKey(current))
            {
                while (Vector2.Distance(Devices[current].GetComponent<RectTransform>().anchoredPosition, Vector2.zero) > 1)
                {
                    Devices[current].GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(Devices[current].GetComponent<RectTransform>().anchoredPosition, Vector3.zero, 600f * Time.unscaledDeltaTime);
                    yield return new WaitForEndOfFrame();
                }

                Destroy(Devices[current]);
            }
        }

        Devices.Clear();

        Lines.Clear();
        PopulateNodes();
    }

}


