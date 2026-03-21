#nullable enable
using IniParser;

namespace IniParserTests;

[TestClass]
public sealed class IniParserTests
{
    [TestMethod]
    public void IniSectionIsEmpty()
    {
        var s = new IniSection("");
        Assert.IsTrue(s.IsEmpty());
        s.SectionName = "X";
        Assert.IsFalse(s.IsEmpty());
        s.SectionName = "";
        s.Add(new IniValue());
        Assert.IsFalse(s.IsEmpty());
        s.SectionName = "Y";
        Assert.IsFalse(s.IsEmpty());
        s.SectionName = "";
        s.Clear();
        Assert.IsTrue(s.IsEmpty());
    }

    [TestMethod]
    public void IniSectionIsSameAs()
    {
        var s1 = new IniSection("A");
        var s2 = new IniSection("A");
        Assert.IsTrue(s1.IsSameAs(s2));
        s2.SectionName = "B";
        Assert.IsFalse(s1.IsSameAs(s2));
    }

    [TestMethod]
    public void IniSectionMerge()
    {
        var s1 = new IniSection("A");
        var s2 = new IniSection("A");
        s1.Add(new IniValue(2, "A", "S1", "V1", "R1"));
        s2.Add(new IniValue(3, "A", "S2", "V2", "R2"));
        s2.Add(new IniValue(3, "A", "S2", "V2", "R3"));
        s1.Merge(s2);
        Assert.AreEqual("A", s1.SectionName);
        Assert.HasCount(2, s1);
        Assert.AreEqual("S1", s1[0].SettingName);
        Assert.AreEqual("V1", s1[0].SettingValue);
        Assert.AreEqual("R1", s1[0].Comment);
        Assert.AreEqual("S2", s1[1].SettingName);
        Assert.AreEqual("V2", s1[1].SettingValue);
        Assert.AreEqual("R2 R3", s1[1].Comment);
    }

    [TestMethod]
    public void IniSectionGetValueSameAs()
    {
        var s = new IniSection("A")
        {
            new IniValue(1, "A", "S1", "V1", "R1"),
            new IniValue(2, "A", "S1", "V1", "R2"),
            new IniValue(3, "A", "S1", "V2", "R3")
        };

        Assert.IsNull(s.GetValueSameAs(new IniValue(1, "A", "S1", "V3", "")));
        Assert.AreEqual("V2", s.GetValueSameAs(new IniValue(1, "A", "S1", "V2", ""))!.SettingValue);
    }

    [TestMethod]
    public void TryParse()
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
        Assert.AreEqual("A comment without a value or section", iniFile[0][0].Comment);
        Assert.AreEqual("", iniFile[0][1].SectionName);
        Assert.AreEqual("V1", iniFile[0][1].SettingName);
        Assert.AreEqual("A value without a section", iniFile[0][1].SettingValue);
        Assert.AreEqual("", iniFile[0][1].Comment);

        Assert.HasCount(3, iniFile[1]);
        Assert.AreEqual("Section 2 starts here", iniFile[2].Comment);
        Assert.AreEqual("S2", iniFile[2][0].SectionName);
        Assert.AreEqual("V4", iniFile[2][0].SettingName);
        Assert.AreEqual("A value in section 2", iniFile[2][0].SettingValue);
    }

    [TestMethod]
    public void Render()
    {
        const string raw = @";A comment without a value or section
V1 = A value without a section

[S1]
V2=Value 1
V3=Value 2; With comment

;Also, this is a comment in S1
[S2];Section 2 starts here
V4=A value in section 2";
        const string resultText = @"; A comment without a value or section
V1 = A value without a section

[S1]
V2 = Value 1
V3 = Value 2; With comment
; Also, this is a comment in S1

[S2]; Section 2 starts here
V4 = A value in section 2
";
        var parser = new Parser(raw);
        var success = parser.TryParse(out var message, out var iniFile);
        Assert.IsTrue(success);
        Assert.IsTrue(string.IsNullOrWhiteSpace(message));
        var result = iniFile.Render();
        System.Diagnostics.Debug.WriteLine(result);
        Assert.AreEqual(resultText, result);
    }
}