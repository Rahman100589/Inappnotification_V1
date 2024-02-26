using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InAppNotification_Updated
{
    public class Class1: IPlugin
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
