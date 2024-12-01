// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "This is a debug methode to display an array", Scope = "member", Target = "~M:AdventOfCode2024.Tools.Tools.DebugPrint(System.Object[,],System.Collections.Generic.Dictionary{System.String,System.String},System.String)")]
[assembly: SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "This is a debug methode to display an array", Scope = "member", Target = "~M:AdventOfCode2024.Tools.Tools.DebugPrint(System.Char[,])")]
[assembly: SuppressMessage("Performance", "SYSLIB1045:Convert to 'GeneratedRegexAttribute'.", Justification = "<Pending>", Scope = "member", Target = "~M:AdventOfCode2024.Solver.Day01.ExtractData")]