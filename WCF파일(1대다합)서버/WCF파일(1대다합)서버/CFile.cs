using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF파일_1대다합_서버
{

    class CFile :IFile
    {
        public delegate void Data(string name, int idx, string msg, byte[] filedata, string type);

        private static Object syncObj = new Object();

        private static ArrayList Chatter = new ArrayList();

        private Data Myfile;
        private static Data List;
        IFileCallBack callback =null;

        public StudentData[] Join(string name, int idx)
        {
            Myfile = new Data(UserHandler);
            lock (syncObj)
            {
                try
                {
                    if (!Chatter.Contains(name))
                    {
                        StudentData data = new StudentData(true, name, idx);
                        Chatter.Add(data);

                        callback = OperationContext.Current.GetCallbackChannel<IFileCallBack>();
                    
                        Console.WriteLine(name);
                        List += Myfile;
                        Console.WriteLine(name);
                        BroadcastMessage(name, idx, "", null, "UserEnter");
                        StudentData[] list = new StudentData[Chatter.Count];
                        lock (syncObj)
                        {
                            Chatter.CopyTo(list);
                        }
                        return list;
                    }
                    else
                        return null;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public void Leave(string name ,int idx)
        {
            if(name == null )return;
   
            lock(syncObj)
            {
                foreach(StudentData data in Chatter)
                {
                    if(data.Name == name){
                        
                        Chatter.Remove(data);   
                        break;
                    }
                }
            }

        }


        private void BroadcastMessage(string name,int idx,string msg,byte [] filedata,string msgType)
        {
            try{
                UserHandler(name, idx, msg, filedata, msgType);
             //if (List != null)
             //{
                foreach (Data handler in List.GetInvocationList())
                {
                    //Console.WriteLine("1"+msgType);

                    ////if (handler == Myfile && msgType == "FileSender")
                    ////{
                    ////    Console.WriteLine("2" + msgType);
                    ////    continue;
                    ////}

                    //Console.WriteLine("3" + msgType);
                  //  BeginInvoke(name, idx, msg, filedata, msgType, new AsyncCallback(EndAsync), null);
                  
                    
                }
           // }
              }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Leave(name, idx);
            }
        }

        private void EndAsync(IAsyncResult ar)
        {
            Data d = null;
            try
            {
                System.Runtime.Remoting.Messaging.AsyncResult asres = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((Data)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                List -= d;
            }
        }

        private void UserHandler(string name,int idx,string msg,byte[] filedata,string msgType)
        {
            try
            {
                Console.WriteLine("항상 여기까지옴"+ msgType);
                switch (msgType)
                {
                    case "UserEnter": Console.WriteLine("엔터"); callback.UserEnter(name, idx); break;
                    case "UserLeave":    callback.UserLeave(name, idx); break;
                    case "FileSender": Console.WriteLine("센더"); callback.FileRecive(name, idx, msg, filedata); break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Leave(name, idx);
            }
        }

        public bool UpLoadFile(string name, int idx, string filename, byte[] data)
        {
            Myfile = new Data(UserHandler);
            try
            {

                FileStream writeFileStream = new FileStream(@"C:\Users\DUMB\Pictures\Saved Pictures\" + filename, FileMode.Create, FileAccess.Write);
                BinaryWriter dataWriter = new BinaryWriter(writeFileStream);

                dataWriter.Write(data);
                writeFileStream.Close();

                callback = OperationContext.Current.GetCallbackChannel<IFileCallBack>();
                Console.WriteLine(name + idx + filename + data.Length.ToString());
                List += Myfile;
                BroadcastMessage(name, idx, filename, data, "FileSender");
               // BroadcastMessage(name, idx, filename, data, "FileSender");
                return true;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
