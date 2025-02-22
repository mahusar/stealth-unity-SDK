using Core;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GetTransaction : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> FetchTransaction(string txid)
    {
        if (string.IsNullOrEmpty(txid))
            return "Invalid TXID.";

        string response = await rpcHandler.SendRpcRequest("gettransaction", new object[] { txid });

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            var result = jsonResponse["result"];

            if (result != null)
            {
                string transactionDetails = $"TXID: {result["txid"]}\n" +
                                            $"Amount: {result["amount"]} XST\n" +
                                            $"Confirmations: {result["confirmations"]}\n" +
                                            $"Block Hash: {result["blockhash"]}\n" +
                                            $"Time: {UnixTimeStampToDateTime((long)result["time"])}\n" +
                                            $"Details:\n";

                foreach (var detail in result["details"])
                {
                    transactionDetails += $"- Address: {detail["address"]}\n" +
                                          $"  Category: {detail["category"]}\n" +
                                          $"  Amount: {detail["amount"]} XST\n\n";
                }

                return transactionDetails;
            }
        }

        return "Transaction not found.";
    }

    private string UnixTimeStampToDateTime(long unixTimeStamp)
    {
        System.DateTime dt = System.DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime;
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
