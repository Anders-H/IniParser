using IniParser;

namespace IniParserTests;

[TestClass]
public sealed class IniParserTests
{
    [TestMethod]
    public void TryParse1()
    {
        const string raw = @";A comment without a value or section
V1 = A value without a section

[S1]
V2=Value 1
V3=Value 2; With comment

;Also, this is a comment in S1
[S2];Section 2 starts here
V4=A value in section 2";

        var parser = new Parser(raw);
        var success = parser.TryParse(out var message, out var iniFile);
        Assert.IsTrue(success);
        Assert.AreEqual("", message);
        Assert.IsNotNull(iniFile);
        Assert.HasCount(3, iniFile);
        Assert.AreEqual("", iniFile[0].SectionName);
        Assert.AreEqual("S1", iniFile[1].SectionName);
        Assert.AreEqual("S2", iniFile[2].SectionName);

        Assert.HasCount(2, iniFile[0]);
        Assert.AreEqual("", iniFile[0][0].SectionName);
        Assert.AreEqual("", iniFile[0][0].SettingName);
        Assert.AreEqual("", iniFile[0][0].SettingValue);
        Assert.AreEqual("A comment without a value or section", iniFile[0][0].Remark);
        Assert.AreEqual("", iniFile[0][1].SectionName);
        Assert.AreEqual("V1", iniFile[0][1].SettingName);
        Assert.AreEqual("A value without a setting", iniFile[0][1].SettingValue);
        Assert.AreEqual("", iniFile[0][1].Remark);

        Assert.HasCount(2, iniFile[2]);
        Assert.AreEqual("S2", iniFile[2][0].SectionName);
        Assert.AreEqual("", iniFile[2][0].SettingName);
        Assert.AreEqual("", iniFile[2][0].SettingValue);
        Assert.AreEqual("Section 2 starts here", iniFile[2][0].Remark);
        Assert.AreEqual("S2", iniFile[2][1].SectionName);
        Assert.AreEqual("V4", iniFile[2][1].SettingName);
        Assert.AreEqual("A value in section 2", iniFile[2][1].SettingValue);
        Assert.AreEqual("", iniFile[2][1].Remark);
    }
}