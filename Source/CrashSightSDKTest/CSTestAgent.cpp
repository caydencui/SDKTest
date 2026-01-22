// Fill out your copyright notice in the Description page of Project Settings.

// #define USE_CRASHSIGHT 


#ifdef USE_CRASHSIGHT

#if PLATFORM_ANDROID

#elif PLATFORM_IOS

#elif PLATFORM_WINDOWS

#endif


#include "CSTestAgent.h"
#include "CrashSightAgent.h"
#include "UQMCrash.h"

using namespace UQM;
using namespace GCloud::CrashSight;

CSTestAgent::CSTestAgent()
{
}

CSTestAgent::~CSTestAgent()
{
}


void CSTestAgent::TestInitCS() {
    UE_LOG(LogTemp, Warning, TEXT("Init CrashSight!"));
    GEngine->AddOnScreenDebugMessage(0, 1.0f, FColor::Red, TEXT("Init CrashSight!"));

#if PLATFORM_ANDROID
    CrashSightAgent::ConfigDebugMode (true);
    CrashSightAgent::SetAppVersion("android appVersion");
    CrashSightAgent::ConfigCrashServerUrl("https://android.crashsight.qq.com/pb/async");
    CrashSightAgent::InitWithAppId("f6c49b1ac2");
#elif PLATFORM_IOS
    CrashSightAgent::ConfigDebugMode (true);
    CrashSightAgent::SetAppVersion("ios appVersion");
    CrashSightAgent::ConfigCrashServerUrl("https://ios.crashsight.qq.com/pb/sync");
    CrashSightAgent::InitWithAppId("1f29ad1a5a");
#elif PLATFORM_WINDOWS
    CrashSightAgent::ConfigDebugMode (true);
    CrashSightAgent::SetAppVersion("windows appVersion");
    CrashSightAgent::ConfigCrashServerUrl("pc.crashsight.qq.com");
    CrashSightAgent::InitWithAppId("b9d1753475"); 
    CrashSightAgent::SetVehEnable(true);
    CrashSightAgent::UnrealCriticalErrorEnable(true);
#endif

}

void CSTestAgent::TestReportException()
{
    UE_LOG(LogTemp, Warning, TEXT("Report Exception!"));
    GEngine->AddOnScreenDebugMessage(0, 1.0f, FColor::Red, TEXT("Report Exception!"));

    //不带附件的上报错误
#if PLATFORM_ANDROID
    CrashSightAgent::ReportException(4,"AndroidRepeortException", "TestReason", "TestStackTrace", "TestExtras", false, 1);
#elif PLATFORM_IOS
    CrashSightAgent::ReportException(5,"iOSReportException", "TestReason", "TestStackTrace", "TestExtras", false, 1);
#elif PLATFORM_WINDOWS
    CrashSightAgent::ReportException(6,"WindowsReportException", "TestReason", "TestStackTrace", "TestExtras", false, 1);
#endif

    //带附件的上报错误
#if PLATFORM_ANDROID
    // Android: 使用应用专用外部存储
    FString AndroidAttachmentPath = FPaths::Combine(
        FPlatformMisc::ProjectDir(), 
        TEXT("Saved"),
        TEXT("test_attachment_android.txt")
    );
    FPlatformFileManager::Get().GetPlatformFile().CreateDirectoryTree(*FPaths::GetPath(AndroidAttachmentPath));
    FFileHelper::SaveStringToFile(TEXT("Android test attachment content"), *AndroidAttachmentPath);
    
    CrashSightAgent::ReportException(4, "AndroidRepeortAttachmentException", "TestAttachmentReason", "TestAttachmentStackTrace", "TestAttachmentExtras", false, 1, true, TCHAR_TO_UTF8(*AndroidAttachmentPath));
#elif PLATFORM_IOS
    // iOS: 使用文档目录
    FString IOSAttachmentPath = FPaths::Combine(
        FPlatformProcess::UserDir(), 
        TEXT("Documents"),
        TEXT("test_attachment_ios.txt")
    );
    FPlatformFileManager::Get().GetPlatformFile().CreateDirectoryTree(*FPaths::GetPath(IOSAttachmentPath));
    FFileHelper::SaveStringToFile(TEXT("iOS test attachment content"), *IOSAttachmentPath);
    
    CrashSightAgent::ReportException(5, "iOSReportAttachmentException", "TestAttachmentReason", "TestAttachmentStackTrace", "TestAttachmentExtras", false, 1, true, TCHAR_TO_UTF8(*IOSAttachmentPath));

#elif PLATFORM_WINDOWS
    // 创建Windows附件文件 - 使用项目根目录下的Saved目录
    FString WindowsAttachmentPath = FPaths::Combine(
        FPaths::ProjectDir(), 
        TEXT("Saved"),
        TEXT("test_attachment_windows.txt")
    );
    
    // 确保目录存在
    FPlatformFileManager::Get().GetPlatformFile().CreateDirectoryTree(*FPaths::GetPath(WindowsAttachmentPath));
    FFileHelper::SaveStringToFile(TEXT("Windows test attachment content"), *WindowsAttachmentPath);
    
    // 添加路径日志以便调试
    UE_LOG(LogTemp, Warning, TEXT("Attachment path: %s"), *WindowsAttachmentPath);
    GEngine->AddOnScreenDebugMessage(0, 1.0f, FColor::Red, FString::Printf(TEXT("Attachment path: %s"), *WindowsAttachmentPath));
    
    CrashSightAgent::ReportException(6, "WindowsReportAttachmentException", "TestAttachmentReason", "TestAttachmentStackTrace", "TestAttachmentExtras", false, 1, true, TCHAR_TO_UTF8(*WindowsAttachmentPath));
#endif
}

void CSTestAgent::TestReportException2()
{
    UE_LOG(LogTemp, Warning, TEXT("Report Exception2!"));
    GEngine->AddOnScreenDebugMessage(0, 1.0f, FColor::Red, TEXT("Report Exception2!"));

    // 定义不同的字符串数组用于生成变化的内容
    TArray<FString> ExceptionTypes = {
        TEXT("NullPointerException"), TEXT("OutOfMemoryError"), TEXT("NetworkTimeoutException"),
        TEXT("FileNotFoundException"), TEXT("SecurityException"), TEXT("IllegalArgumentException"),
        TEXT("RuntimeException"), TEXT("ClassCastException"), TEXT("IndexOutOfBoundsException"),
        TEXT("ConcurrentModificationException")
    };
    
    TArray<FString> ReasonPrefixes = {
        TEXT("UserAction"), TEXT("SystemError"), TEXT("NetworkFailure"), TEXT("DataCorruption"),
        TEXT("PermissionDenied"), TEXT("ResourceExhausted"), TEXT("ConfigurationError"),
        TEXT("ValidationFailed"), TEXT("TimeoutOccurred"), TEXT("UnexpectedBehavior")
    };
    
    TArray<FString> StackComponents = {
        TEXT("MainActivity"), TEXT("GameEngine"), TEXT("NetworkManager"), TEXT("FileHandler"),
        TEXT("UserInterface"), TEXT("DatabaseConnector"), TEXT("AudioSystem"), TEXT("GraphicsRenderer"),
        TEXT("InputController"), TEXT("MemoryManager")
    };
    
    TArray<FString> ExtraData = {
        TEXT("UserID_Alpha"), TEXT("SessionID_Beta"), TEXT("DeviceInfo_Gamma"), TEXT("AppVersion_Delta"),
        TEXT("Timestamp_Epsilon"), TEXT("Location_Zeta"), TEXT("Performance_Eta"), TEXT("Settings_Theta"),
        TEXT("Network_Iota"), TEXT("Battery_Kappa")
    };

    for (int i = 1; i <= 20; i++) {
       // 使用模运算来循环使用数组中的元素，并添加随机字符
        int typeIndex = (i - 1) % ExceptionTypes.Num();
        int reasonIndex = (i - 1) % ReasonPrefixes.Num();
        int stackIndex = (i - 1) % StackComponents.Num();
        int extraIndex = (i + 1) % ExtraData.Num();  // 隐秘bug: 这里应该是(i-1)而不是(i+1)，当i=19时会越界
        
        // 添加一些随机字符和特殊符号
        FString randomSuffix = FString::Printf(TEXT("_%c%c_%d"), 
            'A' + (i % 26), 'a' + ((i * 3) % 26), i);
        
        FString exceptionName = FString::Printf(TEXT("Android_%s%s"), 
            *ExceptionTypes[typeIndex], *randomSuffix);
        
        FString reason = FString::Printf(TEXT("%s_Reason_%c%c_%d"), 
            *ReasonPrefixes[reasonIndex], 'X' + (i % 3), 'Z' - (i % 3), i);
        
        FString stackTrace = FString::Printf(TEXT("StackTrace_%s_Line%d_%c%c"), 
            *StackComponents[stackIndex], i, '0' + (i % 10), 'A' + ((i * 7) % 26));
        
        FString extras = FString::Printf(TEXT("Extras_%s_%c%c%c_%d"), 
            *ExtraData[extraIndex], 
            'a' + (i % 26), 'A' + ((i * 2) % 26), '0' + ((i * 5) % 10), i);
        
            #if PLATFORM_ANDROID
            CrashSightAgent::ReportException(4, TCHAR_TO_UTF8(*exceptionName), TCHAR_TO_UTF8(*reason), TCHAR_TO_UTF8(*stackTrace), TCHAR_TO_UTF8(*extras), false, 1);
            #endif
        }
}

void CSTestAgent::TestPrintLog()
{
    int errorCode = 404;
    FString errorMsg = TEXT("Test %errorMsg Not F%oun%d");
#if PLATFORM_ANDROID
    CrashSightAgent::PrintLog(LogSeverity::LogError, "Error code: %d, message: %s %f %c %u", errorCode, TCHAR_TO_UTF8(*errorMsg)); 
#elif PLATFORM_IOS
    CrashSightAgent::PrintLog(LogSeverity::LogError, "Error code: %d, message: %s %f %c %u", errorCode, TCHAR_TO_UTF8(*errorMsg)); 
#elif PLATFORM_WINDOWS
    CrashSightAgent::PrintLog(LogSeverity::LogError, "Error code: %d, message: %s %f %c %u", errorCode, TCHAR_TO_UTF8(*errorMsg)); 
#endif
}

void CSTestAgent::TestSetEnvironment(){
    UE_LOG(LogTemp, Warning, TEXT("Set Environment!"));
    GEngine->AddOnScreenDebugMessage(0, 1.0f, FColor::Red, TEXT("Set Environment!"));
    CrashSightAgent::SetEnvironmentName("test9.19");
}


void CSTestAgent::TestPlatformCrash()
{
#if PLATFORM_ANDROID
    CrashSightAgent::TestJavaCrash();
#elif PLATFORM_IOS
    CrashSightAgent::TestOcCrash();
#elif PLATFORM_WINDOWS

#endif
}

void CSTestAgent::TestNativeCrash()
{
    CrashSightAgent::TestNativeCrash();
}



void CSTestAgent::TestOOMCrash()
{
#if PLATFORM_IOS || PLATFORM_ANDROID
    CrashSightAgent::TestOomCrash();
#endif
}

void CSTestAgent::TestANR()
{
#if PLATFORM_IOS || PLATFORM_ANDROID
    CrashSightAgent::TestANR();
#endif
}

 

#endif