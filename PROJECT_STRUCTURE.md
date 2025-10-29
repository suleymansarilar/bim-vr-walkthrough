# Project Structure

BIM_VR_Portfolio/
├─ 01_VR_Walkthrough/
│ ├─ Unity_Project/
│ │ ├─ Assets/
│ │ │ ├─ Scenes/ # Unity scene(s), e.g., SampleScene.unity
│ │ │ ├─ Scripts/ # FPSController, Hotspot, TourController, etc.
│ │ │ └─ Materials/Models/... # Imported FBX, materials
│ │ ├─ Packages/
│ │ └─ ProjectSettings/
│ ├─ Builds/
│ │ ├─ Windows/ # Desktop build: .exe + Data/
│ │ └─ WebGL/ # WebGL export: index.html + Build/ + TemplateData/
│ ├─ Demo_Videos/ # final_tour.mp4 and other clips
│ └─ Documentation/ # README, methodology notes, screenshots (optional)
│
├─ 02_BIM_Analytics/
│ ├─ Python_Scripts/ # analytics notebooks/scripts
│ ├─ Data/ # CSV/JSON/GML/IFC-derived data
│ ├─ Results/ # charts/tables
│ └─ Documentation/
│
├─ 03_Digital_Twin/
│ ├─ Unity_Project/ # visualization/dashboard
│ ├─ Python_Backend/ # simulated IoT stream + REST/WebSocket
│ ├─ Data_Streams/ # sample sensor data
│ └─ Documentation/
│
└─ Portfolio_Materials/
├─ Case_Studies/ # concise write-ups (problem → method → result)
├─ Research_Statement/ # 1-page statement tailored for professors/labs
├─ Outreach_Emails/ # short, personalized emails with links
└─ Screenshots/ # 4–6 high-quality images from the walkthrough
