## Optimizely Fullstack ContentProvider

Optimizely Fullstack ContentProvider provides and easy way to bring in Optimizely Fullstack feature flags, events, audiences, variations, and variables into the edit ui seamlessly.

### Configuration

1. Web.config Application Settings
   ```sh
    <add key="optimizely:full-stack:projectId" value="20296260335" />
    <add key="optimizely:full-stack:token" value="2:n9aekv2qnbbCG6ZEV8iYYyiwE8pS20R_tZRMYutd3JZ8y8OIvYZg" />
    <add key="optimizely:full-stack:APIVersion" value="v2" />
    <add key="optimizely:full-stack:Environment" value="development" />
    <add key="optimizely:full-stack:sdkkey" value="MKDwmNvmtcjWPzFMHYUoS" />
    <add key="optimizely:full-stack:cacheinminutes" value="10" />
   ```
2. Rebuild the project.  You might have to clear your personalized view to view the new content provider in the assets tab. 
    1.  Go into edit mode and click on your user icon in the upper right hand corner of the screen.
    2.  Click "My Settings" link in the dropdown.
    3.  Click on the Views and then click Reset View button clear the views.