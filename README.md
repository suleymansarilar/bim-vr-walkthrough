# VR Walkthrough

A desktop-and-WebGL walkthrough of a BIM model built in Unity (2022 LTS), featuring:

* First-person navigation (WASD + Mouse, Shift to run, Space to jump)
* World-space Info Panel (toggle with I)
* Interactive safety hotspots (approach to show, press E for details)
* Guided tour (press Space) with smoothed turns
* Post-processing (Bloom, Color Adjustments), baked GI lighting
* OpenXR-ready project setup (Mock Runtime for desktop testing)
* FPS HUD for performance
* **Collaborative tools for spatial communication research** (role switching, colored markers, route sketching, session logging)

## Demo and Downloads

* Live WebGL Demo: <https://bim-vr-walkthrough.netlify.app>
* Windows build (ZIP): <https://github.com/suleymansarilar/bim-vr-walkthrough/releases/download/v1.0.0/windows.zip>
* Demo videos (60–90s): <https://drive.google.com/drive/folders/1_Cn1NJPH1DRPJr-AKnsSVfuLhjRgYabY?usp=drive_link>

## Controls

* Movement: `W A S D` (recently updated from arrow keys)
* Look: Mouse
* Run: `Left Shift`
* Jump: `Space` (in FPS mode, when not used for task navigation)
* Guided Tour: `Space` to start
* Info Panel: `I` toggle
* Hotspot Detail: `E`
* Cursor Lock/Unlock: `Esc` (toggle cursor lock state)

### Collaborative Tools (New)

* Role Switch: `Tab` (cycles between Site Engineer and Safety Officer)
* Task Cards: 
  * `T` - Toggle task card display (show/hide)
  * `Space` - Next task (cycle through problem-based learning scenarios)
  * `Backspace` - Previous task
  * `Q` - Reset current task
* Place Marker: `E` (drops colored marker at camera target; color matches active role)
* Route Sketch: `R` (adds waypoint), `C` (clears current route)
* Session logs are saved automatically to `AppData/LocalLow/<Company>/<Product>/SessionLogs/` as JSON lines

## Folder Highlights

* `Assets/Scripts/`  
   * `FPSController.cs`: first-person movement  
   * `Hotspot.cs`: proximity + E to toggle details  
   * `LookAtCamera.cs`: billboard behavior  
   * `TourController.cs`: guided tour with anti-stuck logic and camera FOV/pitch control  
* `SimpleFPSHUD.cs`: FPS overlay
* `SessionLogger.cs`: logs all interactions (markers, routes, role changes) to JSON
* `CollaborativeSessionManager.cs`: manages roles, tasks, and session state
* `TaskCardUI.cs`: task card display system with T key toggle
* `CollaborativeMarkerTool.cs`: marker placement with role-based coloring
* `RouteSketchTool.cs`: route waypoint system with line rendering
* `Assets/Scenes/`: main scene (e.g., `SampleScene.unity`)
* `01_VR_Walkthrough/Builds/Windows/`: desktop build
* `01_VR_Walkthrough/Builds/WebGL/`: WebGL export
* `01_VR_Walkthrough/Demo_Videos/`: final demo videos

## How to Run

### Windows (.exe)

1. Download the Windows ZIP from Releases and extract.
2. Run `BIM_VR_Walkthrough.exe`.
3. If SmartScreen warns, choose "More info" → "Run anyway".

### WebGL (local test)

* Recommended: VS Code + Live Server  
   * Open the WebGL folder in VS Code → click "Go Live".
* Python (if installed):  
   * Open a terminal in the WebGL folder:  
         * `py -m http.server 8000` OR `python -m http.server 8000`  
   * Visit `http://localhost:8000`
* Node alternative:  
   * `npx http-server -p 8000`  
   * If your build uses Brotli/Gzip, use: `npx http-server -p 8000 --brotli --gzip`

## Build Notes

* Unity 2022.3 LTS
* Desktop build: Mono backend (quick path). IL2CPP works when "Windows Build Support (IL2CPP)" and VS C++ toolchain are installed.
* WebGL build:  
   * Easiest: Publishing → Compression = Disabled  
   * Or Compression = Brotli (+ Decompression Fallback = Enabled)

## Method (short)

* Cleaned BIM mesh in Blender (merge-by-distance, normal fixes), exported as FBX
* Imported to Unity; added FPS controller, hotspots, world-space UI
* Configured lighting (Directional Light, baked GI) and post-processing
* Implemented guided tour and performance HUD
* Added collaborative tools (roles, markers, routes) and session logging for spatial communication research

## Results

* High-fidelity 60–90s walkthrough video
* Windows desktop and WebGL builds
* Ready for research demos, safety discussions, and design validation
* Session logs for analyzing spatial communication patterns in virtual construction sites

## Research Notes

I am exploring how teams communicate about space when they work together in virtual construction sites. The collaborative tools (markers, routes, role switching) help me capture interaction patterns, and I review the logs to see where people agree about space and where they get confused.

For more details on my research questions and reading notes, see:
* `SESSION_NOTES.md` - How I use the tools and what I'm curious about
* `POST_SESSION_PROMPTS.md` - Simple observation questions I ask after each session
* `READING_NOTES.md` - Key papers I'm reading (spatial cognition, collaborative learning)

## Next Steps

* BIM Analytics in Python (room counts/areas/types)
* Simple rule-based safety checks on BIM metadata
* Digital Twin prototype (fusing simulated IoT data with BIM spaces)

## License

For academic portfolio and demo purposes. Please contact the author for other uses.

## About

No description, website, or topics provided.

