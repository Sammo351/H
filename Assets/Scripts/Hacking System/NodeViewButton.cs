using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeViewButton : MonoBehaviour
{
    public NetworkDevice Device;

    public void OnButtonPressed()
    {
        FindObjectOfType<TerminalLogic>().OnInputRecieved("connect " + Device.GetDeviceName());
    }

    public void Destroy()
    {
        Destroy(GetComponent<UnityEngine.UI.Button>());
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
		GetComponent<UnityEngine.UI.Image>().CrossFadeAlpha(0f, 0.3f, false);
		GetComponentInChildren<UnityEngine.UI.Text>().CrossFadeAlpha(0, 0.3f, false);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
    }
}
