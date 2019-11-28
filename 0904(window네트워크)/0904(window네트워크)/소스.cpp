#define WIN32_LEAN_AND_MEAN
#include <stdio.h>
#include<Windows.h>
#include <tchar.h>
#include <WinSock2.h>
#pragma comment(lib,"ws2_32.lib")

#define SeverPort 2018
#define BUFFERSIZE 2014

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
		//또는 memset(&serveraddr,0,sizeof(serveraddr));

		ZeroMemory(&serveraddr, sizeof(serveraddr)); //그냥 전부0 시키는거 위에는 명시적 1
		
		serveraddr.sin_family = AF_INET;
		serveraddr.sin_port = htons(3000);  //!!! 필수적으로 htons 해줘야함 그 이유는 이렇게 해야 변환하여 사용 가능.
		serveraddr.sin_addr.s_addr = htonl(INADDR_ANY);  //htonl해주는건 숫자가 전달 되기 때문에 필수적으로 htonl사용해야함 (임의 지정 할 필요 x)
		
		int retval = bind(listenSock, (SOCKADDR*)&serveraddr, sizeof(serveraddr)); // 전화 번호 할당이라 보면 되는 부분
		                                                                                                  // IP나 그런 것들 대기 상태를 위해 생성

		if (retval == SOCKET_ERROR)
		{
			DisplayMessage();
			return -1;
		}

		retval = listen(listenSock,SOMAXCONN); // 대기하게 해줄 것인지 혹은 접속 대기 큐 해서 기다리게 해줄것인지. 그냥 반환할 것인지.
		if (retval == SOCKET_ERROR)
		{
			DisplayMessage();
			return -1;
		}

		//================================================================
		//2. 접속 처리 
		//================================================================
		//통신 소켓 생성.
		SOCKET clientSocket;
		SOCKADDR_IN clientaddr;
		int addrlen = sizeof(clientaddr);
		char buf[BUFFERSIZE];
	    
		bool test = false;


		while (true)
		{

			printf("클라이언트 접속 대기중 ...\n");


			clientSocket = accept(listenSock, (SOCKADDR*)&clientaddr,
								  &addrlen); // 마지막 인자는 필수 적으로 조심해야하는건데 값의 크기를 넘기는것 즉. 앞에 clientaddr의 크기를 넘겨줌!.



			if (retval == INVALID_SOCKET)
			{
				DisplayMessage();
				continue;
			}
			printf("\n[TCP서버] 클라이언트 접속 ( %s : %d)\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));



			//========================
			while (true){
				retval = recv(clientSocket, buf, BUFFERSIZE, 0);
				//retval = recv(clientSocket, buf, BUFFERSIZE, 0);
				if (retval == SOCKET_ERROR)
				{
					DisplayMessage();
					break;
				}
				else if (retval == 0){ //상대방이 끊었을 때 value 값의 결과  즉 위에 recv의 정상적 반환값
					DisplayMessage();
					break;
				}
				else  //상대방이 정상적으로 수신하고 있는 경우..
				{
					buf[retval] = '\0';
					printf("[TCP 서버]%s : %d : %s\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port), buf);
				}
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

				retval = send(clientSocket, buf, strlen(buf), 0);

			}

			printf("[TCP] 클라이언트 종료 : %d : %d\n", inet_ntoa(clientaddr.sin_addr), ntohs(clientaddr.sin_port));

			closesocket(clientSocket);

		}
		
	closesocket(listenSock);
	WSACleanup();
	return 0;
}