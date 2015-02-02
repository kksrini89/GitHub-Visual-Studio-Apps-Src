using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Drawing;
using System.IO;
//using Excel = Microsoft.Office.Interop.Excel;

namespace ReportGen
{
    //public class ExportToExcel<T, U>
    //    where T : class
    //    where U : ObservableCollection<T>
    //{
    //    public ObservableCollection<T> dataToPrint;
    //    // Excel object references.
    //    private Excel.Application _excelApp = null;
    //    private Excel.Workbooks _books = null;
    //    private Excel._Workbook _book = null;
    //    private Excel.Sheets _sheets = null;
    //    private Excel._Worksheet _sheet = null;
    //    private Excel.Range _range = null;
    //    private Excel.Font _font = null;
    //    // Optional argument variable
    //    private object _optionalValue = Missing.Value;

    //    /// <summary>
    //    /// Generate report and sub functions
    //    /// </summary>
    //    public void GenerateReport()
    //    {
    //        try
    //        {
    //            if (dataToPrint != null)
    //            {
    //                if (dataToPrint.Count != 0)
    //                {
    //                    Mouse.SetCursor(Cursors.Wait);
    //                    CreateExcelRef();
    //                    FillSheet();
    //                    OpenReport();
    //                    Mouse.SetCursor(Cursors.Arrow);
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            MessageBox.Show("Error while generating Excel report");
    //        }
    //        finally
    //        {
    //            ReleaseObject(_sheet);
    //            ReleaseObject(_sheets);
    //            ReleaseObject(_book);
    //            ReleaseObject(_books);
    //            ReleaseObject(_excelApp);
    //        }
    //    }
    //    /// <summary>
    //    /// Make MS Excel application visible
    //    /// </summary>
    //    private void OpenReport()
    //    {
    //        _excelApp.Visible = true;
    //    }
    //    /// <summary>
    //    /// Populate the Excel sheet
    //    /// </summary>
    //    private void FillSheet()
    //    {
    //        object[] header = CreateHeader();
    //        WriteData(header);
    //    }
    //    /// <summary>
    //    /// Write data into the Excel sheet
    //    /// </summary>
    //    /// <param name="header"></param>
    //    private void WriteData(object[] header)
    //    {
    //        object[,] objData = new object[dataToPrint.Count, header.Length];

    //        for (int j = 0; j < dataToPrint.Count; j++)
    //        {
    //            var item = dataToPrint[j];
    //            for (int i = 0; i < header.Length; i++)
    //            {
    //                var y = typeof(T).InvokeMember(header[i].ToString(), BindingFlags.GetProperty, null, item, null);
    //                objData[j, i] = (y == null) ? "" : y.ToString();
    //            }
    //        }
    //        AddExcelRows("A2", dataToPrint.Count, header.Length, objData);
    //        AutoFitColumns("A1", dataToPrint.Count + 1, header.Length);
    //    }
    //    /// <summary>
    //    /// Method to make columns auto fit according to data
    //    /// </summary>
    //    /// <param name="startRange"></param>
    //    /// <param name="rowCount"></param>
    //    /// <param name="colCount"></param>
    //    private void AutoFitColumns(string startRange, int rowCount, int colCount)
    //    {
    //        _range = _sheet.get_Range(startRange, _optionalValue);
    //        _range = _range.get_Resize(rowCount, colCount);
    //        _range.Columns.AutoFit();
    //    }
    //    /// <summary>
    //    /// Create header from the properties
    //    /// </summary>
    //    /// <returns></returns>
    //    private object[] CreateHeader()
    //    {
    //        PropertyInfo[] headerInfo = typeof(T).GetProperties();

    //        // Create an array for the headers and add it to the
    //        // worksheet starting at cell A1.
    //        List<object> objHeaders = new List<object>();
    //        for (int n = 0; n < headerInfo.Length; n++)
    //        {
    //            objHeaders.Add(headerInfo[n].Name);
    //        }

    //        var headerToAdd = objHeaders.ToArray();
    //        AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
    //        SetHeaderStyle();

    //        return headerToAdd;
    //    }
    //    /// <summary>
    //    /// Set Header style as bold
    //    /// </summary>
    //    private void SetHeaderStyle()
    //    {
    //        _font = _range.Font;
    //        _font.Bold = true;
    //    }
    //    /// <summary>
    //    /// Method to add an excel rows
    //    /// </summary>
    //    /// <param name="startRange"></param>
    //    /// <param name="rowCount"></param>
    //    /// <param name="colCount"></param>
    //    /// <param name="values"></param>
    //    private void AddExcelRows(string startRange, int rowCount, int colCount, object values)
    //    {
    //        _range = _sheet.get_Range(startRange, _optionalValue);
    //        _range = _range.get_Resize(rowCount, colCount);
    //        /*_range.set_Value(_optionalValue, values);
    //        _range.NumberFormat = "dd-mmm-yyyy";*/
    //        _range.set_Value(_optionalValue, values);
    //    }
    //    /// <summary>
    //    /// Create Excel applicaiton parameters instances
    //    /// </summary>
    //    private void CreateExcelRef()
    //    {
    //        _excelApp = new Excel.Application();
    //        _books = (Excel.Workbooks)_excelApp.Workbooks;
    //        _book = (Excel._Workbook)(_books.Add(_optionalValue));
    //        _sheets = (Excel.Sheets)_book.Worksheets;
    //        _sheet = (Excel._Worksheet)(_sheets.get_Item(1));
    //    }
    //    /// <summary>
    //    /// Release unused COM objects
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private void ReleaseObject(object obj)
    //    {
    //        try
    //        {
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
    //            obj = null;
    //        }
    //        catch (Exception ex)
    //        {
    //            obj = null;
    //            MessageBox.Show(ex.Message.ToString());
    //        }
    //        finally
    //        {
    //            GC.Collect();
    //        }
    //    }
    //}

    public class EPPlusExportToExcel
    {
        private readonly DirectoryInfo outputDir;

        private ObservableCollection<Report> reportCollection = new ObservableCollection<Report>();

        public EPPlusExportToExcel(string outputDir, ObservableCollection<Report> reports)
        {
            this.outputDir = new DirectoryInfo(outputDir);
            this.reportCollection = reports;
        }

        public string ExportToExcel()
        {
            string fileName = "Report.xlsx";

            FileInfo newFile = new FileInfo(Path.Combine(outputDir.FullName, fileName));
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(Path.Combine(outputDir.FullName, fileName));
            }
            ExcelPackage package = new ExcelPackage();

            #region Worksheet1
            var ws1 = package.Workbook.Worksheets.Add("SALES");

            //Format the header
            using (var rng = ws1.Cells["A1:H1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.WrapText = true;
                rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(237, 237, 237));
                rng.Style.Font.Size = 12;
            }
            ws1.View.FreezePanes(2, 1);

            ws1.Cells["A1"].Value = "Date";
            ws1.Cells["A1"].AutoFitColumns(25);

            ws1.Cells["B1"].Value = "Hospital";
            ws1.Cells["B1"].AutoFitColumns(35);

            ws1.Cells["C1"].Value = "Doctor";
            ws1.Cells["C1"].AutoFitColumns(12);

            ws1.Cells["D1"].Value = "Test";
            ws1.Cells["D1"].AutoFitColumns(10);

            ws1.Cells["E1"].Value = "Quantity";
            ws1.Cells["E1"].AutoFitColumns(12);

            ws1.Cells["F1"].Value = "Price";
            ws1.Cells["F1"].AutoFitColumns(15);

            ws1.Cells["G1"].Value = "Discount";
            ws1.Cells["G1"].AutoFitColumns(15);

            ws1.Cells["H1"].Value = "Total";
            ws1.Cells["H1"].AutoFitColumns(15);

            //var orders = new Reports();
            int rowIndexBeginOrders = 2;
            int rowIndexCurrentRecord = rowIndexBeginOrders;

            foreach (var order in reportCollection)
            {
                ws1.Cells["A" + rowIndexCurrentRecord].Value = order.Date;
                ws1.Cells["A" + rowIndexCurrentRecord].Style.Numberformat.Format = "dd.MM.yyyy";
                ws1.Cells["B" + rowIndexCurrentRecord].Value = order.Hospital;
                ws1.Cells["C" + rowIndexCurrentRecord].Value = order.Doctor;
                //ws1.Cells["C" + rowIndexCurrentRecord].Style.Numberformat.Format = "dd.MM.yyyy";
                ws1.Cells["D" + rowIndexCurrentRecord].Value = order.Test;
                ws1.Cells["E" + rowIndexCurrentRecord].Value = order.Quantity;
                ws1.Cells["F" + rowIndexCurrentRecord].Value = order.Price;
                ws1.Cells["G" + rowIndexCurrentRecord].Value = order.Discount;
                ws1.Cells["H" + rowIndexCurrentRecord].Value = order.Total;
                //ws1.Cells["F" + rowIndexCurrentRecord].FormulaR1C1 = "RC[-2]*RC[-1]";
                rowIndexCurrentRecord++;
            }

            #endregion

            package.Workbook.Properties.Title = "Sales Format";
            package.Workbook.Properties.Author = string.Join(Environment.NewLine, "kksrini89@gmail.com");
            package.Workbook.Properties.Company = "PersoDev Organization";

            package.SaveAs(newFile);
            return newFile.FullName;
        }

        public void EPPlusReadExcel()
        {
            throw new NotImplementedException();
        }
    }
}
