<img width="1076" height="605" alt="Image" src="https://github.com/user-attachments/assets/b014494a-4bb3-4946-b5d5-2bb17fd86f45" />

### 1. 소개
* 게임 이름: 숲으로 돌아가는 길
* 장르: 액션, 플랫포머
* 개발 기간: 2026년 6월 ~ 2026년 9월
* 게임 설명: 숲을 걷던 아이가 누군가에게 납치당한 후, 다시 숲으로 돌아가기 위한 여정
---
### 2. 개발 환경
* 사용 툴: Unity 6.4, Visual Studio 2026, Aseprite
* AI 사용 여부: O(에셋 생성 및 학습 목적으로 사용)
---
### 3. 사용 기술
기술|설명
--|--|
싱글톤 패턴|씬 이동시 삭제되면 안되거나 많은 접근이 필요한 오브젝트에 사용
오브젝트 풀|오브젝트 생성 및 삭제 과정에서 발생하는 메모리 누수 및 메모리 파편화를 방지하기 위해 사용
오디오 믹서|음소거 기능 구현 및 여러 오디오 소스를 통합적으로 관리하기 위해 사용
인터페이스|적들의 기절 상태효과를 구현하기 위해 사용
---
###  4. 구현 기능
* 플레이어 캐릭터
  * 좌우 이동
  * 점프
  * 대쉬
  * 공격
  * 패링
  * 피격
  * 사망
  * 상호작용
  * 포션 사용
* 적
  * 아이템 드롭
  * 공격
  * 피격
  * 기절
  * 사망
* UI
  * 플레이어 체력바
  * 플레이어 포션 수
  * 홀딩 상호작용 게이지바
---
### 5. 사용 에셋 및 출처
* 에셋 이름(제작자 명): 출처
* Hooded Protagonist Animated Character(Penzilla): https://penzilla.itch.io/hooded-protagonist
* Dungeon Platformer Tile Set (Pixel Art)(David G): https://incolgames.itch.io/dungeon-platformer-tile-set-pixel-art
* Hearts and health bar(VampireGirl): https://fliflifly.itch.io/hearts-and-health-bar
* Free character - Satyr(LuckyLoops): https://lucky-loops.itch.io/character-satyr
* 도스필기(hurss): https://github.com/hurss/fonts
* Pixel Keyboard Keys - for UI(Dream Mix): https://dreammixgames.itch.io/keyboard-keys-for-ui
* FX Pixel Texture(BDragon1727): https://bdragon1727.itch.io/fx-pixel-texture
* Easy-Text-Effects-for-Unity(LeiQiaoZhi): https://github.com/LeiQiaoZhi/Easy-Text-Effects-for-Unity
* Pixel Art Skeletons Pack(MonoPixelArt): https://monopixelart.itch.io/skeletons-pack
* 2D Pixel Art Platformer | Biome - American Forest(Superposition Principle): https://assetstore.unity.com/packages/2d/environments/2d-pixel-art-platformer-biome-american-forest-255694#description
* Free Effect and Bullet 16x16(BDragon1727): https://bdragon1727.itch.io/free-effect-and-bullet-16x16
* 750 Effect and FX Pixel All(BDragon1727): https://bdragon1727.itch.io/750-effect-and-fx-pixel-all
* Flask Icons 32x32px(Allnew): https://allnew.itch.io/flask-icons-32x32px
* Free Fantasy 200 SFX Pack(TomMusic): https://tommusic.itch.io/free-fantasy-200-sfx-pack
* Horror Sound Effects(YourPalRob): https://yourpalrob.itch.io/must-have-horror-sound-effects
* 400 Sounds Pack(Chequered Ink): https://ci.itch.io/400-sounds-pack

   
