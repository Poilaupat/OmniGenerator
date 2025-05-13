using OmniGenerator.Lib.Hierarchy;
using OmniGenerator.Lib.Infrastructure;
using OmniGenerator.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OmniGenerator.Plugin.Tessi.Packagers.Compliance
{
    [Export(typeof(IPackager))]
    [PluginMetadata("packager.tessi.eligibility", "A packager that produces eligibility resquest from Wecheck Compliance")]

    public class EligibilityPackager : IPackager
    {
        public async Task ProcessAsync(Root root, string path)
        {
            var header = new Header(
                root.Fields["bankCode"].StringValue,
                root.Fields["bankUnitCode"].StringValue,
                root.Fields["providerCode"].StringValue,
                root.Fields["culture"].StringValue,
                root.Fields["purpose"].StringValue,
                root.Fields["bankFlow"].StringValue);

            var jsonRoot = new JsonRoot(
                root.Fields["schema"].StringValue,
                root.Fields["version"].StringValue,
                header
                );

            var documents = root
                .GetDocuments()
                .ToArray();

            for (var i = 0; i < documents.Count(); i++)
            {
                var deposit = new Deposit(
                    root.Fields["culture"].StringValue,
                    root.Fields["bankUnitCode"].StringValue,
                    root.Fields["providerCode"].StringValue,
                    documents[i].Fields["scanner"].StringValue,
                    documents[i].Fields["scanType"].StringValue,
                    documents[i].Fields["chain"].StringValue
                );

                var micr = new Micr(
                    documents[i].Fields["z4"].StringValue,
                    documents[i].Fields["z3"].StringValue,
                    documents[i].Fields["z2"].StringValue
                    );

                var check = new Check(
                    root.Fields["culture"].StringValue,
                    (int)documents[i].Fields["amount"].Value,
                    documents[i].Fields["providerId"].StringValue,
                    i,
                    micr
                );

                var transaction = new Transaction(
                    (int)documents[i].Fields["amount"].Value,
                    documents[i].Fields["remittingBranchCode"].StringValue,
                    documents[i].Fields["deskCode"].StringValue,
                    documents[i].Fields["accountNumber"].StringValue,
                    deposit,
                    check
                );

                jsonRoot.Transactions.Add(transaction);
            }

            var packagename = $"BosComplianceEligibility.{root.Fields["bankCode"].StringValue}.{root.Fields["bankUnitCode"].StringValue}.{root.Fields["providerCode"].StringValue}.{root.Fields["numlot"].Value}.{DateTime.Now:yyyyMMddHHmmss}";
            var packagepath = Path.Combine(path, packagename);

            if (!Directory.Exists(packagepath))
                Directory.CreateDirectory(packagepath);

            var jsonfilename = Path.Combine(packagepath, $"{packagename}.json");
            var topfilename = Path.Combine(packagepath, $"{packagename}.top");
            var jsonContent = JsonSerializer.Serialize(jsonRoot, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            await File.WriteAllTextAsync(jsonfilename, jsonContent);
            await File.WriteAllTextAsync(topfilename, string.Empty);
        }
    }

    #region Serialization

    public class Check
    {
        [JsonPropertyName("culture")]
        public string Culture { get; set; }

        [JsonPropertyName("imageRequest")]
        public bool ImageRequest { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("providerId")]
        public string ProviderId { get; set; }

        [JsonPropertyName("complianceId")]
        public int ComplianceId { get; set; }

        [JsonPropertyName("complianceReceptionDate")]
        public string ComplianceReceptionDate { get; set; }

        [JsonPropertyName("complianceLogicalReceptionDate")]
        public string ComplianceLogicalReceptionDate { get; set; }

        [JsonPropertyName("complianceRequestImageDate")]
        public string ComplianceRequestImageDate { get; set; }

        [JsonPropertyName("sequenceNumber")]
        public int SequenceNumber { get; set; }

        [JsonPropertyName("scanDate")]
        public DateTime ScanDate { get; set; }

        [JsonPropertyName("isStandard")]
        public bool IsStandard { get; set; }

        [JsonPropertyName("micr")]
        public Micr Micr { get; set; }

        [JsonPropertyName("images")]
        public List<object> Images { get; set; }

        [JsonPropertyName("otherReferences")]
        public List<OtherReference> OtherReferences { get; set; }

        [JsonPropertyName("result")]
        public object? Result { get; set; }

        public Check(string culture, int amount, string providerId, int sequenceNumber, Micr micr)
        {
            Culture = culture;
            ImageRequest = false;
            Amount = amount;
            ProviderId = providerId;
            ComplianceId = 0;
            SequenceNumber = sequenceNumber;
            ScanDate = DateTime.Now;
            IsStandard = true;
            Micr = micr;
            Images = new List<object>();
            OtherReferences = new List<OtherReference> { new OtherReference("additionnalLabel1"), new OtherReference("additionnalLabel2") };

            ComplianceReceptionDate = "0001-01-01T00:00:00+00:00";
            ComplianceLogicalReceptionDate = "0001-01-01T00:00:00+00:00";
            ComplianceRequestImageDate = "0001-01-01T00:00:00+00:00";
        }
    }

    public class Deposit
    {
        [JsonPropertyName("culture")]
        public string Culture { get; set; }

        [JsonPropertyName("remittingBranchCode")]
        public string RemittingBranchCode { get; set; }

        [JsonPropertyName("scanBranchCode")]
        public string ScanBranchCode { get; set; }

        [JsonPropertyName("refOp")]
        public string RefOp { get; set; }

        [JsonPropertyName("scanner")]
        public string Scanner { get; set; }

        [JsonPropertyName("scanType")]
        public string ScanType { get; set; }

        [JsonPropertyName("chain")]
        public string Chain { get; set; }

        [JsonPropertyName("region")]
        public object? Region { get; set; }

        [JsonPropertyName("depositSlip")]
        public object? DepositSlip { get; set; }

        public Deposit(string culture, string remittingBranchCode, string scanBranchCode, string scanner, string scanType, string chain)
        {
            Culture = culture;
            RemittingBranchCode = remittingBranchCode;
            ScanBranchCode = scanBranchCode;
            RefOp = "000000000000000000000000";
            Scanner = scanner;
            ScanType = scanType;
            Chain = chain;
        }
    }

    public class Header
    {
        [JsonPropertyName("bankCode")]
        public string BankCode { get; set; }

        [JsonPropertyName("bankUnitCode")]
        public string BankUnitCode { get; set; }

        [JsonPropertyName("providerCode")]
        public string ProviderCode { get; set; }

        [JsonPropertyName("culture")]
        public string Culture { get; set; }

        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }

        [JsonPropertyName("bankFlow")]
        public string BankFlow { get; set; }

        public Header(string bankCode, string bankUnitCode, string providerCode, string culture, string purpose, string bankFlow)
        {
            BankCode = bankCode;
            BankUnitCode = bankUnitCode;
            ProviderCode = providerCode;
            Culture = culture;
            Purpose = purpose;
            BankFlow = bankFlow;
        }
    }

    public class Micr
    {
        [JsonPropertyName("zone")]
        public List<Zone> Zone { get; set; }

        public Micr(string z4, string z3, string z2)
        {
            Zone = new List<Zone>
            {
                new Zone("Z4", z4),
                new Zone("Z3", z3),
                new Zone("Z2", z2),
            };
        }
    }

    public class OtherReference
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        public OtherReference(string key)
        {
            Key = key;
        }
    }

    public class JsonRoot
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; }

        [JsonPropertyName("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("header")]
        public Header Header { get; set; }

        [JsonPropertyName("transactions")]
        public List<Transaction> Transactions { get; set; }

        public JsonRoot(string schema, string version, Header header)
        {
            Schema = schema;
            Version = version;
            Header = header;
            Transactions = new List<Transaction>();
            TimeStamp = DateTime.Now;
        }
    }

    public class Transaction
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("bankUnitCode")]
        public string BankUnitCode { get; set; }

        [JsonPropertyName("deskCode")]
        public string DeskCode { get; set; }

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("isTechnicalAccount")]
        public bool IsTechnicalAccount { get; set; }

        [JsonPropertyName("riskInformation")]
        public string RiskInformation { get; set; }

        [JsonPropertyName("accountHolders")]
        public List<object> AccountHolders { get; set; }

        [JsonPropertyName("deposit")]
        public Deposit Deposit { get; set; }

        [JsonPropertyName("checks")]
        public List<Check> Checks { get; set; }

        public Transaction(int amount, string bankUnitCode, string deskCode, string accountNumber, Deposit deposit, Check cheque)
        {
            Date = DateTime.Now;
            Amount = amount;
            BankUnitCode = bankUnitCode;
            DeskCode = deskCode;
            AccountNumber = accountNumber;
            IsTechnicalAccount = false;
            RiskInformation = string.Empty;
            AccountHolders = new List<object>();
            Deposit = deposit;
            Checks = new List<Check> { cheque };
        }
    }

    public class Zone
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        public Zone(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    #endregion
}
