using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestParser.Models;

namespace TestParser.Converters
{
    public interface IFormatConverter
    {
        string ConvertToCsv(List<MonthlyDataModel> data);

        List<MonthlyDataModel> ConvertFromExcel(string filePath);
    }
}
