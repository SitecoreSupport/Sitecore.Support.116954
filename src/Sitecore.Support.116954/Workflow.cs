    namespace Sitecore.Support.Workflows.Simple
    {
        using Sitecore.Configuration;
        using Sitecore.Data;
        using Sitecore.Data.Items;
        using Sitecore.Workflows.Simple;
        using System;
        using System.Runtime.Serialization;

        public class Workflow : Sitecore.Workflows.Simple.Workflow
        {
            public Workflow(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            public Workflow(string workflowID, Sitecore.Support.Workflows.Simple.WorkflowProvider owner) : base(workflowID, owner)
            {
            }

            public override bool IsApproved(Item item, Database targetDatabase)
            {
                if (Settings.ItemCloning.InheritWorkflowData && item.IsClone)
                {
                    return base.IsApproved(item.Source, targetDatabase);
                }
                return base.IsApproved(item, targetDatabase);
            }
        }
    }
