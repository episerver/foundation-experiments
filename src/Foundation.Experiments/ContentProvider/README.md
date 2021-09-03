## Optimizely Fullstack ContentProvider

Optimizely Fullstack ContentProvider provides and easy way to bring in Optimizely Fullstack feature flags, events, audiences, variations, and variables into the edit ui seamlessly.

### Configuration

1. Web.config Application Settings
   ```sh
    <add key="episerver:setoption:Optimizely.DeveloperFullStack.Core.ProjectId,Optimizely.DeveloperFullStack" value="ProjectID" />
    <add key="episerver:setoption:Optimizely.DeveloperFullStack.Core.RestAuthToken,Optimizely.DeveloperFullStack" value="AUTHTOKEN" />
    <add key="episerver:setoption:Optimizely.DeveloperFullStack.Core.APIVersion,Optimizely.DeveloperFullStack" value="v2" />
    <add key="episerver:setoption:Optimizely.DeveloperFullStack.Core.EnviromentKey,Optimizely.DeveloperFullStack" value="development" />
    <add key="episerver:setoption:Optimizely.DeveloperFullStack.Core.SDKKey,Optimizely.DeveloperFullStack" value="SDKKEY" />
    <add key="episerver:setoption:Optimizely.DeveloperFullStack.Core.CacheInMinutes,Optimizely.DeveloperFullStack" value="10" />
   ```
2. Rebuild the project.  You might have to clear your personalized view to view the new content provider in the assets tab. 
    1.  Go into edit mode and click on your user icon in the upper right hand corner of the screen.
    2.  Click "My Settings" link in the dropdown.
    3.  Click on the Views and then click Reset View button clear the views.