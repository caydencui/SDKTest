// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class CrashSightSDKTest : ModuleRules
{
	public CrashSightSDKTest(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { "Core", "CoreUObject", "Engine", "InputCore" });

		PrivateDependencyModuleNames.AddRange(new string[] { });

		PrivateDependencyModuleNames.AddRange(new string[] { "CrashSight" });
		
		PublicDefinitions.Add("USE_CRASHSIGHT=1");





		// Uncomment if you are using Slate UI
		// PrivateDependencyModuleNames.AddRange(new string[] { "Slate", "SlateCore" });

		// Uncomment if you are using online features
		// PrivateDependencyModuleNames.Add("OnlineSubsystem");

		// To include OnlineSubsystemSteam, add it to the plugins section in your uproject file with the Enabled attribute set to true
	}
}
