// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/NoExportTypes.h"
#include "SDKObject.generated.h"

/**
 * 
 */
UCLASS(Blueprintable)
class CRASHSIGHTSDKTEST_API USDKObject : public UObject
{
	GENERATED_BODY()
	
	public:
	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSInit();

	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSTestReportException();

	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSTestReportException2();

	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSTestPrintLog();

	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSTestSetEnvironment();

	//CrashSight 测试接口，不同类型的Crash
	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSTestPlatformCrash();

	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSTestNativeCrash();

	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSTestOOMCrash();

	UFUNCTION(BlueprintCallable, Category = "CrashSight Functions")
	void CSTestANR();
	

};
