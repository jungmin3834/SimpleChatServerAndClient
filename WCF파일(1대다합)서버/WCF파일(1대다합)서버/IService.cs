using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF파일_1대다합_서버
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IFileCallBack))]
     public interface IFile
     {
        [OperationContract]
        bool UpLoadFile(string name,int idx,string filename,byte[] data);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        StudentData[] Join(string name, int idx);

        [OperationContract(IsOneWay = false, IsInitiating = false, IsTerminating = true)]
        void Leave(string name, int idx);

     }

    [ServiceContract]
    public interface IFileCallBack
    {
         [OperationContract(IsOneWay = false)]
         void FileRecive(string name,int idx,string msg,byte[] filedata);
         [OperationContract(IsOneWay = false)]
          void UserEnter(string name, int idx);
         [OperationContract(IsOneWay = false)]
         void UserLeave(string name, int idx);
    }

    [MessageContract]
    public class StudentData
    {
        [MessageBodyMember]
        public bool Flag { get; set; }
        [MessageBodyMember]
        public string Name { get; set; }
        [MessageBodyMember]
        public int SeatNum { get; set; }

        public StudentData()
        {

        }

        public StudentData(bool f,string n,int sn)
        {
            Flag = f;
            Name = n;
            SeatNum = sn;
        }
    }
}
