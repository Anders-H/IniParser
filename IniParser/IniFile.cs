using System.Collections.Generic;
using System.Text;

namespace IniParser;

public class IniFile : List<IniSection>
{
    public string Render()
    {
        var s = new StringBuilder();
        var currentSection = "";

        foreach (var section in this)
        {
            if (section.SectionName != currentSection)
            {
                if (s.ToString() == "")
                {
                    s.AppendLine(string.IsNullOrWhiteSpace(section.Comment)
                        ? $"[{section.SectionName}]"
                        : $"[{section.SectionName}]; {section.Comment}");

                    currentSection = section.SectionName;
                }
            }
            else
            {
                s.AppendLine();
                s.AppendLine(string.IsNullOrWhiteSpace(section.Comment)
                    ? $"[{section.SectionName}]"
                    : $"[{section.SectionName}]; {section.Comment}");
            }

            foreach (var v in section)
            {
                s.AppendLine(string.IsNullOrWhiteSpace(v.Remark)
                    ? $"{v.SettingName} = {v.SettingValue}".Trim()
                    : $"{v.SettingName} = {v.SettingValue}; {v.Remark}".Trim());
            }
        }

        return s.ToString();
    }
}