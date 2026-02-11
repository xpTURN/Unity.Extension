# MainToolbarPresetEx

Unity 에디터 메인 툴바 프리셋을 자동으로 적용하는 에디터 확장입니다.  
리플렉션을 통해 Unity 내부의 `MainToolbarWindow` / `OverlayCanvas`에 접근하여, 원하지 않는 툴바 요소를 숨기는 프리셋을 에디터 로드 시 적용합니다.

## 기능

- **자동 프리셋 적용**: 에디터가 로드될 때 `Assets/Settings/ToolbarPreset.asset` 프리셋을 자동 적용
- **버전 관리**: 적용 버전을 `EditorPrefs`에 저장해 프리셋 로직 변경 시에만 재적용

## 요구 사항

- Unity 6.3 이상

## 설치

1. 이 저장소를 클론하거나 다운로드합니다.
2. Unity 프로젝트에서 `MainToolbarPresetEx` 폴더를 프로젝트의 `Assets` 아래(또는 원하는 위치)에 복사합니다.
3. `Assets/Settings/ToolbarPreset.asset`이 존재하는지 확인합니다.  
   - 없으면 Unity 에디터에서 **Window → Overlays → Save Preset** 등으로 프리셋을 만들고,  
     해당 경로에 저장하거나 `MainToolbarPresetEx.cs`의 `k_PresetPath`를 실제 프리셋 경로로 수정합니다.

## 사용 방법

- 별도 조작 없이 Unity 에디터를 열면 프리셋이 자동 적용됩니다.
- 프리셋을 바꾸려면 `Assets/Settings/ToolbarPreset.asset`을 Unity에서 수정하거나 교체한 뒤 에디터를 다시 시작합니다.
- 적용 버전을 초기화해 다시 적용하려면 **Edit → Clear All PlayerPrefs** (또는 `EditorPrefs.DeleteKey("MainToolbarPresetEx.AppliedVersion")`) 후 에디터 재시작을 고려할 수 있습니다.

## 주의 사항

- Unity 내부 API(`MainToolbarWindow`, `OverlayCanvas`, `ApplyPreset` 등)를 리플렉션으로 사용하므로, Unity 버전 업그레이드 시 동작이 바뀌거나 깨질 수 있습니다.
- 프리셋 파일이 없으면 콘솔에 경고만 출력하고 적용을 건너뜁니다.

## License

The rest of the xpTURN.AssetLink code is under the Apache License, Version 2.0. See [LICENSE](./LICENSE.md) for details.

## Links

- **License**: [LICENSE](./LICENSE.md)
- **Author**: [xpTURN](https://github.com/xpTURN)
