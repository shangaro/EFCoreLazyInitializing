# EFCoreLazyInitializing
Repository Pattern, Unit of Work

---
public class FileExportingService : IFileExportingService
    {

        /// <summary>
        /// Writes dataset to excelsheet
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="sheetNames"></param>
        /// <returns></returns>
        public async Task<Stream> WriteToExcel(IEnumerable<HistoricalPriceReturn> prices)
        {
            if (prices == null) throw new ArgumentNullException($"{nameof(prices)}");
            var tempFolder = Path.GetTempPath();
            var tempFileName = Path.GetTempFileName();
            var tempFile = Path.Combine(tempFolder, tempFileName);

            using (var document = SpreadsheetDocument.Create(tempFile, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                var stylesPart=workbookPart.AddNewPart<WorkbookStylesPart>();
				stylesPart.Stylesheet = CreateStylesheet();
				stylesPart.Stylesheet.Save();
                workbookPart.Workbook = new Workbook();
                var sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                
                // - create the workpart & sheets collection
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var workSheet = new Worksheet();
                var sheetData = new SheetData();
                // add headers to the sheet 
                AddHeaderRow(sheetData);


                // data rows
                foreach (var p in prices)
                {
                    var row = new Row();

                    AddDataRow(p, row);

                    sheetData.Append(row);
                }

                // - add data to the  sheet
                workSheet.AppendChild(sheetData);
                worksheetPart.Worksheet = workSheet;
                // - add a new sheet
                var sheet = new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)(1),
                    Name = "historicalreturns",
                    State = SheetStateValues.Visible,
                };
                sheets.Append(sheet);


                // - save worksheet changes
                worksheetPart.Worksheet.Save();


                // - save to temp file
                document.Save();
            }

            // - read the temp excel file
            var memStream = new MemoryStream();
            using (var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(memStream);
                memStream.Position = 0;
            }

            // - delete the temp excel file
            File.Delete(tempFile);

            // - return the excel file content
            return memStream;
        }

        private static void AddHeaderRow(SheetData sheetData)
        {
            var headerRow = new Row();
            var price = new HistoricalPriceReturn();
            

            headerRow.AppendChild(new Cell { DataType = new EnumValue<CellValues>(CellValues.String), CellValue = new CellValue(nameof(price.Price)),CellReference="A1",StyleIndex=0});
            headerRow.AppendChild(new Cell { DataType = new EnumValue<CellValues>(CellValues.String), CellValue = new CellValue(nameof(price.PriceChange)),CellReference="B1",StyleIndex = 0 });
            headerRow.AppendChild(new Cell { DataType = new EnumValue<CellValues>(CellValues.String), CellValue = new CellValue(nameof(price.PriceChangePercent)),CellReference="C1", StyleIndex = 0 });
            headerRow.AppendChild(new Cell { DataType = new EnumValue<CellValues>(CellValues.String), CellValue = new CellValue(nameof(price.PreviousPrice)),CellReference="D1", StyleIndex = 0 });
            headerRow.AppendChild(new Cell { DataType = new EnumValue<CellValues>(CellValues.String), CellValue = new CellValue(nameof(price.ClosingDate)),CellReference="E1", StyleIndex = 0 });
            headerRow.AppendChild(new Cell { DataType = new EnumValue<CellValues>(CellValues.String), CellValue = new CellValue(nameof(price.Distribution)),CellReference="F1", StyleIndex = 0 });
            headerRow.AppendChild(new Cell { DataType = new EnumValue<CellValues>(CellValues.String), CellValue = new CellValue(nameof(price.Yield)),CellReference="G1", StyleIndex = 0 });


            sheetData.InsertAt(headerRow, 0);
        }

        private static void AddDataRow(HistoricalPriceReturn p, Row row)
        {
            //var date = DateTime.FromOADate(p.ClosingDate.ToOADate());
            int i = 0;
            row.InsertAt(
            new Cell()
            {
                CellValue = new CellValue(p.Price.ToString()),
                DataType = new EnumValue<CellValues>(CellValues.Number),
                StyleIndex=2
            }, i++);
            row.InsertAt(
           new Cell()
           {
              
               CellValue = new CellValue(p.PriceChange.ToString()),
               DataType = new EnumValue<CellValues>(CellValues.Number),
               StyleIndex = 2
           }, i++);
            row.InsertAt(
           new Cell()
           {
               CellValue = new CellValue(p.PriceChangePercent.ToString()),
               DataType = new EnumValue<CellValues>(CellValues.Number),
               StyleIndex = 2

           }, i++);
            row.InsertAt(
           new Cell()
           {
               CellValue = new CellValue(p.PreviousPrice.ToString()),
               DataType = new EnumValue<CellValues>(CellValues.Number),
               StyleIndex=2
           }, i++);
            row.InsertAt(
           new Cell()
           {
               CellValue = new CellValue(p.ClosingDate),
               DataType = new EnumValue<CellValues>(CellValues.Date),
               StyleIndex=1
           }, i++);
            row.InsertAt(
           new Cell()
           {
               CellValue = new CellValue(p.Distribution.ToString()),
               DataType = new EnumValue<CellValues>(CellValues.Number),
               StyleIndex=2
           }, i++);
            row.InsertAt(
           new Cell()
           {
               CellValue = new CellValue(p.Yield.ToString()),
               DataType = new EnumValue<CellValues>(CellValues.Number),
               StyleIndex=2
           }, i++);

        }

		private static Stylesheet CreateStylesheet()
		{
            uint iExcelIndex = 164;
            var dateFormatIndex = iExcelIndex++;
            var pctFormatIndex = iExcelIndex++;

            Stylesheet ss = new Stylesheet()
            {
                Fonts = new Fonts(new Font()),
                Fills = new Fills(new Fill()),
                Borders = new Borders(new Border()),
                CellStyleFormats = new CellStyleFormats(new CellFormat()),
                NumberingFormats = new NumberingFormats(
                    new NumberingFormat()
                    {
                        NumberFormatId = UInt32Value.FromUInt32(dateFormatIndex),
                        FormatCode = StringValue.FromString("yyyy-mm-dd")
                    },
                    new NumberingFormat()
                    {
                        NumberFormatId = UInt32Value.FromUInt32(pctFormatIndex),
                        FormatCode = StringValue.FromString("#,##0.0000")
                    }),
                CellFormats = new CellFormats(
                    new CellFormat(),
                    new CellFormat
                    {
                        NumberFormatId = dateFormatIndex,
                        ApplyNumberFormat = true
                    },
                    new CellFormat
                    {
                        NumberFormatId = pctFormatIndex,
                        ApplyNumberFormat = true
                    })
            };

			return ss;
		}

	}

