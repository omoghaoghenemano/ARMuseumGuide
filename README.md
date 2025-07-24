## Vuforia Engine Installation

This project uses the Vuforia Engine package for AR functionality.

### How to Install Vuforia

1. **Open the project in Unity.**
2. Go to **Window > Package Manager**.
3. Click the **+** button (top left) and select **Add package from git URL...**
4. Enter the following URL and click **Add**:

   ```
   https://registry.packages.developer.vuforia.com/com.ptc.vuforia.engine/10.16.5
   ```

   *(Replace `10.16.5` with the version specified in `Packages/manifest.json` if different.)*

5. Unity will download and install the Vuforia Engine package automatically.

**Note:**  
You do **not** need to run `npm install` or manually download Vuforia files. The Unity Package Manager handles everything.