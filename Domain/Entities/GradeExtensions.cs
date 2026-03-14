using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Entities;

public static class GradeExtensions
{
    private static Dictionary<GradeValue, string> _polishGradeTranslations = new Dictionary<GradeValue, string>
    {
        { GradeValue.Grade20, "Niedostateczny" },
        { GradeValue.Grade30, "Dostateczny" },
        { GradeValue.Grade35, "Dostateczny plus" },
        { GradeValue.Grade40, "Dobry" },
        { GradeValue.Grade45, "Dobry plus" },
        { GradeValue.Grade50, "Bardzo dobry" }
    };
    
    public static double Value(this GradeValue gradeType)
    {
        return (int)gradeType / 10.0;
    }
    

    public static GradeValue Parse(string gradeString)
    {
        return gradeString switch
        {
            "2.0" => GradeValue.Grade20,
            "3.0" => GradeValue.Grade30,
            "3.5" => GradeValue.Grade35,
            "4.0" => GradeValue.Grade40,
            "4.5" => GradeValue.Grade45,
            "5.0" => GradeValue.Grade50,
            _ => throw new ArgumentException($"Invalid grade: {gradeString}")
        };
    }

    public static List<string> GradeValues()
    {
        return Enum.GetValues<GradeValue>().Select(g => g.Value().ToString("N1")).ToList();
    }

    public static string PolishName(GradeValue gradeValue)
    {
        return _polishGradeTranslations[gradeValue];
    }
}