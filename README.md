# 게임 소개
![image](https://github.com/choi-m-09/Zomvinter/assets/80871047/34ca965d-6f5f-4f0a-9140-f1c2aa847b14)

인디 게임인 Long Vinter, Project_Zomboid,Battle Ground 등 여러 게임에서 레퍼런스를 얻어 제작한 3D 쿼터뷰 생존 게임 입니다. 현재 튜토리얼 단계까지 개발되었으며 차후 기능 추가 및 컨텐츠를 늘릴 예정입니다.
# 플레이 영상
https://www.youtube.com/watch?v=zgkwzUstLhE
# 주요 기능
## 플레이어 및 카메라 무브먼트
### 플레이어
플레이어 클래스는 기본적인 움직임이 구현되어 있는 플레이어 컨트롤러 클래스를 상속받아 추후 캐릭터 개별 능력 및 능력치를 구현할 수 있도록 설계하였습니다. Axis 값에 기반하여 상하좌우로 움직일 수 있으며, 메인 카메라가 회전하면 플레이어도 따라서 회전합니다. 왼쪽 Shift 키를 누를 시 달리기가 가능하고 스테미너가 소모됩니다. 인벤토리 주무기 또는 보조무기 슬롯에 아이템이 장착되어 있는 경우 뼈대에 저장되어 있는 소켓 위치로 오브젝트가 나타나며, 슬롯에 맞는 키보드 넘버키를 누르게 되면 손에 있는 소켓 위치로 오브젝트가 나타나며 무기에 맞는 애니메이션이 실행됩니다. 무기를 들고 있는 상태에서 우클릭 시 CrossHair UI가 나타나며 마우스 커서 방향으로 회전합니다.
각 무기마다 실행되는 로직은 아래와 같습니다.

### 무기
플레이어 무기는 현재 3개 구현되어 있으며 차후 추가할 예정입니다. 무기 마다 로직은 다음과 같습니다.
+ AKM(주무기) : 플레이어 손에 있는 경우 우클릭 시 Aim 상태 전환. Aim 상태에서 좌클릭 시 총구의 Forward 방향으로 클래스에 저장되어 있는 총알 프리팹 Instantiate 후 내구도 감소
+ AXE(주무기) : 플레이어 손에 있는 경우 우클릭 시 공격 준비. 공격 준비 상태에서 좌클릭 시 공격 애니메이션 실행. 애니메이션 이벤트로 델리게이트 Invoke(구독된 Attack 함수 호출)
+ Pistal(보조무기): 주무기와 로직은 동일하나 총알 프리팹이 상이합니다.

플레이어는 상태 기계 패턴(FSM)을 기반으로 Cycle이 흘러갑니다. 살아있는 상태(Alive)인 경우 허기, 갈증이 시간이 지남에 따라 줄어들고 0이 되면 체력이 지속적으로 깎이고, 스테미너는 시간이 지남에 따라 자동으로 회복됩니다.
체력이 0이 되면 Dead 상태로 전이되고 Game Over 됩니다.
### 카메라
카메라는 키보드 Q, E를 통해 양방향으로 회전할 수 있으며 내부 함수 Slerp를 사용하여 부드럽게 움직일 수 있도록 구현하였습니다. 또한 Mouse Wheel Axis값을 기반으로 카메라 Zoom In/Out을 구현하였습니다.

## 인벤토리 및 아이템 시스템
### 아이템
아이템의 이름, ID, 타입 등 공통 변수 및 정적 변수들을 저장할 아이템 데이터 클래스와 아이템의 실체이며 아이템 수량, 총의 장탄수와 같은 동적 변수들과 아이템의 동작 메소드 및 인터페이스를 저장하는 클래스로 나누어 설계하였습니다. 아이템 데이터 클래스에서 공통 변수 및 정적 변수들을 Scriptable Object 클래스를 상속하여 데이터 사본만을 저장함으로써 메모리 사용을 최소화 하고, 유니티 내에서 에셋 형태로 관리할 수 있도록 설계하였습니다. 아이템 데이터의 클래스 구조는 다음과 같습니다.
![image](https://github.com/choi-m-09/Zomvinter/assets/80871047/ddefcf73-d2f6-4abf-8772-6159d1ee5763)
재료 아이템 및 회복 아이템 등 수량이 있는 아이템과 총, 칼, 방어구 같은 내구도가 있고 착용 가능한 아이템을 구분하여 각각의 타입에 맞는 필드값을 세팅하였습니다. 아이템 클래스도 위와 동일하지만 동적 변수와 각 아이템의 메소드 및 인터페이스가 구현되어 있으며, 고정 데이터 값은 아이템 데이터 스크립터블 오브젝트를 참조하여 데이터를 불러오도록 설계하였습니다.
### 인벤토리
![image](https://github.com/choi-m-09/Zomvinter/assets/80871047/b92b322a-d940-4ad4-8d19-992f43e2f990)
장착 가능한 슬롯들과 플레이어가 습득한 아이템을 담을 백팩 슬롯으로 인벤토리 슬롯 UI를 설계하였습니다. 아이템 우클릭 시 아이템 타입을 검사합니다. 장착 가능한 아이템의 경우 해당 아이템이 타입에 맞는 인벤토리 슬롯에 할당되어 이미지 및 텍스트가 표시되고 플레이어의 저장되어 있는 소켓의 위치로 아이템 오브젝트가 나타납니다. 사용 가능한 아이템 우클릭 시 수량이 줄어들고 해당 아이템에 구현되어 있는 인터페이스를 호출합니다. 

인벤토리 내 아이템 드래그 앤 드랍이 구현되어 있으며 인벤토리 내에서 아이템 위치 스왑이 가능하고 인벤토리 밖으로 아이템을 끌어다 놓으면 해당 아이템 오브젝트가 플레이어 위치로 나타납니다.

### 상호작용 오브젝트
게임 내 곳곳에는 상호작용 가능한 오브젝트(상자, 가구)가 있고 가까이 다가가 F키를 눌러 상호작용 할 수 있습니다. 상호작용 시 게이지가 표시되며 게이지가 다 차면 해당 오브젝트 배열에 있는 아이템이 랜덤하게 생성되어 표시됩니다.

## Zombie
좀비는 3가지 종류가 있으며 공통적으로 Roaming을 하다가 일정 거리 안에 플레이어가 감지되면 플레이어를 쫒아가는 로직으로 구성되어 있습니다. 
근접 공격 시 애니메이션 이벤트로 함수가 호출됩니다. 공격 소켓 위치로 내부 함수 OverlapSphere를 사용하여 Sphere 콜라이더를 생성하고 충돌된 오브젝트들에 구현되어 있는 데미지 함수가 호출됩니다. 이 외 특수 좀비들은 다음과 같은 로직을 구현하였습니다.
+ 원거리 좀비 : 플레이어를 쫒다가 사거리 안에 도달하면 애니메이션 이벤트의 함수가 호출되며 원거리 공격을 수행합니다.
+ 탱크 좀비 : 플레이어를 쫒는 중 특수 공격을 수행합니다. 특수 공격 시 플레이어의 카메라에 Shake 이벤트가 발생합니다.



# 구동 방법
+ + Unity
