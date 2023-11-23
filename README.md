# TextFile to PDF Converter

To run this TextFile to PDF Converter successfully, follow these instructions:

1. **Install .NET SDK**
    - Go to `dotnet.microsoft.com` to install the latest version of Microsoft .NET SDK.
    - Run the installation package.

2. **Adjust File Paths:**
    - Update the file paths in `Program.cs` (lines 9 and 10) to specify your input text file and desired output PDF file paths:
        ```csharp
        string textFilePath = @"path\to\input\textfile.txt"; // Change to your input text file path
        string pdfFilePath = @"path\to\desired\output.pdf"; // Change to your desired output PDF file path
        ```

3. **Supported Commands:**
    - The input text file should contain the following commands:
        - `.large`: Sets a larger font size.
        - `.normal` or `.regular`: Resets font settings to default.
        - `.italics`: Applies italic formatting to text.
        - `.regular`: Resets font settings to default.
        - `.paragraph`: Starts a new paragraph.
        - `.indent <int>`: Indents the text by the specified integer value.
        - `.fill`: Adjusts text alignment to fill margins.
        - `.nofill`: Resets text alignment to default.
        - `.bold`: Applies bold formatting to text.

    **Example Usage:**
    ```
    .large
    This text will be larger.
    .italics
    This text will be in italics.
    .paragraph
    This text starts a new paragraph.
    ```

4. **Customizing Formatting:**
    - To add or modify commands, adjust the methods `Format` and `AddTexttoParagraph` in the code.
    - Example:
        ```csharp
        case ".custom":
            // Add code to handle the custom command here.
            break;
        ```

5. **Modifying Default Settings:**
    - Adjust the initial formatting in the `GetInitialFormats()` method:
        ```csharp
        static Dictionary<string, object> GetInitialFormats()
        {
            return new Dictionary<string, object>()
            {
                {"currentFontSize", 12},
                // Add or modify other default settings here
            };
        }
        ```

6. **Setting Maximum Pages:**
    - Modify the `maxPages` variable (line 35) to specify the maximum number of pages for the PDF output.

7. **Running the Program:**
    - Navigate to the TextFileToPDFConverter folder in your terminal or command prompt.
    - Use an IDE or the command line interface such as Visual Studio Code.
    - Ensure you have the .NET SDK installed.
    - Run `dotnet build` to compile the program, then `dotnet run` to execute it.

By following these instructions, you can customize the formatting and behavior of the TextFile to PDF Converter based on your requirements.