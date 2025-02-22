using Core;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class WalletStatus : MonoBehaviour
{
    public TMP_Text walletStatusText;

    private RpcHandler rpcHandler;

    private async void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
        await UpdateWalletStatus();
    }

    public async Task UpdateWalletStatus()
    {
        string response = await rpcHandler.SendRpcRequest("getinfo");

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            var result = jsonResponse["result"];

            if (result != null)
            {
                long unlockedUntil = result["unlocked_until"]?.ToObject<long>() ?? 0;

                if (unlockedUntil == 0)
                {
                    walletStatusText.text = "Locked";
                    walletStatusText.color = Color.red;
                }
                else
                {
                    walletStatusText.text = "Unlocked";
                    walletStatusText.color = Color.green;
                }
            }
            else
            {
                walletStatusText.text = "Error fetching wallet status";
                walletStatusText.color = Color.yellow;
            }
        }
    }
}
