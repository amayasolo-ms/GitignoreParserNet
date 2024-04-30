using System.Text.RegularExpressions;

namespace GitignoreParserNet;

internal static
#if NET7_0_OR_GREATER
    partial
#endif
    class RegexPatterns
{
#if NET7_0_OR_GREATER
    public static readonly Regex MatchEmptyRegex = GeneratedMatchEmptyRegex();
    public static readonly Regex RangeRegex = GeneratedRangeRegex();
    public static readonly Regex BackslashRegex = GeneratedBackslashRegex();
    public static readonly Regex SpecialCharactersRegex = GeneratedSpecialCharactersRegex();
    public static readonly Regex QuestionMarkRegex = GeneratedQuestionMarkRegex();
    public static readonly Regex SlashDoubleAsteriksSlashRegex = GeneratedSlashDoubleAsteriksSlashRegex();
    public static readonly Regex DoubleAsteriksSlashRegex = GeneratedDoubleAsteriksSlashRegex();
    public static readonly Regex SlashDoubleAsteriksRegex = GeneratedSlashDoubleAsteriksRegex();
    public static readonly Regex DoubleAsteriksRegex = GeneratedDoubleAsteriksRegex();
    public static readonly Regex SlashAsteriksEndOrSlashRegex = GeneratedSlashAsteriksEndOrSlashRegex();
    public static readonly Regex AsteriksRegex = GeneratedAsteriksRegex();
    public static readonly Regex SlashRegex = GeneratedSlashRegex();

    [GeneratedRegex("$^")]
    private static partial Regex GeneratedMatchEmptyRegex();
    [GeneratedRegex(@"^((?:[^\[\\]|(?:\\.))*)\[((?:[^\]\\]|(?:\\.))*)\]")]
    private static partial Regex GeneratedRangeRegex();
    [GeneratedRegex(@"\\(.)")]
    private static partial Regex GeneratedBackslashRegex();
    [GeneratedRegex(@"[\-\[\]\{\}\(\)\+\.\\\^\$\|]")]
    private static partial Regex GeneratedSpecialCharactersRegex();
    [GeneratedRegex(@"\?")]
    private static partial Regex GeneratedQuestionMarkRegex();
    [GeneratedRegex(@"\/\*\*\/")]
    private static partial Regex GeneratedSlashDoubleAsteriksSlashRegex();
    [GeneratedRegex(@"^\*\*\/")]
    private static partial Regex GeneratedDoubleAsteriksSlashRegex();
    [GeneratedRegex(@"\/\*\*$")]
    private static partial Regex GeneratedSlashDoubleAsteriksRegex();
    [GeneratedRegex(@"\*\*")]
    private static partial Regex GeneratedDoubleAsteriksRegex();
    [GeneratedRegex(@"\/\*(\/|$)")]
    private static partial Regex GeneratedSlashAsteriksEndOrSlashRegex();
    [GeneratedRegex(@"\*")]
    private static partial Regex GeneratedAsteriksRegex();
    [GeneratedRegex(@"\/")]
    private static partial Regex GeneratedSlashRegex();

#else
    public static readonly Regex MatchEmptyRegex = new("$^", RegexOptions.Compiled);
    public static readonly Regex RangeRegex = new(@"^((?:[^\[\\]|(?:\\.))*)\[((?:[^\]\\]|(?:\\.))*)\]", RegexOptions.Compiled);
    public static readonly Regex BackslashRegex = new(@"\\(.)", RegexOptions.Compiled);
    public static readonly Regex SpecialCharactersRegex = new(@"[\-\[\]\{\}\(\)\+\.\\\^\$\|]", RegexOptions.Compiled);
    public static readonly Regex QuestionMarkRegex = new(@"\?", RegexOptions.Compiled);
    public static readonly Regex SlashDoubleAsteriksSlashRegex = new(@"\/\*\*\/", RegexOptions.Compiled);
    public static readonly Regex DoubleAsteriksSlashRegex = new(@"^\*\*\/", RegexOptions.Compiled);
    public static readonly Regex SlashDoubleAsteriksRegex = new(@"\/\*\*$", RegexOptions.Compiled);
    public static readonly Regex DoubleAsteriksRegex = new(@"\*\*", RegexOptions.Compiled);
    public static readonly Regex SlashAsteriksEndOrSlashRegex = new(@"\/\*(\/|$)", RegexOptions.Compiled);
    public static readonly Regex AsteriksRegex = new(@"\*", RegexOptions.Compiled);
    public static readonly Regex SlashRegex = new(@"\/", RegexOptions.Compiled);
#endif
}
