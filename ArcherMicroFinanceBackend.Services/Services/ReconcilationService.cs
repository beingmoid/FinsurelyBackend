using PanoramBackend.Data.Entities;
using PanoramBackend.Data.Repository;
using PanoramBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data;
using OfficeOpenXml;
using System.Net;
using System.IO;
using System.ComponentModel.DataAnnotations;
using static NukesLab.Core.Common.Constants;
using AutoMapper;

namespace PanoramBackend.Services.Services
{

    public class ReconcilationService : BaseService<Reconcilation, int>, IReconcilationService
    {
        private readonly ISalesInvoiceService _sales;
        private readonly IDocumentService _docService;
        private readonly ILedgerEntriesService _ledger;
        private readonly IAgentService _agentService;
        private readonly IInsuranceCompanyService _companyService;
        private readonly IMapper mapperService;

        public ReconcilationService(RequestScope scopeContext, IReconcilationRepository repo,ISalesInvoiceService sales,
            IDocumentService docService, ILedgerEntriesService ledger,IAgentService agentService, 
            IInsuranceCompanyService companyService) : base(scopeContext, repo)

        {
            _sales = sales;
            _docService = docService;
            _ledger = ledger;
            _agentService = agentService;
            _companyService = companyService;
            mapperService = scopeContext.Mapper;
        }
        private DataTable ExcelPackageToDataTable(ExcelPackage excelPackage)
        {
            DataTable dt = new DataTable();
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.First();

            //check if the worksheet is completely empty
            if (worksheet.Dimension == null)
            {
                return dt;
            }

            //create a list to hold the column names
            List<string> columnNames = new List<string>();

            //needed to keep track of empty column headers
            int currentColumn = 1;

            //loop all columns in the sheet and add them to the datatable
            foreach (var cell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
            {
                string columnName = cell.Text.Trim();

                //check if the previous header was empty and add it if it was
                if (cell.Start.Column != currentColumn)
                {
                    columnNames.Add("Header_" + currentColumn);
                    dt.Columns.Add("Header_" + currentColumn);
                    currentColumn++;
                }

                //add the column name to the list to count the duplicates
                columnNames.Add(columnName);

                //count the duplicate column names and make them unique to avoid the exception
                //A column named 'Name' already belongs to this DataTable
                int occurrences = columnNames.Count(x => x.Equals(columnName));
                if (occurrences > 1)
                {
                    columnName = columnName + "_" + occurrences;
                }

                //add the column to the datatable
                dt.Columns.Add(columnName);

                currentColumn++;
            }

            //start adding the contents of the excel file to the datatable
            for (int i = 2; i <= worksheet.Dimension.End.Row; i++)
            {
                var row = worksheet.Cells[i, 1, i, worksheet.Dimension.End.Column];
                DataRow newRow = dt.NewRow();

                //loop all cells in the row
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }

                dt.Rows.Add(newRow);
            }   

            return dt;
        }
        public async Task<ReconReport> ProcessReconcilation(Recon Recon)
        {

            var doc =await _docService.GetOne(Recon.DocumentId);
            UserDetails agent = null;
            UserDetails company = null;
            List<Statement> data = null;
            AccountsReceviableReport ar = null;
            if (Recon.SalesAgentId!=null)
            {
                agent = await _agentService.GetOne((int)Recon.SalesAgentId);
                ar = (await _ledger.GetAccountReceviableStatement((int)agent.DefaultAccountId, Recon.From, Recon.To));
                data = ar.Statement;
            }
            else
            {
                company = await _companyService.GetOne((int)Recon.InsuranceCompanyId);
                ar = (await _ledger.GetAccountReceviableStatement((int)company.DefaultAccountId, Recon.From, Recon.To));
                data = ar.Statement;
            }
     
            var client = new WebClient();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            String url = doc.BlobURI;
            var fullPath = Path.GetTempFileName();
            client.DownloadFile(url, fullPath);
            var fileInfo = new FileInfo(fullPath);
            var reconData = Recon.ReconData;
            var mappingInfo = new MappingReport();
            DataTable dt = new DataTable();
            var mapper = new Dictionary<string, string>();
            string[] columns;
       
            var config = MappingConfiguration.Mapper;

            //Mapping and Applicablity Checks

            #region Mappings
            mappingInfo.TransactionType = (false, reconData.SingleOrDefault(x => x.MappedTo == 1 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.TransactionType.Applicable  )
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 1).ColumnName,  config.SingleOrDefault(x => x.Id == 1).ColumnName);
            }
            mappingInfo.InvoiceNumber = (false, reconData.SingleOrDefault(x => x.MappedTo == 2 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.InvoiceNumber.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 2).ColumnName,  config.SingleOrDefault(x => x.Id == 2).ColumnName);
            }
            mappingInfo.PolicyNumber = (false, reconData.SingleOrDefault(x => x.MappedTo == 3 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.PolicyNumber.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 3).ColumnName, config.SingleOrDefault(x => x.Id == 3).ColumnName);
            }
            mappingInfo.InvoiceDate = (false, reconData.SingleOrDefault(x => x.MappedTo == 4 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.InvoiceDate.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 4).ColumnName, config.SingleOrDefault(x => x.Id == 4).ColumnName);
            }
            mappingInfo.InsuranceType = (false, reconData.SingleOrDefault(x => x.MappedTo == 5 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.InsuranceType.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 5).ColumnName,  config.SingleOrDefault(x => x.Id == 5).ColumnName);
            }
            mappingInfo.Amount = (false, reconData.SingleOrDefault(x => x.MappedTo == 6 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.Amount.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 6).ColumnName,  config.SingleOrDefault(x => x.Id == 6).ColumnName);
            }
            mappingInfo.Debit = (false, reconData.SingleOrDefault(x => x.MappedTo == 7 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.Debit.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 7).ColumnName, config.SingleOrDefault(x => x.Id == 7).ColumnName);
            }
            mappingInfo.Credit = (false, reconData.SingleOrDefault(x => x.MappedTo == 8 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.Credit.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 8).ColumnName,  config.SingleOrDefault(x => x.Id == 8).ColumnName);
            }
            mappingInfo.Balance = (false, reconData.SingleOrDefault(x => x.MappedTo == 9 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.Balance.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 9).ColumnName,  config.SingleOrDefault(x => x.Id == 9).ColumnName);
            }
            mappingInfo.Vehicle = (false, reconData.SingleOrDefault(x => x.MappedTo == 10 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.Vehicle.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 10).ColumnName,  config.SingleOrDefault(x => x.Id == 10).ColumnName);
            }
            mappingInfo.Customer =(false, reconData.SingleOrDefault(x => x.MappedTo == 11 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.Customer.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 11).ColumnName,  config.SingleOrDefault(x => x.Id == 11).ColumnName);
            }
     
            mappingInfo.TransactionRefNumber = (false, reconData.SingleOrDefault(x => x.MappedTo == 12 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.TransactionRefNumber.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 12).ColumnName, config.SingleOrDefault(x => x.Id == 12).ColumnName);
            }
            mappingInfo.Memo = (false, reconData.SingleOrDefault(x => x.MappedTo == 13 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.Memo.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 13).ColumnName,  config.SingleOrDefault(x => x.Id == 13).ColumnName);
            }
            mappingInfo.AccountName = (false, reconData.SingleOrDefault(x => x.MappedTo == 14 && x.AllowMapping && x.Compare) != null);
            if (mappingInfo.AccountName.Applicable)
            {
                mapper.Add(reconData.SingleOrDefault(x => x.MappedTo == 14).ColumnName, config.SingleOrDefault(x => x.Id == 14).ColumnName);
            }
   
            #endregion
            // mappingInfo.InsuranceCompany = (false, reconData.SingleOrDefault(x => x.ColumnName == "Insurance Company" && x.AllowMapping && x.Compare) != null);
            List<Statement> excel;
            using (ExcelPackage pck = new ExcelPackage(fileInfo))
            {
                var sheet = pck.Workbook.Worksheets.First();
                //dt = this.ExcelPackageToDataTable(pck);
                columns = sheet.GetHeaderColumns();
                excel = sheet.ToList<Statement>(mapper);
            }
            var recon = new Reconcilation();
            if (Recon.SalesAgentId != null)
            {
                recon.SalesAgentId = Recon.SalesAgentId;

            }
            else
            {
                recon.InsuranceCompanyId = Recon.InsuranceCompanyId;
            }
            recon.To = Recon.To;
            recon.From = Recon.From;
            recon.DocumentId = Recon.DocumentId;
            int i = 0;
            List<Corrections> corrections = new List<Corrections>();
            List<SalesStatementReconcilation> reconReport = new List<SalesStatementReconcilation>();
            decimal excelBalance = 0;
            if (mappingInfo.Balance.Applicable && mappingInfo.Amount.Applicable)
            {
                excelBalance = excel.LastOrDefault().Balance;
              
              
            }
             else if (mappingInfo.Credit.Applicable && mappingInfo.Debit.Applicable)
            {
                var debit = excel.Sum(x => x.Debit);
                var credit = excel.Sum(x => x.Credit);
                excelBalance = debit - credit;
            }
            if (excelBalance > ar.TotalBalance)
            {
                recon.AmountDifference = excelBalance - ar.TotalBalance;
            }
            else
            {
                recon.AmountDifference = ar.TotalBalance - excelBalance;
            }
            var tempId = 1;
            var finalReport = new ReconReport();
            Statement last = data.Last();
            foreach (var row in excel)
            {
                var tempId2 = 1;
                Statement foundItem = null;
                Statement errorItem = null;
                foreach (var item in data)
                {
                    if (!string.IsNullOrWhiteSpace(row.PolicyNumber)) //if Receving or Payment
                    {

                        if (mappingInfo.PolicyNumber.Applicable)
                        {

                            if (row.PolicyNumber?.ToLower() == item.PolicyNumber?.ToLower())
                            {
                                mappingInfo.PolicyNumber = (true, true);
                                foundItem = item;
                                //Transaction Type

                                if (mappingInfo.TransactionType.Applicable)
                                {
                                    if (row.TransactionType == item.TransactionType)
                                    {
                                        mappingInfo.TransactionType = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.TransactionType = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "TransactionType",
                                            Description = "Both columns are not identical"
                                        });

                                    }
                                }
                                if (mappingInfo.InvoiceNumber.Applicable)
                                {
                                    //Invoice Number
                                    if (row.InvoiceNumber == item.InvoiceNumber)
                                    {
                                        mappingInfo.InvoiceNumber = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.InvoiceNumber = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "Invoice Number",
                                            Description = "Both columns are not identical"
                                        });
                                    }
                                }

                                if (mappingInfo.InvoiceDate.Applicable)
                                {
                                    // InvoiceDate
                                    if (row.InvoiceDate.Date == item.InvoiceDate.Date)
                                    {
                                        mappingInfo.InvoiceDate = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.InvoiceDate = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "Invoice Date",
                                            Description = "Date not matched"
                                            ,
                                            TempId = tempId
                                        });
                                    }
                                }
                                if (mappingInfo.InsuranceType.Applicable)
                                {
                                    // InvoiceDate
                                    if (row.InsuranceType?.ToLower() == item.InsuranceType?.ToLower())
                                    {
                                        mappingInfo.InsuranceType = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.InsuranceType = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "Invoice Number",
                                            Description = "Both columns are not identical"
                                              ,
                                            TempId = tempId
                                        });
                                    }
                                }

                                if (mappingInfo.Amount.Applicable)
                                {
                                    // Amount
                                    if (row.Amount == item.Amount)
                                    {
                                        mappingInfo.Amount = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.Amount = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "Amount",
                                            Description = "Both columns are not identical"
                                              ,
                                            TempId = tempId
                                        });
                                    }
                                }

                                if (mappingInfo.Debit.Applicable)
                                {
                                    // Amount
                                    if (row.Debit == item.Debit)
                                    {
                                        mappingInfo.Debit = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.Debit = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "Debit",
                                            Description = "Both columns are not identical"
                                              ,
                                            TempId = tempId
                                        });
                                    }
                                }

                                if (mappingInfo.Credit.Applicable)
                                {
                                    // Amount
                                    if (row.Credit == item.Credit)
                                    {
                                        mappingInfo.Credit = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.Credit = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "Credit",
                                            Description = "Both columns are not identical"
                                              ,
                                            TempId = tempId
                                        });
                                    }
                                }

                                if (mappingInfo.Balance.Applicable)
                                {
                                    // Amount
                                    if (row.Balance == item.Balance)
                                    {
                                        mappingInfo.Balance = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.Balance = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "Balance",
                                            Description = "Both columns are not identical"
                                              ,
                                            TempId = tempId
                                        });
                                    }
                                }

                                if (mappingInfo.Vehicle.Applicable)
                                {
                                    // Vehicle
                                    if (row.Vehicle.ToLower().Contains(item.Vehicle.ToLower(), StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        mappingInfo.Vehicle = (true, true);
                                    }
                                    else
                                    {
                                        mappingInfo.Vehicle = (false, true);
                                        recon.Corrections.Add(new Corrections()
                                        {
                                            Name = "Vehicle",
                                            Description = "Both columns are not identical"
                                              ,
                                            TempId = tempId
                                        });
                                    }
                                }

                                if (mappingInfo.Customer.Applicable)
                                {
                                    if (!string.IsNullOrWhiteSpace(row.Name) && !string.IsNullOrWhiteSpace(item.Name))
                                    {
                                        // Vehicle
                                        if (row.Name.ToLower().Contains(item.Name?.ToLower(), StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            mappingInfo.Customer = (true, true);
                                        }
                                        else
                                        {
                                            mappingInfo.Customer = (false, true);
                                            recon.Corrections.Add(new Corrections()
                                            {
                                                Name = "Customer",
                                                Description = "Both columns are not identical"
                                                  ,
                                                TempId = tempId
                                            });
                                        }
                                    }
                                }
                                foundItem = item;
                                data.Remove(foundItem);
                                var temp = mapperService.Map<Statement, SalesStatementReconcilation>(item);
                                temp.MappingReport = mappingInfo;
                                finalReport.Statement.Add(temp);
                                break;
                            }
                            if (item.Equals(last) && foundItem == null)
                            {
                                mappingInfo.PolicyNumber = (false, true);
                                recon.Corrections.Add(new Corrections()
                                {
                                    Name = "Policy Number",
                                    Description = $"Policy Number --{row.PolicyNumber}-- could not be found the whole row is rejected"
                                              ,
                                    TempId = tempId
                                });
                                errorItem = item;
                                var temp = mapperService.Map<Statement, SalesStatementReconcilation>(item);
                                temp.MappingReport = mappingInfo;
                                temp.Rejected = true;
                                finalReport.Statement.Add(temp);
                                //Error mapperService.Map< Statement, SalesStatementReconcilation >(item);
                                break;
                            }

                        }
                   
                     
                   
                     
                    }
                    else
                    {
                        if (row.TransactionRefNumber?.ToLower() == item.TransactionRefNumber?.ToLower())
                        {
                            mappingInfo.TransactionRefNumber = (true, true);
                            foundItem = item;
                            if (mappingInfo.TransactionRefNumber.Applicable)
                            {
                                // TransactionRefNumber
                                if (row.TransactionRefNumber == item.TransactionRefNumber)
                                {
                                    mappingInfo.TransactionRefNumber = (true, true);
                                }
                                else
                                {
                                    mappingInfo.TransactionRefNumber = (false, true);
                                    recon.Corrections.Add(new Corrections()
                                    {
                                        Name = "TransactionRefNumber",
                                        Description = "Both columns are not identical"
                                          ,
                                        TempId = tempId
                                    });
                                }
                            }

                            if (mappingInfo.Debit.Applicable)
                            {
                                // Amount
                                if (row.Debit == item.Debit)
                                {
                                    mappingInfo.Debit = (true, true);
                                }
                                else
                                {
                                    mappingInfo.Debit = (false, true);
                                    recon.Corrections.Add(new Corrections()
                                    {
                                        Name = "Debit",
                                        Description = "Both columns are not identical"
                                          ,
                                        TempId = tempId
                                    });
                                }
                            }

                            if (mappingInfo.Credit.Applicable)
                            {
                                // Amount
                                if (row.Credit == item.Credit)
                                {
                                    mappingInfo.Credit = (true, true);
                                }
                                else
                                {
                                    mappingInfo.Credit = (false, true);
                                    recon.Corrections.Add(new Corrections()
                                    {
                                        Name = "Credit",
                                        Description = "Both columns are not identical"
                                          ,
                                        TempId = tempId
                                    });
                                }
                            }

                            if (mappingInfo.Balance.Applicable)
                            {
                                // Amount
                                if (row.Balance == item.Balance)
                                {
                                    mappingInfo.Balance = (true, true);
                                }
                                else
                                {
                                    mappingInfo.Balance = (false, true);
                                    recon.Corrections.Add(new Corrections()
                                    {
                                        Name = "Balance",
                                        Description = "Both columns are not identical"
                                          ,
                                        TempId = tempId
                                    });
                                }
                            }
                            data.Remove(foundItem);
                            var temp = mapperService.Map<Statement, SalesStatementReconcilation>(item);
                            temp.MappingReport = mappingInfo;
                            finalReport.Statement.Add(temp);
                            break;

                        }
                        if (item.Equals(last) && foundItem == null)
                        {
                            mappingInfo.PolicyNumber = (false, true);
                            recon.Corrections.Add(new Corrections()
                            {
                                Name = "Transaction Reference Number",
                                Description = "Ambigious entry or transaction reference number could not be found the whole row is rejected"
                                          ,
                                TempId = tempId
                            });
                            errorItem = item;
                            var temp = mapperService.Map<Statement, SalesStatementReconcilation>(item);
                            temp.MappingReport = mappingInfo;
                            temp.Rejected = true;
                            finalReport.Statement.Add(temp);
                            //Error mapperService.Map< Statement, SalesStatementReconcilation >(item);
                            break;
                        }

                    }

                    tempId2++;
                    //end of child loop
                }
                if (data.Count == 0)
                {
                    corrections.Add(new Corrections()
                    {
                        Name = "Data not found",
                        Description = $"Data could not be found between dates ${Recon.From.Date} to ${Recon.To.Date} "

                    });
                }
                tempId++;

                //finalReport.Mapping.Add(mappingInfo);
            }

            recon.NoOfSalesMissing = reconReport.Count;
            finalReport.Columns = columns;
            finalReport.SalesAgentId = recon?.SalesAgentId;
            finalReport.SalesAgent = await _agentService.GetOne( recon.SalesAgentId ?? default(int));
            finalReport.InsuranceCompanyId = recon?.InsuranceCompanyId;
            finalReport.DocumentId = doc.Id;
            finalReport.Documents = doc;
            finalReport.GeneratedDate = DateTime.Now;
            finalReport.From = Recon.From;
            finalReport.To = Recon.To;
            finalReport.AmountDifference = recon.AmountDifference;
            finalReport.NoOfSalesMissing = recon.NoOfSalesMissing;
            finalReport.Corrections.AddRange(recon.Corrections);
            //finalReport.Statement = reconReport;
        


            return finalReport;


        }

        protected override Task WhileInserting(IEnumerable<Reconcilation> entities)
        {
            foreach (var item in entities)
            {
                item.GeneratedDate = DateTime.Now;

            }
            return base.WhileInserting(entities);
        }
    }
    public interface IReconcilationService : IBaseService<Reconcilation, int>
    {
        Task<ReconReport> ProcessReconcilation(Recon report);
    }
    public class ReconReport
    {
        public int? SalesAgentId { get; set; }
        public UserDetails SalesAgent { get; set; }
        public int DocumentId { get; set; }
        public Documents Documents { get; set; }
        public DateTime GeneratedDate { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal AmountDifference { get; set; }
        public string[] Columns { get; set; }
        public int NoOfSalesMissing { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public UserDetails InsuranceCompany { get; set; }
        private List<Corrections> _corrections;
        public List<Corrections> Corrections => _corrections ?? (_corrections = new List<Corrections>());
        public List<SalesStatementReconcilation> Statement { get; set; } = new List<SalesStatementReconcilation>();
   
    }
    public class Recon
    {
        public int? SalesAgentId { get; set; }
        public int? InsuranceCompanyId { get; set; }
        public int DocumentId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<ReconData> ReconData { get; set; }
    }
    public class ReconData
    {
        public string ColumnName { get; set; }
        public int MappedTo { get; set; }
        public bool AllowMapping { get; set; }
        public bool Compare { get; set; }
     

    }

    public class SalesStatementReconcilation
    {
        public DateTime Date { get; set; }
        public int TransactionType { get; set; }
        public int InvoiceNumber { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InsuranceType { get; set; }
        public string TransactionRefNumber { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string Vehicle { get; set; }
        public bool Rejected { get; set; }
        public MappingReport MappingReport { get; set; } = new MappingReport();


    }
    public class MappingReport
    {
        public (bool Status, bool Applicable ) Date { get; set; }
        public (bool Status, bool Applicable) Customer { get; set; }
        public (bool Status, bool Applicable) InsuranceCompany { get; set; }
        public (bool Status, bool Applicable) TransactionType { get; set; }
        public (bool Status, bool Applicable) InvoiceNumber { get; set; }
        public (bool Status, bool Applicable) PolicyNumber { get; set; }
        public (bool Status, bool Applicable) InvoiceDate { get; set; }
        public (bool Status, bool Applicable) InsuranceType { get; set; }
        public (bool Status, bool Applicable) TransactionRefNumber { get; set; }
        public (bool Status, bool Applicable) Amount { get; set; }
        public (bool Status, bool Applicable) Gross{ get; set; }
        public (bool Status, bool Applicable) Comission { get; set; }
        public (bool Status, bool Applicable) ComissionRate { get; set; }
        public (bool Status, bool Applicable) Debit { get; set; }
        public (bool Status, bool Applicable) VAT { get; set; }
        public (bool Status, bool Applicable) Memo { get; set; }
        public (bool Status, bool Applicable) NET { get; set; }
        public (bool Status, bool Applicable) Total { get; set; }
        public (bool Status, bool Applicable) Credit { get; set; }
        public (bool Status, bool Applicable) Balance { get; set; }
        public (bool Status, bool Applicable) Vehicle { get; set; }
        public (bool Status, bool Applicable) AccountName { get; set; }





    }

    public static partial class Extensions
    {
        /// <summary>
        ///     An object extension method that converts the @this to a date time.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a DateTime.</returns>
        public static DateTime ToDateTime(this object @this)
        {
            return Convert.ToDateTime(@this);
        }
      
            public static List<T> ToList<T>(this ExcelWorksheet worksheet, Dictionary<string, string> map = null) where T : new()
            {
                //DateTime Conversion
                var convertDateTime = new Func<double, DateTime>(excelDate =>
                {
                    if (excelDate < 1)
                        throw new ArgumentException("Excel dates cannot be smaller than 0.");

                    var dateOfReference = new DateTime(1900, 1, 1);

                    if (excelDate > 60d)
                        excelDate = excelDate - 2;
                    else
                        excelDate = excelDate - 1;
                    return dateOfReference.AddDays(excelDate);
                });

                var props = typeof(T).GetProperties()
                    .Select(prop =>
                    {
                        var displayAttribute = (DisplayAttribute)prop.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                        return new
                        {
                            Name = prop.Name,
                            DisplayName = displayAttribute == null ? prop.Name : displayAttribute.Name,
                            Order = displayAttribute == null || !displayAttribute.GetOrder().HasValue ? 999 : displayAttribute.Order,
                            PropertyInfo = prop,
                            PropertyType = prop.PropertyType,
                            HasDisplayName = displayAttribute != null
                        };
                    })
                .Where(prop => !string.IsNullOrWhiteSpace(prop.DisplayName))
                .ToList();

                var retList = new List<T>();
                var columns = new List<ExcelMap>();

                var start = worksheet.Dimension.Start;
                var end = worksheet.Dimension.End;
                var startCol = start.Column;
                var startRow = start.Row;
                var endCol = end.Column;
                var endRow = end.Row;

                // Assume first row has column names
                for (int col = startCol; col <= endCol; col++)
                {
                    var cellValue = (worksheet.Cells[startRow, col].Value ?? string.Empty).ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        columns.Add(new ExcelMap()
                        {
                            Name = cellValue,
                            MappedTo = map == null || map.Count == 0 ?
                                cellValue :
                                map.ContainsKey(cellValue) ? map[cellValue] : string.Empty,
                            Index = col
                        });
                    }
                }

                // Now iterate over all the rows
                for (int rowIndex = startRow + 1; rowIndex <= endRow; rowIndex++)
                {
                    var item = new T();
                    columns.ForEach(column =>
                    {
                        var value = worksheet.Cells[rowIndex, column.Index].Value;
                        var valueStr = value == null ? string.Empty : value.ToString().Trim();
                        var prop = string.IsNullOrWhiteSpace(column.MappedTo) ?
                            null :
                            props.FirstOrDefault(p => p.Name.Contains(column.MappedTo));

                        // Excel stores all numbers as doubles, but we're relying on the object's property types
                        if (prop != null)
                        {
                            var propertyType = prop.PropertyType;
                            object parsedValue = null;

                            if (propertyType == typeof(int?) || propertyType == typeof(int))
                            {
                                int val;
                                if (!int.TryParse(valueStr, out val))
                                {
                                    val = default(int);
                                }

                                parsedValue = val;
                            }
                            else if (propertyType == typeof(short?) || propertyType == typeof(short))
                            {
                                short val;
                                if (!short.TryParse(valueStr, out val))
                                    val = default(short);
                                parsedValue = val;
                            }
                            else if (propertyType == typeof(long?) || propertyType == typeof(long))
                            {
                                long val;
                                if (!long.TryParse(valueStr, out val))
                                    val = default(long);
                                parsedValue = val;
                            }
                            else if (propertyType == typeof(decimal?) || propertyType == typeof(decimal))
                            {
                                decimal val;
                                if (!decimal.TryParse(valueStr, out val))
                                    val = default(decimal);
                                parsedValue = val;
                            }
                            else if (propertyType == typeof(double?) || propertyType == typeof(double))
                            {
                                double val;
                                if (!double.TryParse(valueStr, out val))
                                    val = default(double);
                                parsedValue = val;
                            }
                            else if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
                            {
                                if (value!=null)
                                {
                                    if (DateTime.TryParse(value.ToString(), out DateTime Temp) == true)

                                    {
                                        parsedValue = Convert.ToDateTime(value);
                                    }
                                    else
                                    {
                                        parsedValue = convertDateTime((double)value);
                                    }
                                }
               
                            
                           
                            }
                            else if (propertyType.IsEnum)
                            {
                                try
                                {
                                    parsedValue = Enum.ToObject(propertyType, int.Parse(valueStr));
                                }
                                catch
                                {
                                    parsedValue = Enum.ToObject(propertyType, 0);
                                }
                            }
                            else if (propertyType == typeof(string))
                            {
                                parsedValue = valueStr;
                            }
                            else
                            {
                                try
                                {
                                    parsedValue = Convert.ChangeType(value, propertyType);
                                }
                                catch
                                {
                                    parsedValue = valueStr;
                                }
                            }

                            try
                            {
                                prop.PropertyInfo.SetValue(item, parsedValue);
                            }
                            catch (Exception ex)
                            {
                                // Indicate parsing error on row?
                            }
                        }
                    });

                    retList.Add(item);
                }

                return retList;
            }

    }
public class ExcelMap
    {
        public string Name { get; set; }
        public string MappedTo { get; set; }
        public int Index { get; set; }
    }
}
