using OfficeOpenXml;
using System.Text;
using TestParser.Models;

namespace TestParser.Converters
{
    public class FormatConverter: IFormatConverter
    {
        public string ConvertToCsv(List<MonthlyDataModel> data)
        {
            if(data.Count<0)
                return string.Empty;

            StringBuilder csvContent = new StringBuilder();

            var groupedByYear = data.Where(x=>x.Year>=DateTime.Now.Year-1).GroupBy(d => d.Year);

            foreach (var group in groupedByYear)
            {
                csvContent.AppendLine($"{group.Key},Latin America,Europe,Africa,Middle East,Asia Pacific,Total Intl.,Canada,U.S.,Total World");

                foreach (var item in group)
                {
                    csvContent.AppendLine(GenerateCSVLine(item));
                }
                csvContent.AppendLine(",,,,,,,,,");
            }

            return csvContent.ToString();
        }

        public List<MonthlyDataModel> ConvertFromExcel(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<MonthlyDataModel>();

            List<MonthlyDataModel> monthlyData = new List<MonthlyDataModel>();

            FileInfo fileInfo = new FileInfo(filePath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                int currentYear = 0;
                for (int row = 2; row <= rowCount; row++)
                {

                    if(worksheet.Cells[row, 2].Value == null)
                        continue;

                    if ( int.TryParse(worksheet.Cells[row, 2].Value?.ToString(),out var year))
                    {
                        currentYear = year;
                        continue;
                    }

                    MonthlyDataModel data = new MonthlyDataModel
                    {
                        Year = currentYear,
                        Month = worksheet.Cells[row, 2].Value?.ToString(),

                        LatinAmerica = Convert.ToInt32(worksheet.Cells[row, 3].Value),
                        Europe = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                        Africa = Convert.ToInt32(worksheet.Cells[row, 5].Value),
                        MiddleEast = Convert.ToInt32(worksheet.Cells[row, 6].Value),
                        AsiaPacific = Convert.ToInt32(worksheet.Cells[row, 7].Value),
                        TotalInternational = Convert.ToInt32(worksheet.Cells[row, 8].Value),
                        Canada = Convert.ToInt32(worksheet.Cells[row, 9].Value),
                        US = Convert.ToInt32(worksheet.Cells[row, 10].Value),
                        TotalWorld = Convert.ToInt32(worksheet.Cells[row, 11].Value)
                    };

                    monthlyData.Add(data);
                }
            }

            return monthlyData;
        }

        private string GenerateCSVLine(MonthlyDataModel item)
        {
            StringBuilder line = new StringBuilder();
            line.Append(item.Month);

            AppendValue(line, item.LatinAmerica);
            AppendValue(line, item.Europe);
            AppendValue(line, item.Africa);
            AppendValue(line, item.MiddleEast);
            AppendValue(line, item.AsiaPacific);
            AppendValue(line, item.TotalInternational);
            AppendValue(line, item.Canada);
            AppendValue(line, item.US);
            AppendValue(line, item.TotalWorld);

            return line.ToString();
        }

        private void AppendValue(StringBuilder line, int value)
        {
            if (value != 0)
            {
                line.Append(",").Append(value);
            }
            else
            {
                line.Append(",");
            }
        }
    }
}
