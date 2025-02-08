# Windows 11 24H2 Text Rendering Image Comparison Bug #

## Overview ##

This repository contains a reproducible test case demonstrating a text rendering issue introduced in Windows 11 24H2.Previously, the provided C# code generated and compared images with rendered text pixel-perfectly. However, after updating to *Windows 11 24H2*, the same code now produces inconsistent results, failing pixel-by-pixel comparisons.

## Reproduction Steps ##

1. Clone this repository:
```
git clone https://github.com/tracktownsoftware/Win11-24H2-ImageTextBug.git
cd Win11-24H2-ImageTextBug
```
2. Open the solution in Visual Studio 2022.

3. Build and run the application.

4. Click the "Compare Images" button.

5. Observe that image comparisons fail, even though the same bitmaps are being generated twice.

## Expected Behavior (Before Win11 24H2) ##

- The code generates **identical images** every time.

- Pixel-by-pixel comparison **always passes**.

## Actual Behavior (After Win11 24H2 Update) ##

- The same images are generated twice but **fail pixel-by-pixel comparison**.

- Small variations in text rendering cause mismatches, suggesting a change in **anti-aliasing, font hinting, or DPI scaling behavior** in Windows 11 24H2.
