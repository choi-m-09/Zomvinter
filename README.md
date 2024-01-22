# 게임 소개
![image](https://github.com/choi-m-09/Zomvinter/assets/80871047/34ca965d-6f5f-4f0a-9140-f1c2aa847b14)

인디 게임인 Long Vinter, Project_Zomboid, Battle Ground등 여러 게임에서 레퍼런스를 얻어 제작한 3D 쿼터뷰 생존 게임 입니다. 현재 튜토리얼 단계까지 개발되었으며 차후 기능 추가 및 컨텐츠를 늘릴 예정입니다.
# 플레이 영상
https://www.youtube.com/watch?v=zgkwzUstLhE
# 주요 기능
## 플레이어 및 카메라 무브먼트
### 플레이어
플레이어는 PlayerController를 상속받고 있으며 부모 클래스에서 이동 및 회전 함수가 구현되어 있습니다. 회전은 ScreenPointToRay 함수를 통하여 메인 카메라 방향으로 Ray를 발사한 벡터 값과 플레이어 위치 벡터를 빼준 거리값을 Normalize 하여 내부 함수인 LookRotation의 인자로 전달하여 마우스 방향으로 회전하도록 설계하였습니다. 이동은 내부 함수인 Translate의 인자로 Axis 값(Left, Right)과 과 카메라 Transform을 전달하여 카메라가 바라보고 있는 방향으로 이동합니다. 플레이어가 총을 장착하면 플레이어 등 뒤에 총 오브젝트가 나타나고 1번 또는 2번을 누르면 총이 플레이어 손에 오도록 구현하였습니다. X를 누르면 총이 장착 해제되며 다시 플레이어 등으로 돌아갑니다. 총을 들고있는 경우 마우스 오른쪽 클릭을 통하여 Aim 상태로 전환할 수 있고 마우스 왼쪽 클릭 시 Bullet이 Instantiate 됩니다.

### 카메라
카메라는 키보드 Q, E를 통해 Left, Right로 회전할 수 있으며 내부 함수 Slerp를 사용하여 부드럽게 움직일 수 있도록 구현하였습니다. 또한 Mouse Wheel Axis값을 기반으로 카메라 Zoom In/Out을 구현하였습니다.

## 인벤토리 및 상호작용 오브젝트
### 인벤토리
아이템 슬롯은 주무기 2칸, 보조무기 1칸, 퀵슬롯창 3개로 구성되있으며 인벤토리 안에 있는 아이템으로 장착이 가능합니다. Tab 버튼 클릭 시 인벤토리 UI가 나타나거나 사라지고 인벤토리는 최대 수용칸이 있으며 넘어가게 되는 경우 더 이상 수용할 수 없게 됩니다. 인벤토리 내 아이템을 마우스 오른쪽 클릭을 하게 되면 해당 슬롯에 장착됩니다.

### 상호작용 오브젝트
게임 내 곳곳에는 상호작용 가능한 오브젝트(상자, 가구)가 있고 가까이 다가가 F키를 눌러 상호작용 할 수 있습니다. 상호작용 시 게이지가 표시되며 게이지가 다 차면 해당 오브젝트 안의 아이템이 랜덤하게 생성되어 표시됩니다.

## Zombie
좀비는 3가지 종류가 있으며 공통적으로 Roaming을 하다가 일정 거리 안에 플레이어가 감지되면 플레이어를 쫒아가는 로직으로 구성되어 있습니다. 
근접 공격의 경우 Animation Event로 함수를 호출합니다. 공격 소켓 Transform 위치로 내부 함수 OverlapSphere를 사용하여 Sphere 콜라이더를 잠시 생성해주고 충돌한 오브젝트들의 데미지 함수가 호출됩니다. 이 외 특수 좀비들은 다음과 같은 로직을 구현하였습니다.
+ 원거리 공격 좀비 : 플레이어를 쫒다가 사거리 안에 도달하면 원거리 공격을 수행합니다.
+ 탱크 좀비 : 플레이어를 쫒는 중 특수 공격을 수행합니다. 특수 공격 시 플레이어의 카메라에 Shake 이벤트가 발생합니다.

+ # 구동 방법
+ + Unity
