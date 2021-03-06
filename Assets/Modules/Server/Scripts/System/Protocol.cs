﻿using BackEnd.Tcp;
using UnityEngine;
using System.Collections.Generic;
using GameSystem;

namespace Protocol
{
    // 이벤트 타입
    public enum Type : sbyte
    {
        Key = 0,        // 키(가상 조이스틱) 입력
        PlayerMove,     // 플레이어 이동
        PlayerJump,     // 플레이어 점프
        PlayerGetItem,
        PlayerUseItem,   // 플레이어 아이템 사용
        PlayerGetScore,
        PlayerUseSkill,  // 플레이어 스킬 사용
        PlayerNoMove,   // 플레이어 이동 멈춤

        SpawnItem,

        AIPlayerInfo,   // AI가 존재하는 경우 AI 정보
        LoadRoomScene,      // 룸 씬으로 전환
        LoadGameScene,      // 인게임 씬으로 전환
        StartCount,     // 시작 카운트
        GameStart,      // 게임 시작
        GameEnd,        // 게임 종료
        GameSync,       // 플레이어 재접속 시 게임 현재 상황 싱크
        Max
    }

    // 조이스틱 키 이벤트 코드
    public static class KeyEventCode
    {
        public const int NONE = 0;
        public const int MOVE = 1;      // 이동 메시지
        public const int JUMP = 2;      // 점프 메시지
        public const int USEITEM = 3;    // 아이템 사용 메시지
        public const int USESKILL = 4;      // 스킬 사용 메시지
        public const int NO_MOVE = 5;   // 이동 멈춤 메시지
    }

    public class Message
    {
        public Type type;

        public Message(Type type)
        {
            this.type = type;
        }
    }

    public class KeyMessage : Message
    {
        public int keyData;
        public float x;
        public float y;
        public float z;

        public KeyMessage(int data, Vector3 pos) : base(Type.Key)
        {
            this.keyData = data;
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }
    }

    public class PlayerMoveMessage : Message
    {
        public SessionId playerSession;
        public float xPos;
        public float yPos;

        public int xDir;

        public float speed;

        public PlayerMoveMessage(SessionId session, Vector2 pos, int xDir, float speed) : base(Type.PlayerMove)
        {
            this.playerSession = session;
            this.xPos = pos.x;
            this.yPos = pos.y;
            this.xDir = xDir;
            this.speed = speed;
        }
    }

    public class PlayerJumpMessage : Message
    {
        public SessionId playerSession;

        public PlayerJumpMessage(SessionId session) : base(Type.PlayerJump)
        {
            this.playerSession = session;
        }
    }

    public class PlayerUseItemMessage : Message
    {
        public SessionId playerSession;

        public PlayerUseItemMessage(SessionId session) : base(Type.PlayerUseItem)
        {
            this.playerSession = session;
        }
    }

    public class PlayerGetItemMessage : Message
    {
        public SessionId playerSession;
        public int itemIndex;
        public PlayerGetItemMessage(SessionId session, int itemIndex) : base(Type.PlayerGetItem)
        {
            this.playerSession = session;
            this.itemIndex = itemIndex;
        }
    }

    public class PlayerUseSkillMessage : Message
    {
        public SessionId playerSession;
        public PlayerUseSkillMessage(SessionId session) : base(Type.PlayerUseSkill)
        {
            this.playerSession = session;
        }
    }

    public class PlayerGetScoreMessage : Message
    {
        public SessionId playerSession;
        public PlayerGetScoreMessage(SessionId session) : base(Type.PlayerGetScore)
        {
            this.playerSession = session;
        }
    }

    public class PlayerNoMoveMessage : Message
    {
        public SessionId playerSession;
        public float xPos;
        public float yPos;
        public PlayerNoMoveMessage(SessionId session, Vector2 pos) : base(Type.PlayerNoMove)
        {
            this.playerSession = session;
            this.xPos = pos.x;
            this.yPos = pos.y;
        }
    }

    public class SpawnItemMessage : Message
    {
        public int itemIndex;
        public float xPos;
        public float speed;
        public float rotate;

        public SpawnItemMessage(int itemIndex, float xPos, float speed, float rotate) : base(Type.SpawnItem)
        {
            this.itemIndex = itemIndex;
            this.xPos = xPos;
            this.speed = speed;
            this.rotate = rotate;
        }
    }

    public class AIPlayerInfo : Message
    {
        public SessionId m_sessionId;
        public string m_nickname;
        public byte m_teamNumber;
        public int m_numberOfMatches;
        public int m_numberOfWin;
        public int m_numberOfDraw;
        public int m_numberOfDefeats;
        public int m_points;
        public int m_mmr;

        public AIPlayerInfo(MatchUserGameRecord gameRecord) : base(Type.AIPlayerInfo)
        {
            this.m_sessionId = gameRecord.m_sessionId;
            this.m_nickname = gameRecord.m_nickname;
            this.m_teamNumber = gameRecord.m_teamNumber;
            this.m_numberOfWin = gameRecord.m_numberOfWin;
            this.m_numberOfDraw = gameRecord.m_numberOfDraw;
            this.m_numberOfDefeats = gameRecord.m_numberOfDefeats;
            this.m_points = gameRecord.m_points;
            this.m_mmr = gameRecord.m_mmr;
            this.m_numberOfMatches = gameRecord.m_numberOfMatches;
        }

        public MatchUserGameRecord GetMatchRecord()
        {
            MatchUserGameRecord gameRecord = new MatchUserGameRecord();
            gameRecord.m_sessionId = this.m_sessionId;
            gameRecord.m_nickname = this.m_nickname;
            gameRecord.m_numberOfMatches = this.m_numberOfMatches;
            gameRecord.m_numberOfWin = this.m_numberOfWin;
            gameRecord.m_numberOfDraw = this.m_numberOfDraw;
            gameRecord.m_numberOfDefeats = this.m_numberOfDefeats;
            gameRecord.m_mmr = this.m_mmr;
            gameRecord.m_points = this.m_points;
            gameRecord.m_teamNumber = this.m_teamNumber;

            return gameRecord;
        }
    }

    public class LoadRoomSceneMessage : Message
    {
        public LoadRoomSceneMessage() : base(Type.LoadRoomScene)
        {

        }
    }

    public class LoadGameSceneMessage : Message
    {
        public LoadGameSceneMessage() : base(Type.LoadGameScene)
        {
            // SceneManager.Instance.LoadScene(SceneManager.Instance.ingameSceneName);
        }
    }

    public class StartCountMessage : Message
    {
        public int time;
        public StartCountMessage(int time) : base(Type.StartCount)
        {
            this.time = time;
        }
    }

    public class GameStartMessage : Message
    {
        public GameStartMessage() : base(Type.GameStart) { }
    }

    public class GameEndMessage : Message
    {
        public int count;
        public int[] sessionList;
        public GameEndMessage(Stack<SessionId> result) : base(Type.GameEnd)
        {
            count = result.Count;
            sessionList = new int[count];
            for (int i = 0; i < count; ++i)
            {
                sessionList[i] = (int)result.Pop();
            }
        }
    }

    public class GameSyncMessage : Message
    {
        public SessionId host;
        public int count = 0;
        public float[] xPos = null;
        public float[] zPos = null;
        public int[] hpValue = null;
        public bool[] onlineInfo = null;

        public GameSyncMessage(SessionId host, int count, float[] x, float[] z, int[] hp, bool[] online) : base(Type.GameSync)
        {
            this.host = host;
            this.count = count;
            this.xPos = new float[count];
            this.zPos = new float[count];
            this.hpValue = new int[count];
            this.onlineInfo = new bool[count];

            for (int i = 0; i < count; ++i)
            {
                xPos[i] = x[i];
                zPos[i] = z[i];
                hpValue[i] = hp[i];
                onlineInfo[i] = online[i];
            }
        }
    }
}
