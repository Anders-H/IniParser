using System;
using System.Linq;
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
        iniFile = [new IniSection("")];
        var rows = _raw.Split([Environment.NewLine], StringSplitOptions.None);
        var lineNumber = 0;
        var messageBuilder = new StringBuilder();

        foreach (var row in rows)
        {
            var currentSection = iniFile.Last();
            lineNumber++;
            var r = row.Trim();

            if (string.IsNullOrWhiteSpace(r))
                continue;

            if (IsSection(row, lineNumber, iniFile.Last().SectionName, messageBuilder, out var iniSection))
            {
                if (iniFile.Count == 1 && iniFile.First().IsEmpty())
                {
                    iniFile.RemoveAt(0);
                    iniFile.Add(iniSection);
                    continue;
                }

                if (iniFile.First().IsSameAs(iniSection))
                    iniFile.First().Merge(iniSection);
                else
                    iniFile.Add(iniSection);
            }
            else if (IsValue(row, lineNumber, currentSection.SectionName, messageBuilder, out var iniValueValue))
            {
                if (iniValueValue.SectionName == currentSection.SectionName)
                {
                    currentSection.Add(iniValueValue);
                }
                else
                {
                    currentSection = new IniSection(iniValueValue.SectionName);
                    iniFile.Add(currentSection);
                    currentSection.Add(iniValueValue);
                }
            }
            else if (IsComment(row, lineNumber, currentSection.SectionName, messageBuilder, out var iniValueComment))
            {
                if (iniValueComment.SectionName == currentSection.SectionName)
                {
                    currentSection.Add(iniValueComment);
                }
                else
                {
                    currentSection = new IniSection(iniValueComment.SectionName);
                    iniFile.Add(currentSection);
                    currentSection.Add(iniValueComment);
                }
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

    private bool IsSection(string row, int lineNumber, string currentSection, StringBuilder messageBuilder, out IniSection s)
    {
        s = new IniSection(currentSection);
        var openingBrackets = row.IndexOf('[');

        if (openingBrackets < 0)
            return false;

        var closingBrackets = row.IndexOf(']');

        if (closingBrackets <= openingBrackets)
        {
            messageBuilder.AppendLine($"Row number {lineNumber}: Closing brackets before opening brackets.");
            return false;
        }

        s.SectionName = row.Substring(openingBrackets + 1, closingBrackets - openingBrackets - 1);
        var commentStart = row.IndexOf(';', closingBrackets);

        if (commentStart > closingBrackets)
            s.Comment = row.Substring(commentStart + 1).Trim();

        return true;
    }

    private bool IsValue(string row, int lineNumber, string currentSection, StringBuilder messageBuilder, out IniValue v)
    {
        v = new IniValue(lineNumber, currentSection);

        try
        {
            var equalsPosition = row.IndexOf('=');

            if (equalsPosition < 0)
                return false;

            var valueName = row.Substring(0, equalsPosition).Trim();
            var valuePart = row.Substring(equalsPosition + 1).Trim();
            var commentPosition = valuePart.IndexOf(';');

            if (commentPosition > -1)
            {
                var newValuePart = row.Substring(0, commentPosition).Trim();
                v.Remark = row.Substring(commentPosition + 1).Trim();
                valuePart = newValuePart;
            }

            v.SectionName = currentSection;
            v.SettingName = valueName;
            v.SettingValue = valuePart;

            if (string.IsNullOrWhiteSpace(v.SettingName))
                messageBuilder.AppendLine($"Row {lineNumber}: No setting name found.");

            if (string.IsNullOrWhiteSpace(v.SettingValue))
                messageBuilder.AppendLine($"Row {lineNumber}: No setting value found.");

            return true;
        }
        catch
        {
            messageBuilder.AppendLine("Failed to parse value.");
            return false;
        }
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