namespace Sitecore.Support.Workflows.Simple
{
    using Sitecore.Configuration;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Workflows;

    public class WorkflowProvider : Sitecore.Workflows.Simple.WorkflowProvider
    {
        public WorkflowProvider(string databaseName, HistoryStore historyStore) : base(databaseName, historyStore)
        {
        }

        private static string GetInheritedWorkflowID(Item item)
        {
            string workflowID = string.Empty;
            Assert.ArgumentNotNull(item, "item");
            if (item.Database.DataManager.DataSource.GetWorkflowInfo(item.ID, item.Language, item.Version) == null)
            {
                string str2 = item.Fields[FieldIDs.Workflow].GetValue(false, false, false, true);
                string str3 = item.Fields[FieldIDs.WorkflowState].GetValue(false, false, false, true);
                if (!(string.IsNullOrEmpty(str2) || string.IsNullOrEmpty(str3)))
                {
                    WorkflowInfo info = new WorkflowInfo(str2, str3);
                    workflowID = info.WorkflowID;
                }
            }
            return workflowID;
        }

        public override IWorkflow GetWorkflow(Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            string workflowId = (Settings.ItemCloning.InheritWorkflowData && item.IsClone) ? GetInheritedWorkflowID(item) : Sitecore.Workflows.Simple.WorkflowProvider.GetWorkflowID(item);
            if (workflowId.Length > 0)
            {
                return this.InstantiateWorkflow(workflowId, this);
            }
            return null;
        }

        protected override IWorkflow InstantiateWorkflow(string workflowId, Sitecore.Workflows.Simple.WorkflowProvider owner) =>
            new Sitecore.Support.Workflows.Simple.Workflow(workflowId, this);
    }
}
