	#define WIN32_LEAN_AND_MEAN
	#include <stdio.h>
	#include<Windows.h>
	#include <tchar.h>
	#include <WinSock2.h>
	#pragma comment(lib,"ws2_32.lib")


	#define SeverPort 3000	
	#define BUFFERSIZE 2024
	#define SERVER_IP  "127.0.0.1"//"192.168.0.52"   //192.168.0.52 // "127.0.0.1" 

	//���� ���� �ذ��� ���� WinSock2�� pragma�� ���� �÷��� ���� : Windows.h �� ��ġ�� �κ� �� ���Ƽ� .
	//Ȥ�� �ļ��� #define WIN32_LEAN_AND_MEAN ���ִ� ��(�� ������� �ʴ� �͵��� �ּ�ó���ϴ� ��)
	void DisplayMessage()
	{
		LPVOID pMsg;
		//���� �ڵ� => ���ڿ��� �޾Ƴ�°�
		FormatMessage(
			FORMAT_MESSAGE_ALLOCATE_BUFFER |  //���� �Ҵ� ��û
			FORMAT_MESSAGE_FROM_SYSTEM,      //�ý��ۿ� ����� ���� �޼����� ������ ��
			NULL, WSAGetLastError(),                   //�����ڵ�ȹ�� (���� ���� ���� �� ������ ����)
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), //Lang_Neutral ��� ���� �츮�� �ѱ� OS�� �ѱ� ���� �޾ƿ�
			(LPTSTR)&pMsg, //������ LPVOid ����Ʈ ����Ʈ�� �� ������ �������� ũ�Ⱑ �ٸ��� �������̶� ���� ũ�⸦ �޾ƿ� ���� �ʱ�ȭ
			0, NULL);

		//TCHAR buf[20];
		//wsprintf(buf, TEXT("%d"), pMsg);
		MessageBox(0, TEXT((LPTSTR)pMsg), TEXT("�˸�"), MB_OK);
		//printf("%d", pMsg);
		LocalFree(pMsg);
	}

	int main()
	{
		WSADATA wsa;

		if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
		{

			printf("������ ���� �ʱ�ȭ ����");
			return 0;
		}
		//=========================
		//1. ��� ���� ����, �ּ� ���� , ������
		//=========================
		SOCKET sock;
		sock = socket(
			AF_INET,
			SOCK_STREAM,
			IPPROTO_TCP
			);
		if (sock == INVALID_SOCKET)
		{
			DisplayMessage();
			return -1;
		}

		SOCKADDR_IN serveraddr = { 0 };
		//�Ǵ� memset(&serveraddr,0,sizeof(serveraddr));

		ZeroMemory(&serveraddr, sizeof(serveraddr)); //�׳� ����0 ��Ű�°� ������ ����� 1

		serveraddr.sin_family = AF_INET;
		serveraddr.sin_port = htons(SeverPort);  //!!! �ʼ������� htons ������� �� ������ �̷��� �ؾ� ��ȯ�Ͽ� ��� ����.
		serveraddr.sin_addr.s_addr = inet_addr(SERVER_IP);  //inet_addr �� ������ ��ȯ 

		
		while (true)
		{

			int retval = connect(sock, (SOCKADDR*)&serveraddr, sizeof(serveraddr));

			if (retval == SOCKET_ERROR)
			{
				DisplayMessage();
				return -1;
			}
			else
				printf("����");
			while (true){
				//================================
				char buf[BUFFERSIZE];
				int len;

				ZeroMemory(buf, sizeof(buf));
				printf("���ڿ� �Է� : ");
				if (fgets(buf, BUFFERSIZE, stdin) == NULL)
					break;

				len = strlen(buf);

				if (buf[len - 1] == '\0')
					buf[len - 1] = '\0';
				if (strlen(buf) == 0)
					break;

				retval = send(sock, buf, strlen(buf), 0);

				if (retval == SOCKET_ERROR)
				{
					DisplayMessage();
					break;
				}

				printf("[TCPŬ����Ʈ] %d ����Ʈ ���� \n", retval);

				retval = recv(sock, buf, BUFFERSIZE, 0);
				printf("[TCPŬ����Ʈ] %s\n", buf);
			}
		}

		closesocket(sock);
		//===============================
		WSACleanup();
		return 0;
	}