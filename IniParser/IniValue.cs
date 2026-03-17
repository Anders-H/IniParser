namespace IniParser;

public class IniValue
{
    public int SourceLine { get; set; }
    public string SectionName { get; set; }
    public string SettingName { get; set; }
    public string SettingValue { get; set; }
    public string Remark { get; set; }

    public IniValue() : this(0, "", "", "", "")
    {
    }

    public IniValue(int sourceLine, string sectionName) : this(sourceLine, sectionName, "", "", "")
    {
    }

    public IniValue(int sourceLine, string sectionName, string settingName, string settingValue, string remark)
    {
        SourceLine = sourceLine;
        SectionName = sectionName;
        SettingName = settingName;
        SettingValue = settingValue;
        Remark = remark;
    }
}