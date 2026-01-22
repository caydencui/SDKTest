// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

// #define USE_CRASHSIGHT 





#ifdef USE_CRASHSIGHT

#include "CoreMinimal.h"

/**
 * 
 */
class CRASHSIGHTSDKTEST_API CSTestAgent
{
public:
	CSTestAgent();
	~CSTestAgent();

	//SetUserId  AddSceneData 
	//SetAppVersion (需要在InitWithAppId接口之前调用。)


	static void TestInitCS();


	//上报异常
	static void TestReportException();
	static void TestReportException2(); //测试积压情况下上报异常 （需要断网等条件）


	static void TestPrintLog();

	static void TestSetEnvironment();

	//CrashSight 测试接口，不同类型的Crash
	static void TestPlatformCrash();
	static void TestNativeCrash();
	static void TestOOMCrash();
	static void TestANR();
	// 暂时未测试TestUseAfterFree()
};


#endif
