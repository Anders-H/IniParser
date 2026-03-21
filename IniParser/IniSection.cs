#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace IniParser;

public class IniSection : List<IniValue>
{
    public string SectionName { get; set; }
    public string Comment { get; set; }

    public IniSection(string sectionName)
    {
        SectionName = sectionName;
        Comment = "";
    }

    public bool IsEmpty() =>
        string.IsNullOrWhiteSpace(SectionName) && Count <= 0;

    public bool IsSameAs(IniSection other) =>
        SectionName == other.SectionName;

    public void Merge(IniSection other)
    {
        foreach (var o in other)
        {
            var v = GetValueSameAs(o);

            if (v == null)
                Add(o);
            else
                v.Merge(o);
        }
    }

    public IniValue? GetValueSameAs(IniValue other) =>
        this.FirstOrDefault(v => v.SettingName == other.SettingName && v.SettingValue == other.SettingValue);
}