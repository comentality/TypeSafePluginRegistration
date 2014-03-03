namespace TestPlugin
{
    using RegistrationTarget;

    public partial class Plugin
    {
        public Steps Register()
        {
            var qd = new QuoteDetail();

            var preCreate = new Step(
                MessageName.Create,
                PipelineStage.PreOperation,
                qd);

            var preUpdateImage =
                new Image(
                    ImageType.Pre,
                    new FilteringAttributes
                        {
                            qd.PricePerUnit, qd.Quantity, qd.Tax, qd.ManualDiscountAmount
                        });

            var preUpdate = new Step(
                MessageName.Update,
                PipelineStage.PreOperation,
                qd,
                new FilteringAttributes
                    {
                        qd.PricePerUnit, qd.Quantity, qd.Tax
                    },
                preUpdateImage);

            var regu =
                new Steps
                {
                    preCreate, preUpdate
                };

            return regu;
        }
    }
}
