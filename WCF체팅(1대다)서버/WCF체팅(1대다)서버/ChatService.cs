using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF체팅_1대다_서버
{
    class ChatService : IChat
    {
        public delegate void Chat(int idx, string msg, string type);
        //동기화 작업을 위해서 가상의 객체 생성
        private static Object syncObj = new Object();
        //채팅방에 있는 유저 이름 목록
        private static ArrayList Chatter = new ArrayList();
        //델리게이트 =========================================================
        // 개인용 델리게이트
        private Chat MyChat;
        //전체에게 보낼 정보를 담고 있는 델리게이트
        private static Chat List;
        IChatCallback callback = null;     //  


        //==========================  IChat 메서드 =======================================================

        #region 1. Join(로그인하기)
        public bool Join(int idx)
        {
            MyChat = new Chat(UserHandler);
            lock (syncObj)
            {

                if (!Chatter.Contains(idx)) //이름이 기존 채터에 있는지 검색한다.
                {
                    //2. 사용자에게 보내 줄 채널을 설정한다.
                    callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();

                    //현재 접속자 정보를 모두에게 전달
                    BroadcastMessage(idx, "", "UserEnter");

                    //델리게이터 추가(향후 데이터 수신이 가능하도록 구성)
                    List += MyChat;

                    return true;
                }
                return false;
            }
        }
        #endregion

        #region 2. Say(메시지 보내기)
        public void Say(int idx, string msg)
        {
            BroadcastMessage(idx, msg, "Receive");
        }
        #endregion

        #region 3. Leave(로그아웃 하기)
        public void Leave(int idx)
        {


            //메시지 수신에서 제외
            List -= MyChat;

            //모든 사람에게 전송
            string msg = string.Format(idx + "이가 나갔습니다");
            BroadcastMessage(idx, msg, "UserLeave");
        }
        #endregion
        //===================================================================================================        

        private void BroadcastMessage(int idx, string msg, string msgType)
        {
            if (List != null)
            {
                //현재 이벤트들을 전달한다.
                foreach (Chat handler in List.GetInvocationList())
                {
                    handler.BeginInvoke(idx, msg, msgType, new AsyncCallback(EndAsync), null);
                }
            }
        }

        private void UserHandler(int idx, string msg, string msgType)
        {
            try
            {
                //클라이언트에게 보내기
                switch (msgType)
                {
                    case "Receive":
                        callback.Receive(idx, msg);
                        break;
                    case "UserEnter":
                        callback.UserEnter(idx);
                        break;
                    case "UserLeave":
                        callback.UserLeave(idx);
                        break;
                }
            }
            catch//에러가 발생했을 경우
            {
                Leave(idx);
            }
        }

        private void EndAsync(IAsyncResult ar)
        {
            Chat d = null;
            try
            {
                System.Runtime.Remoting.Messaging.AsyncResult asres = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((Chat)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                List -= d;
            }
        }

    }
}
