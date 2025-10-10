Ironhollow
Overview

Ironhollow is a one-mechanic 2D action platformer focused on custom magnetic physics and procedural level generation.
Built in Unity 6.2, the game challenges players to master a polarity-flip mechanic to traverse a decaying industrial factory.
This project emphasizes code quality, performance optimization, and physics systems rather than visuals.

Build Instructions
Unity Configuration

Open the project in Unity 6.2.

Navigate to File → Build Settings → Android, then click Switch Platform.

In Edit → Project Settings → Player → Android, confirm the following:

Scripting Backend: IL2CPP

Target Architectures: ARM64 (ARMv7 optional)

Build App Bundle (AAB): Off (build an APK for CA1)

Package Name: com.AdamKenny.ironhollow

Version: 0.1.0

Bundle Version Code: Increment for each new build

Signing the Build

Open Publishing Settings under the Android Player section.

Set up your keystore:

Path: keystore/Ironhollow.keystore

Alias: android

Validity: 50 years

Keep keystore passwords stored securely and do not commit them to version control.

Build your signed APK and save it to:

/releases/Ironhollow-0.1.0-release-arm64.apk

Sideloading on Android

Use ADB to install the game on a connected Android device:

adb install -r releases/Ironhollow-0.1.0-release-arm64.apk


Expected output:

Performing Streamed Install
Success

Repository Structure
/releases/
    Ironhollow-0.1.0-release-arm64.apk

/docs/
    install-proof.txt
    device-photo.png
    store-assets-checklist.md
    descriptions.md
    privacy-statement.md
    mda-onepager.md
    dev-journal.md

/keystore/
    Ironhollow.keystore   (excluded from repo)
README.md

Branching Strategy
Branch	Purpose
main	Stable, tagged releases only
develop	Integration branch for upcoming features
feature/polarity-system	Implements or refines the core polarity mechanic
feature/procedural-stitching	Adds or improves procedural generation
hotfix/android-build	Urgent fixes or build configuration corrections

Workflow:
feature/* → merge into develop → test → merge into main → tag a release (e.g., v0.1.0).

Release Artefacts

Each tagged release or submission ZIP must include:

/releases/Ironhollow-0.1.0-release-arm64.apk

/docs/install-proof.txt (ADB log output)

/docs/device-photo.png (in-game device screenshot)

/docs/mda-onepager.md

/docs/privacy-statement.md

/docs/store-assets-checklist.md

/docs/descriptions.md

/docs/dev-journal.md

README.md (this file)

Technical Summary

Engine: Unity 6.2 (URP, 2D)

Build Target: Android

Backend: IL2CPP

Architecture: ARM64

Input: Single-tap polarity toggle (mobile-compatible)

Data Persistence: Local PlayerPrefs

Performance Goal: 60 FPS, <150 MB build size

Academic Declaration

This project was developed for Mobile Game Development (A12581) at South East Technological University.
All code is original and authored for assessment purposes.
AI tools (such as GitHub Copilot and ChatGPT) were used for productivity and documentation support only.
