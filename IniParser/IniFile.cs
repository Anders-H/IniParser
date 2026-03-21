#nullable enable
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
                currentSection = section.SectionName;
                s.AppendLine();

                s.AppendLine(string.IsNullOrWhiteSpace(section.Comment)
                    ? $"[{section.SectionName}]"
                    : $"[{section.SectionName}]; {section.Comment}");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(section.SectionName))
                {
                    if (!string.IsNullOrWhiteSpace(section.Comment))
                        s.AppendLine($"; {section.Comment}");
                }
                else
                {
                    s.AppendLine();

                    s.AppendLine(string.IsNullOrWhiteSpace(section.Comment)
                        ? $"[{section.SectionName}]"
                        : $"[{section.SectionName}]; {section.Comment}");
                }
            }

            foreach (var v in section)
            {
                if (string.IsNullOrWhiteSpace(v.SettingName))
                {
                    if (!string.IsNullOrWhiteSpace(v.Comment))
                        s.AppendLine($"; {v.Comment}");
                }
                else
                {
                    s.AppendLine(string.IsNullOrWhiteSpace(v.Comment)
                        ? $"{v.SettingName} = {v.SettingValue}".Trim()
                        : $"{v.SettingName} = {v.SettingValue}; {v.Comment}".Trim());
                }
            }
        }

        return s.ToString();
    }
}