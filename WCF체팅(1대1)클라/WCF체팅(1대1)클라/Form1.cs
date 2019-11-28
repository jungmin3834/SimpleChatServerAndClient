using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WCF체팅_1대1_클라.ServiceReference1;

namespace WCF체팅_1대1_클라
{
    public partial class Form1 : Form , IChatCallback
    {
        int idx;
        private IChat chat; 

        public Form1()
        {
            InitializeComponent();
        }
        #region IChatCallback 인터페이스 함수 생성
        public void Receive(int idx, string message)
        {
            string msgtemp = string.Format("{0}", message);
            chatlist.Items.Add(msgtemp);
        }


        // 메시지 전송
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (btnJoin.Text.Equals("로그아웃"))
            {
                string msg = msgbox.Text;

                string temp = string.Format("[{0}]", msg);

                chat.Say(idx, msg);

                msgbox.Clear();
            }
            else
                MessageBox.Show("로그인을 하시오");


        }

        public void UserEnter(int idx)
        {
            string msgtemp = string.Format("{0}님이 로그인하셨습니다.", idx);
            chatlist.Items.Add(msgtemp);
        }
        #endregion




        #region 로그인/로그아웃 핸들러

        private void btnJoin_Click(object sender, EventArgs e)
        {

            if (btnJoin.Text.Equals("로그인"))
            {
                InstanceContext site = new InstanceContext(this);
                chat = new WCF체팅_1대1_클라.ServiceReference1.ChatClient(site);
                this.Connect();
            }
            else this.DisConnect();

        }


        private void Connect()
        {
            try
            {
                idx = int.Parse(seatbox.Text);

                //서버 접속
                if (chat.Join(idx))
                {
                    MessageBox.Show("접속 성공");
                }
                else
                    MessageBox.Show("접속 실패");
     

                btnJoin.Text = "로그아웃";
             
                string login = string.Format("{0}님이 로그인하셨습니다.", seatbox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("접속 오류 : " +ex.Message);
            }
        }

        private void DisConnect()
        {
            try
            {
                chat.Leave(idx);
                chat = null;
                btnJoin.Text = "로그인";

                string logout = string.Format("{0}님이 로그아웃하셨습니다.", seatbox.Text);
                chatlist.Items.Add(logout);
            }
            catch (Exception ex)
            {
                MessageBox.Show("나가기 오류 :{0}", ex.Message);
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            InstanceContext site = new InstanceContext(this);
            chat = new WCF체팅_1대1_클라.ServiceReference1.ChatClient(site);

        }

    }
}
