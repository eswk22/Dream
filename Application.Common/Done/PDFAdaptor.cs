using Application.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.exceptions;
using System.IO;
using iTextSharp.text;
using ExecutionEngine.Common.Connect;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf.parser;

namespace Application.Common.Done
{
    public class PDFAdaptor
    {
        private ILogger _logger = new CrucialLogger();
        private Document _document;
        public PDFAdaptor()
        {


        }

        private Document PDFDoc
        {
            get
            {
                return _document;
            }
            set
            {
                _document = value;
            }
        }

        public virtual string createPDF(string fileName, string content, string contentType)
        {
            string result = null;
            Document document = null;
            try
            {
                string absoluteFilePath = fileName; //Add temp directory
                document = new Document();
                PdfWriter.GetInstance(document, new System.IO.FileStream(absoluteFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write));
                document.Open();
                document.AddCreationDate();
                if (string.IsNullOrEmpty(contentType))
                {
                    contentType = "html";
                }
                if ("html".Equals(contentType, StringComparison.CurrentCultureIgnoreCase))
                {
                    HTMLWorker htmlWorker = new HTMLWorker(document);
                    htmlWorker.Parse(new StringReader(content));
                }
                else
                {
                    addContent(document, content);
                }
                result = absoluteFilePath;
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            catch (DocumentException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            finally
            {
                if (document != null)
                {
                    document.Close();
                }
            }
            return result;
        }


        public virtual bool deletePDF(string file)
        {
            bool result = true;
            if (!String.IsNullOrEmpty(file))
            {
                try
                {
                 
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message, e);
                    throw new ConnectException(e.Message, e);
                }
            }
            else
            {
                throw new ConnectException("\"file\" parameter must not be empty or null");
            }
            return result;
        }

        public virtual string readPDF(System.IO.Stream inputStream)
        {
            StringBuilder result = new StringBuilder();
            PdfReader pdfReader = null;
            try
            {
                pdfReader = new PdfReader(inputStream);
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    result.Append(currentText);
                }
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            finally
            {
                if (pdfReader != null)
                {
                    try
                    {
                        pdfReader.Close();
                    }
                    catch (IOException e)
                    {
                        _logger.Warn(e.Message);
                    }
                }
            }
            return result.ToString();
        }

        public virtual string readPDF(string file)
        {
            StringBuilder result = new StringBuilder();
            PdfReader pdfReader = null;
            try
            {
                if (File.Exists(file))
                {
                    pdfReader = new PdfReader(file);
                    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                    {
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                        result.Append(currentText);
                    }
                }
                else
                {
                    throw new FileNotFoundException("File not found", file);
                }
            }
            catch (IOException e)
            {
                _logger.Error(e.Message, e);
                throw new ConnectException(e.Message, e);
            }
            finally
            {
                if (pdfReader != null)
                {
                    try
                    {
                        pdfReader.Close();
                    }
                    catch (IOException e)
                    {
                        _logger.Warn(e.Message);
                    }
                }
            }
            return result.ToString();
        }
        private void addContent(Document document, string content)
        {
            document.Add(new Paragraph(content));
        }

    }
}
