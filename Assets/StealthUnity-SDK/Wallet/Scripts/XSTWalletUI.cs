using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Wallet
{
    public class XSTWalletUI : MonoBehaviour
    {
        [Header("Refresh")]
        [SerializeField] private Button refreshButton;
        [Header("GetInfo")]
        [SerializeField] private Button fetchInfoButton;
        [SerializeField] private TMP_InputField getInfoText;
        private GetInfo getInfo;
        [Header("GetBalance")]
        [SerializeField] private TMP_Text balanceText;
        private GetBalance getBalance;
        [Header("ListTransactions")]
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private GameObject transactionPrefab;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Button listTransactionsButton;
        private ListTransactions listTransactions;
        [Header("GetNewAddress")]
        [SerializeField] private TMP_InputField newAddressText;
        [SerializeField] private Button generateAddressButton;
        private GetNewAddress getNewAddress;
        private const string WalletAddressKey = "WalletAddress";
        [Header("WalletLock")]
        [SerializeField] private Button lockWalletButton; 
        private WalletLock walletLock;
        [Header("WalletUnLock")]
        [SerializeField] private Button unlockWalletButton;
        [SerializeField] private TMP_InputField passwordInput;
        private WalletPassPhrase walletPassPhrase;
        private WalletStatus walletStatus;
        [Header("SendToAddress")]
        [SerializeField] private TMP_InputField recipientAddressInput;
        [SerializeField] private TMP_InputField amountInput;
        [SerializeField] private Button sendTransactionButton;
        [SerializeField] private TMP_InputField transactionStatusText;  
        private SendToAddress sendToAddress;
        [Header("GetTransaction")]
        [SerializeField] private TMP_InputField txidInputField;
        [SerializeField] private TMP_InputField resultInputField;
        private GetTransaction getTransaction;
        [SerializeField] private Button fetchTransactionButton;
        [Header("ValidateAddress")]
        [SerializeField] private TMP_InputField addressInputField;
        [SerializeField] private TMP_InputField addressResultField;
        [SerializeField] private Button validateButton;
        private ValidateAddress validateAddress;
        [Header("GetNewAccountAddress")]
        [SerializeField] private TMP_InputField accountNewNameInput;
        [SerializeField] private TMP_InputField accountNewAddressText;
        [SerializeField] private Button getNewAccountButton;
        private GetNewAddress getNewAccountAddress;
        [Header("ListAccounts")]
        [SerializeField] private TMP_InputField accountAddressInput;  
        private ListAccounts listAccounts;
        [SerializeField] private Button listAccountsButton;
        [Header("StealthAddress")]
        [SerializeField] private TMP_InputField stealthLabelInput;
        [SerializeField] private Button generateStealthButton;
        [SerializeField] private TMP_InputField stealthAddressOutput; 
        private GetNewStealthAddress getNewStealthAddress;
        [Header("SendToStealthAddress")]
        [SerializeField] private TMP_InputField stealthRecipientAddressInput;
        [SerializeField] private TMP_InputField stealthAmountInput;
        [SerializeField] private Button sendStealthTransactionButton;
        [SerializeField] private TMP_InputField stealthTransactionStatusText;
        private SendToStealthAddress sendToStealthAddress;
        [Header("Staker Authorities")]
        [SerializeField] private TMP_InputField stakerNameInput;
        [SerializeField] private Button fetchAuthoritiesButton;
        [SerializeField] private TMP_InputField resultAuthorityInputField;
        private GetStakerAuthorities getStakerAuthorities;
        [Header("Addresses by Account")]
        [SerializeField] private TMP_InputField accountAddressNameInput;
        [SerializeField] private Button fetchAddressesButton;
        [SerializeField] private TMP_InputField addressesResultInput;
        private GetAddressesByAccount getAddressesByAccount;
        [Header("Wallet Repair")]
        [SerializeField] private Button repairWalletButton;
        [SerializeField] private TMP_InputField repairWalletResult;
        private RepairWallet repairWallet;

        async void Start()
        {
            refreshButton.onClick.AddListener(async () => await RefreshWallet());

            getInfo = GetComponent<GetInfo>();
            fetchInfoButton.onClick.AddListener(async () => await UpdateGetInfo());

            getBalance = GetComponent<GetBalance>();
            await Task.Delay(500);
            await UpdateBalance();

            listTransactions = GetComponent<ListTransactions>();
            listTransactionsButton.onClick.AddListener(async () => await UpdateListTransactions());

            getNewAddress = GetComponent<GetNewAddress>();
            generateAddressButton.onClick.AddListener(async () => await UpdateGetNewAddress());
            string savedAddress = PlayerPrefs.GetString(WalletAddressKey, "No address found");
            newAddressText.text = "Last wallet address: " + savedAddress;

            walletLock = GetComponent<WalletLock>(); 
            lockWalletButton.onClick.AddListener(async () => await LockWallet());

            walletPassPhrase = GetComponent<WalletPassPhrase>();
            unlockWalletButton.onClick.AddListener(async () => await UnlockWallet());
            walletStatus = GetComponent<WalletStatus>();
            passwordInput.onSelect.AddListener(ClearPlaceholder);

            sendToAddress = GetComponent<SendToAddress>();
            sendTransactionButton.onClick.AddListener(async () => await SendTransaction());
            recipientAddressInput.onSelect.AddListener(SendTransactionClearPlaceholder);
            amountInput.onSelect.AddListener(SendTransactionAmountClearPlaceholder); 

            getTransaction = GetComponent<GetTransaction>();
            fetchTransactionButton.onClick.AddListener(async () => await FetchTransactionDetails());
            txidInputField.onSelect.AddListener(GetTransactionClearPlaceholder);

            validateAddress = GetComponent<ValidateAddress>();
            validateButton.onClick.AddListener(async () => await ValidateAddressDetails());
            addressInputField.onSelect.AddListener(ValidateTransactionClearPlaceholder);

            getNewAccountAddress = GetComponent<GetNewAddress>();
            getNewAccountButton.onClick.AddListener(async () => await OnGetAccountAddressButtonClicked());
            accountNewNameInput.onSelect.AddListener(GetAccountClearPlaceholder);

            listAccounts = GetComponent<ListAccounts>();
            listAccountsButton.onClick.AddListener(async () => await OnListAccountsButtonClicked());

            getNewStealthAddress = GetComponent<GetNewStealthAddress>();
            generateStealthButton.onClick.AddListener(async () => await OnGenerateStealthAddress());
            stealthLabelInput.onSelect.AddListener(GetStealthAddressClearPlaceholder);

            sendToStealthAddress = GetComponent<SendToStealthAddress>();
            sendStealthTransactionButton.onClick.AddListener(async () => await SendStealthTransaction());
            stealthRecipientAddressInput.onSelect.AddListener(GetStealthRecipientAddressClearPlaceholder);
            stealthAmountInput.onSelect.AddListener(GetStealthAmountClearPlaceholder);

            getStakerAuthorities = GetComponent<GetStakerAuthorities>();
            fetchAuthoritiesButton.onClick.AddListener(async () => await FetchAuthorities());
            stakerNameInput.onSelect.AddListener(GetStakerAuthoritiesClearPlaceholder);

            getAddressesByAccount = GetComponent<GetAddressesByAccount>();
            fetchAddressesButton.onClick.AddListener(async () => await FetchAddresses());
            accountAddressNameInput.onSelect.AddListener(GetAccountAddressClearPlaceholder);

            repairWallet = gameObject.AddComponent<RepairWallet>();
            repairWalletButton.onClick.AddListener(async () => await RunRepairWallet());
        }

        private async Task RefreshWallet()
        {
            await UpdateGetInfo();
            await UpdateBalance();
            await UpdateListTransactions();       
        }
        private async Task UpdateGetInfo()
        {
            string walletInfo = await getInfo.FetchWalletInfo();
            getInfoText.text = walletInfo; 
        }
        private async Task UpdateBalance()
        {
            float balance = await getBalance.FetchBalance();
            balanceText.text = $"balance: {balance} XST";  
        }
        private async Task UpdateListTransactions()
        {
            List<Transaction> transactions = await listTransactions.FetchTransactions();
            foreach (Transform child in contentPanel)
            {
                Destroy(child.gameObject);
            }

            if (transactions.Count > 0)
            {
                foreach (var tx in transactions) 
                {
                    GameObject newInputFieldObj = Instantiate(transactionPrefab, contentPanel);
                    TMP_InputField inputField = newInputFieldObj.GetComponent<TMP_InputField>();

                    inputField.text = $"TX: {tx.txid}\n" +
                                      $"Amount: {tx.amount:F2} XST\n" +
                                      $"Confirmations: {tx.confirmations}\n\n";

                    newInputFieldObj.transform.SetParent(contentPanel, false);
                    newInputFieldObj.transform.SetAsFirstSibling();
                }
            }
            else
            {
                GameObject newInputFieldObj = Instantiate(transactionPrefab, contentPanel);
                newInputFieldObj.GetComponent<TMP_InputField>().text = "No transactions found.";
            }

            await Task.Yield();
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel);
            await Task.Delay(100);
            scrollRect.verticalNormalizedPosition = 1;
        }

        private async Task UpdateGetNewAddress()
        {
            string newAddress = await getNewAddress.FetchNewAddress();
            PlayerPrefs.SetString(WalletAddressKey, newAddress);
            PlayerPrefs.Save();
            newAddressText.text = "New address: " + newAddress;
        }
        private async Task LockWallet()
        {
            await walletLock.FetchWalletLock();
            await UpdateWalletStatus();
        }

        private async Task UnlockWallet()
        {
            string password = passwordInput.text;
            bool success = await walletPassPhrase.FetchWalletPassPhrase(password);
            await UpdateWalletStatus();           
            if (passwordInput.placeholder != null)
            {
                passwordInput.text = "";
                passwordInput.placeholder.gameObject.SetActive(true); 
            }          
        }
        private void ClearPlaceholder(string text)
        {
            if (passwordInput.placeholder != null)
            {
                passwordInput.placeholder.gameObject.SetActive(false); 
            }
        }
        private async Task UpdateWalletStatus()
        {
            await walletStatus.UpdateWalletStatus();
        }
        private async Task SendTransaction()
        {
            string address = recipientAddressInput.text.Trim();

            if (!decimal.TryParse(amountInput.text, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
            {
                transactionStatusText.text = "Invalid amount!";
                return;
            }

            transactionStatusText.text = "Sending...";

            string txid = await sendToAddress.SendTransaction(address, amount); 

            if (txid.StartsWith("Error") || txid.StartsWith("Parsing error"))
            {
                transactionStatusText.text = txid; 
            }
            else
            {
                transactionStatusText.text = "Transaction Successful! TXID:\n" + txid;
            }
            if (recipientAddressInput.placeholder != null)
            {
                recipientAddressInput.text = "";
                recipientAddressInput.placeholder.gameObject.SetActive(true);
            }
            if (amountInput.placeholder != null)
            {
                amountInput.text = "";
                amountInput.placeholder.gameObject.SetActive(true);
            }
        }
        private void SendTransactionClearPlaceholder(string text)
        {
            if (recipientAddressInput.placeholder != null)
            {
                recipientAddressInput.placeholder.gameObject.SetActive(false);
            }
        }
        private void SendTransactionAmountClearPlaceholder(string text)
        {
            if (amountInput.placeholder != null)
            {
                amountInput.placeholder.gameObject.SetActive(false);
            }
        }
        private async Task FetchTransactionDetails()
        {
            if (getTransaction == null)
            {
                resultInputField.text = "Error: GetTransaction script not found.";
                return;
            }

            string txid = txidInputField.text.Trim();

            if (string.IsNullOrEmpty(txid))
            {
                resultInputField.text = "Please enter a TXID.";
                return;
            }

            resultInputField.text = "Fetching transaction...";
            string transactionInfo = await getTransaction.FetchTransaction(txid);
            resultInputField.text = transactionInfo;
            if (txidInputField.placeholder != null)
            {
                txidInputField.text = "";
                txidInputField.placeholder.gameObject.SetActive(true);
            }
        }
        private void GetTransactionClearPlaceholder(string text)
        {
            if (txidInputField.placeholder != null)
            {
                txidInputField.placeholder.gameObject.SetActive(false);
            }
        }
        private async Task ValidateAddressDetails()
        {
            if (validateAddress == null)
            {
                addressResultField.text = "Error: ValidateAddress script not found.";
                return;
            }

            string address = addressInputField.text.Trim();

            if (string.IsNullOrEmpty(address))
            {
                addressResultField.text = "Please enter an address.";
                return;
            }

            addressResultField.text = "Validating address...";
            string validationResult = await validateAddress.FetchValidation(address);
            addressResultField.text = validationResult;
            if (addressInputField.placeholder != null)
            {
                addressInputField.text = "";
                addressInputField.placeholder.gameObject.SetActive(true);
            }
        }
        private void ValidateTransactionClearPlaceholder(string text)
        {
            if (addressInputField.placeholder != null)
            {
                addressInputField.placeholder.gameObject.SetActive(false);
            }
        }
        private async Task OnGetAccountAddressButtonClicked()
        {
            string accountName = accountNewNameInput.text;

            if (string.IsNullOrEmpty(accountName))
            {
                accountNewAddressText.text = "Please enter a valid account name.";
                return;
            }

            string accountAddress = await getNewAccountAddress.FetchNewAddress(accountName);
            accountNewAddressText.text = $"Account Address: {accountAddress}";
            if (accountNewNameInput.placeholder != null)
            {
                accountNewNameInput.text = "";
                accountNewNameInput.placeholder.gameObject.SetActive(true);
            }
        }
        private void GetAccountClearPlaceholder(string text)
        {
            if (accountNewNameInput.placeholder != null)
            {
                accountNewNameInput.placeholder.gameObject.SetActive(false);
            }
        }
        private void GetAccountAddressClearPlaceholder(string text)
        {
            if (accountAddressNameInput.placeholder != null)
            {
                accountAddressNameInput.placeholder.gameObject.SetActive(false);
            }
        }
        private void GetStakerAuthoritiesClearPlaceholder(string text)
        {
            if (stakerNameInput.placeholder != null)
            {
                stakerNameInput.placeholder.gameObject.SetActive(false);
            }
        }
        private void GetStealthAddressClearPlaceholder(string text)
        {
            if (stealthLabelInput.placeholder != null)
            {
                stealthLabelInput.placeholder.gameObject.SetActive(false);
            }
        }
        private void GetStealthRecipientAddressClearPlaceholder(string text)
        {
            if (stealthRecipientAddressInput.placeholder != null)
            {
                stealthRecipientAddressInput.placeholder.gameObject.SetActive(false);
            }
        }
        private void GetStealthAmountClearPlaceholder(string text)
        {
            if (stealthAmountInput.placeholder != null)
            {
                stealthAmountInput.placeholder.gameObject.SetActive(false);
            }
        }

        private async Task OnListAccountsButtonClicked()
        {         
            string accountDetails = await listAccounts.FetchAllAccounts();
            accountAddressInput.text = accountDetails;
        }
        private async Task OnGenerateStealthAddress()
        {
            string label = stealthLabelInput.text;
            if (string.IsNullOrEmpty(label))
            {
                stealthAddressOutput.text = "Label cannot be empty.";
                return;
            }

            string stealthAddress = await getNewStealthAddress.FetchNewStealthAddress(label);
            stealthAddressOutput.text = stealthAddress;
        }
        private async Task SendStealthTransaction()
        {
            string stealthAddress = stealthRecipientAddressInput.text.Trim();

            if (!decimal.TryParse(stealthAmountInput.text, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
            {
                stealthTransactionStatusText.text = "Invalid amount!";
                return;
            }

            stealthTransactionStatusText.text = "Sending...";

            string txid = await sendToStealthAddress.SendStealthTransaction(stealthAddress, amount);

            if (txid.StartsWith("Error") || txid.StartsWith("Parsing error"))
            {
                stealthTransactionStatusText.text = txid;
            }
            else
            {
                stealthTransactionStatusText.text = "Transaction Successful! TXID:\n" + txid;
            }
        }
        private async Task FetchAuthorities()
        {
            string stakerName = stakerNameInput.text.Trim();
            if (string.IsNullOrEmpty(stakerName))
            {
                resultAuthorityInputField.text = "Error: Staker name is required!";
                return;
            }

            string jsonResponse = await getStakerAuthorities.FetchStakerAuthorities(stakerName);

            try
            {
                JObject result = JObject.Parse(jsonResponse);

                string formattedData =
                    $"Owner Address: {result["owner"]?["address"]?.ToString() ?? "N/A"}\n" +
                    $"Owner PubKey: {result["owner"]?["pubkey"]?.ToString() ?? "N/A"}\n\n" +
                    $"Manager Address: {result["manager"]?["address"]?.ToString() ?? "N/A"}\n" +
                    $"Manager PubKey: {result["manager"]?["pubkey"]?.ToString() ?? "N/A"}\n\n" +
                    $"Delegate Address: {result["delegate"]?["address"]?.ToString() ?? "N/A"}\n" +
                    $"Delegate PubKey: {result["delegate"]?["pubkey"]?.ToString() ?? "N/A"}\n\n" +
                    $"Controller Address: {result["controller"]?["address"]?.ToString() ?? "N/A"}\n" +
                    $"Controller PubKey: {result["controller"]?["pubkey"]?.ToString() ?? "N/A"}";

                resultAuthorityInputField.text = formattedData;
            }
            catch
            {
                resultAuthorityInputField.text = "Error: Failed to parse the response!";
            }
        }
        private async Task FetchAddresses()
        {
            string account = accountAddressNameInput.text.Trim();
            if (string.IsNullOrEmpty(account))
            {
                addressesResultInput.text = "Error: Account name is required!";
                return;
            }

            string jsonResponse = await getAddressesByAccount.FetchAddresses(account);
            addressesResultInput.text = jsonResponse;
        }
        private async Task RunRepairWallet()
        {
            repairWalletResult.text = "Checking wallet...";
            string result = await repairWallet.RunRepairWallet();
            repairWalletResult.text = result;
        }
    }

}

