using Server;

namespace ServerTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }

    [Fact]
    public void TestServer()
    {
        var db = new DataBase();
        Assert.True(true);
    }
}
