using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AIB.Common.ExcelKit
{

    public class ExcelColumnConfig
    {
        public string Header { get; set; }
        public Func<object, object> MappingFunction { get; set; }

        public ExcelColumnConfig(string header, Func<object, object> mappingFunction)
        {
            Header = header;
            MappingFunction = mappingFunction;
        }
    }
    public static  class ExcelGenerics
    {
        public static byte[] GenerateExcel(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            // Add headers
            for (var i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
            }

            // Add data
            var dataList = data.ToList();
            for (var row = 0; row < dataList.Count; row++)
            {
                for (var col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(dataList[row]);
                }
            }

            return package.GetAsByteArray();
        }

        public static void ToExcel<T>(this IEnumerable<T> data, string filePath, List<ExcelColumnConfig> columnMapping, string sheetName = "Sheet1")
        {
            if (data == null || !data.Any())
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            if (columnMapping == null || !columnMapping.Any())
            {
                throw new ArgumentException("Column mapping cannot be null or empty.", nameof(columnMapping));
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // Add column headers
            var headerRow = 1;
            var headerIndex = 1;
            foreach (var mapping in columnMapping)
            {
                worksheet.Cells[headerRow, headerIndex].Value = mapping.Header;
                headerIndex++;
            }

            // Add data rows
            var rowIndex = 2;
            foreach (var item in data)
            {
                var columnIndex = 1;
                foreach (var mapping in columnMapping)
                {
                    var value = mapping.MappingFunction(item);
                    worksheet.Cells[rowIndex, columnIndex].Value = value??"";
                    columnIndex++;
                }
                rowIndex++;
            }

            // Set column widths
            for (var i = 1; i <= columnMapping.Count; i++)
            {
                worksheet.Column(i).AutoFit();
            }

            // Save the file
            package.SaveAs(new FileInfo(filePath));
        }

    }
}
