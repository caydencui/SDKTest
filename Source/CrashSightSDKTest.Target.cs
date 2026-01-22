// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System.Collections.Generic;

public class CrashSightSDKTestTarget : TargetRules
{
	public CrashSightSDKTestTarget(TargetInfo Target) : base(Target)
	{
		Type = TargetType.Game;
		DefaultBuildSettings = BuildSettingsVersion.V2;
		ExtraModuleNames.AddRange(new string[] { "CrashSightSDKTest" });
		if (Target.Platform == UnrealTargetPlatform.IOS)
		{
			bOverrideBuildEnvironment = true;
			AdditionalCompilerArguments += " -Wno-error=bitwise-instead-of-logical -Wno-unused-but-set-variable";
		}
		
	}
}
