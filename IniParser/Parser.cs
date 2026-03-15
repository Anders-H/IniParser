using System;

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
        var rows = _raw.Split([Environment.NewLine], StringSplitOptions.None);
        var currentSectionName = "";
        var result = new IniFile();

        foreach (var row in rows)
        {
            var r = row.Trim();

            if (string.IsNullOrWhiteSpace(r))
                continue;


        }
    }
}