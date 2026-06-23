using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:PrefixLocalCallsWithThis", Justification = "Following Frends documentation guidelines")]
[assembly: SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1200:UsingDirectivesMustBePlacedWithinNamespace", Justification = "Following Frends documentation guidelines")]
[assembly: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:BracesMustNotBeOmitted", Justification = "Following Frends documentation guidelines")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Documentation checked by custom analyzers")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Following Frends Tasks guidelines")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Following Frends Tasks guidelines")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1633:FileMustHaveHeader", Justification = "Following Frends documentation guidelines")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:FileNameMustMatchTypeName", Justification = "Following Frends Tasks guidelines")]
[assembly: SuppressMessage("TaskClass", "FT0004:Task class should be static", Justification = "Profile is a data model class, not a task class", Scope = "type", Target = "~T:Netum.Truugo.ValidatorListProfiles.Definitions.Profile")]
[assembly: SuppressMessage("Parameters", "FT0007:Missing a required parameter", Justification = "This task does not require any input parameters.", Scope = "member", Target = "~M:Netum.Truugo.ValidatorListProfiles.Truugo.ValidatorListProfiles(Netum.Truugo.ValidatorListProfiles.Definitions.Connection,Netum.Truugo.ValidatorListProfiles.Definitions.Options,System.Threading.CancellationToken)~System.Threading.Tasks.Task{Netum.Truugo.ValidatorListProfiles.Definitions.Result}")]
