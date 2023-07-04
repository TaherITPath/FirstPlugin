using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;
namespace FirstPlugin
{
    public class TestPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {

                Entity followup = new Entity("task");
                followup["subject"] = "Send e-mail to the new customer";
                followup["description"] = "Follow up with";
                followup["scheduledstart"] = DateTime.Now.AddDays(7);
                followup["scheduledend"] = DateTime.Now.AddDays(7);
                followup["category"] = context.PrimaryEntityName;

                if (context.OutputParameters.Contains("id")) {
                    Guid guId = new Guid(context.OutputParameters["id"].ToString());
                    string regardingType = "lead";
                    followup["regardingobjectid"] = new EntityReference(regardingType, guId);
                }
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
                service.Create(followup);
            }
            else {
                return;
            }
        }
    }
}
