#define WIN32_LEAN_AND_MEAN
#include <stdio.h>
#include<Windows.h>
#include <tchar.h>
#include <WinSock2.h>
#pragma comment(lib,"ws2_32.lib")

#define SeverPort 2018
#define BUFFERSIZE 2014

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
	SOCKET listenSock;
		listenSock = socket(
		AF_INET,
		SOCK_STREAM,
		IPPROTO_TCP
		);
		if (listenSock == INVALID_SOCKET)
		{
			DisplayMessage();
			return -1;
		}
		
		SOCKADDR_IN serveraddr = { 0 };
		//�Ǵ� memset(&serveraddr,0,sizeof(serveraddr));

		ZeroMemory(&serveraddr, sizeof(serveraddr)); //�׳� ����0 ��Ű�°� ������ ����� 1
		
		serveraddr.sin_family = AF_INET;
		serveraddr.sin_port = htons(3000);  //!!! �ʼ������� htons ������� �� ������ �̷��� �ؾ� ��ȯ�Ͽ� ��� ����.
		serveraddr.sin_addr.s_addr = htonl(INADDR_ANY);  //htonl���ִ°� ���ڰ� ���� �Ǳ� ������ �ʼ������� htonl����ؾ��� (���� ���� �� �ʿ� x)
		
		int retval = bind(listenSock, (SOCKADDR*)&serveraddr, sizeof(serveraddr)); // ��ȭ ��ȣ �Ҵ��̶� ���� �Ǵ� �κ�
		                                                                                                  // IP�� �׷� �͵� ��� ���¸� ���� ����

		if (retval == SOCKET_ERROR)
		{
			DisplayMessage();
			return -1;
		}

		retval = listen(listenSock,SOMAXCONN); // ����ϰ� ���� ������ Ȥ�� ���� ��� ť �ؼ� ��ٸ��� ���ٰ�����. �׳� ��ȯ�� ������.
		if (retval == SOCKET_ERROR)
		{
			DisplayMessage();
			return -1;
		}

		//================================================================
		//2. ���� ó�� 
		//================================================================
		//��� ���� ����.
		SOCKET clientSocket;
		SOCKADDR_IN clientaddr;
		int addrlen = sizeof(clientaddr);
		char buf[BUFFERSIZE];
	    
		bool test = false;


		while (true)
		{

			printf("Ŭ���̾�Ʈ ���� ����� ...\n");


			clientSocket = accept(listenSock, (SOCKADDR*)&clientaddr,
								  &addrlen); // ������ ���ڴ� �ʼ� ������ �����ؾ��ϴ°ǵ� ���� ũ�⸦ �ѱ�°� ��. �տ� clientaddr�� ũ�⸦ �Ѱ���!.



			if (retval == INVALID_SOCKET)
			{
				DisplayMessage();
				continue;
			}
			printf("\n[TCP����] Ŭ���̾�Ʈ ���� ( %s : %d)\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));



			//========================
			while (true){
				retval = recv(clientSocket, buf, BUFFERSIZE, 0);
				//retval = recv(clientSocket, buf, BUFFERSIZE, 0);
				if (retval == SOCKET_ERROR)
				{
					DisplayMessage();
					break;
				}
				else if (retval == 0){ //������ ������ �� value ���� ���  �� ���� recv�� ������ ��ȯ��
					DisplayMessage();
					break;
				}
				else  //������ ���������� �����ϰ� �ִ� ���..
				{
					buf[retval] = '\0';
					printf("[TCP ����]%s : %d : %s\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port), buf);
				}
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

				retval = send(clientSocket, buf, strlen(buf), 0);

			}

			printf("[TCP] Ŭ���̾�Ʈ ���� : %d : %d\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));

			closesocket(clientSocket);

		}
		
	closesocket(listenSock);
	WSACleanup();
	return 0;
}