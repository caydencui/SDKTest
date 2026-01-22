// Fill out your copyright notice in the Description page of Project Settings.


#include "SDKObject.h"

// #define USE_CRASHSIGHT

#ifdef USE_CRASHSIGHT

#endif


#ifdef USE_CRASHSIGHT
#include "CSTestAgent.h"
#endif

void USDKObject::CSInit()
{
#ifdef USE_CRASHSIGHT
    UE_LOG(LogTemp, Warning, TEXT("SDKObject Init CrashSight!"));
    GEngine->AddOnScreenDebugMessage(0, 1.0f, FColor::Red, TEXT("SDKObject Init CrashSight!"));
    CSTestAgent::TestInitCS();
#endif
}

void USDKObject::CSTestReportException()
{
#ifdef USE_CRASHSIGHT
    CSTestAgent::TestReportException();
#endif

}

void USDKObject::CSTestReportException2()
{
#ifdef USE_CRASHSIGHT
    CSTestAgent::TestReportException2();
#endif
}

void USDKObject::CSTestPrintLog()
{
#ifdef USE_CRASHSIGHT
    CSTestAgent::TestPrintLog();
#endif
}

void USDKObject::CSTestSetEnvironment()
{
#ifdef USE_CRASHSIGHT
    CSTestAgent::TestSetEnvironment();
#endif
}

void USDKObject::CSTestPlatformCrash()
{
#ifdef USE_CRASHSIGHT
    CSTestAgent::TestPlatformCrash();
#endif
}

void USDKObject::CSTestNativeCrash()
{
#ifdef USE_CRASHSIGHT
    CSTestAgent::TestNativeCrash();
#endif
}

void USDKObject::CSTestOOMCrash()
{
#ifdef USE_CRASHSIGHT
    CSTestAgent::TestOOMCrash();
#endif
}

void USDKObject::CSTestANR()
{
#ifdef USE_CRASHSIGHT
    CSTestAgent::TestANR();
#endif
}
