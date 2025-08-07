using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace Enfo.WebApp.Platform.Utilities;

public static class FileSize
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    private enum FileSizeUnits
    {
        bytes = 0,
        KB = 1,
        MB = 2,
        GB = 3,
        TB = 4,
        PB = 5,
        EB = 6,
    }

    public static string ToFileSizeString(this long value, int precision = 1)
    {
        if (value == 0) return "0 bytes";
        var pow = Math.Min((int)Math.Floor(Math.Log(Math.Abs(value)) / Math.Log(1024)),
            Enum.GetNames<FileSizeUnits>().Length); // Total number of FileSizeUnits available
        var valueString = (value / Math.Pow(1024, pow)).ToString(pow == 0 ? "N0" : "N" + precision);
        var unitString = ((FileSizeUnits)pow).ToString();
        return $"{valueString} {unitString}";
    }
}
