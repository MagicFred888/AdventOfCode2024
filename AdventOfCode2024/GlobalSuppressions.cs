// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "Part of a toolbox, maybe used later", Scope = "member", Target = "~M:AdventOfCode2024.Tools.Tools.DebugPrint(System.Object[,],System.Collections.Generic.Dictionary{System.String,System.String},System.String)")]
[assembly: SuppressMessage("Blocker Code Smell", "S2368:Public methods should not have multidimensional array parameters", Justification = "Part of a toolbox, maybe used later", Scope = "member", Target = "~M:AdventOfCode2024.Tools.Tools.DebugPrint(System.Char[,])")]
[assembly: SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "Part of a toolbox, maybe used later", Scope = "member", Target = "~M:AdventOfCode2024.Tools.Tools.LCM(System.Int64[])~System.Int64")]
[assembly: SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Part of a toolbox, maybe used later", Scope = "member", Target = "~M:AdventOfCode2024.Tools.Tools.LCM(System.Int64[])~System.Int64")]