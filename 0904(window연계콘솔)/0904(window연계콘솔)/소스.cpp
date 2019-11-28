	#define WIN32_LEAN_AND_MEAN
	#include <stdio.h>
	#include<Windows.h>
	#include <tchar.h>
	#include <WinSock2.h>
	#pragma comment(lib,"ws2_32.lib")


	#define SeverPort 3000	
	#define BUFFERSIZE 2024
	#define SERVER_IP  "127.0.0.1"//"192.168.0.52"   //192.168.0.52 // "127.0.0.1" 

	//빌드 에러 해결을 위해 WinSock2와 pragma를 위로 올려줌 이유 : Windows.h 와 겹치는 부분 이 많아서 .
	//혹은 꼼수로 #define WIN32_LEAN_AND_MEAN 해주는 것(즉 사용하지 않는 것들은 주석처리하는 것)
	void DisplayMessage()
	{
		LPVOID pMsg;
		//에러 코드 => 문자열로 받아노는것
		FormatMessage(
			FORMAT_MESSAGE_ALLOCATE_BUFFER |  //버퍼 할당 요청
			FORMAT_MESSAGE_FROM_SYSTEM,      //시스템에 저장된 오류 메세지를 가지고 옴
			NULL, WSAGetLastError(),                   //에러코드획득 (성공 실패 여부 다 가지고 있음)
			MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), //Lang_Neutral 언어 설정 우리는 한글 OS라 한글 정보 받아옴
			(LPTSTR)&pMsg, //위에서 LPVOid 보이트 포인트를 준 이유는 오류마다 크기가 다르고 가변적이라 오류 크기를 받아와 변숫 초기화
			0, NULL);

		//TCHAR buf[20];
		//wsprintf(buf, TEXT("%d"), pMsg);
		MessageBox(0, TEXT((LPTSTR)pMsg), TEXT("알림"), MB_OK);
		//printf("%d", pMsg);
		LocalFree(pMsg);
	}

	int main()
	{
		WSADATA wsa;

		if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
		{

			printf("윈도우 소켓 초기화 실패");
			return 0;
		}
		//=========================
		//1. 대기 소켓 생성, 주소 연결 , 망연결
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
		//또는 memset(&serveraddr,0,sizeof(serveraddr));

		ZeroMemory(&serveraddr, sizeof(serveraddr)); //그냥 전부0 시키는거 위에는 명시적 1

		serveraddr.sin_family = AF_INET;
		serveraddr.sin_port = htons(SeverPort);  //!!! 필수적으로 htons 해줘야함 그 이유는 이렇게 해야 변환하여 사용 가능.
		serveraddr.sin_addr.s_addr = inet_addr(SERVER_IP);  //inet_addr 은 아이피 변환 

		
		while (true)
		{

			int retval = connect(sock, (SOCKADDR*)&serveraddr, sizeof(serveraddr));

			if (retval == SOCKET_ERROR)
			{
				DisplayMessage();
				return -1;
			}
			else
				printf("성공");
			while (true){
				//================================
				char buf[BUFFERSIZE];
				int len;

				ZeroMemory(buf, sizeof(buf));
				printf("문자열 입력 : ");
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

				printf("[TCP클리언트] %d 바이트 전송 \n", retval);

				retval = recv(sock, buf, BUFFERSIZE, 0);
				printf("[TCP클리언트] %s\n", buf);
			}
		}

		closesocket(sock);
		//===============================
		WSACleanup();
		return 0;
	}