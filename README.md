## Vuforia Engine Installation

This project uses the Vuforia Engine package for AR functionality.

### How to Install Vuforia

#### Option 1: Manual Download

1. Visit the [Vuforia SDK Download Page](https://developer.vuforia.com/downloads/sdk).
2. Download the **Vuforia Engine for Unity** package.
3. Extract the downloaded package if necessary.
4. Drag and drop the package into your project's `Packages` folder in Unity.

#### Option 2: Install via Unity Package Manager

1. **Open the project in Unity.**
2. Go to **Window > Package Manager**.
3. Click the **+** button (top left) and select **Add package from git URL...**
4. Enter the following URL and click **Add**:

   ```
   https://registry.packages.developer.vuforia.com/com.ptc.vuforia.engine/10.16.5
   ```

   *(Replace `10.16.5` with the version specified in `Packages/manifest.json` if different.)*

5. Unity will automatically download and install the Vuforia Engine package.

---

## How to Build
Ensure Necessary configuration has been set for the Vuforia (e.g license key)
1. In the **Build Settings** window, make sure to add and select both scenes:
    - `Mainscene`
    - `Vuforiascene`
2. Set your desired build platform (e.g., Android or iOS).
3. Click **Build** to generate your application.