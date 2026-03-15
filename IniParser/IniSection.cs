using System.Collections.Generic;

namespace IniParser;

public class IniSection : List<IniValue>
{
    public string SectionName { get; set; }

    public IniSection(string sectionName)
    {
        SectionName = sectionName;
    }
}