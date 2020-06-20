using System;
using System.IO;
using System.Reflection;

namespace EdiconPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args[0] == "-c")
                {
                    CreateEdiconPdf(args[1], args[2], args[3]);
                }

                else if (args[0] == "-x")
                {
                    ExtractEdiconPfd(args[1], args.Length > 2 ? args[2] : null);
                }
                else
                {
                    Usage();
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Usage();
                Console.ReadLine();
            }
        }

        static void ExtractEdiconPfd(string ediconPdf, string saveToFilename)
        {
            string data = EdiconPdf.ExtractEdiconData(ediconPdf);
            EdiconXml ediconXml = new EdiconXml(data);
            //
            if (saveToFilename != null)
            {
                File.WriteAllText(saveToFilename, ediconXml.GetDecodedContent());
            }
            else
            {
                //Console.WriteLine("Document type      : " + ediconXml.GetDocumentType());
                //Console.WriteLine("Format             : " + ediconXml.GetFileFormat());
                //Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine(ediconXml.GetDecodedContent());
            }
        }

        static void CreateEdiconPdf(string sourcePdfFilename, string sourceDataFilename, string outputEdiconPdfFilename)
        {
            string ediconXml = EdiconPdf.CreateEdiconXmlData(File.ReadAllBytes(sourceDataFilename), 
                    Path.GetExtension(sourceDataFilename).Substring(1));
            //
            EdiconPdf.EmbedEdiconData(sourcePdfFilename, ediconXml, outputEdiconPdfFilename);
            //
            Console.WriteLine("File " + outputEdiconPdfFilename + " created");
        }

        static  void Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("");
            Console.WriteLine("Create EdiconPdf file:");
            Console.WriteLine("    EdiconPdf.exe -c <input_pdf_filename> <data_filename_to_embed> <output_edicon_pdf_filename>");
            Console.WriteLine("");
            Console.WriteLine("Extract data from EdiconPdf file:");
            Console.WriteLine("    EdiconPdf.exe -x <input_pdf_filename> [<save_to_file>]");
        }
    }
}
