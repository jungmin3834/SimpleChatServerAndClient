using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WCF체팅_1대다_서버
{

   #region 1. 메세지 관련 Contract InterFace (클라이언트->서버)
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatCallback))]
    public interface IChat
    {
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        bool Join(int idx);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void Leave(int idx);


        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void Say(int idx, string msg);

     

    }
    #endregion

    #region 2. 클라이언트에 콜백할 CallBackContract  (서버->클라이언트)
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receive(int idx, string message);

        [OperationContract(IsOneWay = true)]
        void UserEnter(int idx);
        
        [OperationContract(IsOneWay = true)]
        void UserLeave(int idx);
    }
    #endregion

    
}
