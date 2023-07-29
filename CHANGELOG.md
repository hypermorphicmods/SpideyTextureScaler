﻿# Change Log

## 1.3.3

### Added
- Texture IDs for Ratchet & Clank: Rift Apart textures

## 1.3.2

### Fixed
- Allow export of cubemaps to DDS

## 1.3.1

### Fixed
- Bugfix for BytesPerPixel calculation, causing it to be rounded down to a whole number

## 1.3

### Added
- Option to increase base texture resolution (GUI and command line)
- Directories for each type of input are remembered throughout the session

## 1.2.3

### Added
- Add command line interface
- Version number display in title
- Auto-suggest output filename

### Change
- Rename of various UI components for better clarity

### Fixed
- Bug: Crash when extracting array without hd component


## 1.2.2

### Added
- Size check on hd file to help reduce user errors

### Fixed
- More bugfixes for hd filename process (argh)

## 1.2.1

### Added
- Automatic switch from .hd.texture to .texture

### Fixed
- Bugs in .hd file location code

## 1.2

### Added
- Support for texture arrays (export/modify/scale)

### Changed
- Verbiage

### Fixed
- Grid location of various error markings


## 1.1

### Added
- Support for non-square and single-part textures
- Option to ignore format discrepancies
- Also looks for _hd.texture named files

### Changed
- Source import reports whether the HD part is needed or found
- BytesPerPixel is now floating point
- Layout

