#nullable enable
namespace IniParser;

public class IniValue
{
    public int SourceLine { get; set; }
    public string SectionName { get; set; }
    public string SettingName { get; set; }
    public string SettingValue { get; set; }
    public string Comment { get; set; }

    public IniValue() : this(0, "", "", "", "")
    {
    }

    public IniValue(int sourceLine, string sectionName) : this(sourceLine, sectionName, "", "", "")
    {
    }

    public IniValue(int sourceLine, string sectionName, string settingName, string settingValue, string comment)
    {
        SourceLine = sourceLine;
        SectionName = sectionName;
        SettingName = settingName;
        SettingValue = settingValue;
        Comment = comment;
    }

    public void Merge(IniValue other)
    {
        if (string.IsNullOrEmpty(Comment))
            Comment = other.Comment;
        else if (!string.IsNullOrEmpty(Comment) && !string.IsNullOrEmpty(other.Comment))
            Comment = $"{Comment} {other.Comment}".Trim();
    }
}