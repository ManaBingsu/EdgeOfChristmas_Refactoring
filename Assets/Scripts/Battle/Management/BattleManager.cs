using BackEnd.Tcp;
using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance;

        private Dictionary<SessionId, Player> players;

        [SerializeField]
        private GameObject playerPrefab;

        private SessionId myPlayerIndex = SessionId.None;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this.gameObject);
        }

        private void Start()
        {
            players = new Dictionary<SessionId, Player>();
            var gamers = BackEndMatchManager.GetInstance().sessionIdList;
            BackEndMatchManager.GetInstance().SetPlayerSessionList(gamers);

            SetPlayerInfo();
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
                GameObject player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

                players.Add(sessionId, player.GetComponent<Player>());

                if (BackEndMatchManager.GetInstance().IsMySessionId(sessionId))
                {
                    myPlayerIndex = sessionId;
                    //players[sessionId].Initialize(true, myPlayerIndex, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w);
                }
                else
                {
                    //players[sessionId].Initialize(false, sessionId, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w);
                }
                index += 1;
            }
            Debug.Log("Num Of Current Player : " + size);
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
                    Debug.Log("키 명령을 받음!");
                    ProcessKeyEvent(args.From.SessionId, keyMessage);
                    break;
                case Protocol.Type.PlayerMove:
                    PlayerMoveMessage moveMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                    Debug.Log("움직이라는 명령을 받음!");
                    ProcessPlayerData(moveMessage);
                    break;
                case Protocol.Type.PlayerAttack:
                    PlayerAttackMessage attackMessage = DataParser.ReadJsonData<PlayerAttackMessage>(args.BinaryUserData);
                    //ProcessPlayerData(attackMessage);
                    break;
                case Protocol.Type.PlayerDamaged:
                    PlayerDamegedMessage damegedMessage = DataParser.ReadJsonData<PlayerDamegedMessage>(args.BinaryUserData);
                    //ProcessPlayerData(damegedMessage);
                    break;
                case Protocol.Type.PlayerNoMove:
                    PlayerNoMoveMessage noMoveMessage = DataParser.ReadJsonData<PlayerNoMoveMessage>(args.BinaryUserData);
                    //ProcessPlayerData(noMoveMessage);
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
            Vector3 moveVecotr = new Vector3(data.xDir, data.yDir, data.zDir);
            // moveVector가 같으면 방향 & 이동량 같으므로 적용 굳이 안함
            if (!moveVecotr.Equals(players[data.playerSession].moveVector))
            {
                players[data.playerSession].SetPosition(data.xPos, data.yPos, data.zPos);
                players[data.playerSession].SetMoveVector(moveVecotr);
            }
        }

        private void ProcessKeyEvent(SessionId index, KeyMessage keyMessage)
        {
            if (BackEndMatchManager.GetInstance().IsHost() == false)
            {
                //호스트만 수행
                return;
            }
            bool isMove = false;
            bool isAttack = false;
            bool isNoMove = false;

            int keyData = keyMessage.keyData;

            Vector3 moveVecotr = Vector3.zero;
            Vector3 attackPos = Vector3.zero;
            Vector3 playerPos = players[index].GetPosition();
            if ((keyData & KeyEventCode.MOVE) == KeyEventCode.MOVE)
            {
                moveVecotr = new Vector3(keyMessage.x, keyMessage.y, keyMessage.z);
                moveVecotr = Vector3.Normalize(moveVecotr);
                isMove = true;
            }
            /*
            if ((keyData & KeyEventCode.ATTACK) == KeyEventCode.ATTACK)
            {
                attackPos = new Vector3(keyMessage.x, keyMessage.y, keyMessage.z);
                players[index].Attack(attackPos);
                isAttack = true;
            }
            */
            if ((keyData & KeyEventCode.NO_MOVE) == KeyEventCode.NO_MOVE)
            {
                isNoMove = true;
            }
            
            if (isMove)
            {
                players[index].SetMoveVector(moveVecotr);
                PlayerMoveMessage msg = new PlayerMoveMessage(index, playerPos, moveVecotr);
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerMoveMessage>(msg);
            }
            if (isNoMove)
            {
                PlayerNoMoveMessage msg = new PlayerNoMoveMessage(index, playerPos);
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerNoMoveMessage>(msg);
            }
            if (isAttack)
            {
                PlayerAttackMessage msg = new PlayerAttackMessage(index, attackPos);
                BackEndMatchManager.GetInstance().SendDataToInGame<PlayerAttackMessage>(msg);
            }
        }
    }
}

