
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.ServiceModel;
    using System.Collections;
    using System.IO;

    namespace Chating
    {
        class ChatService : IChat
        {
            //델리게이트 선언
            public delegate void Chat(int sidx, string msg, string type);
            // 개인용 델리게이트
            private Chat MyChat;
            //전체에게 보낼 정보를 담고 있는 델리게이트
            IChatCallback callback = null;


            #region 1. Join(로그인하기)
            public bool Join(int idx)
            {
                MyChat = new Chat(UserHandler);
                //2. 사용자에게 보내 줄 채널을 설정한다.
                callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();

                //현재 접속자 정보를 모두에게 전달
                BroadcastMessage(idx, "", "UserEnter");
                return true;
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
                Chat d = null;
                MyChat -= d;
            }
            #endregion

            private void BroadcastMessage(int idx, string msg, string msgType)
            {
                MyChat.BeginInvoke(idx, msg, msgType, new AsyncCallback(EndAsync), null);
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
                    }
                }
                catch//에러가 발생했을 경우
                {
                    Console.WriteLine("{0} 에러", idx);
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
                }
            }




        }


        #region 1. 메세지 관련 Contract InterFace (클라이언트->서버)
        [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatCallback))]
        public interface IChat
        {
            [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
            bool Join(int idx);

            [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
            void Say(int idx, string msg);

            [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
            void Leave(int idx);
        }
        #endregion

        #region 2. 클라이언트에 콜백할 CallBackContract  (서버->클라이언트)
        public interface IChatCallback
        {
            [OperationContract(IsOneWay = true)]
            void Receive(int idx, string message);

            [OperationContract(IsOneWay = true)]
            void UserEnter(int idx);
        }
        #endregion
    }


