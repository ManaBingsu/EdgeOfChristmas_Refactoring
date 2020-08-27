﻿using System;
using System.Collections.Generic;
using UnityEngine;
// Include Backend
using BackEnd;
using static BackEnd.BackendAsyncClass;
//  Include GPGS namespace
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using Debug = Helper.Debug;
#if UNITY_IOS
using UnityEngine.SignInWithApple;
#endif
/*
 * Backend-4.2.1
 * 뒤끝의 기본 기능을 정의
 * 뒤끝 초기화
 * 커스텀 회원가입
 * 커스텀 로그인
 * 유저정보 불러오기
 */
public class BackEndServerManager : MonoBehaviour
{

    private static BackEndServerManager instance;   // 인스턴스
    public bool isLogin { get; private set; }   // 로그인 여부

    private bool isSuccess = false;                     // 로그인 성공 여부 (true이면 로그인 성공한 것)
    private bool isUpdateNickName = true;              // 닉네임을 설정해야 되는지 여부(true이면 닉네임 설정해야됨)
    private string tempNickName;                        // 설정할 닉네임 (id와 동일)
    public string myNickName { get; private set; } = string.Empty;  // 로그인한 계정의 닉네임
    public string myIndate { get; private set; } = string.Empty;    // 로그인한 계정의 inDate

    private BackendReturnObject tokenInfo = null;
    private Action<bool, string> loginSuccessFunc = null;

    private const string BackendError = "statusCode : {0}\nErrorCode : {1}\nMessage : {2}";

    public string appleToken = ""; // SignInWithApple.cs에서 토큰값을 받을 문자열
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        // 모든 씬에서 유지
        DontDestroyOnLoad(this.gameObject);
    }

    public static BackEndServerManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("BackEndServerManager 인스턴스가 존재하지 않습니다.");
            return null;
        }

        return instance;
    }

    /*
	 * 서버 초기화
	 */
    void Start()
    {
#if UNITY_ANDROID
/*        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestIdToken()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();*/
#endif
        isLogin = false;
        try
        {
            Backend.Initialize(() =>
            {
                if (Backend.IsInitialized)
                {
#if UNITY_ANDROID
                    Debug.Log("GoogleHash - " + Backend.Utils.GetGoogleHash());
#endif

                    // 비동기 함수 큐 초기화
                    if (backendAsyncQueueState != BackendAsyncQueueState.Running)
                    {
                        var tmp = InitializeAsyncQueue(AsyncQueueExceptionEvent);
                        Debug.Log("InitializeAsyncQueue : " + tmp);
                    }
                }
                else
                {
                    Debug.Log("뒤끝 초기화 실패");
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log("[예외]뒤끝 초기화 실패\n" + e.ToString());
        }
    }

    // 비동기 큐 쓰레드 에러 이벤트 핸들러
    void AsyncQueueExceptionEvent(string errorMsg)
    {
        Debug.Log(errorMsg);
        // 에러가 발생하고, 비동기 큐가 멈췄으면 다시 시작
        if (backendAsyncQueueState == BackendAsyncQueueState.Stopped)
        {
            var tmp = StartAsyncQueueThread();
            Debug.Log("AsyncThreadRestart Result : " + tmp);
        }
    }

    // 게임 종료, 에디터 종료 시 호출
    // 비동기 큐 쓰레드를 중지시킴
    // 해당 함수는 실제 안드로이드, iOS 환경에서 호출이 안될 수도 있다 (각 os의 특징 때문)
    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        if (backendAsyncQueueState == BackendAsyncQueueState.Running)
        {
            try
            {
                StopAsyncQueueThread();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

        }
    }

    // 게임 시작, 게임 종료, 백그라운드로 돌아갈 때(홈버튼 누를 때) 호출됨
    // 위의 종료함수와는 달리 무조건 호출됨
    // 비동기 큐 종료, 재시작
    void OnApplicationPause(bool isPause)
    {
        Debug.Log("OnApplicationPause : " + isPause);
        if (isPause == false)
        {
            if (backendAsyncQueueState == BackendAsyncQueueState.Stopped)
            {
                StartAsyncQueueThread();
            }
        }
        else
        {
            if (backendAsyncQueueState == BackendAsyncQueueState.Running)
            {
                StopAsyncQueueThread();
            }
        }
    }

    // 뒤끝 토큰으로 로그인
    public void BackendTokenLogin(Action<bool, string> func)
    {
        BackendAsyncClass.BackendAsync(Backend.BMember.LoginWithTheBackendTokenAsync, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("토큰 로그인 성공");
                tokenInfo = callback;
                isSuccess = true;
                isUpdateNickName = false;
                loginSuccessFunc = func;
                return;
            }

            Debug.Log("토큰 로그인 실패\n" + callback.ToString());
            func(false, string.Empty);
        });
    }

    // 커스텀 로그인
    public void CustomLogin(string id, string pw, Action<bool, string> func)
    {
        BackendAsyncClass.BackendAsync(Backend.BMember.CustomLoginAsync, id, pw, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("커스텀 로그인 성공");
                tokenInfo = callback;
                isSuccess = true;
                isUpdateNickName = false;
                loginSuccessFunc = func;
                return;
            }

            Debug.Log("커스텀 로그인 실패\n" + callback);
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

    // 커스텀 회원가입
    public void CustomSignIn(string id, string pw, Action<bool, string> func)
    {
        tempNickName = id;
        BackendAsyncClass.BackendAsync(Backend.BMember.CustomSignUpAsync, id, pw, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("커스텀 회원가입 성공");
                tokenInfo = callback;
                isSuccess = true;
                isUpdateNickName = true;
                loginSuccessFunc = func;
                return;
            }

            Debug.LogError("커스텀 회원가입 실패\n" + callback.ToString());
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

    // 닉네임 설정
    public void RegistNickname(string nickname, Action<bool, string> func)
    {
        BackendAsyncClass.BackendAsync(Backend.BMember.UpdateNickname, nickname, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("닉네임 생성 성공");
                isSuccess = true;
                isUpdateNickName = false;
                return;
            }

            Debug.LogError("닉네임 생성 실패\n" + callback.ToString());
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

    /*
    // 구글 페더레이션 로그인/회원가입
    public void GoogleAuthorizeFederation(Action<bool, string> func)
    {
#if UNITY_ANDROID
        // 이미 gpgs 로그인이 된 경우
        if (Social.localUser.authenticated == true)
        {
            var token = GetFederationToken();
            if (token.Equals(string.Empty))
            {
                Debug.LogError("GPGS 토큰이 존재하지 않습니다.");
                func(false, "GPGS 인증을 실패하였습니다.\nGPGS 토큰이 존재하지 않습니다.");
                return;
            }

            BackendAsyncClass.BackendAsync(Backend.BMember.AuthorizeFederationAsync, token, FederationType.Google, "gpgs 인증", callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("GPGS 인증 성공");
                    tokenInfo = callback;
                    isSuccess = true;
                    isUpdateNickName = false;
                    loginSuccessFunc = func;
                    return;
                }

                Debug.LogError("GPGS 인증 실패\n" + callback.ToString());
                func(false, string.Format(BackendError,
                    callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
            });
        }
        // gpgs 로그인을 해야하는 경우
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    var token = GetFederationToken();
                    if (token.Equals(string.Empty))
                    {
                        Debug.LogError("GPGS 토큰이 존재하지 않습니다.");
                        func(false, "GPGS 인증을 실패하였습니다.\nGPGS 토큰이 존재하지 않습니다.");
                        return;
                    }

                    BackendAsyncClass.BackendAsync(Backend.BMember.AuthorizeFederationAsync, token, FederationType.Google, "gpgs 인증", callback =>
                    {
                        if (callback.IsSuccess())
                        {
                            Debug.Log("GPGS 인증 성공");
                            tokenInfo = callback;
                            isSuccess = true;
                            isUpdateNickName = false;
                            loginSuccessFunc = func;
                            return;
                        }

                        Debug.LogError("GPGS 인증 실패\n" + callback.ToString());
                        func(false, string.Format(BackendError,
                            callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
                    });
                }
                else
                {
                    Debug.LogError("GPGS 로그인 실패");
                    func(false, "GPGS 인증을 실패하였습니다.\nGPGS 로그인을 실패하였습니다.");
                    return;
                }
            });
        }
#endif
    }
    
    // 애플 페더레이션 로그인/회원가입
    public void AppleAuthorizeFederation(Action<bool, string> func)
    {
#if UNITY_IOS
        loginSuccessFunc = func;
        var siwa = gameObject.GetComponent<SignInWithApple>();
        siwa.Login(AppleFedeCallback);
#endif
    }

#if UNITY_IOS
    private void AppleFedeCallback(SignInWithApple.CallbackArgs args)
    {
        Debug.Log("애플 토큰으로 뒤끝에 로그인");
        BackendAsyncClass.BackendAsync(Backend.BMember.AuthorizeFederationAsync, appleToken, FederationType.Apple, "apple 인증", callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("Apple 인증 성공");
                tokenInfo = callback;
                isSuccess = true;
                isUpdateNickName = false;

                return;
            }

            Debug.LogError("Apple 인증 실패\n" + callback.ToString());
            loginSuccessFunc(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });

    }
#endif

    private string GetFederationToken()
    {
#if UNITY_ANDROID
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Debug.LogError("GPGS에 접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return string.Empty;
        }
        // 유저 토큰 받기
        string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
        tempNickName = PlayGamesPlatform.Instance.GetUserDisplayName();
        Debug.Log(tempNickName);
        return _IDtoken;

#elif UNITY_IOS
        return string.Empty;
#else
        return string.Empty;
#endif
    }

    public void UpdateNickname(string nickname, Action<bool, string> func)
    {
        BackendAsyncEnqueue(Backend.BMember.UpdateNickname, nickname, bro =>
        {
            // 닉네임이 없으면 매치서버 접속이 안됨
            if (!bro.IsSuccess())
            {
                Debug.LogError("닉네임 생성 실패\n" + bro.ToString());
                func(false, string.Format(BackendError,
                    bro.GetStatusCode(), bro.GetErrorCode(), bro.GetMessage()));
                return;
            }
            loginSuccessFunc = func;
            OnBackendAuthorized();
        });
    }*/

    // 유저 정보 불러오기 사전작업
    private void OnPrevBackendAuthorized()
    {
        isLogin = true;
        isSuccess = false;
        // 토큰정보 저장
        if (tokenInfo == null)
        {
            Debug.LogError("토큰 정보가 존재하지 않습니다.");
            return;
        }
        var callback = Backend.BMember.SaveToken(tokenInfo);
        if (!callback.IsSuccess())
        {
            Debug.LogError("토큰정보 저장 실패");
            return;
        }
        // 닉네임 업데이트
        if (isUpdateNickName)
        {
            isUpdateNickName = false;
            BackendAsyncEnqueue(Backend.BMember.UpdateNickname, tempNickName, bro =>
            {
                // 닉네임이 없으면 매치서버 접속이 안됨
                if (!bro.IsSuccess())
                {
                    Debug.LogError("닉네임 생성 실패\n" + bro.ToString());
                    return;
                }
                OnBackendAuthorized();
            });
            return;
        }

        OnBackendAuthorized();
    }

    // 실제 유저 정보 불러오기
    private void OnBackendAuthorized()
    {
        BackendAsyncEnqueue(Backend.BMember.GetUserInfo, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("유저 정보 불러오기 실패\n" + callback);
                loginSuccessFunc(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
                return;
            }
            Debug.Log("유저정보\n" + callback);

            var info = callback.GetReturnValuetoJSON()["row"];
            if (info["nickname"] == null)
            {
                //LoginUI.GetInstance().ActiveNickNameObject();
                return;
            }
            myNickName = info["nickname"].ToString();
            myIndate = info["inDate"].ToString();

            if (loginSuccessFunc != null)
            {
                loginSuccessFunc(true, string.Empty);
            }
        });
    }

    void Update()
    {
        // 비동기 함수를 사용하므로 회원가입/로그인 성공 시 update 문에서 호출
        if (isSuccess)
        {
            OnPrevBackendAuthorized();
        }
    }
}