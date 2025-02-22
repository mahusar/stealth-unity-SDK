using Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RpcSettingsUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField urlInput;
    [SerializeField] private Button saveButton;

    private void Start()
    {
        RpcHandler rpc = RpcHandler.GetInstance();
        userInput.text = rpc.rpcUser;
        passwordInput.text = rpc.rpcPassword;
        urlInput.text = rpc.rpcUrl;

        saveButton.onClick.AddListener(SaveSettings);
    }

    private void SaveSettings()
    {
        RpcHandler rpc = RpcHandler.GetInstance();
        rpc.SaveRpcSettings(userInput.text, passwordInput.text, urlInput.text);
    }
}
