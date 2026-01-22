// Copyright Epic Games, Inc. All Rights Reserved.


using UnrealBuildTool;
using System.IO;
using System;
using System.Reflection;
public class CrashSight : ModuleRules
{
    public CrashSight(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

#if UE_4_20_OR_LATER
        // ue 4.20 or later do not need PublicIncludePaths
#else
        PublicIncludePaths.AddRange(
            new string[] {
                "CrashSight/Public",
            }
            );
#endif

        PrivateIncludePaths.AddRange(
            new string[] {
                "CrashSight/Private"
            }
            );


        PublicDependencyModuleNames.AddRange(
            new string[]
            {
                "Core",
                "CoreUObject",
                "Engine",
                "Slate",
                "SlateCore",
                // ... add other public dependencies that you statically link with here ...
            }
            );


        if (Target.Platform == UnrealTargetPlatform.IOS)
        {
            PrivateDependencyModuleNames.AddRange(
            new string[]
            {
                "CoreUObject",
                "Engine",
                // ... add private dependencies that you statically link with here ...
            }
            );
        }
        else
        {
            PrivateDependencyModuleNames.AddRange(
            new string[]
            {
                "Core",
                "CoreUObject",
                "Engine",
                // ... add private dependencies that you statically link with here ...

            }
            );
        }


        if (Target.Type == TargetRules.TargetType.Editor)
        {

            DynamicallyLoadedModuleNames.AddRange(
                        new string[] {
                    "Settings",
                    }
                    );

            PrivateIncludePathModuleNames.AddRange(
                new string[] {
                    "Settings",
            }
            );
        }


        if (Target.Platform == UnrealTargetPlatform.Android)
        {
            string[] androidArchitectures = new string[] {
                "armeabi-v7a",
                "arm64-v8a",
                "x86",
                "x86_64"
            };

            bool bFoundAnyAndroidLib = false;
            string baseLibPath = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib/Android/libs"));

            foreach (string arch in androidArchitectures)
            {
                string libDir = Path.Combine(baseLibPath, arch);
                string libFile = Path.Combine(libDir, "libCrashSight.so");

                if (Directory.Exists(libDir) && File.Exists(libFile))
                {
#if UE_4_24_OR_LATER
                    PublicSystemLibraryPaths.Add(libDir);
#else
                    PublicLibraryPaths.Add(libDir);
#endif
                    bFoundAnyAndroidLib = true;
                    System.Console.WriteLine("-------------- Added Android " + arch + " library path");
                }
                else
                {
                    System.Console.WriteLine("-------------- Warning: Android " + arch + " library not found at " + libFile);
                }
            }

            if (bFoundAnyAndroidLib)
            {
                PrivateDependencyModuleNames.AddRange(new string[] { "Launch" });
                PublicAdditionalLibraries.Add("CrashSight");

                string PluginPath = Utils.MakePathRelativeTo(ModuleDirectory, Target.RelativeEnginePath);
#if UE_4_20_OR_LATER
                AdditionalPropertiesForReceipt.Add("AndroidPlugin", Path.Combine(PluginPath, "CrashSight_APL.xml"));
#else
                AdditionalPropertiesForReceipt.Add(new ReceiptProperty("AndroidPlugin", Path.Combine(PluginPath, "CrashSight_APL.xml")));
#endif
                System.Console.WriteLine("CrashSight APL Path = " + Path.Combine(PluginPath, "CrashSight_APL.xml"));
            }
            else
            {
                System.Console.WriteLine("-------------- Error: No valid Android CrashSight libraries found!");
            }
        }
        else if (Target.Platform == UnrealTargetPlatform.IOS)
        {
            string[] iosFrameworks = new string[] {
                "CrashSight.embeddedframework.zip",
                "CrashSightAdapter.embeddedframework.zip",
                "CrashSightCore.embeddedframework.zip",
                "CrashSightPlugin.embeddedframework.zip"
            };

            bool bAllFrameworksExist = true;
            string iosFrameworkPath = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib/iOS"));

            foreach (string fw in iosFrameworks)
            {
                string frameworkFullPath = Path.Combine(iosFrameworkPath, fw);
                if (!File.Exists(frameworkFullPath))
                {
                    System.Console.WriteLine("-------------- Warning: iOS framework " + fw + " not found at " + frameworkFullPath);
                    bAllFrameworksExist = false;
                }
            }

            if (bAllFrameworksExist)
            {
#if UE_4_24_OR_LATER
                PublicSystemLibraries.AddRange(new string[] { "z","c++","z.1.1.3","sqlite3","xml2","resolv" });
#else
                PublicAdditionalLibraries.AddRange(new string[] { "z","c++","z.1.1.3" });
#endif
                PublicFrameworks.AddRange(new string[] { "Foundation", "Security" });
                PublicWeakFrameworks.AddRange(new string[] { "MetricKit", "OSLog", "CFNetwork" });

#if UE_5_2_OR_LATER
                PublicAdditionalFrameworks.Add(new Framework("CrashSight", "../CrashSightLib/iOS/CrashSight.embeddedframework.zip", null, true));
                PublicAdditionalFrameworks.Add(new Framework("CrashSightAdapter", "../CrashSightLib/iOS/CrashSightAdapter.embeddedframework.zip", null, true));
                PublicAdditionalFrameworks.Add(new Framework("CrashSightCore", "../CrashSightLib/iOS/CrashSightCore.embeddedframework.zip", null, true));
                PublicAdditionalFrameworks.Add(new Framework("CrashSightPlugin", "../CrashSightLib/iOS/CrashSightPlugin.embeddedframework.zip", null, true));
#elif UE_4_22_OR_LATER
                PublicAdditionalFrameworks.Add(new Framework("CrashSight", "../CrashSightLib/iOS/CrashSight.embeddedframework.zip", "Frameworks"));
                PublicAdditionalFrameworks.Add(new Framework("CrashSightAdapter", "../CrashSightLib/iOS/CrashSightAdapter.embeddedframework.zip", "Frameworks"));
                PublicAdditionalFrameworks.Add(new Framework("CrashSightCore", "../CrashSightLib/iOS/CrashSightCore.embeddedframework.zip", "Frameworks"));
                PublicAdditionalFrameworks.Add(new Framework("CrashSightPlugin", "../CrashSightLib/iOS/CrashSightPlugin.embeddedframework.zip", "Frameworks"));
#else
                PublicAdditionalFrameworks.Add(new UEBuildFramework("CrashSight", "../CrashSightLib/iOS/CrashSight.embeddedframework.zip", "Frameworks"));
                PublicAdditionalFrameworks.Add(new UEBuildFramework("CrashSightAdapter", "../CrashSightLib/iOS/CrashSightAdapter.embeddedframework.zip", "Frameworks"));
                PublicAdditionalFrameworks.Add(new UEBuildFramework("CrashSightCore", "../CrashSightLib/iOS/CrashSightCore.embeddedframework.zip", "Frameworks"));
                PublicAdditionalFrameworks.Add(new UEBuildFramework("CrashSightPlugin", "../CrashSightLib/iOS/CrashSightPlugin.embeddedframework.zip", "Frameworks"));
#endif
#if UE_4_22_OR_LATER && !UE_5_2_OR_LATER
                string PluginPath = Utils.MakePathRelativeTo(ModuleDirectory, Target.RelativeEnginePath);
                AdditionalPropertiesForReceipt.Add("IOSPlugin", Path.Combine(PluginPath, "CrashSight_iOS_UPL.xml"));
#endif
            }
            else
            {
                System.Console.WriteLine("-------------- Error: Missing one or more iOS frameworks!");
            }
        }
        else if (Target.Platform == UnrealTargetPlatform.Mac)
        {
            string CrashSightLibPath = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib/Mac"));
            string dylibPath = Path.Combine(CrashSightLibPath, "CrashSight.dylib");

            if (File.Exists(dylibPath))
            {
#if UE_4_20_OR_LATER
#else
                PublicIncludePaths.AddRange(new string[] {"Runtime/ApplicationCore/Public/Apple"});
#endif
                PublicDelayLoadDLLs.Add(dylibPath);
#if UE_4_22_OR_LATER
                RuntimeDependencies.Add("$(BinaryOutputDir)/CrashSight.dylib", dylibPath);
#endif
                System.Console.WriteLine("-------------- Mac CrashSight library added at " + dylibPath);
            }
            else
            {
                System.Console.WriteLine("-------------- Warning: Mac CrashSight.dylib not found at " + dylibPath);
            }
        }
        else if (Target.Platform == UnrealTargetPlatform.Win64)
        {
            string winLibPath = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib/Win/X86_64"));
            string dllPath = Path.Combine(winLibPath, "CrashSight64.dll");

            if (File.Exists(dllPath))
            {
                PublicDelayLoadDLLs.Add("CrashSight64.dll");
#if UE_4_22_OR_LATER
                RuntimeDependencies.Add("$(TargetOutputDir)/CrashSight64.dll", dllPath);
#endif
                System.Console.WriteLine("-------------- Win64 CrashSight library added at " + dllPath);
            }
            else
            {
                System.Console.WriteLine("-------------- Warning: CrashSight64.dll not found at " + dllPath);
            }
        }
        else if (Target.Platform == UnrealTargetPlatform.Linux)
        {
            System.Console.WriteLine("--------------Add Linux CrashSight");
            string CrashSightThirdPartyDir = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib"));
            string CrashSightLibPath = Path.Combine(CrashSightThirdPartyDir, "Linux");
            System.Console.WriteLine("--------------Linux CrashSightLibPath = " + CrashSightLibPath);

            // 检查库文件是否存在
            string libraryPath = Path.Combine(CrashSightLibPath, "libCrashSight.so");
            if (File.Exists(libraryPath))
            {
                // 添加库路径和依赖
#if UE_4_24_OR_LATER
                PublicSystemLibraryPaths.Add(Path.GetFullPath(CrashSightLibPath));
#else
                PublicLibraryPaths.Add(Path.GetFullPath(CrashSightLibPath));
#endif
                PublicAdditionalLibraries.Add(libraryPath);
#if UE_4_22_OR_LATER
                RuntimeDependencies.Add("$(TargetOutputDir)/libCrashSight.so", libraryPath);
                PrivateRuntimeLibraryPaths.Add(Path.GetFullPath(CrashSightLibPath));
#endif
                System.Console.WriteLine("-------------- CrashSight library added successfully");
            }
            else
            {
                // 输出警告信息，避免报错
                System.Console.WriteLine("-------------- Warning: libCrashSight.so not found at " + libraryPath + ", skipping CrashSight setup.");
            }
        }
        else if (IsSameUnrealTargetPlatformCrashSight("XSX", Target.Platform))
        {
            string xsxLibPath = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib/XSX"));
            string dllPath = Path.Combine(xsxLibPath, "CrashSightXbox.dll");

            if (File.Exists(dllPath))
            {
#if UE_4_20_OR_LATER
                PublicDefinitions.Add("CRASHSIGHT_XSX=1");
#else
                Definitions.Add("CRASHSIGHT_XSX=1");
#endif
#if UE_4_24_OR_LATER
                PublicSystemLibraryPaths.Add(xsxLibPath);
#else
                PublicLibraryPaths.Add(xsxLibPath);
#endif
#if UE_4_22_OR_LATER
                RuntimeDependencies.Add("$(TargetOutputDir)/CrashSightXbox.dll", dllPath);
#endif
            }
            else
            {
                System.Console.WriteLine("-------------- Warning: XSX CrashSight library not found at " + dllPath);
            }
        }
        else if (IsSameUnrealTargetPlatformCrashSight("PS4", Target.Platform))
        {
            string CrashSightThirdPartyDir = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib"));
            string CrashSightLibPath = Path.Combine(CrashSightThirdPartyDir, "PS4");
            string stubLibPath = Path.Combine(CrashSightLibPath, "libcs_stub.a");
            string prxPath = Path.Combine(CrashSightLibPath, "libcs.prx");

            if (File.Exists(stubLibPath) && File.Exists(prxPath))
            {
#if UE_4_20_OR_LATER
                PublicDefinitions.Add("CRASHSIGHT_PS4=1");
#else
                Definitions.Add("CRASHSIGHT_PS4=1");
#endif
#if UE_4_24_OR_LATER
                PublicSystemLibraries.AddRange(new string[]
                {
                    "SceNetCtl_stub_weak",
                    "SceNet_stub_weak",
                    "SceHttp_stub_weak",
                    "SceSsl_stub_weak"
                });
#else
                PublicAdditionalLibraries.AddRange(new string[]
                {
                    "SceNetCtl_stub_weak",
                    "SceNet_stub_weak",
                    "SceHttp_stub_weak",
                    "SceSsl_stub_weak"
                });
#endif
#if UE_4_24_OR_LATER
                PublicSystemLibraryPaths.Add(CrashSightLibPath);
#else
                PublicLibraryPaths.Add(CrashSightLibPath);
#endif
                PublicAdditionalLibraries.Add(stubLibPath);
#if UE_4_22_OR_LATER
                RuntimeDependencies.Add(prxPath);
#endif
                PublicDelayLoadDLLs.Add(prxPath);
                System.Console.WriteLine("-------------- PS4 CrashSight configured at " + CrashSightLibPath);
            }
            else
            {
                System.Console.WriteLine("-------------- Warning: PS4 libraries not found! Expected: " + stubLibPath + " and " + prxPath);
            }
        }
        else if (IsSameUnrealTargetPlatformCrashSight("PS5", Target.Platform))
        {
            string CrashSightThirdPartyDir = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib"));
            string CrashSightLibPath = Path.Combine(CrashSightThirdPartyDir, "PS5");
            string stubLibPath = Path.Combine(CrashSightLibPath, "libcs_stub.a");
            string prxPath = Path.Combine(CrashSightLibPath, "libcs.prx");

            if (File.Exists(stubLibPath) && File.Exists(prxPath))
            {
#if UE_4_20_OR_LATER
                PublicDefinitions.Add("CRASHSIGHT_PS5=1");
#else
                Definitions.Add("CRASHSIGHT_PS5=1");
#endif
#if UE_4_24_OR_LATER
                PublicSystemLibraries.AddRange(new string[]
                {
                    "SceNetCtl_stub_weak",
                    "SceNet_stub_weak",
                    "SceHttp_stub_weak",
                    "SceSsl_stub_weak"
                });
#else
                PublicAdditionalLibraries.AddRange(new string[]
                {
                    "SceNetCtl_stub_weak",
                    "SceNet_stub_weak",
                    "SceHttp_stub_weak",
                    "SceSsl_stub_weak"
                });
#endif
#if UE_4_24_OR_LATER
                PublicSystemLibraryPaths.Add(CrashSightLibPath);
#else
                PublicLibraryPaths.Add(CrashSightLibPath);
#endif
                PublicAdditionalLibraries.Add(stubLibPath);
#if UE_4_22_OR_LATER
                RuntimeDependencies.Add(prxPath);
#endif
                PublicDelayLoadDLLs.Add(prxPath);
                System.Console.WriteLine("-------------- PS5 CrashSight configured at " + CrashSightLibPath);
            }
            else
            {
                System.Console.WriteLine("-------------- Warning: PS5 libraries not found! Expected: " + stubLibPath + " and " + prxPath);
            }
        }
        else if (IsSameUnrealTargetPlatformCrashSight("Switch", Target.Platform))
        {
            string CrashSightThirdPartyDir = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib"));
            string CrashSightLibPath = Path.Combine(CrashSightThirdPartyDir, "Switch");
            string switchLibPath = Path.Combine(CrashSightLibPath, "libcs.a");

            if (File.Exists(switchLibPath))
            {
#if UE_4_20_OR_LATER
                PublicDefinitions.Add("CRASHSIGHT_SWITCH=1");
#else
                Definitions.Add("CRASHSIGHT_SWITCH=1");
#endif
                PublicAdditionalLibraries.Add(switchLibPath);
                System.Console.WriteLine("-------------- Switch CrashSight configured at " + switchLibPath);
            }
            else
            {
                System.Console.WriteLine("-------------- Warning: Switch library not found at " + switchLibPath);
            }
        }
        else if (IsSameUnrealTargetPlatformCrashSight("OpenHarmony", Target.Platform))
        {
            string PluginPath = Path.GetFullPath(Path.Combine(ModuleDirectory, "../CrashSightLib"));
            string oplPath = Path.Combine(PluginPath, "OpenHarmony/CrashSight_OPL.xml");
            string harPath = Path.Combine(PluginPath, "OpenHarmony/crashsight.har");

            if (File.Exists(oplPath) && Directory.Exists(Path.GetDirectoryName(harPath)))
            {
                PrivateDependencyModuleNames.AddRange(new string[] { "Launch", "aki_jsbind" });
#if UE_4_20_OR_LATER
                PublicDefinitions.Add("CRASHSIGHT_OHOS");
#else
                Definitions.Add("CRASHSIGHT_OHOS");
#endif
#if UE_OPENHARMONY
                PublicAdditionalHars.Add(harPath);
#endif
#if UE_4_20_OR_LATER
                AdditionalPropertiesForReceipt.Add("OpenHarmonyPlugin", oplPath);
#else
                AdditionalPropertiesForReceipt.Add(new ReceiptProperty("OpenHarmonyPlugin", oplPath));
#endif
                string crashSightRootLibDir = Path.GetFullPath(Path.Combine(ModuleDirectory, "OpenHarmony/libs"));
                PublicAdditionalLibraries.Add("crashsight");
                System.Console.WriteLine("-------------- OpenHarmony CrashSight configured at " + PluginPath);
            }
            else
            {
                System.Console.WriteLine("-------------- Error: OpenHarmony files missing! Check " + oplPath + " and " + harPath);
            }
        }
    }

    private bool IsSameUnrealTargetPlatformCrashSight(String platformName, UnrealTargetPlatform currentPlatform)
    {
        FieldInfo platformField = typeof(UnrealTargetPlatform).GetField(platformName);
        if (platformField != null)
        {
            UnrealTargetPlatform platform = (UnrealTargetPlatform)platformField.GetValue(null);
            System.Console.WriteLine("Find platform: " + platformName);
            return platform == currentPlatform;
        }
        else
        {
            System.Console.WriteLine("Unable to get platform: " + platformName);
            return false;
        }
    }
}
