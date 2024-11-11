using System.Text;

namespace DatabaseConnector.Models;

public class HashGenerator : IProblemIdGenerator
{
    private const ulong Modulo = 18446744073709551557;
    private const string Base16Chars = "0123456789ABCDEF";
    
    public string GenerateFromStatement(string source, string statement)
    {
        var hash = GenerateHash(statement);
        return IntegerToBase16(hash);
    }

    private string IntegerToBase16(ulong number)
    {
        string result = "";
        for (int i = 0; i < 16; i++)
        {
            result += Base16Chars[(int)(number % 16)];
            number /= 16;
        }
        
        return result;
    }

    private ulong GenerateHash(string source)
    {
        byte[] asciiBytes = Encoding.ASCII.GetBytes(source);

        UInt128 total = 0;
        foreach (byte c in asciiBytes)
        {
            total *= 16;
            total %= Modulo;
            total += c;
        }

        return (ulong)total;
    }
}