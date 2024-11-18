using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Proser.Products.Monitoring
{
    interface ICsvHeader
    {
        List<string> GetCsvHeader();
    }

    interface ICsvColumns
    {
        List<string> GetCsvColumns();
    }

    interface ICsvContent
    {
        List<object> GetCsvRows();
    }
    
    class CsvHelper
    {
        // Variables.
        private string csvSeparator;
        private string decimalSeparator;
        private ICsvHeader header;
        private List<string> columns;
        private List<List<object>> rows;


        public CsvHelper(ICsvHeader header)
        {
            rows = new List<List<object>>();
            this.header = header;
            columns = new List<string>();
            rows = new List<List<object>>();

            csvSeparator = ";";
            decimalSeparator = ".";
        }

        public void SetColumns(ICsvColumns container)
        {
            columns = container.GetCsvColumns();
        }

        public void AppendRow(ICsvContent content)
        {
            rows.Add(content.GetCsvRows());
        }

        public string MakeCsv(bool lineNumber = false)
        {
            StringBuilder result = new StringBuilder();

            // Header.
            if (header != null)
            {
                List<string> hContent = header.GetCsvHeader();
                hContent.ForEach(r => result.AppendLine(r));
                result.AppendLine();
            }

            // Columns.
            if (this.columns.Count > 0)
            {
                string columns = lineNumber ? ("Registro" + csvSeparator) : String.Empty;
                this.columns.ForEach(c => columns += c + csvSeparator);
                columns = columns.Remove(columns.Length - 1);
                result.AppendLine(columns);
            }

            // Body.
            int register = 1;
            rows.ForEach(r =>
            {
                string row = lineNumber ? ((register++).ToString() + csvSeparator) : String.Empty;
                r.ForEach(c => row += ObjectToString(c) + csvSeparator);
                row = row.Remove(row.Length - 1);
                result.AppendLine(row);
            });

            return result.ToString();
        }

        private string ObjectToString(object o)
        {
            return o is String ? o.ToString() : GetNumberCsv(o);
        }

        private string GetNumberCsv(object v)
        {
            string result = v.ToString().Replace('.', ',');
            Type t = v.GetType();

            if (t == typeof(float))
            {
                try
                {
                    float aux = Convert.ToSingle(v);
                    result = float.IsNaN(aux) ? "-" : aux.ToString("F").Replace(".", decimalSeparator).Replace(",", decimalSeparator);
                }
                catch
                {
                    result = "--";
                }
            }

            return result;
        }
    }
}
