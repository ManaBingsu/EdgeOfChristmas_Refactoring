using BackEnd.Tcp;
using Protocol;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

namespace Battle
{
    public partial class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;

        public Dictionary<SessionId, Player> players;

        [SerializeField]
        private GameObject playerPrefab;

        public SessionId myPlayerIndex = SessionId.None;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this.gameObject);

            InitStateEvents();
            RegistEvent();
        }

        private void Start()
        {
            //players = new Dictionary<SessionId, Player>();
            if (BackEndMatchManager.GetInstance() != null)
            {
                var gamers = BackEndMatchManager.GetInstance().sessionIdList;
                BackEndMatchManager.GetInstance().SetPlayerSessionList(gamers);
                gameRecord = new Stack<SessionId>();
                SetPlayerInfo();
            }

            StartCoroutine(StartTimer());
        }

        private IEnumerator StartTimer()
        {
            float time = 0f;
            while (time <= 1f)
            {
                time += Time.deltaTime;
                yield return null;
            }
            FlowState = EFlowState.Start;
        }

        public void SetPlayerInfo()
        {
            if (BackEndMatchManager.GetInstance().sessionIdList == null)
            {
                // 현재 세션ID 리스트가 존재하지 않으면, 0.5초 후 다시 실행
                Invoke("SetPlayerInfo", 0.5f);
                return;
            }
            var gamers = BackEndMatchManager.GetInstance().sessionIdList;
            int size = gamers.Count;
            if (size <= 0)
            {
                Debug.Log("No Player Exist!");
                return;
            }
            /*if (size > MAXPLAYER)
            {
                Debug.Log("Player Pool Exceed!");
                return;
            }*/

            players = new Dictionary<SessionId, Player>();
            BackEndMatchManager.GetInstance().SetPlayerSessionList(gamers);

            int index = 0;
            foreach (var sessionId in gamers)
            {
                Player player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Player>();

                players.Add(sessionId, player);

                player.sessionId = sessionId;

                if (BackEndMatchManager.GetInstance().IsMySessionId(sessionId))
                {
                    myPlayerIndex = sessionId;
                    players[sessionId].IsMyPlayer = true;
                    //players[sessionId].Initialize(true, myPlayerIndex, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w);
                }
                else
                {
                    //players[sessionId].Initialize(false, sessionId, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w);
                }
                index += 1;
            }
            Debug.Log("Num Of Current Player : " + size);
            Debug.Log("Num of players : " + players.Count);
            /*
            // 스코어 보드 설정
            alivePlayer = size;
            InGameUiManager.GetInstance().SetScoreBoard(alivePlayer);

            if (BackEndMatchManager.GetInstance().IsHost())
            {
                StartCoroutine("StartCount");
            }*/
        }

        public void OnRecieve(MatchRelayEventArgs args)
        {
            if (args.BinaryUserData == null)
            {
                Debug.LogWarning(string.Format("빈 데이터가 브로드캐스팅 되었습니다.\n{0} - {1}", args.From, args.ErrInfo));
                // 데이터가 없으면 그냥 리턴
                return;
            }
            Message msg = DataParser.ReadJsonData<Message>(args.BinaryUserData);
            if (msg == null)
            {
                return;
            }
            if (BackEndMatchManager.GetInstance().IsHost() != true && args.From.SessionId == myPlayerIndex)
            {
                return;
            }
            if (players == null)
            {
                Debug.LogError("Players 정보가 존재하지 않습니다.");
                return;
            }
            switch (msg.type)
            {
                case Protocol.Type.StartCount:
                    //StartCountMessage startCount = DataParser.ReadJsonData<StartCountMessage>(args.BinaryUserData);
                    //Debug.Log("wait second : " + (startCount.time));
                    //InGameUiManager.GetInstance().SetStartCount(startCount.time);
                    break;
                case Protocol.Type.GameStart:
                    //InGameUiManager.GetInstance().SetStartCount(0, false);
                    //GameManager.GetInstance().ChangeState(GameManager.GameState.InGame);
                    break;
                case Protocol.Type.GameEnd:
                    //GameEndMessage endMessage = DataParser.ReadJsonData<GameEndMessage>(args.BinaryUserData);
                    //SetGameRecord(endMessage.count, endMessage.sessionList);
                    //GameManager.GetInstance().ChangeState(GameManager.GameState.Over);
                    break;

                case Protocol.Type.Key:
                    KeyMessage keyMessage = DataParser.ReadJsonData<KeyMessage>(args.BinaryUserData);
                    ProcessKeyEvent(args.From.SessionId, keyMessage);
                    break;
                case Protocol.Type.PlayerMove:
                    PlayerMoveMessage moveMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                    ProcessPlayerData(moveMessage);
                    break;
                case Protocol.Type.PlayerJump:
                    PlayerJumpMessage playerJumpMessage = DataParser.ReadJsonData<PlayerJumpMessage>(args.BinaryUserData);
                    ProcessPlayerData(playerJumpMessage);
                    break;
                case Protocol.Type.PlayerUseItem:
                    PlayerUseItemMessage useItemMessage = DataParser.ReadJsonData<PlayerUseItemMessage>(args.BinaryUserData);
                    ProcessPlayerData(useItemMessage);
                    break;
                case Protocol.Type.PlayerGetItem:
                    PlayerGetItemMessage playerGetItemMessage = DataParser.ReadJsonData<PlayerGetItemMessage>(args.BinaryUserData);
                    ProcessGetItemEvent(playerGetItemMessage);
                    break;
                case Protocol.Type.PlayerUseSkill:
                    PlayerUseSkillMessage useSkillMessage = DataParser.ReadJsonData<PlayerUseSkillMessage>(args.BinaryUserData);
                    //ProcessPlayerData(damegedMessage);
                    break;
                case Protocol.Type.PlayerNoMove:
                    PlayerNoMoveMessage noMoveMessage = DataParser.ReadJsonData<PlayerNoMoveMessage>(args.BinaryUserData);
                    //ProcessPlayerData(noMoveMessage);
                    break;
                case Protocol.Type.SpawnItem:
                    SpawnItemMessage spawnItemMessage = DataParser.ReadJsonData<SpawnItemMessage>(args.BinaryUserData);
                    ProcessPlayerData(spawnItemMessage);
                    break;
                case Protocol.Type.GameSync:
                    GameSyncMessage syncMessage = DataParser.ReadJsonData<GameSyncMessage>(args.BinaryUserData);
                    ProcessSyncData(syncMessage);
                    break;

                default:
                    Debug.Log("Unknown protocol type");
                    return;
            }
        }

        public void OnRecieveForLocal(KeyMessage keyMessage)
        {
            ProcessKeyEvent(myPlayerIndex, keyMessage);
        }

        private void ProcessSyncData(GameSyncMessage syncMessage)
        {/*
            // 플레이어 데이터 동기화
            int index = 0;
            if (players == null)
            {
                Debug.LogError("Player Poll is null!");
                return;
            }
            foreach (var player in players)
            {
                var y = player.Value.GetPosition().y;
                player.Value.SetPosition(new Vector3(syncMessage.xPos[index], y, syncMessage.zPos[index]));
                player.Value.SetHP(syncMessage.hpValue[index]);
                index++;
            }*/
            BackEndMatchManager.GetInstance().SetHostSession(syncMessage.host);
        }

        private void ProcessPlayerData(PlayerMoveMessage data)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == true)
            {
                //호스트면 리턴
                return;
            }
            int xDir = data.xDir;
            // moveVector가 같으면 방향 & 이동량 같으므로 적용 굳이 안함
            if (true/*players[data.playerSession].GoalDirection != xDir*/)
            {
                players[data.playerSession].SetPosition(data.xPos, data.yPos, 0);
                players[data.playerSession].SetMoveVector(xDir);
            }
        }

        private void ProcessPlayerData(PlayerUseItemMessage data)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == true)
            {
                //호스트면 리턴
                return;
            }
            players[data.playerSession].UseItem();
        }

        private void ProcessPlayerData(PlayerGetItemMessage data)
        {

        }

        private void ProcessPlayerData(PlayerJumpMessage data)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == true)
            {
                //호스트면 리턴
                return;
            }
            players[data.playerSession].Jump();
        }

        private void ProcessPlayerData(PlayerNoMoveMessage data)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == true)
            {
                //호스트면 리턴
                return;
            }
            players[data.playerSession].Jump();
        }
        private void ProcessPlayerData(SpawnItemMessage data)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == true)
            {
                //호스트면 리턴
                return;
            }
            ItemManager.Instance.SpawnItem(data.itemIndex, data.xPos, data.speed, data.rotate);
        }

        public void ProcessGetItemEvent(PlayerGetItemMessage data)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == true)
            {
                //호스트면 리턴
                return;
            }
            players[data.playerSession].GetItem(data.itemIndex);
        }

        public void ProcessSpawnEvent(SpawnItemMessage spawnItemMessage)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == false)
            {
                //호스트만 수행
                return;
            }
            // Item 소환
            ItemManager.Instance.SpawnItem(spawnItemMessage.itemIndex, spawnItemMessage.xPos, spawnItemMessage.speed, spawnItemMessage.rotate);
            BackEndMatchManager.GetInstance().SendDataToInGame<SpawnItemMessage>(spawnItemMessage);
        }

        private void ProcessKeyEvent(SessionId index, KeyMessage keyMessage)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == false)
            {
                //호스트만 수행
                return;
            }
            bool isMove = false;
            bool isJump = false;
            bool isUseItem = false;
            bool isUseSkill = false;
            bool isNoMove = false;

            int keyData = keyMessage.keyData;

            int xDir = 0;
            Vector3 playerPos = players[index].GetPosition();
            if ((keyData & KeyEventCode.MOVE) == KeyEventCode.MOVE)
            {
                xDir = (int)keyMessage.x;
                isMove = true;
            }
            if ((keyData & KeyEventCode.JUMP) == KeyEventCode.JUMP)
            {
                isJump = true;
            }
            if ((keyData & KeyEventCode.USEITEM) == KeyEventCode.USEITEM)
            {
                isUseItem = true;
            }
            if ((keyData & KeyEventCode.USESKILL) == KeyEventCode.USESKILL)
            {
                isUseSkill = true;
            }
            if ((keyData & KeyEventCode.NO_MOVE) == KeyEventCode.NO_MOVE)
            {
                isNoMove = true;
            }
            
            if (isMove)
            {
                Vector3 moveVector = players[index].GetPosition();
                players[index].SetPosition(moveVector.x, moveVector.y, 0);
                players[index].SetMoveVector(xDir);

                PlayerMoveMessage msg = new PlayerMoveMessage(index, players[index].GetPosition(), xDir);
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerMoveMessage>(msg);
            }
            if (isJump)
            {
                players[index].Jump();

                PlayerJumpMessage msg = new PlayerJumpMessage(index);
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerJumpMessage>(msg);
            }
            if (isUseItem)
            {
                players[index].UseItem();

                PlayerUseItemMessage msg = new PlayerUseItemMessage(index);
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerUseItemMessage>(msg);
            }
            if (isUseSkill)
            {
                PlayerUseSkillMessage msg = new PlayerUseSkillMessage(index);
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerUseSkillMessage>(msg);
            }

            if (isNoMove)
            {
                players[index].SetMoveVector(xDir);

                PlayerMoveMessage msg = new PlayerMoveMessage(index, players[index].GetPosition(), keyMessage.keyData);
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerMoveMessage>(msg);
            }
        }
    }
}

