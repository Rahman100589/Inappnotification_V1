In-app notification in dynamic 365 Customer Service Workspace
In this article we are going implement notification message in customer service workspace when a new case is created or update.
We are going to activate In-app notification features in model driven app and write a plugin code to get message.
•	Sing in to https://make.powerapps.com/
•	Open the solution that contains the model-driven app.
•	Select the model – driven app and select the edit.
•	Click on settings, Navigate to Features and search for in – app notification and active.
 

 

Write a plugin to get notification when a new case is created in customer service workspace.
Open visual studio select create new project.
 

Search for class library and select the Class Library (.Net Framework) and click on Next.
 
Give the project name and select the latest .Net farmwork and click on create.
 
To install CRM Core Assemblies right click on solution and select Manage NuGet packages as show in below screen and search for Microsoft.CRMSdk.CoreAssemblies and install it.
 

 

Next to install plugin registration tool in solution right click on solution once aging and select Manage NuGet packages as show in below screen and search for Microsoft.CRMSdk.XrmTooling.PluginRegistrationTool and install it.
 
Below references will be get added to the solution once we install both packages.

 

Next Update the below code in Class1.cs and build the solution.
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace InApp_Notofication_Demo
{
    public class Class1 : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            Entity entity = (Entity)context.InputParameters["Target"];

            if (entity.LogicalName != "incident")
            {
                return;
            }
            if (context.MessageName == "Create")
            {
                var request = new OrganizationRequest()
                {
                    RequestName = "SendAppNotification",
                    Parameters = new ParameterCollection
                    {
                        ["Title"] = "A New Case has been Created",
                        ["Recipient"] = entity.GetAttributeValue<EntityReference>("ownerid"),
                        ["Body"] = "A new Case has been created  you.",
                        ["Expiry"] = 1209600,
                        ["Actions"] = new Entity()
                        {
                            Attributes =
                        {
                            ["actions"] = new EntityCollection()
                            {
                                Entities =
                                {
                                    new Entity()
                                    {
                                        Attributes =
                                        {
                                            ["title"] = "Open Case",
                                            ["data"] = new Entity()
                                            {
                                                Attributes =
                                                {
                                                    ["type"] = "url",
                                                    ["url"] = $"?pagetype=entityrecord&etn={entity.LogicalName}&id={entity.Id}",
                                                    ["navigationTarget"] = "newWindow"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        }
                    }
                };

                service.Execute(request);

            }
        }
    }
}
Next open the plugin registration tool to register your plugin, once tool get open click on Create New Connection button, a new window will get open there please select Display list of available originations and show advanced check box and enter the user id and password as show in below screen.
 
Select your environment for list options and click on login.

 


Once tool get open click on register and select register New Assembly and show in below screen. 
 
A New window will get open select our DLL from Load Assembly. And click on register selected plugin. 
 

Once plugin is register right click on that and select Register New Step
 

Select the Message as “Create” and Primary Entity as “incident”.
Select the execution stage as “PostOperation” as shown in like below screen. 
 
 

Finally Navigate to CRM customer service workspace and create new cases in CRM.
Once is new cases is created in CRM notification message received in notification section as shown in below screen.
 

 
 

Thanks for Reading…!
