using System.Text;
using System.Text.Json;

namespace MeDsCoreTests;

public static class TestExtensions
{
    public static byte[] SerializeByte(this object a) => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(a));
}