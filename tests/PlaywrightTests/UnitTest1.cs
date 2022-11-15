namespace PlaywrightTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        Console.WriteLine(Environment.CurrentDirectory);
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [TearDown]
    public void Teardown()
    {
        // Windows Command Prompt should be "cmd"
        //Array.ForEach(Process.GetProcessesByName("cmd"), x => x.Kill());
    }
}