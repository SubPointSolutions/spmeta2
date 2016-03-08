using System;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint 2010 Workflow ids.
    /// </summary>
    public class BuiltInWorkflowIds
    {
        public static Guid ApprovalSharePoint2010 = new Guid("8ad4d8f0-93a7-4941-9657-cf3706f00409");
        public static Guid CollectFeedbackSharePoint2010 = new Guid("3bfb07cb-5c6a-4266-849b-8d6711700409");
        public static Guid CollectSignaturesSharePoint2010 = new Guid("77c71f43-f403-484b-bcb2-303710e00409");

        public static Guid DispositionApproval = new Guid("dd19a800-37c1-43c0-816d-f8eb5f4a4145");
        public static Guid PublishingApproval = new Guid("e43856d2-1bb4-40ef-b08b-016d89a00409");
        public static Guid ThreeState = new Guid("c6964bff-bf8d-41ac-ad5e-b61ec111731a");
    }
}
