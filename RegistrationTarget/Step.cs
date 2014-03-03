namespace RegistrationTarget
{
    using System.Collections.Generic;

    public class Step
    {
        public Step(string messageName, PipelineStage stage, object primaryEntity, FilteringAttributes filteringAttributes, string runningUser = "CallingUser")
        {
        }

        public Step(string messageName, PipelineStage stage, object primaryEntity, string runningUser = "CallingUser")
        {
        }

        public Step(string messageName, PipelineStage stage, object primaryEntity, FilteringAttributes filteringAttributes, Image image, string runningUser = "CallingUser")
        {
        }

        public Step(string messageName, PipelineStage stage, object primaryEntity, Image image, string runningUser = "CallingUser")
        {
        }
    }
}
