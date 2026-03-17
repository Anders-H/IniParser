using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace IniParser;

public class Parser
{
    private readonly string _raw;

    public Parser(string raw)
    {
        _raw = raw;
    }

    public bool TryParse(out string message, out IniFile iniFile)
    {
        iniFile = [];
        var rows = _raw.Split([Environment.NewLine], StringSplitOptions.None);
        var currentSection = new IniSection("");
        var result = new IniFile();
        var lineNumber = 0;
        var messageBuilder = new StringBuilder();

        foreach (var row in rows)
        {
            lineNumber++;
            var r = row.Trim();

            if (string.IsNullOrWhiteSpace(r))
                continue;

            if (IsSection(row, lineNumber, messageBuilder, out var iniSection))
            {

            }
            else if (IsValue(row, lineNumber, currentSection.SectionName, messageBuilder, out var iniValueValue))
            {

            }
            else if (IsComment(row, lineNumber, currentSection.SectionName, messageBuilder, out var iniValueComment))
            {

            }
            else
            {
                messageBuilder.AppendLine($"Row number {lineNumber} has unknown content: {row}");
                message = messageBuilder.ToString();
                return false;
            }
        }

        message = messageBuilder.ToString();
        return true;
    }

    private bool IsSection(string row, int lineNumber, StringBuilder messageBuilder, out IniSection s)
    {
        s = new IniSection("Hello");
        return false;
    }

    private bool IsValue(string row, int lineNumber, string currentSection, StringBuilder messageBuilder, out IniValue v)
    {
        v = new IniValue(lineNumber, currentSection);
        var equalsPosition = row.IndexOf('=');

        if (equalsPosition < 0)
            return false;

        var valueName = 
    }

    private bool IsComment(string row, int lineNumber, string currentSection, StringBuilder messageBuilder, out IniValue v)
    {
        v = new IniValue(lineNumber, currentSection);

        if (!row.StartsWith(";"))
        {
            v.Remark = row;
            return false;
        }

        while (row.StartsWith(";"))
            row = row.Substring(1).Trim();

        if (string.IsNullOrWhiteSpace(row))
            row = "Empty comment.";

        v.Remark = row;
        return true;
    }
}