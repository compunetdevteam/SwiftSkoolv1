using OfficeOpenXml;
using System.Data;
using System.Linq;

namespace SwiftSkoolv1.WebUI.Services
{
    public static class ExcelPackageExtension
    {
        public static DataTable ToDataTable(this ExcelPackage package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
            DataTable dt = new DataTable();
            foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
            {
                dt.Columns.Add((firstRowCell.Text));
            }

            for (var rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
            {
                var row = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                var newRow = dt.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                dt.Rows.Add(newRow);
            }
            return dt;
        }

    }
}