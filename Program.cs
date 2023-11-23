using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

class Program
{
    static void Main()
    {
        string textFilePath = "input.txt"; // path to input textfile
        string pdfFilePath = "output.pdf"; // path to desired output pdf file

        ConvertTextToPdf(textFilePath, pdfFilePath); // Method to convert textfile to pdf file

        Console.WriteLine("Conversion complete.");
    }

    static void ConvertTextToPdf(string textFilePath, string pdfFilePath)
    {
        using (FileStream fs = new FileStream(pdfFilePath, FileMode.Create))
        {
            using (PdfWriter writer = new PdfWriter(fs))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    using (Document document = new Document(pdf))
                    {
                        try
                        {
                            string[] lines = File.ReadAllLines(textFilePath); // read through every line of the text file, and store into an array variable of strings, called 'lines'

                            Paragraph currentParagraph = new Paragraph(); // Initialize a paragraph to start with (using iText.Layout.Element.BlockElement)
                            Dictionary<string, object> formats = GetInitialFormats(); // Initialize the formatting dictionary
                            
                            int totalPageCount = 0; // Initialize current total page count
                            int maxPages = 3; // Set the target number of pages

                            while (totalPageCount <= maxPages)
                            {
                                int linesAdded = 0;

                                while (linesAdded < lines.Length)
                                {
                                    if (pdf.GetNumberOfPages() > maxPages)
                                    {
                                        break; // Break out if the maximum pages limit is reached
                                    }
                                    // iterate through every line in text file, format the context lines according to the command lines, reuse the variables 'formats' and 'currentParagraph'
                                    Format(lines[linesAdded], ref formats, ref currentParagraph, document);
                                    linesAdded++;

                                    totalPageCount = pdf.GetNumberOfPages();
                                }

                                // After iterating through the text file, add the final paragraph (currentParagraph), and initialize currentParagraph to a new empty one
                                if (totalPageCount <= maxPages)
                                {
                                    document.Add(currentParagraph);
                                    currentParagraph = new Paragraph(); // Start a new paragraph
                                }
                            }

                            // Check if the number of pages exceeds the limit
                            if (pdf.GetNumberOfPages() > maxPages)
                            {
                                int pagesToRemove = pdf.GetNumberOfPages() - maxPages;

                                // Remove content on the final pages if they exceed the limit
                                for (int i = 0; i < pagesToRemove; i++)
                                {
                                    pdf.RemovePage(pdf.GetLastPage());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                        }
                    }
                }
            }
        }
    }

    static Dictionary<string, object> GetInitialFormats()
    {
        // Initialisation of formats
        return new Dictionary<string, object>()
        {
            {"currentFontSize", 12},
            {"newParagraph", true}, 
            {"italic", false},
            {"indent", 0},
            {"fill", true},
            {"bold", false}
        };
    }

    static void Format(string line, ref Dictionary<string, object> formats, ref Paragraph currentParagraph, Document document)
    {
        switch (line.Split(' ')[0]) // Check the first substring of every line
        {
            // Decide what to change in the formats dictionary according to each case
            case ".paragraph":
                formats["newParagraph"] = true;
                break;
            case ".fill":
                formats["fill"] = true;
                break;
            case ".nofill":
                formats["indent"] = 0; // Set the indent back to zero
                formats["newParagraph"] = true;
                formats["italic"] = false;
                formats["bold"] = false;;
                break;
            case ".regular":
                formats["currentFontSize"] = 12; 
                formats["italic"] = false;
                formats["bold"] = false;
                break;
            case ".normal":
                formats["currentFontSize"] = 12;
                formats["italic"] = false;
                formats["bold"] = false;
                break;
            case ".italics":
                formats["italic"] = true;
                break;
            case ".bold":
                formats["bold"] = true;
                break;
            case ".indent":
                int indentValue = int.Parse(line.Split(' ')[1]); // take the element of the second substring (which should be an integer)
                formats["indent"] = indentValue * (int)formats["currentFontSize"];
                break;
            case ".large":
                formats["currentFontSize"] = 24;
                break;
            default:
                AddTextToParagraph(line, formats, ref currentParagraph, document); // if the line is not any of the above commands, the line will be treated as the text that should be added to the pdf
                break;
        }
    }

    static void AddTextToParagraph(string line, Dictionary<string, object> formats, ref Paragraph currentParagraph, Document document)
    {
        Text text = new Text(line); // Use iText.Layout.Element.Text

        if ((bool)formats["italic"])
        {
            text.SetItalic();
        }
        if ((bool)formats["bold"])
        {
            text.SetBold();
        }

        if ((bool)formats["fill"])
        {
            text.SetTextAlignment(iText.Layout.Properties.TextAlignment.JUSTIFIED); // set the text to fill in the whole margin (both left and right, except for the final line)
        }

        if ((bool)formats["newParagraph"])
        {
            formats["newParagraph"] = false;
            document.Add(currentParagraph); // Add the previous paragraph
            currentParagraph = new Paragraph(); // Start a new paragraph
        }

        if ((int)formats["indent"] > 0)
        {
            currentParagraph.SetMarginLeft((int)formats["indent"]); // set indentation according to the desired indentation value
        }

        bool nextLineStartsWithPunctuation = char.IsPunctuation(line[0]); // This is to consider for the case where the next line starts with a punctuation, such as in the example input.txt
        if (nextLineStartsWithPunctuation)
        {
            currentParagraph.Add(text.SetFontSize((int)formats["currentFontSize"])); // if next line starts with punctuation, do not add space between current sentence and the next line
        }
        else
        {
            currentParagraph.Add(new Text(" "));
            currentParagraph.Add(text.SetFontSize((int)formats["currentFontSize"]));
        }
    }
}