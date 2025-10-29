# BIM VR Walkthrough

A desktop-and-WebGL walkthrough of a BIM model built in Unity (2022 LTS), featuring:
- First-person navigation (WASD + Mouse, Shift to run, Space to jump)
- World-space Info Panel (toggle with I)
- Interactive safety hotspots (approach to show, press E for details)
- Guided tour (press Space) with smoothed turns
- Post-processing (Bloom, Color Adjustments), baked GI lighting
- OpenXR-ready project setup (Mock Runtime for desktop testing)
- FPS HUD for performance

## Demo and Downloads
- Live WebGL Demo”: https://bim-vr-walkthrough.netlify.app
- Windows build (ZIP): https://github.com/YOUR-USER/bim-vr-walkthrough/releases/download/v1.0.0/YOUR_WINDOWS_ZIP.zip
- Demo videos (60–90s): https://drive.google.com/drive/folders/1_Cn1NJPH1DRPJr-AKnsSVfuLhjRgYabY?usp=drive_link

## Controls
- Movement: `W A S D`
- Look: Mouse
- Run: `Left Shift`
- Jump: `Space` (in FPS mode)
- Guided Tour: `Space` to start
- Info Panel: `I` toggle
- Hotspot Detail: `E`
- Release cursor: `Esc` (in editor)

## Folder Highlights
- `Assets/Scripts/`
  - `FPSController.cs`: first-person movement
  - `Hotspot.cs`: proximity + E to toggle details
  - `LookAtCamera.cs`: billboard behavior
  - `TourController.cs`: guided tour with anti-stuck logic and camera FOV/pitch control
  - `SimpleFPSHUD.cs`: FPS overlay
- `Assets/Scenes/`: main scene (e.g., `SampleScene.unity`)
- `01_VR_Walkthrough/Builds/Windows/`: desktop build
- `01_VR_Walkthrough/Builds/WebGL/`: WebGL export
- `01_VR_Walkthrough/Demo_Videos/`: final demo videos

## How to Run
### Windows (.exe)
1. Download the Windows ZIP from Releases and extract.
2. Run `BIM_VR_Walkthrough.exe`.
3. If SmartScreen warns, choose “More info” → “Run anyway”.

### WebGL (local test)
- Recommended: VS Code + Live Server
  - Open the WebGL folder in VS Code → click “Go Live”.
- Python (if installed):
  - Open a terminal in the WebGL folder:
    - `py -m http.server 8000` OR `python -m http.server 8000`
  - Visit `http://localhost:8000`
- Node alternative:
  - `npx http-server -p 8000`
  - If your build uses Brotli/Gzip, use: `npx http-server -p 8000 --brotli --gzip`

## Build Notes
- Unity 2022.3 LTS
- Desktop build: Mono backend (quick path). IL2CPP works when “Windows Build Support (IL2CPP)” and VS C++ toolchain are installed.
- WebGL build:
  - Easiest: Publishing → Compression = Disabled
  - Or Compression = Brotli (+ Decompression Fallback = Enabled)

## Method (short)
- Cleaned BIM mesh in Blender (merge-by-distance, normal fixes), exported as FBX
- Imported to Unity; added FPS controller, hotspots, world-space UI
- Configured lighting (Directional Light, baked GI) and post-processing
- Implemented guided tour and performance HUD

## Results
- High-fidelity 60–90s walkthrough video
- Windows desktop and WebGL builds
- Ready for research demos, safety discussions, and design validation

## Next Steps
- BIM Analytics in Python (room counts/areas/types)
- Simple rule-based safety checks on BIM metadata
- Digital Twin prototype (fusing simulated IoT data with BIM spaces)

## License
For academic portfolio and demo purposes. Please contact the author for other uses.
