using System;
using System.IO;
using Tesseract;
using System.Diagnostics;
using System.Text;

public partial class DBMaintenance_DevTest_OCR_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string FileFromQuerystring = Request.QueryString["File"];
        bool AcceptedFormat = bool.Parse(Request.QueryString["AcceptedFormat"]);
        StringBuilder sb = new StringBuilder();
        string AbsolutePath = Server.MapPath(System.IO.Path.GetDirectoryName(Request.Url.AbsolutePath));
        string ServerSideFilesFolder = AbsolutePath + @"\Files\";
        string OcrFileName = "";

        if (FileFromQuerystring != "" && !IsPostBack)
        {
            try
            {
                if (!AcceptedFormat)
                {
                    OcrFileName = FileFromQuerystring.Split('.')[0] + ".png";

                    ProcessStartInfo startInfo = new ProcessStartInfo()
                    {
                        FileName = "soffice",
                        Arguments = "--headless --convert-to png \"" + ServerSideFilesFolder + FileFromQuerystring + "\" --outdir \"" + AbsolutePath + @"\Files" + "\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,  // To capture any error messages
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Process process = new Process() { StartInfo = startInfo };

                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    Console.WriteLine("Output:\n" + output);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("Error:\n" + error);
                    }

                    process.WaitForExit();
                }
                else
                {
                    OcrFileName = FileFromQuerystring;
                }

                //get text from image file using OCR
                using (var engine = new TesseractEngine(AbsolutePath + @"\Libraries\tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(ServerSideFilesFolder + OcrFileName))
                    {
                        using (var page = engine.Process(img))
                        {
                            //all the text at once
                            string text = page.GetText();
                            sb.AppendLine("Mean confidence: " + page.GetMeanConfidence());
                            sb.AppendLine("<br/><br/> Text (GetText): <br/>" + text);

                            //iterating through the file line by line
                            sb.AppendLine("<br/><br/> Text (iterator): <br/>");
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();

                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            do
                                            {
                                                sb.Append(iter.GetText(PageIteratorLevel.Word)); // Append each word.
                                                sb.Append(" "); // Add space between words.

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                    sb.Append("<br/>"); // New line after each line of text.
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                                sb.Append("<br/>"); // Add a blank line after each paragraph.
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                            }
                            Session["OcrError"] = null;
                            Session[FileFromQuerystring] = sb.ToString();

                            //convert file into blob then add to Session
                            byte[] file;
                            using (var stream = new FileStream(ServerSideFilesFolder + OcrFileName, FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = new BinaryReader(stream))
                                {
                                    file = reader.ReadBytes((int)stream.Length);
                                }
                            }
                            Session["Blob"] = file;
                            //convert file into blob then add to Session

                            Response.Redirect("UploadFileToServer.aspx?File=" + FileFromQuerystring, false); // Prevent the ThreadAbortException
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Session["OcrError"] = "Error: Could not read contents of file. Please try again";
                Response.Redirect("UploadFileToServer.aspx");
            }
        }
    }
}