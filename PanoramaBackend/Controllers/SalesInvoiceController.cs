using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Api;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramaBackend.Data.Entities;
using PanoramaBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static NukesLab.Core.Common.Constants;
using PanoramaBackend.Services;
using System.IO;
using OfficeOpenXml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Globalization;
using System.Threading;
using Nest;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Elasticsearch.Net;
using Nest.JsonNetSerializer;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using NLog;
using PanoramaBackend.Data.Repository;
using Microsoft.AspNetCore.Hosting;
using PanoramaBackend.Data;
using PanoramaBackend.Services;
using FluentExcel;
using System.Linq.Expressions;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace PanoramaBackend.Api.Controllers
{

    public class SalesInvoiceController : BaseController<SalesInvoice, int>
    {
        private readonly ISalesInvoiceService _service;
        private readonly AMFContext _context;
        private readonly IFileUploader _fileUploader;
        private readonly IWebHostEnvironment _env;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDocumentService _docService;

        private readonly IServiceProvider _serviceProvider;

        //private readonly IElasticClient _elasticClient;
        private readonly ISalesInvoiceRepository _salesRepo;

        //        public string GetElasticsearchBulkJsonFromJson(string jsonStringWithArrayOfObjects, string firstParameterNameOfObjectInJsonStringArrayOfObjects)
        //        {
        //            return @"{ ""index"":{ } } 
        //" + jsonStringWithArrayOfObjects.Substring(1, jsonStringWithArrayOfObjects.Length - 2).Replace(@",{""" + firstParameterNameOfObjectInJsonStringArrayOfObjects + @"""", @" 
        //{ ""index"":{ } } 
        //{""" + firstParameterNameOfObjectInJsonStringArrayOfObjects + @"""") + @"
        //";
        //        }
        public SalesInvoiceController(RequestScope requestScope, ISalesInvoiceService service,
                   IFileUploader fileUploader,
                   IServiceProvider serviceProvider,
                   ISalesInvoiceRepository salesRepo,
                   IWebHostEnvironment webHostEnvironment,
                   AMFContext context,
                   //IElasticClient elasticClient,
                   IDocumentService docService)
            : base(requestScope, service)
        {
            _service = service;
            _context = context;
            _fileUploader = fileUploader;
            _env = webHostEnvironment;
            _docService = docService;
            _serviceProvider = serviceProvider;
            //_elasticClient = elasticClient;
            _salesRepo = salesRepo;
        } 
        [HttpPost("process")]
        public async Task<BaseResponse> Process(IFormFile file)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=insurancebrokersblob;AccountKey=fGdVMwRZ1W4Q3Q4o7qbWlIi/E815G14cg/rguE57fgmdh2FKLcJf0zJj4wxupggIb9xyLhoeoKUU+ASt9gRtiA==;EndpointSuffix=core.windows.net";
            string containerName = "policyuploads";
            string blobName = file.FileName;

            // Upload file to Azure Blob Storage
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(file.OpenReadStream(), true);

            // Process file with Azure Form Recognizer
            string endpoint = "https://qouteafnan.cognitiveservices.azure.com/";
            string apiKey = "d6fa3875c5544c1eb906178a45ed90bc";

            DocumentAnalysisClientOptions options = new DocumentAnalysisClientOptions(DocumentAnalysisClientOptions.ServiceVersion.V2022_08_31);

            DocumentAnalysisClient client = new DocumentAnalysisClient(new Uri(endpoint),
                new AzureKeyCredential(apiKey)
);
            string modelId = "CustomModel";
            AnalyzeDocumentOperation operation = await client.
                AnalyzeDocumentFromUriAsync(WaitUntil.Completed, modelId, blobClient.Uri);
            AnalyzeResult result = operation.Value;
            List<KeyValuePair<string, DocumentField>> keyValuePair = new List<KeyValuePair<string, DocumentField>>();
       
            return constructResponse(new {result= result.Documents.ToList(),url = blobClient.Uri.ToString()});

        }

        [HttpGet("GetPaginated")]
        public async Task<BaseResponse> GetPaginated(int page, int pageSize)
        {
            var data = (await this._service.Get(x => x
            .Include(x => x.SalesInvoicePerson)
            //Sales Agent
            .Include(x => x.PaymentMethod)

            .Include(x => x.BodyType)
         
            .Include(x => x.Vehicle)
            .Include(x => x.InsuranceCompany)
            .Include(x => x.Branch)
            )).ToList();
            //Insurance Company

            if (data != null)
            {
                var response = new { data = data.Skip((page - 1) * pageSize).Take(pageSize).ToList(), totalRows = data.Count };

                OtherConstants.isSuccessful = true;
                return constructResponse(response);
            }
            else
            {
                var response = new { totalRows = 0, data = new List<SalesInvoice>() };
                OtherConstants.isSuccessful = false;
                return constructResponse(response);
            }

        }
    
        [AllowAnonymous]
        public async override Task<BaseResponse> Get()
        {
            var sales = (await _service.Get(x =>x  //Customer
            .Include(x => x.SalesInvoicePerson)
            //Sales Agent
            .Include(x => x.PaymentMethod)


                                    .Include(x => x.Vehicle)
                       

            .Include(x => x.BodyType)
            .Include(x => x.InsuranceCompany)
            .Include(x => x.Branch)
            )); //Insurance Company



            OtherConstants.isSuccessful = true;
            OtherConstants.messageType = MessageType.Success;
            return constructResponse(sales);
        }

        public async override Task<BaseResponse> Put(int id, [FromBody] SalesInvoice entity)
        {
            var result = await _service.UpdateAsync(id, entity);

            return result ? constructResponse(OtherConstants.isSuccessful=true) : constructResponse(OtherConstants.isSuccessful = false);
           
        }
        //[HttpGet("ElasticSearch")]
        //public async override Task<BaseResponse> Search(string search)
        //{
        //    var sales = _salesRepo.Get(x=>x.)
        //}


        [HttpPost("UploadExcel")]
        public async Task<BaseResponse> UploadExcel([FromForm(Name = "file")] IFormFile file)
        {
            if (file != null)
            {

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    var blob = await _fileUploader.UploadFileAsync(file.FileName, fileBytes, "lym-files");
                    var doc = new Documents()
                    {
                        BlobFileName = blob.BlobFileName,
                        BlobURI = blob.BlobURI
                    };
                    var result = (await _docService.Insert(new[] { doc }));
                    //var headers = _service.GetExcelColumnHeader(blob.BlobURI);
                    OtherConstants.isSuccessful = result.Success;
                    return constructResponse(result.Entities.FirstOrDefault());
                    //blobs.Add(blob);
                }

                //OtherConstants.isSuccessful = true;
                //return new JsonResult(constructResponse(blobs));

            }
            OtherConstants.isSuccessful = false;
            return constructResponse(null);
        }
        [HttpGet("GetColumnHeader")]
        public BaseResponse GetColumnHeader(string fileUrl)
        {

            var headers = _service.GetExcelColumnHeader(fileUrl);
            OtherConstants.isSuccessful = true;
            return constructResponse(headers);
        }
        //public FileResult downloadFile(string filePath, string fileName)
        //{
        //    IFileProvider provider = new PhysicalFileProvider(filePath);
        //    IFileInfo fileInfo = provider.GetFileInfo(fileName);
        //    var readStream = fileInfo.CreateReadStream();
        //    var mimeType = "application/vnd.ms-excel";
        //    return File(readStream, mimeType, fileName);
        //}
        //[HttpPost("BulkUpload")]
        //public async Task<BaseResponse> BulkUploadExcelFile([FromForm(Name = "file")] IFormFile file, string? salesDate)
        //{
        //    string wwwPath = _env.WebRootPath;
        //    string contentPath = _env.ContentRootPath;


        //    if (file != null)
        //    {
        //        var filename = Path.GetFileName(file.FileName);
        //        var filePath = contentPath + $"\\uploads\\{new DateTime().Ticks.ToString()}" + filename;
        //        var ms = new MemoryStream();


        //        file.CopyTo(ms);
        //        var fileBytes = ms.ToArray();


        //        var stream = new FileStream(filePath, FileMode.CreateNew);

        //        await file.CopyToAsync(stream);




        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;



        //        //FileInfo fileInfo = new FileInfo(doc.BlobFileName);
        //        //fileInfo.Open(FileMode.Open, FileAccess.Read);
        //        //File fileInfo = downloadFile(doc.BlobURI, doc.BlobFileName);

        //        List<SalesInvoice> ListToInsert = new List<SalesInvoice>();
        //        List<SalesInvoice> RejectedList = new List<SalesInvoice>();
        //        ExcelPackage.LicenseContext = LicenseContext.Commercial;

        //        //using (var stream = file.OpenReadStream())
        //        using (ExcelPackage package = new ExcelPackage(stream))
        //        {


        //            ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
        //            int rowCount = worksheet.Dimension.Rows;
        //            int ColCount = worksheet.Dimension.Columns;
        //            int col = 1;


        //            for (int row = 2; row <= rowCount; row++)
        //            {
        //                var sales = new SalesInvoice();
        //                var saleLine = new SaleLineItem();
        //                bool isCustomer = false;
        //                bool isBranch = false;
        //                bool isPolicyType = false;
        //                bool isService = false;
        //                bool isBodyType = false;
        //                bool isInsuranceType = false;
        //                bool isInsuranceCompanies = false;
        //                bool isSalesAgent = false;
        //                bool isVehicle = false;
        //                bool isPaymentMethod = false;
        //                bool isRowRejected = false;










        //                var Branches = await _serviceProvider.GetRequiredService<IBranchService>().Get();
        //                var Policy = await _serviceProvider.GetRequiredService<IPolicyTypeService>().Get();
        //                var Services = await _serviceProvider.GetRequiredService<IServiceService>().Get();
        //                var Body = await _serviceProvider.GetRequiredService<IBodyTypeService>().Get();
        //                var InsuranceTypes = await _serviceProvider.GetRequiredService<IInsuranceTypeService>().Get();
        //                var InsuranceCompanies = await _serviceProvider.GetRequiredService<IInsuranceCompanyService>().Get();
        //                var SalesAgent = await _serviceProvider.GetRequiredService<IAgentService>().Get();
        //                var Vehicles = await _serviceProvider.GetRequiredService<IVehicleService>().Get();
        //                var PaymentMethods = await _serviceProvider.GetRequiredService<IPaymentMethodService>().Get();



        //                var PolicyNumber = worksheet.Cells[row, col++].Value.ToString();
        //                var Customer = worksheet.Cells[row, col++].Value.ToString();
        //                var Make = worksheet.Cells[row, col++].Value.ToString();
        //                var Model = worksheet.Cells[row, col++].Value.ToString();
        //                var BodyType = worksheet.Cells[row, col++].Value.ToString();
        //                var PolicyType = worksheet.Cells[row, col++].Value.ToString();
        //                var Service = worksheet.Cells[row, col++].Value.ToString();
        //                var Branch = worksheet.Cells[row, col++].Value.ToString();
        //                var Notes = worksheet.Cells[row, col++].Value.ToString();
        //                var PaymentMethod = worksheet.Cells[row, col++].Value.ToString();
        //                var InsuranceType = worksheet.Cells[row, col++].Value.ToString();
        //                var Premium = Convert.ToDecimal(worksheet.Cells[row, col++].Value.ToString());
        //                var Comission = Convert.ToDecimal(worksheet.Cells[row, col++].Value.ToString());
        //                var SalesPrice = Convert.ToDecimal(worksheet.Cells[row, col++].Value.ToString());
        //                var SalesDate = worksheet.Cells[row, col++].Value.ToString();
        //                var Agent = worksheet.Cells[row, col++].Value.ToString();
        //                var InsuranceCompany = worksheet.Cells[row, col++].Value.ToString();
        //                var Underwritter = worksheet.Cells[row, col++].Value.ToString();


        //                isPolicyType = PolicyType != null ? Policy?.FirstOrDefault(x => x.Name == PolicyType) != null : false;

        //                isVehicle = Vehicles != null ? Vehicles?.FirstOrDefault(x => x.Make == Make && x.Model == Model) != null : false;
        //                isBodyType = Body != null ? Body?.FirstOrDefault(x => x.Name == BodyType) != null : false;
        //                isService = Service != null ? Services?.FirstOrDefault(x => x.Name == Service) != null : false;
        //                isBranch = Branches != null ? Branches?.FirstOrDefault(x => x.BranchName == Branch) != null : false;
        //                isPaymentMethod = PaymentMethods != null ? PaymentMethods?.FirstOrDefault(x => x.Name == PaymentMethod) != null : false;
        //                isInsuranceType = InsuranceTypes != null ? PaymentMethods?.FirstOrDefault(x => x.Name == PaymentMethod) != null : false;
        //                isInsuranceCompanies = InsuranceCompanies != null ?
        //                InsuranceCompanies?.FirstOrDefault(x => x.DisplayNameAs == InsuranceCompany)?.Id != null : false;
        //                isSalesAgent = SalesAgent != null ? SalesAgent?.FirstOrDefault(x => x.DisplayNameAs == Agent) != null : false;

        //                //if (isCustomer)
        //                //{
        //                //    sales.CustomerDetailId = Customers.FirstOrDefault(x => x.DisplayNameAs == Customer).Id;
        //                //}
        //                //else
        //                //{
        //                //    var newCustomer = await _serviceProvider.GetRequiredService<ICustomerService>().Insert(new[] { new UserDetails() { DisplayNameAs = Customer, IsCustomer = true } });
        //                //    sales.CustomerDetailId = newCustomer.Entities.FirstOrDefault().Id;
        //                //}
        //                if (isVehicle)
        //                {
        //                    saleLine.VehilcleId = Vehicles?.FirstOrDefault(x => x.Make == Make && x.Model == Model).Id;

        //                }
        //                else
        //                {
        //                    var newVehicle = await _serviceProvider.GetRequiredService<IVehicleService>().Insert(new[] { new Vehicle() { Make = Make, Model = Model } });
        //                    saleLine.VehilcleId = newVehicle.Entities.FirstOrDefault().Id;
        //                }
        //                if (isBodyType)
        //                {
        //                    sales.BodyTypeId = Body?.FirstOrDefault(x => x.Name == BodyType)?.Id;

        //                }
        //                else
        //                {
        //                    var newBodyType = await _serviceProvider.GetRequiredService<IBodyTypeService>().Insert(new[] { new BodyType() { Name = BodyType } });
        //                    sales.BodyTypeId = newBodyType.Entities.FirstOrDefault().Id;
        //                }
        //                if (isPolicyType)
        //                {
        //                    sales.PolicyTypeId = Policy.FirstOrDefault(x => x.Name == PolicyType).Id;
        //                }
        //                else
        //                {
        //                    var newPolicyType = await _serviceProvider.GetRequiredService<IPolicyTypeService>().Insert(new[] { new PolicyType() { Name = PolicyType } });
        //                    sales.PolicyTypeId = newPolicyType.Entities.FirstOrDefault().Id;
        //                }
        //                if (isService)
        //                {
        //                    sales.ServiceId = Services?.FirstOrDefault(x => x.Name == Service)?.Id;
        //                }
        //                else
        //                {
        //                    var newService = await _serviceProvider.GetRequiredService<IServiceService>().Insert(new[] { new Service() { Name = Service } });
        //                    sales.ServiceId = newService.Entities.FirstOrDefault().Id;


        //                }
        //                if (isBranch)
        //                {
        //                    sales.BranchId = Convert.ToInt32(Branches?.FirstOrDefault(x => x.BranchName == Branch).Id);
        //                }
        //                else
        //                {
        //                    var newBranch = await _serviceProvider.GetRequiredService<IBranchService>().Insert(new[] { new Branch() { BranchName = Branch, BranchAddress = Branch } });
        //                    sales.BranchId = newBranch.Entities.FirstOrDefault().Id;
        //                }
        //                if (isPaymentMethod)
        //                {
        //                    sales.PaymentMethodId = PaymentMethods.FirstOrDefault(x => x.Name == PaymentMethod).Id;
        //                }
        //                if (isInsuranceType)
        //                {
        //                    sales.InsuranceTypeId = InsuranceTypes.FirstOrDefault(x => x.Name == InsuranceType).Id;
        //                }
        //                if (isSalesAgent)
        //                {
        //                    sales.SalesInvoicePersonId = SalesAgent.FirstOrDefault(x => x.DisplayNameAs == Agent).Id;

        //                }
        //                else
        //                {
        //                    sales.SalesInvoicePersonId = 930;
        //                }

        //                if (isInsuranceCompanies)
        //                {
        //                    sales.InsuranceCompanyId = InsuranceCompanies.FirstOrDefault(x => x.DisplayNameAs == InsuranceCompany).Id;
        //                }


        //                sales.SalesInvoiceDate = DateTime.FromOADate(Convert.ToDouble(SalesDate));
        //                sales.UnderWritter = Underwritter;
        //                sales.Notes = Notes;

        //                saleLine.PolicyNumber = PolicyNumber;

        //                saleLine.CommisionRate = Comission;
        //                saleLine.Gross = Premium;

        //                var com = (saleLine.CommisionRate) / 100;

        //                double commisionRate = (double) * (double)com;


        //                saleLine.ActualComission = (decimal)commisionRate;
        //                saleLine.VAT = (decimal);
        //                saleLine.SalesPrice = SalesPrice;
        //                saleLine.Net = (decimal)(( - commisionRate));
        //                var cultureInfo = new CultureInfo("en-AE");

        //                sales.SaleLineItem.Add(saleLine);

        //                if (sales.SalesInvoicePersonId != null && sales.BodyTypeId != null &&
        //                    sales.PolicyTypeId != null && sales.ServiceId != null && sales.InsuranceCompanyId != null &&
        //                    sales.InsuranceTypeId != null && sales?.BranchId != null && sales.PaymentMethodId != null &&
        //                    !string.IsNullOrWhiteSpace(sales.UnderWritter) && !string.IsNullOrWhiteSpace(sales.Notes)
        //                    && !string.IsNullOrWhiteSpace(sales.SaleLineItem.FirstOrDefault().PolicyNumber)

        //                    && sales.SaleLineItem.FirstOrDefault().CommisionRate != null && sales.SaleLineItem.FirstOrDefault().SalesPrice != null)
        //                {
        //                    ListToInsert.Add(sales);
        //                    isRowRejected = false;
        //                    var status = await _service.Insert(new[] { sales });

        //                }
        //                else
        //                {
        //                    RejectedList.Add(sales ?? new SalesInvoice());
        //                    isRowRejected = true;

        //                }
        //                col = 1;



        //            }

        //        }



        //        OtherConstants.isSuccessful = true;
        //        return constructResponse(RejectedList);
        //    }
        //    else
        //    {

        //        throw new ServiceException("File Does not exists");
        //    }
        //}


        [AllowAnonymous]
        [HttpPost("SalesSearch")]
        public async Task<BaseResponse> SalesSearch([FromBody] PaginationParams<int> data)
        {
            var page = new PageConfig();

            var query = _context.Set<SalesInvoice>()
                  .Include(x => x.Vehicle)
             .Include(x => x.SalesInvoicePerson)
            //Sales Agent
            .Include(x => x.PaymentMethod)
         
          
                        

            .Include(x => x.BodyType)
            .Include(x => x.InsuranceCompany)
            .Include(x => x.Branch);

            List<SalesInvoice> queryData = new List<SalesInvoice>();

            if (data.SearchQuery!=null && (data.from==null || data.to==null) && data.BranchId==null )
            {
                Expression<Func<SalesInvoice, bool>> predicate = x =>


                (x.PolicyNumber.Contains(data.SearchQuery))

                ||
                (x.CustomerName.Contains(data.SearchQuery))
                ||
                (x.InsuranceCompanyName.Contains(data.SearchQuery));


                
                queryData = (await query.Where(predicate)
.OrderByDescending(x => x.SalesInvoiceDate).Skip(((data.Page - 1) * data.ItemsPerPage)).Take(data.ItemsPerPage)
                    .ToListAsync());

                page.TotalCount = query.Count(predicate);
       


            }
            else if (data.SearchQuery!=null && (data.from !=null && data.to!=null) && data.BranchId!=null)
            {
                Expression<Func<SalesInvoice, bool>> predicate = x =>

                (
                (x.SalesInvoiceDate.Date >= data.from.ToDateTime().Date)

                &&

                (x.SalesInvoiceDate.Date <= data.to.ToDateTime().Date)
                )
                &&
                (x.BranchId == data.BranchId)

                &&

            ((x.PolicyNumber.Contains(data.SearchQuery))

               ||
               (x.CustomerName.Contains(data.SearchQuery))
               ||
               (x.InsuranceCompanyName.Contains(data.SearchQuery))
               ||
               (x.InsuranceType.Name.Contains(data.SearchQuery)));
                queryData = await query.Where(predicate)

               .ToListAsync();
                page.TotalCount = query.Count(predicate);
            }
            else if(data.SearchQuery==null && (data.from != null && data.to != null) && data.BranchId==null )
            {
                Expression<Func<SalesInvoice, bool>> predicate  = x =>

                (
                (x.SalesInvoiceDate.Date >= data.from.ToDateTime().Date)

                &&

                (x.SalesInvoiceDate.Date <= data.to.ToDateTime().Date)

                );
                queryData = await query.Where(predicate)

                    .OrderByDescending(x => x.SalesInvoiceDate).Skip(((data.Page - 1) * data.ItemsPerPage)).Take(data.ItemsPerPage)
                    .ToListAsync();

                page.TotalCount = query.Count(predicate);
            }
            else
            {

                queryData = await query.OrderByDescending(x => x.SalesInvoiceDate).Skip(((data.Page - 1) * data.ItemsPerPage)).Take(data.ItemsPerPage).ToListAsync();

                page.TotalCount = query.Count();
            }

            if (data.RequestExcel!=null)
            {
                List<SalesInvoiceReport> report = new List<SalesInvoiceReport>();
                foreach (var item in queryData)
                {
                    var salesInvoiceReport = new SalesInvoiceReport();

                    salesInvoiceReport.Date = item.SalesInvoiceDate.Date.ToShortDateString();
                    salesInvoiceReport.PolicyNumber = item.PolicyNumber;
                    salesInvoiceReport.CustomerName = item.CustomerName;
                    salesInvoiceReport.InsuranceBroker = item.InsuranceCompany.DisplayNameAs;
                    salesInvoiceReport.InsuranceCompany = item.InsuranceCompanyName;
                    salesInvoiceReport.SalesAgent = item.SalesInvoicePerson.DisplayNameAs;
                    salesInvoiceReport.Branch = item.Branch.BranchName;
                    salesInvoiceReport.Vehicle = item.Vehicle.Make + item.Vehicle.Model;
                    salesInvoiceReport.UnderWritter = item.UnderWritter;
                    salesInvoiceReport.Gross = item.Gross.ToString();
                    salesInvoiceReport.VAT = item.VAT.ToString();
                    salesInvoiceReport.NET = item.Net.ToString();
                    salesInvoiceReport.Commission = item.Commission.ToString() + "%";

                    report.Add(salesInvoiceReport);
                }

                #region Excel Export Method

                string wwwPath = _env.WebRootPath;
                string contentPath = _env.ContentRootPath;
                string path = Path.Combine(contentPath, $"\\uploads\\salesReport{new DateTime().Ticks.ToString()}.xlsx");
                var serverUrl = this.HttpContext.Request.Host.ToString();
                var ransomeNameStr = new Random().Next(DateTime.Now.Second, 10000).ToString() +
                    new DateTime().Ticks.ToString();
                var isHttps = this.HttpContext.Request.IsHttps;

                var serverPath = isHttps ? "https://" : "http://" + serverUrl + $"/uploads/{ransomeNameStr}.xlsx";
                report.ToExcel(contentPath + $"\\uploads\\{ransomeNameStr}.xlsx");

                page.ExcelFileUrl = serverPath;

#endregion
            }

            var finalSales = queryData;
            page.Data.AddRange(finalSales);

            OtherConstants.isSuccessful = true;
            return constructResponse(page);
            
        }
        //public async Task Search([FromBody] PaginationParams<int> data)
        //{
            
        //}
        //public Task ExportExcel(int id)
        //{

        //}

    }

    public class SalesInvoiceReport
    {

        public string Date { get; set; }
        public string PolicyNumber { get; set; }
        public string CustomerName { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsuranceBroker { get; set; }
        public string SalesAgent { get; set; }
        public string Branch { get; set; }
        public string Vehicle { get; set; }
        public string UnderWritter { get; set; }
        public string Gross { get; set; }
        public string VAT { get; set; }
        public string NET  { get; set; }
        public string Commission { get; set; }


    }
}
